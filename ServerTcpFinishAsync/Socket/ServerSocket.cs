using System.Net;
using System.Net.Sockets;

namespace ServerTcpFinishAsync
{
    class ServerSocket
    {
        public static ServerSocket Instance;
        Socket socket;
        Dictionary<int, ClientSocket> dicClientSocket;
        List<ClientSocket> waitRemoveClientSocket;
        bool isLanuch;
        public bool IsConnect => isLanuch;
        public ServerSocket()
        {
            Instance = this;
            isLanuch = false;
            dicClientSocket = new Dictionary<int, ClientSocket>();
            waitRemoveClientSocket = new List<ClientSocket>();
        }
        public void AddRemoveClientSocket(ClientSocket cs)
        {
            waitRemoveClientSocket.Add(cs);
        }
        public void Start(string ip, int port, int connectMaxCount)
        {
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Bind(new IPEndPoint(IPAddress.Parse(ip), port));

                socket.Listen(connectMaxCount);


                isLanuch = true;


                socket.BeginAccept(Accept, socket);

                Task.Run(CheckHeartMsg);
            }
            catch (SocketException se)
            {
                Console.WriteLine($"错误{se.ErrorCode}：{se.Message}");
            }
        }

        public void Accept(IAsyncResult ar)
        {
            try
            {
                Socket server = ar.AsyncState as Socket;
                if (server == null)
                {
                    Close();
                    return;
                }
                if (isLanuch)
                {

                    var clientSkt = server.EndAccept(ar);
                    if (clientSkt != null)
                    {
                        var tmp = new ClientSocket(clientSkt);
                        tmp.StartReceive();
                        lock (dicClientSocket)
                        {
                            dicClientSocket.Add(tmp.id, tmp);
                        }
                        Console.WriteLine($"连入：{clientSkt.RemoteEndPoint}");
                    }
                }
                server.BeginAccept(Accept, server);

            }
            catch (SocketException se)
            {

                Console.WriteLine($"错误{se.ErrorCode}：{se.Message}");

            }


        }

        public void CheckHeartMsg()
        {

            try
            {

                while (isLanuch)
                {
                    if (socket == null)
                        break;

                    lock (dicClientSocket)
                    {

                        long curTime = DateTime.Now.Ticks / TimeSpan.TicksPerSecond;

                        foreach (var item in dicClientSocket)
                        {
                            //检测 心跳 
                            item.Value?.CheckHeartMsg(curTime);
                        }

                        if (waitRemoveClientSocket.Count > 0)
                        {
                            for (int i = 0; i < waitRemoveClientSocket.Count; i++)
                            {
                                waitRemoveClientSocket[i]?.Close();
                                dicClientSocket.Remove(waitRemoveClientSocket[i].id);
                            }
                            waitRemoveClientSocket.Clear();
                        }


                    }








                }

            }
            catch (SocketException se)
            {
                Console.WriteLine($"错误{se.ErrorCode}：{se.Message}");
            }
        }
        public void Broadcast(BaseMsgData md)
        {

            try
            {

                if (isLanuch)
                {
                    lock (dicClientSocket)
                    {
                        foreach (var item in dicClientSocket)
                        {
                            item.Value?.Send(md);
                        }

                    }

                }

            }
            catch (SocketException se)
            {
                Console.WriteLine($"错误{se.ErrorCode}：{se.Message}");
            }
        }
        public void Close()
        {

            isLanuch = false;
            foreach (var item in dicClientSocket)
            {
                item.Value.Close();
            }
            dicClientSocket.Clear();


        }

    }
}
