using System.Diagnostics;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace StepUdpFinishSync
{
    //写udp 服务端 （1.区分消息类型  完成，2.能够接收多个客户端消息，完成 3.能够发送广播 给 接收过的 客户端，完成 4.主动记录上一次收到  完成 
    // 客户端消息的时间，如果上时间没有收到该客户端消息，要移除次客户端消息）


    internal class Program
    {
        static ServerMgr serverMgr = ServerMgr.Instance;
        static void Main(string[] args)
        {

            serverMgr.Start();
            Console.WriteLine($"启动udp服务端");


            while (true)
            {
                var str = Console.ReadLine();
                if (str.Contains("B:") && str.Length > 2)
                {
                    Console.WriteLine("发送广播消息！");
                    PlayerDataMsg playerDataMsg = new PlayerDataMsg()
                    {
                        msg = new PlayerData()
                        {
                            age = 12,
                            name = str.Replace("B:", ""),
                        }
                    };


                    serverMgr.BraodCast(playerDataMsg);

                }
                else if (str == "close")
                {
                    Console.WriteLine("关闭服务器！");
                    serverMgr.Close();
                    break;
                }

            }






            Console.ReadLine();
        }
    }
}