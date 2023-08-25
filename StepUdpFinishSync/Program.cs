using System.Diagnostics;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace StepUdpFinishSync
{
    //写udp 服务端 （1.区分消息类型 ，2.能够接收多个客户端消息，3.能够发送广播 给 接收过的 客户端，4.主动记录上一次收到
    // 客户端消息的时间，如果上时间没有收到该客户端消息，要移除次客户端消息）

    internal class Program
    {
        static void Main(string[] args)
        {
            int udpLimteByteNum = 512;
            Socket socketUdp = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Udp);
            socketUdp.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8089));

            Console.WriteLine($"启动简易udp服务端");

            byte[] receiveByte = new byte[udpLimteByteNum];
            EndPoint remPoint = new IPEndPoint(IPAddress.Any, 0);
            int rcNum = socketUdp.ReceiveFrom(receiveByte, 0, udpLimteByteNum, SocketFlags.None, ref remPoint);
            Console.WriteLine($"收到客户端{remPoint}的消息：" + Encoding.UTF8.GetString(receiveByte, 0, rcNum));


            byte[] sendMsg = Encoding.UTF8.GetBytes("客户端登录！");
            if (sendMsg.Length <= udpLimteByteNum)
            {

                socketUdp.SendTo(sendMsg, remPoint);
                Console.WriteLine($"发送{socketUdp.LocalEndPoint}消息,给{remPoint}");
            }
            Console.ReadLine();
        }
    }
}