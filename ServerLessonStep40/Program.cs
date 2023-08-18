using ServerLessonStep40;

namespace TestThreadServerClientSocket
{
    class Program
    {


        static void Main(string[] args)
        {
            //在vs的应用程序 中跑主线程 MainSynchronizationContext 有问题 
            //ThreadPool.QueueUserWorkItem(a =>
            //{




            //});


            MainSynchronizationContext.Instance.Init(Thread.CurrentThread.ManagedThreadId);

            ServerSocket server = new ServerSocket();
            server.Start("127.0.0.1", 8088, 100);


            Console.WriteLine("启动服务器！");
            while (true)
            {
                MainSynchronizationContext.Instance.Update();
                //string str = Console.ReadLine();
                //if (str == "quit")
                //{
                //    server.Close();

                //    break;
                //}
                //else if (str.Contains("B:") && str.Length > 2)
                //{
                //    string gg = str.Substring(2);
                //    QuitRoomMsg data = new QuitRoomMsg();
                //    data.msg.age = 13;
                //    data.msg.name = gg;


                //    server.Broadcast(data);

                //}

            }
            Console.WriteLine("结束！");
            Console.ReadKey();



        }



    }
}