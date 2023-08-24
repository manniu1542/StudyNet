namespace ServerTcpFinishAsync
{
    class MainSynchronizationContext : SynchronizationContext
    {

        public static readonly MainSynchronizationContext Instance = new MainSynchronizationContext();
        private int mainThreadId;
        public void Init(int _mainThreadId)
        {
            this.mainThreadId = _mainThreadId;
        }



        // 线程同步队列,发送接收socket回调都放到该队列,由poll线程统一执行
        private Queue<Action> queue = new Queue<Action>();

        private Action a;

        public void Update()
        {
            while (true)
            {
                if (!queue.TryDequeue(out a))
                {
                    return;
                }
                a?.Invoke();
            }
        }

        public override void Post(SendOrPostCallback callback, object state)
        {
            if (Thread.CurrentThread.ManagedThreadId == this.mainThreadId)
            {
                callback(state);
                return;
            }
            try
            {
                queue.Enqueue(() => { callback(state); });
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
          
        }
    }
}
