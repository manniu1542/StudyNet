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



        // �߳�ͬ������,���ͽ���socket�ص����ŵ��ö���,��poll�߳�ͳһִ��
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