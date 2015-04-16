using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using SocketHelper;

namespace SocketHelperDemo
{
    public partial class FrmServer : Form
    {
        TCPServer server; 
        public FrmServer()
        {
            InitializeComponent();
            //掩耳盗铃线程控制UI控件
            CheckForIllegalCrossThreadCalls = false;
            DelegateHelper.TcpServerErrorMsg = ErrorMsgData;
            DelegateHelper.TcpServerReceive = ReceviedData;
            DelegateHelper.TcpServerStateInfo = StateInfoData;
            DelegateHelper.ReturnClientCountCallBack = GetClientCount;
            DelegateHelper.TcpServerAddClient = AddClient;
            DelegateHelper.TcpServerDelClient = DelClient;
        }

        private void BtnCon_Click(object sender, EventArgs e)
        {
            server = new TCPServer(int.Parse(TxtPort.Text));
            server.Start();
        }

        private void GetClientCount(string count)
        {
            try
            {
                label5.Text = (string.Format("客户端数量：{0}", count));
            }
            catch
            {
            }
        }
        private void ReceviedData(Socket temp,string msg)
        {
            try
            {
                string ip = ((IPEndPoint) temp.RemoteEndPoint).Address.ToString();
                string port = ((IPEndPoint) temp.RemoteEndPoint).Port.ToString();
                MsgInfomationList.Items.Add(string.Format("IP：{0}-端口：{1}=>发来消息：{2}", ip, port, msg));
            }
            catch
            {
            }
        }
        private void StateInfoData(string msg)
        {
            try
            {
                StateInfoList.Items.Add(string.Format("状态消息：{0}", msg));
            }
            catch
            {
            }
        }
        private void ErrorMsgData(string msg)
        {
            try
            {
                ErrorMsgList.Items.Add(string.Format("错误消息：{0}", msg));
            }
            catch
            {
            }
        }

        private void AddClient(Socket temp)
        {
            try
            {
                string ip = ((IPEndPoint) temp.RemoteEndPoint).Address.ToString();
                string port = ((IPEndPoint) temp.RemoteEndPoint).Port.ToString();
                ClientList.Items.Add(string.Format("{0}:{1}", ip, port));
            }
            catch
            {
            }
        }

        private void DelClient(Socket temp)
        {
            try
            {
                string ip = ((IPEndPoint) temp.RemoteEndPoint).Address.ToString();
                string port = ((IPEndPoint) temp.RemoteEndPoint).Port.ToString();
                ClientList.Items.Remove(string.Format("{0}:{1}", ip, port));
            }
            catch
            {
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(client =>
            {
                try
                {
                    FrmClient frmClent = new FrmClient();
                    frmClent.ShowDialog();
                }
                catch
                {
                }
            });
        }

        private void FrmServer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (server != null)
                server.Stop();
            System.Environment.Exit(0);
        }

        private void BtnSend_Click(object sender, EventArgs e)
        {
            if (ClientList.SelectedItem != null)
            {
                try
                {
                    string[] strArr = ClientList.Items[ClientList.SelectedIndex].ToString().Split(':');
                    server.SendData(strArr[0], int.Parse(strArr[1]), TxtSendMsg.Text);
                }
                catch
                {
                }
            }
        }

        private void FrmServer_FormClosed(object sender, FormClosedEventArgs e)
        {
           
        }
    }
}
