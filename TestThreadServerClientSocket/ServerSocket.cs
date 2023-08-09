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

        Socket socket;
        Dictionary<int, ClientSocket> dicClientSocket;
        bool isLanuch;
        public ServerSocket()
        {
            isLanuch = false;
            dicClientSocket = new Dictionary<int, ClientSocket>();
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
                        Console.WriteLine($"连入：{sotTmp.RemoteEndPoint}");
                        var tmp = new ClientSocket(sotTmp);
                        dicClientSocket.Add(tmp.id, tmp);
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





                }

            }
            catch (SocketException se)
            {
                Console.WriteLine($"错误{se.ErrorCode}：{se.Message}");
            }
        }
        public void Broadcast(string msg)
        {

            try
            {

                if (isLanuch)
                {
                    var arr = dicClientSocket.Values.ToList();
                    for (int i = 0; i < arr.Count; i++)
                    {
                        arr[i]?.Send(msg);
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
