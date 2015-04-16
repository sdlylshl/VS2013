/********************************************************************
 * *
 * * Copyright (C) 2013-? Corporation All rights reserved.
 * * 作者： BinGoo QQ：315567586 
 * * 请尊重作者劳动成果，请保留以上作者信息，禁止用于商业活动。
 * *
 * * 创建时间：2014-08-05
 * * 说明：DelegateHelper委托类
 * *
********************************************************************/

using System;
using System.Net.Sockets;

namespace SocketHelper
{
    /// <summary>
    /// 回调委托方法
    /// </summary>
    public class DelegateHelper
    {
        #region 委托

        #region 客户端委托方法

        /// <summary>
        /// 客户端返回接收数据委托方法
        /// </summary>
        public static SocketMsgCallBack TcpClientReceive;
        /// <summary>
        /// 客户端返回错误数据委托方法
        /// </summary>
        public static SocketMsgCallBack TcpClientErrorMsg;
        /// <summary>
        /// 客户端返回状态信息委托方法
        /// </summary>
        public static SocketMsgCallBack TcpClientStateInfo;
        #endregion

        #region 服务端委托方法
        /// <summary>
        /// 服务端返回接收数据委托方法
        /// </summary>
        public static SocketReadCallBack TcpServerReceive;
        /// <summary>
        /// 服务端返回错误数据委托方法
        /// </summary>
        public static SocketMsgCallBack TcpServerErrorMsg;
        /// <summary>
        /// 服务端返回状态信息委托方法
        /// </summary>
        public static SocketMsgCallBack TcpServerStateInfo;
        /// <summary>
        /// 添加和删除客户端
        /// </summary>
        public static AddOrDelClientSocketCallBack TcpServerAddClient;
        public static AddOrDelClientSocketCallBack TcpServerDelClient;
        public static SocketMsgCallBack ReturnClientCountCallBack;
        #endregion

        #region 委托申明
        /// <summary>
        /// 接收Socket和数据的委托
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="msg"></param>
        public delegate void SocketReadCallBack(Socket socket,string msg);
        /// <summary>
        /// 接收数据的委托
        /// </summary>
        /// <param name="msg"></param>
        public delegate void SocketMsgCallBack(string msg);
        /// <summary>
        /// 接收Socket状态的委托
        /// </summary>
        /// <param name="socketState"></param>
        public delegate void SocketStateCallBack(EnumClass.SocketState socketState);
        /// <summary>
        /// 添加/删除下线的客户端委托
        /// </summary>
        /// <param name="socket"></param>
        public delegate void AddOrDelClientSocketCallBack(Socket socket);
        #endregion

        public static void BaseVoid(string str)
        {
        }
        public static void BaseVoid(Socket socker)
        {
        }
        public static void BaseVoid(Socket socker, string str)
        {
        }

        #endregion
    }
}
