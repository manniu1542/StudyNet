using System.Net.Sockets;
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
                byte[] arrByte = new byte[1024 * 5];
                int receiveNum = socket.Receive(arrByte);
                //解析出类型：
                byte[] data = new byte[receiveNum];
                Array.Copy(arrByte, data, receiveNum);


                HandleMsg(data);
                //为什么 启用 线程的原因是，怕处理消息时候 ，会造成 其他消息的等待， 但是 ReceiveClientSocket 已经是 一个单独线程了。
                //ThreadPool.QueueUserWorkItem(HandleMsg, str);
            }
        }
        void HandleMsg(object? obj)
        {
            if (socket == null) return;
            byte[] str = obj as byte[];

            PlayData data = new PlayData();
            data.DataByByte(str);
            Console.WriteLine($"收到{socket.RemoteEndPoint}消息：玩家的 id{data.id},名字：{data.name},年龄{data.age}，性别：{data.isMan}");

        }
        public void Send(byte[] arr)
        {
            if (socket == null) return;
            try
            {



                socket.Send(arr);
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
