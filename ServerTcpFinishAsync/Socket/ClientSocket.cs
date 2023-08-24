using System.Net.Sockets;

namespace ServerTcpFinishAsync
{



    internal class ClientSocket
    {
        public const int HeartMsgOutTime = 10;//心跳消息超时 10
        public static int ClientSocketID = 0;
        public int id;
        Socket socket;

        public const int preCount = 1024;
        byte[] arrByte = new byte[preCount];

        ReceiveByteChacheAnlyze rbca;

        public long lastHeartMsgTime = -1;
        public ClientSocket(Socket s)
        {
            id = ++ClientSocketID;
            socket = s;
            rbca = new ReceiveByteChacheAnlyze();
            lastHeartMsgTime = DateTime.Now.Ticks / TimeSpan.TicksPerSecond;

        }
        public void CheckHeartMsg(long curTime)
        {
            if (socket == null) return;

            if (lastHeartMsgTime != -1 && curTime - lastHeartMsgTime > ClientSocket.HeartMsgOutTime)
            {

                this.RemoveSocket();
            }

        }

        public void Receive()
        {
            if (socket == null) return;
            if (!socket.Connected)
            {
                RemoveSocket();
                return;
            }
            try
            {
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

                            rbca.Analyzi(ref arrByte, bytesRead);

                            //取出完整的队列
                            int len = rbca.queueFinish.Count;

                            for (int i = 0; i < len; i++)
                            {
                                var obj = rbca.queueFinish.Dequeue();
                                //HandleMsg(obj);

                                //为什么 启用 线程的原因是，怕处理消息时候 ，会造成 其他消息的等待， 但是 ReceiveClientSocket 已经是 一个单独线程了。
                                ThreadPool.QueueUserWorkItem(HandleMsg, obj);
                            }




                        }




                    }


                }
            }
            catch (Exception)
            {
                RemoveSocket();

            }

        }
        void HandleMsg(object? obj)
        {
            if (socket == null) return;


            if (obj is QuitRoomMsg)
            {
                QuitRoomMsg data = obj as QuitRoomMsg;
                Console.WriteLine($"收到{socket.RemoteEndPoint}消息QuitRoomMsg：玩家的 名字：{data.msg.name},年龄{data.msg.age}");
            }
            else if (obj is QuitGameMsg)
            {
                Console.WriteLine($"收到关闭 客户端 的服务器消息！");
                this.RemoveSocket();
            }
            else if (obj is HeartMsg)
            {
                lastHeartMsgTime = DateTime.Now.Ticks / TimeSpan.TicksPerSecond;
                //Console.WriteLine($"收到 客户端{socket.RemoteEndPoint} 心跳消息！");

            }
            else if (obj is PlayerDataMsg)
            {
                PlayerDataMsg data2 = obj as PlayerDataMsg;
                Console.WriteLine($"收到{socket.RemoteEndPoint}消息PlayerDataMsg：玩家的 名字：{data2.msg.name},年龄{data2.msg.age}");


            }


        }
        public void Send(BaseMsgData md)
        {
            if (socket == null) return;
            if (!socket.Connected)
            {
                RemoveSocket();
            }
            try
            {



                socket.Send(md.ToByte());
            }
            catch (Exception)
            {

                RemoveSocket();
            }


        }
        public void RemoveSocket()
        {

            ServerSocket.Instance.AddRemoveClientSocket(this);
            //MainSynchronizationContext.Instance.Post((a) =>
            //{
            //    Debug.WriteLine("当前的 线程id：" + Thread.CurrentThread.ManagedThreadId);
            //    ServerSocket.Instance.AddRemoveClientSocket(this);
            //}, null);
        }
        public void Close()
        {
            if (socket == null) return;
            try
            {
                Console.WriteLine("关闭客户端的socket：" + socket.RemoteEndPoint);
                socket.Shutdown(SocketShutdown.Both);
                socket.Disconnect(false);
                socket.Close();
                socket = null;

            }
            catch (SocketException e)
            {

                Console.WriteLine(e.Message);
            }

        }
    }
}
