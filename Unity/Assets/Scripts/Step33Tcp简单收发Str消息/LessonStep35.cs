using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class NetMgr
{
    public static readonly NetMgr Ins = new NetMgr();
    public Socket socket;
    public Queue<string> queueSend = new Queue<string>();
    public Queue<string> queueReceive = new Queue<string>();
    //连入 的方法
    public void Connect(string ip, int port)
    {
        try
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            socket.Connect(new IPEndPoint(IPAddress.Parse(ip), port));

            ThreadPool.QueueUserWorkItem(Receive);
            ThreadPool.QueueUserWorkItem(SendServer);


        }
        catch (SocketException e)
        {

            Debug.LogException(e);
        }

    }


    //监听 收入的方法

    private void Receive(object o)
    {

        while (true)
        {

            try
            {
                if (socket.Available > 0)
                {
                    byte[] buffer = new byte[1024 * 4];
                    var i = socket.Receive(buffer);
                    queueReceive.Enqueue(Encoding.UTF8.GetString(buffer, 0, i));

                }


            }
            catch (SocketException e)
            {

                Debug.LogError(e.Message);
            }


        }


    }
    public void ReceivePrint()
    {
        if (queueReceive.Count > 0)
        {

            Debug.Log("收到消息：" + queueReceive.Dequeue());

        }


    }
    //发送 消息的方法
    public void Send(string msg)
    {
        if (socket == null) return;
        queueSend.Enqueue(msg);

    }
    private void SendServer(object o)
    {

        while (true)
        {

            try
            {
                if (queueSend.Count > 0)
                {
                    var str = queueSend.Dequeue();
                    socket.Send(Encoding.UTF8.GetBytes(str));
                    Debug.Log("发送消息：" + str);
                }

            }
            catch (SocketException e)
            {

                Debug.LogError(e.Message);
            }


        }


    }

    //发送 消息的方法

    public void Close()
    {
        if (socket == null) return;
        try
        {

            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            Debug.Log("关闭sokect");
        }
        catch (SocketException e)
        {
            Debug.LogError(e.Message);

        }

    }

}


public class LessonStep35 : MonoBehaviour
{
    // Start is called before the first frame update
    public Button btnLogin;
    public Button btnSend;
    public InputField ifMsg;
    void Start()
    {

        btnLogin.onClick.AddListener(() =>
        {

            NetMgr.Ins.Connect("127.0.0.1", 8088);
        });



        btnSend.onClick.AddListener(() =>
        {

            if (ifMsg.text.Length > 0)
            {

                NetMgr.Ins.Send(ifMsg.text);


            }

        });


    }

    // Update is called once per frame
    void Update()
    {
        NetMgr.Ins.ReceivePrint();

    }
}
