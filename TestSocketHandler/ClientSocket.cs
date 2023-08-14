using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;

namespace TestThreadServerClientSocket
{

 

    internal class ClientSocket
    {
        public static int ClientSocketID = 0;
        public int id;
        Socket socket;



        public ClientSocket(Socket s)
        {
            id = ++ClientSocketID;
            socket = s;
        }

        public void Receive()
        {
            if (socket == null) return;
            if (socket.Available > 0)
            {
                int preCount = 20;
                byte[] arrByte = new byte[preCount];


            
                int receiveNum = socket.Receive(arrByte);


                //解析出类型：
                int handlerType = BitConverter.ToInt32(arrByte, 0);




                object obj = null;
                switch (handlerType)
                {
                    case HandlerCode.Handler_EnterRoom:
                        PlayData data = new PlayData();
                        data.DataByByte(arrByte, 4);

                        obj = data;
                        break;

                    default:
                        break;
                }



                HandleMsg(obj);
                //为什么 启用 线程的原因是，怕处理消息时候 ，会造成 其他消息的等待， 但是 ReceiveClientSocket 已经是 一个单独线程了。
                //ThreadPool.QueueUserWorkItem(HandleMsg, obj);
            }
        }
        void HandleMsg(object? obj)
        {
            if (socket == null) return;
            if (obj is PlayData)
            {
                PlayData data = obj as PlayData;
                Console.WriteLine($"收到{socket.RemoteEndPoint}消息：玩家的 id{data.id},名字：{data.name},年龄{data.age}，性别：{data.isMan}");
            }


        }
        public void Send(BaseMsgData md)
        {
            if (socket == null) return;
            try
            {



                socket.Send(md.ToByte());
            }
            catch (Exception)
            {

                throw;
            }


        }
        public void Close()
        {
            if (socket == null) return;
            try
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
                socket = null;
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
