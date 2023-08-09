using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TestTheardSever
{
    public class MainSynchronizationContext : SynchronizationContext
    {
        public static readonly MainSynchronizationContext Ins = new MainSynchronizationContext();
        private int threadId;
        public List<Action> listAct;
        public void Init(int id)
        {
            threadId = id;
            listAct = new List<Action>();
        }

        public override void Post(SendOrPostCallback d, object? state)
        {
            if (Thread.CurrentThread.ManagedThreadId == threadId)
            {
                d.Invoke(state);
            }
            else
            {
                listAct.Add(() => d.Invoke(state));
            }

        }
        public void Update()
        {
            while (listAct.Count > 0)
            {
                listAct[listAct.Count - 1].Invoke();
                listAct.RemoveAt(listAct.Count - 1);
            }
        }

    }

    internal class Program
    {


        public static Socket socketSever;
        public static List<Socket> listSockets = new List<Socket>();
        public static bool isClose = false;
        static void Main(string[] args)
        {

            MainSynchronizationContext.Ins.Init(Thread.CurrentThread.ManagedThreadId);
            ServerSocketInit();

            //Task.Run(AcceptClientSocket);
            (new Thread(AcceptClientSocket)).Start();
            Console.WriteLine($"开启服务器的连入！");

            (new Thread(ReceiveClientSocket)).Start();
            //Task.Run(ReceiveClientSocket);
            Console.WriteLine($"开启服务器 连入的客户端的监听！");
            while (true)
            {
                MainSynchronizationContext.Ins.Update();
                string str = Console.ReadLine();
                if (str == "quit")
                {
                    isClose = true;
                    for (int i = 0; i < listSockets.Count; i++)
                    {
                        listSockets[i].Shutdown(SocketShutdown.Both);
                        listSockets[i].Close();

                    }
                    listSockets.Clear();

                    Console.WriteLine($"关闭服务器！");
                    return;
                }
                else if (str.Contains("B:"))
                {
                    string gg = str.Substring(2, str.Length - 3);
                    for (int i = 0; i < listSockets.Count; i++)
                    {

                        listSockets[i].Send(Encoding.UTF8.GetBytes(gg));
                    }

                }
            }
            Console.ReadKey();
        }

        static void ServerSocketInit()
        {
            socketSever = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8088);
            try
            {
                socketSever.Bind(ipep);
            }
            catch (SocketException e)
            {
                Console.WriteLine($"绑定 ip 端口号 {ipep.Address}:{ipep.Port}错误！" + e.Message);
                return;
            }

            //监听 socket连接
            socketSever.Listen(100);


            Console.WriteLine($"初始化服务器！");
        }
        static void AcceptClientSocket()
        {
            while (!isClose)
            {
                Socket socketClient = socketSever.Accept();
                listSockets.Add(socketClient);
                Console.WriteLine($"服务器成功接入{socketClient.RemoteEndPoint}！");



            }

        }
        static void ReceiveClientSocket()
        {
            byte[] arrByte = new byte[1024 * 1024];
            int receiveNum;
            int i = 0;
            Socket clientSocket;
            while (!isClose)
            {
                for (i = 0; i < listSockets.Count; i++)
                {
                    clientSocket = listSockets[i];
                    //表示 接下来 收到消息 的字节数 ，如果大于0 证明有 消息 收到
                    if (clientSocket != null && clientSocket.Available > 0)
                    {
                        receiveNum = clientSocket.Receive(arrByte);
                        string str = Encoding.UTF8.GetString(arrByte, 0, receiveNum);

                        HandleMsg((clientSocket, str));
                        //为什么 启用 线程的原因是，怕处理消息时候 ，会造成 其他消息的等待， 但是 ReceiveClientSocket 已经是 一个单独线程了。
                        //ThreadPool.QueueUserWorkItem(HandleMsg, (clientSocket, str));
                    }



                }



            }

        }
        static void HandleMsg(object? obj)
        {

            for (int i = 0; i < 299999; i++)
            {
              
            }
            (Socket s, string str) info = ((Socket s, string str))obj;

            Console.WriteLine($"收到{info.s.RemoteEndPoint}消息：{info.str}");
            info.s.Send(Encoding.UTF8.GetBytes("服务器处理了你的消息！"));

        }


    }
}