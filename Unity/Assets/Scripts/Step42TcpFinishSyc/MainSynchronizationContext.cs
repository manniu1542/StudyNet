using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace TcpFinishSyc
{

    public class MainSynchronizationContext : SynchronizationContext
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
                if (queue.Count <= 0) return;
                a = queue.Dequeue();
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
            queue.Enqueue(() => { callback(state); });
        }
    }
}