namespace TestThreadServerClientSocket
{
    class Program
    {


        static void Main(string[] args)
        {
            ServerSocket server = new ServerSocket();
            server.Start("127.0.0.1", 8088, 100);


            Console.WriteLine("启动服务器！");
            while (true)
            {
                string str = Console.ReadLine();
                if (str == "quit")
                {
                    server.Close();

                    break;
                }
                else if (str.Contains("B:") && str.Length > 2)
                {
                    string gg = str.Substring(2);
                    PlayData data = new PlayData();
                    data.age = 13;
                    data.name = gg;
                    data.isMan = true;
                    data.id = 44;

                    server.Broadcast(data);

                }

            }
            Console.WriteLine("结束！");
            Console.ReadKey();



        }
    }
}