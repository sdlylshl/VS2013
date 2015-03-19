using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.IO;
using System.Net;
using System.Threading;
using Newtonsoft.Json;

namespace SyncChatClient
{
    public partial class ChatClient : Form
    {
        private bool isExit = false;
        private TcpClient client;
        private BinaryReader br;
        private BinaryWriter bw;
        public ChatClient()
        {
            InitializeComponent();       
        }

        private void Main_Form_Load(object sender, EventArgs e)
        {
            Random ran = new Random((int)DateTime.Now.Ticks);
            this.txt_UserName.Text = "Sunshine" + ran.Next(1, 999);
            this.lbx_Online.HorizontalScrollbar = true;
        }

        /// <summary>
        /// 登陆，连接服务器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Login_Click(object sender, EventArgs e)
        {
            this.btn_Login.Enabled = false;
            try
            {
                client = new TcpClient(Dns.GetHostName(), 10008);
                PrintMessage("连接成功");
            }
            catch
            {
                PrintMessage("连接失败");
                this.btn_Login.Enabled = true;
                return;
            }
            //获取网络流
            NetworkStream m_NetStream = client.GetStream();
            //将网络流作为二进制读写对象
            bw = new BinaryWriter(m_NetStream);
            br = new BinaryReader(m_NetStream);
            SendMessage(Command.GetLoginMessage(txt_UserName.Text));
            Thread receiveThread = new Thread(new ThreadStart(ReceiveData));
            receiveThread.IsBackground = true;
            receiveThread.Start();
        }

        /// <summary>
        /// 处理接收到的服务端数据
        /// </summary>
        private void ReceiveData()
        {
            string receivString = null;
            while (isExit == false)
            {
                try
                {
                    //从网络流流中读取字符串
                    //此方法会自动判断字符串长度前缀，并根据长度前缀读取字符串
                    receivString = br.ReadString();
                }
                catch
                {
                    if (isExit == false)
                    {
                        MessageBox.Show("与服务器失去联系");
                    }
                    break;
                }
                Command cmd = JsonConvert.DeserializeObject<Command>(receivString);
                switch (cmd.CmdType)
                {
                    case CmdType.Login:
                        AddOnline(cmd.UserName);
                        break;
                    case CmdType.Logout:
                        RemoveUserName(cmd.UserName);
                        break;
                    case CmdType.Talk:
                        PrintMessage(string.Format("{0}:", cmd.UserName));
                        PrintMessage(cmd.Message);
                        break;
                    default:
                        PrintMessage("什么意思？");
                        break;
                }
            }
        }

        private void PrintMessage(string str)
        {
            if (this.rtb_Dialog.InvokeRequired)
            {
                Action<string> d=PrintMessage;
                this.rtb_Dialog.Invoke(d, new object[] { str });
            }
            else
            {                
                this.rtb_Dialog.AppendText(str + Environment.NewLine);
                this.rtb_Dialog.ScrollToCaret();
            }
        }

        /// <summary>
        /// 添加其他在线客户端信息
        /// </summary>
        /// <param name="str"></param>
        private void AddOnline(string str)
        {
            if (this.lbx_Online.InvokeRequired)
            {
                Action<string> d = AddOnline;
                this.lbx_Online.Invoke(d, new object[] { str });
            }
            else
            {
                this.lbx_Online.Items.Add(str);
                this.lbx_Online.SelectedIndex = this.lbx_Online.Items.Count-1;
                this.lbx_Online.ClearSelected();
            }
        }

        /// <summary>
        /// 向服务器发送信息
        /// </summary>
        /// <param name="message"></param>
        private void SendMessage(string message)
        {
            try
            {
                bw.Write(message);
                bw.Flush();
            }
            catch
            {
                PrintMessage("发送消息失败");
            }
        }

        /// <summary>
        /// 移除掉线的用户
        /// </summary>
        /// <param name="userName"></param>
        private void RemoveUserName(string userName)
        {
            //在UI线程之外(主线程)访问，需要使用委托来间接调用
            if (this.lbx_Online.InvokeRequired)
            {
                Action<string> d = RemoveUserName;
                this.lbx_Online.Invoke(d, userName);
            }
            else
            {
                this.lbx_Online.Items.Remove(userName);
                this.lbx_Online.SelectedIndex = this.lbx_Online.Items.Count - 1;
                this.lbx_Online.ClearSelected();
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Send_Click(object sender, EventArgs e)
        {
            if (this.lbx_Online.SelectedIndex != -1)
            {
                SendMessage(Command.GetSendMessage(this.txt_UserName.Text,this.lbx_Online.SelectedItem.ToString(), this.txt_SendMessage.Text));
                this.txt_SendMessage.Clear();
            }
            else
            {
                MessageBox.Show("请先在[当前在线添加一个对话者]");
            }
        }


        private void Main_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            //未与服务器连接前client为null
            if (client != null)
            {
                SendMessage(Command.GetLogotMessage(this.txt_UserName.Text));
                isExit = true;
                br.Close();
                bw.Close();
                client.Close();
            }
        }

        private void txt_UserName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                //触发btn_Send的Client事件
                this.btn_Send.PerformClick();
            }
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }     
    }
}
