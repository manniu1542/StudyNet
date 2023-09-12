using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace StepUdpFinishSync
{
    class ClientServer
    {
        public int id;
        public long timeStep;
        public EndPoint ip;
        public ClientServer(EndPoint ip)
        {
            id = ++ServerMgr.id;


            timeStep = DateTime.Now.Ticks / TimeSpan.TicksPerSecond;
            this.ip = ip;
        }

        void Send()
        {

        }

    }

    class ServerMgr
    {
        public static int id = 0;
        public const int udpLimteByteNum = 512;
        public Dictionary<int, ClientServer> dicClient = new Dictionary<int, ClientServer>();
        public Socket serverSocket;
        byte[] receiveByte = new byte[udpLimteByteNum];
        public void Init()
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            serverSocket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8088));
            Task.Run(() =>
            {
                Receive();
            });


        }
        public void Receive()
        {
            while (true)
            {


                EndPoint remPoint = new IPEndPoint(IPAddress.Any, 0);
                int rcNum = serverSocket.ReceiveFrom(receiveByte, 0, udpLimteByteNum, SocketFlags.None, ref remPoint);

                int id = BitConverter.ToInt32(receiveByte, 4);
                switch (id)
                {
                    case HandlerCode.Handler_QuitRoom:

                        break;
                    case HandlerCode.Handler_EnterRoom:
                        lock (dicClient)
                        {
                            var tt = new ClientServer(remPoint);
                            dicClient.Add(tt.id, tt);
                        }


                        PlayerDataMsg tmp = new PlayerDataMsg();
                        tmp.DataByByte(receiveByte);
                        Console.WriteLine($"收到客户端{remPoint}的消息：" +
                            $"名字:{tmp.msg.name} 年龄: {tmp.msg.age} 性别:{tmp.msg.isMan} ");




                        byte[] sendMsg = Encoding.UTF8.GetBytes("客户端登录！");
                        if (sendMsg.Length <= udpLimteByteNum)
                        {

                            serverSocket.SendTo(sendMsg, remPoint);
                            Console.WriteLine($"发送{serverSocket.LocalEndPoint}消息,给{remPoint}");
                        }

                        break;
                    case HandlerCode.Handler_QuitGame:

                        Console.WriteLine($"收到客户端{remPoint}的消息：" +
                    $"退出游戏!");
                        break;
                    case HandlerCode.Handler_Heart:

                        break;
                    default:

                        Console.WriteLine($"没有该类型的msg：{id}");
                        return;
                }

            }
        }



        public void Braod()
        {

            var arr = Encoding.UTF8.GetBytes("hello , i'm back");

            foreach (var item in dicClient)
            {
                serverSocket.SendTo(arr, 0, arr.Length, SocketFlags.None, item.Value.ip);
            }

        }
    }
}
