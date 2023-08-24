namespace ServerTcpFinishAsync
{
    class Program
    {

        static void Main(string[] args)
        {


            ServerSocket server = new ServerSocket();
            server.Start("127.0.0.1", 8088, 100);

            Task.Run(() =>
            {
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
                        PlayerDataMsg data = new PlayerDataMsg()
                        {
                            msg = new PlayerData()
                            {
                                age = 13,
                                name = gg,
                                isMan = true,
                            }
                        };

                        server.Broadcast(data);

                    }
                  

                }

            });
            Console.WriteLine("启动服务器！");
            while (true)
            {

            }


            Console.WriteLine("结束！");
            Console.ReadKey();



        }



    }
}