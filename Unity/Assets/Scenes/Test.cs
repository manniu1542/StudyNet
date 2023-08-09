using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class MainSynchronizationContext : SynchronizationContext
{
    public static MainSynchronizationContext Instance { get; } = new MainSynchronizationContext(Thread.CurrentThread.ManagedThreadId);
    private int curThreadId;
    MainSynchronizationContext(int cti)
    {
        curThreadId = cti;
        Debug.Log("当前同步上下文的线程id：" + Thread.CurrentThread.ManagedThreadId);

    }
    public override void Post(SendOrPostCallback d, object state)
    {
        base.Post(d, state);
    }
    public override void Send(SendOrPostCallback d, object state)
    {
        base.Send(d, state);
    }


}


public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    public Canvas canvas;
    public Transform tf;
    public MainSynchronizationContext msc;
    void Start()
    {

        msc = MainSynchronizationContext.Instance;
        print("1当前同步上下文的线程id：" + Thread.CurrentThread.ManagedThreadId);

        SendOrPostCallback sendOrPostCallback = state =>
        {
            print("哈哈哈 " + Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(1000);
            // 在控制台上显示结果
            print("Result: " + state);
            print("3当前线程id：" + Thread.CurrentThread.ManagedThreadId);
        };

        new Thread(() =>
        {
            int result = DoTimeConsumingOperation();
            print("2当前线程id：" + Thread.CurrentThread.ManagedThreadId);

            //Send同步 就是 sendOrPostCallback 方法内所有 同步的方法执行完毕后  才可以执行   print("同步/异步 标记位");
            //synchronizationContext.Send(sendOrPostCallback, result);
            //Post 异步方法  直接执行 后续代码 print("同步/异步 标记位");    sendOrPostCallback方法 异步执行 
            msc.Post(sendOrPostCallback, result);
            print("同步/异步 标记位");
        }).Start();


    }
    private static int DoTimeConsumingOperation()
    {
        print("等待3秒钟");
        // 模拟耗时操作（这里使用 Thread.Sleep 来模拟耗时）
        Thread.Sleep(3000);

        // 返回结果
        return 42;
    }

}
