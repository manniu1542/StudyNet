using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {

            //创建服务器 socket连接 
            Socket tcp = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8088);
            //绑定
            tcp.Bind(ipep);

            //监听3个 socket
            tcp.Listen(3);
            Console.WriteLine("启动服务器，等待客户端接入");
            while (true)
            {
                var socket = tcp.Accept();

                if (socket != null)
                {
                    Console.WriteLine("客户端接入成功！");
                    byte[] buffer = new byte[1024];
                    int i = socket.Receive(buffer);
                    if (i > 0 && i < buffer.Length)
                    {
                        string str = Encoding.UTF8.GetString(buffer, 0, i);
                        Console.WriteLine("收到消息：" + str);
                        socket.Shutdown(SocketShutdown.Both);
                        socket.Close();
                        break;
                    }

                }


            }
            //为什么报错呢。
            tcp.Shutdown(SocketShutdown.Both);
            tcp.Close();
            Console.WriteLine("服务器关闭！");

            Console.ReadKey();


        }
    }
}
