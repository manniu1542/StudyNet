using System.Net;
using System.Net.Sockets;
using System.Text;

namespace StepRealy32Asyc
{

    class Program
    {
        static Socket socket = null;
        static byte[] arrReceive = new byte[1024];
        static void Main(string[] args)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            socket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8088));
            socket.Listen(100);
            Console.WriteLine("启动服务器完成");
            //socket.BeginAccept(AsyncAccept, socket);

            SocketAsyncEventArgs s = new SocketAsyncEventArgs();
            s.Completed += (socket, args) =>
            {

                if (args.SocketError == SocketError.Success)
                {
                    args.AcceptSocket.BeginReceive(arrReceive, 0, arrReceive.Length, SocketFlags.None, AsyncRecevie, args.AcceptSocket);

                    while (true)
                    {
                        string str = Console.ReadLine();
                        if (str.Length > 0)
                        {
                            byte[] bytes = Encoding.UTF8.GetBytes(str);
                            args.AcceptSocket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, (clientRuslt) =>
                            {
                                args.AcceptSocket.EndSend(clientRuslt);
                                Console.WriteLine("发送消息完成：");
                            }, null);
                        }
                    }
                }
                else
                {
                    
                }

            };
            socket.AcceptAsync(s);

            while (true)
            {

            }

            Console.Read();
            Console.WriteLine("Hello, World!");
        }

        static void AsyncAccept(IAsyncResult reslut)
        {
            Socket s = reslut.AsyncState as Socket;
            var client = s.EndAccept(reslut);


            client.BeginReceive(arrReceive, 0, arrReceive.Length, SocketFlags.None, AsyncRecevie, client);

            while (true)
            {
                string str = Console.ReadLine();
                if (str.Length > 0)
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(str);
                    client.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, (clientRuslt) =>
                    {
                        client.EndSend(clientRuslt);
                        Console.WriteLine("发送消息完成：");
                    }, null);
                }
            }
        }
        static void AsyncRecevie(IAsyncResult reslut)
        {
            Socket s = reslut.AsyncState as Socket;
            var receiveNum = s.EndSend(reslut);
            string str = Encoding.UTF8.GetString(arrReceive, 0, receiveNum);
            Console.WriteLine("收到消息：" + str);
            s.BeginReceive(arrReceive, 0, arrReceive.Length, SocketFlags.None, AsyncRecevie, s);

        }
    }
}