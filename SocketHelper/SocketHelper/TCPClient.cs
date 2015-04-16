/********************************************************************
 * *
 * * Copyright (C) 2013-? Corporation All rights reserved.
 * * 作者： BinGoo QQ：315567586 
 * * 请尊重作者劳动成果，请保留以上作者信息，禁止用于商业活动。
 * *
 * * 创建时间：2014-08-05
 * * 说明：
 * *
********************************************************************/
#region 说明
/* 简介：Socket通讯客户端实现网络通讯
 * 功能介绍：Socket通讯客户端实现网络通讯，支持断开重连。
 * socket客户端封装类的调用三步：
 * 1、初始化：
 * string ip="127.0.0.1";
 * int port=5100;
 * TCPClient_tcpClient = new TCPClient(ip,port);
 * 
 * 2、创建委托接收数据方法并绑定（可根据需求定义），此类暂时定义了四种接收数据的委托：返回接收客户端的数据，返回客户端连接状态和监听状态，返回错误信息，返回客户端数量的委托
 * 
 * ①申明返回接收数据的委托方法
 * DelegateHelper.TcpServerReceive= 自定义方法;
 * 
 * ②申明返回状态信息的委托方法
 * DelegateHelper.TcpServerStateInfo= 自定义方法;
 * 
 * ③申明返回错误信息的委托方法
 * DelegateHelper.TcpServerErrorMsg = 自定义方法;
 * 
 * 
 * 3、启动和关闭方法：
 * TCPCliet.StartConnection();
 * TCPCliet.StopConnection();
  */
#endregion
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SocketHelper
{
    public class TCPClient
    {
        #region 属性
        private string _serverip;
        /// <summary>
        /// 服务端IP
        /// </summary>
        public string ServerIp
        {
            set { _serverip = value; }
            get { return _serverip; }
        }
        private int _serverport;
        /// <summary>
        /// 服务端监听端口
        /// </summary>
        public int ServerPort
        {
            set { _serverport = value; }
            get { return _serverport; }
        }
        private TcpClient _tcpclient = null;
        /// <summary>
        /// TcpClient客户端
        /// </summary>
        public TcpClient Tcpclient
        {
            set { _tcpclient = value; }
            get { return _tcpclient; }
        }
        private Thread _tcpthread = null;
        /// <summary>
        /// Tcp客户端连接线程
        /// </summary>
        public Thread Tcpthread
        {
            set { _tcpthread = value; }
            get { return _tcpthread; }
        }
        private bool _isStarttcpthreading = false;
        /// <summary>
        /// 是否启动Tcp连接线程
        /// </summary>
        public bool IsStartTcpthreading
        {
            set { _isStarttcpthreading = value; }
            get { return _isStarttcpthreading; }
        }
        private bool _isclosed=false;
        /// <summary>
        /// 连接是否关闭（用来断开重连）
        /// </summary>
        public bool Isclosed
        {
            set { _isclosed = value; }
            get { return _isclosed; }
        }

        private int _reConnectionTime = 3000;
        /// <summary>
        /// 设置断开重连时间间隔单位（毫秒）（默认3000毫秒）
        /// </summary>
        public int ReConnectionTime {
            get { return _reConnectionTime; }
            set { _reConnectionTime = value; }
        }
        private string _receivestr;
        /// <summary>
        ///  接收Socket数据包 缓存字符串
        /// </summary>
        public string Receivestr
        {
            set { _receivestr = value; }
            get { return _receivestr; }
        }
        /// <summary>
        /// 重连次数
        /// </summary>
        private int _reConectedCount = 0;
        public int ReConectedCount {
            get { return _reConectedCount; }
            set { _reConectedCount = value; }
        }

        #endregion

        #region 方法
        /// <summary>
        /// 十六进制字字符串转为节数组
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static byte[] StringToHexByteArray(string s)
        {
            s = s.Replace(" ", "");
            if ((s.Length % 2) != 0)
                s += " ";
            byte[] returnBytes = new byte[s.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(s.Substring(i * 2, 2), 16);
            return returnBytes;
        }
        /// <summary>
        /// 启动连接Socket服务器
        /// </summary>
        public void StartConnection()
        {
            try
            {
                //Isclosed = false;
                CreateTcpClient();
            }
            catch (Exception ex)
            {
                DelegateHelper.TcpClientErrorMsg("错误信息：" + ex.Message);
            }
        }
        /// <summary>
        /// 创建线程连接
        /// </summary>
        private void CreateTcpClient()
        {

            if (Isclosed)
                return;
            //标示已启动连接，防止重复启动线程
            Isclosed = true;
            Tcpclient = new TcpClient();
            Tcpthread = new Thread(StartTcpThread);
            IsStartTcpthreading = true;
            Tcpthread.Start();
        }
        /// <summary>
        ///  线程接收Socket上传的数据
        /// </summary>
        private void StartTcpThread()
        {
            byte[] receivebyte = new byte[1024];
            int bytelen;
            try
            {
                while (IsStartTcpthreading)
                {
                    if (!Tcpclient.Connected)
                    {
                        try
                        {
                            if (ReConectedCount != 0)
                            {
                                //返回状态信息
                                DelegateHelper.TcpClientStateInfo(string.Format("正在第{0}次重新连接服务器... ...", ReConectedCount));
                            }
                            else
                            {
                                //SocketStateInfo
                                DelegateHelper.TcpClientStateInfo("正在连接服务器... ...");
                            }
                            Tcpclient.Connect(IPAddress.Parse(ServerIp), ServerPort);
                            DelegateHelper.TcpClientStateInfo("已连接服务器");
                            //Tcpclient.Client.Send(Encoding.Default.GetBytes("login"));
                        }
                        catch
                        {
                            //连接失败
                            ReConectedCount++;
                            //强制重新连接
                            Isclosed = false;
                            IsStartTcpthreading = false;
                            //每三秒重连一次
                            Thread.Sleep(ReConnectionTime);
                            continue;
                        }
                    }
                    bytelen = Tcpclient.Client.Receive(receivebyte);
                    // 连接断开
                    if (bytelen == 0)
                    {
                        //返回状态信息
                        DelegateHelper.TcpClientStateInfo("与服务器断开连接... ...");
                        // 异常退出、强制重新连接
                        Isclosed = false;
                        ReConectedCount = 1;
                        IsStartTcpthreading = false;
                        continue;
                    }
                    Receivestr = ASCIIEncoding.Default.GetString(receivebyte, 0, bytelen);
                    if (Receivestr.Trim() != "")
                    {
                        //接收数据
                        DelegateHelper.TcpClientReceive(Receivestr);
                    }
                }
                //此时线程将结束，人为结束，自动判断是否重连
                CreateTcpClient();
            }
            catch (Exception ex)
            {
                //CreateTcpClient();
                //返回错误信息
                DelegateHelper.TcpClientErrorMsg("错误信息：" + ex.Message);
            }
        }
        /// <summary>
        /// 断开连接
        /// </summary>
        public void StopConnection()
        {
            //关闭连接
            Tcpclient.Close();
            //关闭线程
            Tcpthread.Abort();
            //Tcpthread = null;
            Isclosed = false;
            IsStartTcpthreading = false;
            DelegateHelper.TcpClientStateInfo("断开连接");
            //标示线程已关闭可以重新连接
        }

        /// <summary>
        /// 发送Socket消息
        /// </summary>
        /// <param name="cmdstr"></param>
        public void SendCommand(string cmdstr)
        {
            try
            {
                //byte[] _out=Encoding.GetEncoding("GBK").GetBytes(cmdstr);
                byte[] _out = Encoding.Default.GetBytes(cmdstr);
                Tcpclient.Client.Send(_out);
            }
            catch (Exception ex)
            {
                //返回错误信息
                DelegateHelper.TcpClientErrorMsg(ex.Message);
            }
        }
        /// <summary>
        /// 发送Socket消息
        /// </summary>
        /// <param name="byteMsg"></param>
        public void SendCommand(byte[] byteMsg)
        {
            try
            {
                Tcpclient.Client.Send(byteMsg);
            }
            catch (Exception ex)
            {
                //返回错误信息
                DelegateHelper.TcpClientErrorMsg("错误信息：" + ex.Message);
            }
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 初始化TCPClient类
        /// </summary>
        /// <param name="ip">服务端IP</param>
        /// <param name="port">监听端口</param>
        public TCPClient(string ip, int port)
        {
            if (DelegateHelper.TcpClientReceive == null)
            {
                DelegateHelper.TcpClientReceive = DelegateHelper.BaseVoid;
            }
            if (DelegateHelper.TcpClientErrorMsg == null)
            {
                DelegateHelper.TcpClientReceive = DelegateHelper.BaseVoid;
            }
            if (DelegateHelper.TcpClientStateInfo == null)
            {
                DelegateHelper.TcpClientReceive = DelegateHelper.BaseVoid;
            }
            ServerIp = ip;
            ServerPort = port;
        } 
        #endregion
    }
}
