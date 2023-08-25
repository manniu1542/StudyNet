using System.Net.Sockets;
using System.Net;
using System.Text;

namespace StepRealy37UDP
{
    internal class Program
    {
        static void Main(string[] args)
        {

            int udpLimteByteNum = 512;
            Socket socketUdp = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socketUdp.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8089));

            Console.WriteLine($"启动简易udp服务端");

            byte[] receiveByte = new byte[udpLimteByteNum];
            EndPoint remPoint = new IPEndPoint(IPAddress.Any, 0);
            int rcNum = socketUdp.ReceiveFrom(receiveByte, 0, udpLimteByteNum, SocketFlags.None, ref remPoint);
            Console.WriteLine($"收到客户端{remPoint}的消息：" + Encoding.UTF8.GetString(receiveByte, 0, rcNum));


            byte[] sendMsg = Encoding.UTF8.GetBytes("客户端已连入！");
            if (sendMsg.Length <= udpLimteByteNum)
            {

                socketUdp.SendTo(sendMsg, remPoint);
                Console.WriteLine($"发送{socketUdp.LocalEndPoint}消息,给{remPoint}");
            }
            Console.ReadLine();
        }
    }
}