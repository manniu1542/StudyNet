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
                string str = Encoding.UTF8.GetString(arrByte, 0, receiveNum);

                HandleMsg(str);
                //为什么 启用 线程的原因是，怕处理消息时候 ，会造成 其他消息的等待， 但是 ReceiveClientSocket 已经是 一个单独线程了。
                //ThreadPool.QueueUserWorkItem(HandleMsg, str);
            }
        }
        void HandleMsg(object? obj)
        {
            if (socket == null) return;
            string str = (string)obj;


            Console.WriteLine($"收到{socket.RemoteEndPoint}消息：{str}");

        }
        public void Send(string str)
        {
            if (socket == null) return;
            try
            {

                 socket.Send(Encoding.UTF8.GetBytes(str));
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
