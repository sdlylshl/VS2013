using System.Net.Sockets;
using System.IO;

namespace SyncChatServer
{
    /// <summary>
    /// 用于保存与客户端通信需要的信息
    /// </summary>
    class User
    {
        public TcpClient Client { get; private set; }
        public BinaryReader Br { get; private set; }
        public BinaryWriter Bw { get; private set; }
        public string UserName { get; set; }
        public User(TcpClient client)
        {
            this.Client = client;
            NetworkStream m_NetStream = client.GetStream();
            Br = new BinaryReader(m_NetStream);
            Bw = new BinaryWriter(m_NetStream);
        }

        public void Close()
        {
            Br.Close();
            Bw.Close();
            Client.Close();
        }
    }
}
