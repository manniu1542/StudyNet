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


                Task.Run(Accept);
                Task.Run(Receive);
            }
            catch (SocketException se)
            {
                Console.WriteLine($"错误{se.ErrorCode}：{se.Message}");
            }
        }

        public void Accept()
        {

            socket.BeginAccept(async (IAsyncResult ar) =>
            {
                //await ar.AsyncWaitHandle;


            }, null);

            try
            {
                while (isLanuch)
                {
                    var sotTmp = socket.Accept();
                    if (sotTmp != null)
                    {

                        Console.WriteLine($"当前的线程id:{Thread.CurrentThread.ManagedThreadId},连入：{sotTmp.RemoteEndPoint}");
                        var tmp = new ClientSocket(sotTmp);
                        lock (dicClientSocket)
                        {
                            dicClientSocket.Add(tmp.id, tmp);
                        }



                    }

                }

            }
            catch (SocketException se)
            {
                Console.WriteLine($"错误{se.ErrorCode}：{se.Message}");
            }
        }
        public void Receive()
        {

            try
            {

                while (isLanuch)
                {

                    lock (dicClientSocket)
                    {
                        long curTime = DateTime.Now.Ticks / TimeSpan.TicksPerSecond;


                        foreach (var item in dicClientSocket)
                        {
                            item.Value?.Receive();
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
            lock (dicClientSocket)
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
}
