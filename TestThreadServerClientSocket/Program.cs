namespace TestThreadServerClientSocket
{
    class Program
    {


        static void Main(string[] args)
        {
            ServerSocket server = new ServerSocket();
            server.Start("127.0.0.1", 8088, 100);

            Task.Run(server.Accept);
            Task.Run(server.Receive);
            Console.WriteLine("启动服务器！");
            while (true)
            {
                string str = Console.ReadLine();
                if (str == "quit")
                {
                    server.Close();

                    break;
                }
                else if (str.Contains("B:"))
                {
                    string gg = str.Substring(2);
                    server.SendAll(gg);

                }

            }
            Console.WriteLine("结束！");
            Console.ReadKey();



        }
    }
}