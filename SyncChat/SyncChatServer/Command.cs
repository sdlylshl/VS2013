using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SyncChatServer
{
    public enum CmdType
    {
        /// <summary>
        /// 登录
        /// </summary>
        Login=1,
        /// <summary>
        /// 登出
        /// </summary>
        Logout=2,
        /// <summary>
        /// 交谈
        /// </summary>
        Talk=3,
    }
    /// <summary>
    /// 命令类,使用JSON序列化在客户端和服务器端之间消息传递
    /// </summary>
    class Command
    {
        /// <summary>
        /// 命令类型
        /// </summary>
        public CmdType CmdType { get; set; }

        /// <summary>
        /// 聊天消息发送方
        /// </summary>
        public string UserName{ get; set; }

        /// <summary>
        /// 聊天消息接收方
        /// </summary>
        public string ChatReceiver { get; set; }

        /// <summary>
        /// 发送的消息
        /// </summary>
        public string Message { get; set; }
    }
}
