using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {

            hhh();

        }
        static async void hhh()
        {
            //创建服务器 socket连接 
            Socket tcp = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8088);
                //绑定
                tcp.Bind(ipep);
            }
            catch (Exception e)
            {
                Console.WriteLine("绑定失败,地址不可用，或者端口号占用" + e);
                Console.ReadKey();
                return;
            }


            //监听3个 socket

            tcp.Listen(3);
            Console.WriteLine("启动服务器，等待客户端接入");

            var socketClient = tcp.Accept();
            Console.WriteLine("客户端接入成功！");
            socketClient.Send(Encoding.UTF8.GetBytes("登录服务器成功！"));

            byte[] buffer = new byte[1024];

            int i = socketClient.Receive(buffer);
            if (i > 0 && i < buffer.Length)
            {
                string str = Encoding.UTF8.GetString(buffer, 0, i);
                Console.WriteLine("收到消息：" + str);

            }








            socketClient.Shutdown(SocketShutdown.Both);
            socketClient.Close();



            //// 添加延迟等待，让连接稳定建立
            //ThreadPool.SetMinThreads(3, 3);
            //ThreadPool.QueueUserWorkItem(async (a) =>
            //{
            //    await Task.Delay(1000); // 延迟 1 秒钟
            //                            //为什么报错呢。
            //    /


            //});
            Console.WriteLine("服务器关闭！");
            //tcp.Shutdown(SocketShutdown.Receive);
            //tcp.Close();


            Console.ReadKey();


        }

    }
}
