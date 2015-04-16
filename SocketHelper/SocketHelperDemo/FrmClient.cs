using System;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using SocketHelper;

namespace SocketHelperDemo
{
    public partial class FrmClient : Form
    {

        public FrmClient()
        {
            InitializeComponent();
            //掩耳盗铃线程控制UI控件
            CheckForIllegalCrossThreadCalls = false;
            //创建委托接收数据
            DelegateHelper.TcpClientReceive = MessageCallBack;
            DelegateHelper.TcpClientStateInfo = StateInfoCallBack;
            DelegateHelper.TcpClientErrorMsg = ErrorMsgCallBack;
            //获取本机IP
            TxtIp.Text=Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString();
        }
        /// <summary>
        /// 创建TCPClient实例
        /// </summary>
        private TCPClient _tcpClient; 
        /// <summary>
        /// 初始化并连接TCPClient
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnConClick(object sender, EventArgs e)
        {
            if (_tcpClient == null)
            {
                _tcpClient = new TCPClient(TxtIp.Text, int.Parse(TxtPort.Text));
                _tcpClient.ReConnectionTime = 5000;
            }
            _tcpClient.StartConnection();
        }
        /// <summary>
        /// 接收Socket数据
        /// </summary>
        /// <param name="msg"></param>
        private void MessageCallBack(string msg)
        {
            try
            {
                if (MsgInfomationList.Items.Count > 50)
                    MsgInfomationList.Items.Clear();
                //自定义处理接收Socket数据
                MsgInfomationList.Items.Add(msg);
            }
            catch
            {

            }
        }
        /// <summary>
        /// 接收状态数据
        /// </summary>
        /// <param name="msg"></param>
        private void StateInfoCallBack(string msg)
        {
            try
            {
                if (StateInfoList.Items.Count > 50)
                    StateInfoList.Items.Clear();
                //自定义处理状态数据
                StateInfoList.Items.Add(msg);
            }
            catch 
            {

            }
        }
        /// <summary>
        /// 接收错误数据
        /// </summary>
        /// <param name="msg"></param>
        private void ErrorMsgCallBack(string msg)
        {
            try
            {
                if (ErrorMsgList.Items.Count > 50)
                    ErrorMsgList.Items.Clear();
                //自定义处理错误数据
                ErrorMsgList.Items.Add(msg);
            }
            catch
            {
            }
        }
        /// <summary>
        /// 发送Socket数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSendClick(object sender, EventArgs e)
        {
            try
            {
                if (_tcpClient != null)
                    _tcpClient.SendCommand(TxtSendMsg.Text);
            }
            catch
            {
            }
        }
        /// <summary>
        /// 退出时关闭所有线程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            BtnCloseTcpClientClick(null, null);
            iscloseForm = true;
            Close();
        }

        private void BtnCloseTcpClientClick(object sender, EventArgs e)
        {
            if (_tcpClient != null)
            {
                _tcpClient.StopConnection();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Thread t=new Thread(new ThreadStart(yali));
            t.Start();
        }

        public bool iscloseForm = false;
        public void yali()
        {
            for (int i = 0; i < 500; i++)
            {
                if (iscloseForm)
                    return;
                ThreadPool.QueueUserWorkItem(client =>
                {
                    try
                    {

                        TCPClient tc = new TCPClient(TxtIp.Text, int.Parse(TxtPort.Text));
                        tc.ReConnectionTime = 5000;
                        tc.StartConnection();
                    }
                    catch
                    {
                    }
                });
            }
        }
    }
}
