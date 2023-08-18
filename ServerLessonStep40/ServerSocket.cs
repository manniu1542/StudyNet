using ServerLessonStep40;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TestThreadServerClientSocket
{
    internal class ServerSocket
    {
        public static ServerSocket Instance;
        Socket socket;
        Dictionary<int, ClientSocket> dicClientSocket;
        List<ClientSocket> waitRemoveClientSocket;
        bool isLanuch;
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

            try
            {
                while (isLanuch)
                {
                    var sotTmp = socket.Accept();
                    if (sotTmp != null)
                    {
                        MainSynchronizationContext.Instance.Post((a) =>
                        {
                            Console.WriteLine($"当前的线程id:{Thread.CurrentThread.ManagedThreadId},连入：{sotTmp.RemoteEndPoint}");
                            var tmp = new ClientSocket(sotTmp);
                            dicClientSocket.Add(tmp.id, tmp);

                        }, null);

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


                    var arr = dicClientSocket.Values.ToList();


                    for (int i = 0; i < arr.Count; i++)
                    {
                        arr[i]?.Receive();
                    }

                    if(waitRemoveClientSocket.Count>0)
                    {
                        for (int i = 0; i < waitRemoveClientSocket.Count; i++)
                        {
                            waitRemoveClientSocket[i].Close();
                            dicClientSocket.Remove(waitRemoveClientSocket[i].id);
                        }
                        waitRemoveClientSocket.Clear();
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
                    var arr = dicClientSocket.Values.ToList();
                    for (int i = 0; i < arr.Count; i++)
                    {
                        arr[i]?.Send(md);
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
