using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace StepUdpFinishSync
{
 
    class ClientInfo
    {
        public string id;
        public long lastTimeStep;
        public EndPoint endPoint;

        public byte[] receiveByte = new byte[ServerMgr.udpLimteByteNum];
  
        public ClientInfo(string id, EndPoint endPoint)
        {

     
            lastTimeStep = DateTime.Now.Ticks / TimeSpan.TicksPerSecond;
            this.endPoint = endPoint;
            this.id = id;
        }
  

        public void ReceiveMsg()
        {
           


            int id = BitConverter.ToInt32(receiveByte, 4);
            switch (id)
            {
                case HandlerCode.Handler_QuitRoom:
                    Console.WriteLine($"收到客户端{endPoint} 《Handler_QuitRoom》  的消息：" +
              $"退出房间!");
                    break;
                case HandlerCode.Handler_EnterRoom:


                    PlayerDataMsg tmp = new PlayerDataMsg();
                    tmp.DataByByte(receiveByte);
                    Console.WriteLine($"收到客户端{endPoint}的消息：" +
                        $"名字:{tmp.msg.name} 年龄: {tmp.msg.age} 性别:{tmp.msg.isMan} ");





                    break;
                case HandlerCode.Handler_QuitGame:

                    Console.WriteLine($"收到客户端{endPoint} 《Handler_QuitGame》  的消息：" +
                $"退出游戏!");
                    break;
                case HandlerCode.Handler_Heart:
                    lastTimeStep = DateTime.Now.Ticks / TimeSpan.TicksPerSecond;

                    break;
                default:

                    Console.WriteLine($"没有该类型的msg：{id}");
                    return;
            }





        }


    }
}
