using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SyncChatClient
{
    public enum CmdType
    {
        /// <summary>
        /// 登陆
        /// </summary>
        Login = 1,
        /// <summary>
        /// 登出
        /// </summary>
        Logout = 2,
        /// <summary>
        /// 交谈
        /// </summary>
        Talk = 3,
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
        public string UserName { get; set; }

        /// <summary>
        /// 聊天消息接收方
        /// </summary>
        public string ChatReceiver { get; set; }

        /// <summary>
        /// 发送的消息
        /// </summary>
        public string Message { get; set; }

        public static string GetLoginMessage(string userName)
        {
            Command cmd = new Command();
            cmd.CmdType = CmdType.Login;
            cmd.UserName = userName;

            return JsonConvert.SerializeObject(cmd);
        }

        public static string GetLogotMessage(string userName)
        {
            Command cmd = new Command();
            cmd.CmdType = CmdType.Logout;
            cmd.UserName = userName;

            return JsonConvert.SerializeObject(cmd);
        }

        public static string GetSendMessage(string userName, string tagetUserName, string message)
        {
            Command cmd = new Command();
            cmd.CmdType = CmdType.Talk;
            cmd.ChatReceiver = tagetUserName;
            cmd.Message = message;
            cmd.UserName = userName;
            return JsonConvert.SerializeObject(cmd);
        }
    }
}
