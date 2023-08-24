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
        public void Receive(IAsyncResult ar)
        {
            try
            {
                var skt = ar.AsyncState as Socket;

                if (skt == null) return;
                if (!skt.Connected)
                {
                    RemoveSocket();
                    return;
                }

                int bytesRead = skt.EndReceive(ar);

                if (bytesRead <= 0)
                {
                    // 处理接收异常或连接关闭的情况
                    return;
                }
                else // 在这里可以处理接收到的数据，例如将数据拼接到一个大缓冲区中或进行其他处理
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

                    //BeginReceive 在第二次 调用 时。 arrByte赋值 给 arrChache的数据 还没完全 把所有 byte 生成 对应格式的 
                    //baseMsgData的数据。 可能会有 arrByte的新接收数据 更新 arrChache还未使用的数据。  不对 rbca.Analyzi 已经把所有 byte数据转成所需的baseMsgData的数据了。
                    skt.BeginReceive(arrByte, 0, arrByte.Length, SocketFlags.None, Receive, skt);

                }
            }
            catch (SocketException s)
            {
                Console.WriteLine("错误日志" + s.Message);
                RemoveSocket();

            }

        }
        public bool IsRemove()
        {
            if (socket == null) return true;
            if (!socket.Connected)
            {
                RemoveSocket();
                return true;
            }
            return false;
        }
        public void StartReceive()
        {

            if (IsRemove())
            {
                return;
            }

            //默认 客户端单次发送的数据 一定不超过 arrByte.Length 的总长      //接入 begin end的异常处理在哪里处理 。   try catch  捕捉 
            socket.BeginReceive(arrByte, 0, arrByte.Length, SocketFlags.None, Receive, socket);

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
            if (IsRemove())
            {
                return;
            }

            var bytes = md.ToByte();

            //socket 可以用 null方法,   就是捕获不到 发送失败的情况了 
            socket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, ar =>
            {
                try
                {

                    socket?.EndSend(ar);
                }
                catch (SocketException e)
                {
                    Console.WriteLine($"发送失败，错误码：" + e.ErrorCode + e.Message);
                }
            }, null);

        }
        public void RemoveSocket()
        {

            ServerSocket.Instance.AddRemoveClientSocket(this);

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
