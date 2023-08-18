
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace TcpFinishSyc
{
    class Program
    {
        //两个线程使用这个字典 foreach 循环报错
        public static void Test()
        {
            MainSynchronizationContext.Instance.Init(Thread.CurrentThread.ManagedThreadId);
            Dictionary<int, int> dicData = new Dictionary<int, int>();

            Task.Run(() =>
            {
                while (true)
                {
                    if (dicData.Count > 0)
                    {

                        MainSynchronizationContext.Instance.Post((a) =>
                        {

                            foreach (var item in dicData)
                            {
                                Console.WriteLine(item.Key);
                            }

                        }, null);
                    }
                    //var list = dicData.ToList();
                    //for (int i = 0; i < list.Count; i++)
                    //{
                    //    Console.WriteLine(list[i].Key);
                    //}


                }


            });
            Task.Run(() =>
            {
                while (true)
                {
                    string str = Console.ReadLine();
                    if (str == "11")
                    {
                        MainSynchronizationContext.Instance.Post((a) =>
                        {
                            dicData.Add(11, 22);
                        }, null);
                        //dicData.Add(11, 22);

                    }
                }


            });
            while (true)
            {
                MainSynchronizationContext.Instance.Update();
            }
        }

        static void Main(string[] args)
        {

            //启动同步
            MainSynchronizationContext.Instance.Init(Thread.CurrentThread.ManagedThreadId);

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
                MainSynchronizationContext.Instance.Update();
                if (!server.IsConnect)
                    break;
            }


            Console.WriteLine("结束！");
            Console.ReadKey();



        }



    }
}