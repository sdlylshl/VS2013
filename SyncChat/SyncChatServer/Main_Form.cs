using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Newtonsoft.Json;

namespace SyncChatServer
{
    public partial class Main_Form : Form
    {
        /// <summary>
        /// 存放所有的Client
        /// </summary>
        List<User> userList = new List<User>();
        Dictionary<string, User> userDic = new Dictionary<string, User>();
        /// <summary>
        /// 监听端口
        /// </summary>
        private const int port = 10008;

        private TcpListener listener;
        /// <summary>
        /// 是否退出所有监听线程 
        /// </summary>
        bool isNormalExit = false;

        public Main_Form()
        {
            InitializeComponent();
        }

        private void Main_Form_Load(object sender, EventArgs e)
        {
            //加载本机IP列表
            this.cmb_Address.Items.AddRange(this.GetIPList());
            this.cmb_Address.SelectedIndex = this.cmb_Address.Items.Count > 0 ? 0 : -1;

            this.rtb_State.ScrollBars = RichTextBoxScrollBars.Both;
            this.rtb_State.BorderStyle = BorderStyle.FixedSingle;
        }

        /// <summary>
        /// 启动/停止监听
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_listener_Click(object sender, EventArgs e)
        {
            listener = new TcpListener(IPAddress.Parse(this.cmb_Address.Text), port);
            listener.Start();
            PrintMessage(string.Format("开始在{0}:{1}监听客户端连接", this.cmb_Address.Text, port));
            //创建一个监听客户端连接的线程
            Thread connectionThread = new Thread(ListenClientConnection);
            connectionThread.Start();
            this.btn_listener.Enabled = false;
            this.cmb_Address.Enabled = false;
            this.btn_Stop.Enabled = true;
        }

        private void Main_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (listener != null)
            {
                //一定要释放掉资源，否则关闭程序后会仍在后台运行
                this.btn_Stop.PerformClick();
            }
        }

        private void btn_Stop_Click(object sender, EventArgs e)
        {
            PrintMessage("开始停止服务，并依此使用户退出.");
            isNormalExit = true;
            for (int i = 0; i < userList.Count; i++)
            {
                RemoveUser(userList[i]);
            }
            //通过停止监听让listener.AccptTcpClient()产生异常并退出监听线程
            listener.Stop();
            PrintMessage(string.Format("停止在{0}:{1}监听客户端连接", this.cmb_Address.Text, port));

            this.btn_listener.Enabled = true;
            this.cmb_Address.Enabled = true;
            this.btn_Stop.Enabled = false;
        }

        /// <summary>
        /// 接收客户端连接
        /// </summary>
        private void ListenClientConnection()
        {
            TcpClient newClient = null;
            while (true)
            {
                try
                {
                    //阻塞线程，直到有新的客户端请求
                    newClient = listener.AcceptTcpClient();
                }
                catch
                {
                    //当点击"停止监听"按钮，或退出此窗体时AeecptTcpClient()会抛出异常
                    //因此可以利用此异常退出
                    break;
                }
                //没接收一个客户端连接就创建一个对应的线程循环接收客户端发送的消息
                User user = new User(newClient);
                //创建为每个新连接的客户端创建接收数据的线程
                Thread reciveThread = new Thread(ReciveData);
                reciveThread.Start(user);
                //添加到客户端列表中
                userList.Add(user);
                PrintMessage(string.Format("[{0}]进入", newClient.Client.RemoteEndPoint));
                PrintMessage(string.Format("当前用户连接数{0}", userList.Count));
            }
        }

        /// <summary>
        /// 处理接收的客户端数据
        /// </summary>
        /// <param name="client"></param>
        private void ReciveData(object userState)
        {
            User user = (User)userState;
            TcpClient client = user.Client;
            while (isNormalExit == false)
            {
                string receiveString = null;
                try
                {
                    //从网络流中读取字符串
                    receiveString = user.Br.ReadString();
                }
                catch
                {
                    if (isNormalExit == false)
                    {
                        PrintMessage(string.Format("与[{0}]失去连接，已终止接收该用户信息", client.Client.RemoteEndPoint));
                        RemoveUser(user);
                    }
                    break;
                }
                PrintMessage(string.Format("来自[{0}]:{1}", user.Client.Client.RemoteEndPoint, receiveString));
                //反序列化字符串
                Command cmd = JsonConvert.DeserializeObject<Command>(receiveString);

                switch (cmd.CmdType)
                {
                    case CmdType.Login:
                        user.UserName = cmd.UserName;
                        //添加到字典对象中
                        userDic.Add(cmd.UserName, user);
                        //通知在线所有用户
                        SendToAllClient(user, receiveString);
                        break;
                    case CmdType.Logout:
                        //通知在线所有用户
                        SendToAllClient(user, receiveString);
                        break;
                    case CmdType.Talk:
                        //打印消息
                        PrintMessage(string.Format("{0}对{1}说：{2}", cmd.UserName, cmd.ChatReceiver, cmd.Message));
                        SendToClient(user, receiveString);
                        SendToClient(userDic[cmd.ChatReceiver], receiveString);
                        break;
                    default:
                        PrintMessage("你说啥呢？傻孩子:" + receiveString);
                        break;
                }
            }
        }

        /// <summary>
        /// 发送消息给user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        private void SendToClient(User user, string message)
        {
            try
            {
                //将字符串写入网络流,此方法或自动附近字符串长度
                user.Bw.Write(message);
                user.Bw.Flush();
                PrintMessage(string.Format("向[{0}]发送[{1}]", user.UserName, message));
            }
            catch
            {
                PrintMessage(string.Format("向[{0}]发送消息失败", user.UserName));
            }
        }

        /// <summary>
        /// 由用户登陆或者有用户登出，通知所有在线用户
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        private void SendToAllClient(User user, string message)
        {
            Command cmd = JsonConvert.DeserializeObject<Command>(message);
            if (cmd.CmdType == CmdType.Login)
            {
                foreach (User li in userList)
                {
                    this.SendToClient(li, message);
                    if (li.UserName != user.UserName)
                    {
                        //替换JSON反序列化对象的值，再序列化为JSON字符串
                        cmd.UserName = li.UserName;
                        this.SendToClient(user, JsonConvert.SerializeObject(cmd));
                    }
                }
            }
            else if (cmd.CmdType == CmdType.Logout)
            {
                foreach (User li in userList)
                {
                    if (li.UserName != user.UserName)
                    {
                        this.SendToClient(li, message);
                    }
                }
            }
        }

        /// <summary>
        /// 移除用户
        /// </summary>
        /// <param name="user"></param>
        private void RemoveUser(User user)
        {
            userList.Remove(user);
            user.Close();
            PrintMessage(string.Format("当前用户连接数{0}", userList.Count));
        }

        /// <summary>
        /// 在富文本框中打印消息
        /// </summary>
        /// <param name="str"></param>
        private void PrintMessage(string str)
        {
            //判断是否在UI线程所在调用，如果是，需要使用委托来间接调用
            if (this.rtb_State.InvokeRequired)
            {
                Action<string> d = PrintMessage;
                this.rtb_State.Invoke(d, str);
            }
            else
            {
                this.rtb_State.AppendText(str + "\n");
                this.rtb_State.ScrollToCaret();
            }
        }

        /// <summary>
        /// 获取本机所以IP地址
        /// </summary>
        /// <returns></returns>
        private IPAddress[] GetIPList()
        {
            return Dns.GetHostAddresses(Dns.GetHostName());
        }
    }
}
