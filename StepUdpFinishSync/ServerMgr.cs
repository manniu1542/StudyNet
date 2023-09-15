using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace StepUdpFinishSync
{


    class ServerMgr
    {
        public readonly static ServerMgr Instance = new ServerMgr();
        public const int udpLimteByteNum = 512;
        public const int unconnectTime = 10;
        byte[] receiveByte = new byte[udpLimteByteNum];
        public Dictionary<string, ClientInfo> dicClient = new Dictionary<string, ClientInfo>();
        public List<string> waitRemoveClient = new List<string>();
        public Socket serverSocket;
        bool isClose;

        private ServerMgr() { }
        public void Start()
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            serverSocket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8088));
            isClose = false;
            Task.Run(() =>
            {
                Receive();
            });

            Task.Run(() =>
            {
                CheckHeartTime();
            });

        }
        public void CheckHeartTime()
        {
            while (!isClose)
            {
                Thread.Sleep(1000);
                lock (dicClient)
                {
                    long curTimeStep = DateTime.Now.Ticks / TimeSpan.TicksPerSecond;
                    foreach (var item in dicClient)
                    {
                        if (curTimeStep - item.Value.lastTimeStep > unconnectTime)
                        {
                            waitRemoveClient.Add(item.Key);
                        }
                    }

                    if (waitRemoveClient.Count > 0)
                    {
                        for (int i = 0; i < waitRemoveClient.Count; i++)
                        {
                            Console.WriteLine("移除超时的ip" + waitRemoveClient[i]);
                            dicClient.Remove(waitRemoveClient[i]);

                        }
                        waitRemoveClient.Clear();
                    }
                }


            }


        }
        public void Close()
        {
            try
            {
                isClose = true;
                if (serverSocket != null)
                {



                    serverSocket.Shutdown(SocketShutdown.Both);
                    serverSocket.Close();
                    serverSocket = null;
                }
            }
            catch (SocketException e)
            {

                Console.WriteLine("错误：" + e.Message);
            }


        }


        public void Receive()
        {

            while (!isClose)
            {

                try
                {


                    EndPoint remPoint = new IPEndPoint(IPAddress.Any, 0);
                    int rcNum = serverSocket.ReceiveFrom(receiveByte, 0, udpLimteByteNum, SocketFlags.None, ref remPoint);
                    lock (dicClient)
                    {
                        var tmpIPEP = remPoint as IPEndPoint;
                        string id = tmpIPEP.Address.ToString() + ":" + tmpIPEP.Port;




                        if (!dicClient.ContainsKey(id))
                        {
                            var ci = new ClientInfo(id, remPoint);
                            dicClient.Add(ci.id, ci);
                            Console.WriteLine($"收到：客户端{ci.id}登入：");
                        }
                        else
                        {
                            var tmp = dicClient[id];
                            receiveByte.CopyTo(tmp.receiveByte, 0);
                            Task.Run(() =>
                            {

                                tmp.ReceiveMsg();
                            });
                        }

                    }


                }
                catch (SocketException s)
                {
                    Console.WriteLine("SocketException错误：" + s.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception错误：" + e.Message);

                }
            }


        }

        public void SendTo(string ClientId, BaseMsgData baseMsg)
        {
            try
            {
                if (!isClose)
                {
                    dicClient.TryGetValue(ClientId, out var ci);
                    var arr = baseMsg.ToByte();
                    serverSocket.SendTo(arr, 0, arr.Length, SocketFlags.None, ci.endPoint);
                }
            }
            catch (SocketException s)
            {
                Console.WriteLine("SocketException错误：" + s.Message);
            }
            catch (Exception e)
            {

                Console.WriteLine("Exception错误：" + e.Message);
            }


        }

        public void BraodCast(BaseMsgData baseMsg)
        {


            foreach (var item in dicClient)
            {
                SendTo(item.Key, baseMsg);
            }

        }
    }
}
