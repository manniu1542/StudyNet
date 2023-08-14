using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using TestSocketBag;

namespace TestThreadServerClientSocket
{



    internal class ClientSocket
    {
        public static int ClientSocketID = 0;
        public int id;
        Socket socket;

        public const int preCount = 20;
        byte[] arrByte = new byte[preCount];

        ReceiveByteChacheAnlyze rbca;

        public ClientSocket(Socket s)
        {
            id = ++ClientSocketID;
            socket = s;
            rbca = new ReceiveByteChacheAnlyze();
        }


        public void Receive()
        {
            if (socket == null) return;

            if (socket.Available > 0)
            {


                int totalReceivedBytes = 0;
                int lenAva = socket.Available;
                while (totalReceivedBytes < lenAva)
                {
                    int remainingBytes = Math.Min(preCount, lenAva - totalReceivedBytes);
                    int bytesRead = socket.Receive(arrByte, 0, remainingBytes, SocketFlags.None);

                    if (bytesRead <= 0)
                    {
                        // 处理接收异常或连接关闭的情况
                        break;
                    }




                    totalReceivedBytes += bytesRead;

                    // 在这里可以处理接收到的数据，例如将数据拼接到一个大缓冲区中或进行其他处理
                    {

                        //解析 读取的长度
                        var tmp = new byte[arrByte.Length];
                        arrByte.CopyTo(tmp, 0);
                        rbca.Analyzi(tmp, bytesRead);

                        //取出完整的队列
                        int len = rbca.queueFinish.Count;
                        byte[] arrTmp;
                        for (int i = 0; i < len; i++)
                        {
                            arrTmp = rbca.queueFinish.Dequeue();

                            //解析出类型：
                            int handlerType = BitConverter.ToInt32(arrTmp, 4);

                            object obj = null;
                            switch (handlerType)
                            {
                                case HandlerCode.Handler_EnterRoom:
                                    PlayData data = new PlayData();
                                    data.DataByByte(arrTmp, 8);

                                    obj = data;
                                    break;

                                default:
                                    break;
                            }



                            HandleMsg(obj);


                        }

                        //为什么 启用 线程的原因是，怕处理消息时候 ，会造成 其他消息的等待， 但是 ReceiveClientSocket 已经是 一个单独线程了。
                        //ThreadPool.QueueUserWorkItem(HandleMsg, obj);


                    }




                }


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
