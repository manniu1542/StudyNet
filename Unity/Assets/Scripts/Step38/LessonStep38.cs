using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public static class HandlerCode
{
    public static int Handler_EnterRoom = 1000;
    public static int Handler_EnterRoom2 = 1002;

    public static (BaseData, string) GetHandlerStr(byte[] msg)
    {
        int value = BitConverter.ToInt32(msg, 0);
        if (Handler_EnterRoom == value)
        {
            return (new PlayData().DataByByte(msg), "进入房间");
        }
        else if (Handler_EnterRoom2 == value)
        {

            return (new PlayData().DataByByte(msg), "进入房间2");
        }
        else
        {
            return (null, "错误：消息不存在");
        }
    }
    public static int TypeToHandlerId(Type type)
    {
        if (type == typeof(PlayData))
        {
            return Handler_EnterRoom;
        }
        else if (type == typeof(PlayData))
        {

            return Handler_EnterRoom2;
        }
        else
        {
            return -1;
        }
    }

}

public class PlayData : BaseMsgData
{
    public string name;
    public int age;
    public int id;
    public bool isMan;
}
public class BaseMsgData : BaseData
{



    public override byte[] ToByte()
    {
        byte[] tmp = base.ToByte();
        int handlerId = HandlerCode.TypeToHandlerId(this.GetType());

        byte[] arrMsg = new byte[tmp.Length + 4];
        BitConverter.GetBytes(handlerId).CopyTo(arrMsg, 0);

        tmp.CopyTo(arrMsg, 4);

        return arrMsg;
    }

    public override BaseData DataByByte(byte[] arrByte)
    {
        //拿出前四个

        if (arrByte == null || arrByte.Length <= 4)
        {
            return null;
        }


        int handlerID = BitConverter.ToInt32(arrByte, 0);

        byte[] arrData = new byte[arrByte.Length - 4];
        System.Array.Copy(arrByte, 4, arrData, 0, arrData.Length);


        if (handlerID != -1)
        {
            return base.DataByByte(arrData);
        }
        else
        {
            Debug.Log("这个 消息码没有定义：" + handlerID);
            return null;
        }



    }




}



public class NetMgr38
{
    public static readonly NetMgr38 Ins = new NetMgr38();
    public Socket socket;
    public Queue<byte[]> queueSend = new Queue<byte[]>();
    public Queue<byte[]> queueReceive = new Queue<byte[]>();
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
                    queueReceive.Enqueue(buffer);

                }


            }
            catch (SocketException e)
            {

                Debug.LogError(e.Message);
            }


        }


    }
    public void ReceiveMsg()
    {
        if (queueReceive.Count > 0)
        {
            byte[] arr = queueReceive.Dequeue();
            var info = HandlerCode.GetHandlerStr(arr);
            Debug.Log("收到消息：" + info.Item2);

            if (info.Item1 != null)
            {
                int value = HandlerCode.TypeToHandlerId(info.Item1.GetType());
                if (value == HandlerCode.Handler_EnterRoom)
                {
                    PlayData data = info.Item1 as PlayData;
                    Debug.Log($"收到消息：玩家的 id{data.id},名字：{data.name},年龄{data.age}，性别：{data.isMan}");
                }


            }



        }


    }
    //发送 消息的方法
    public void Send(byte[] msg)
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

                    socket.Send(queueSend.Dequeue());
                    Debug.Log("发送消息：");
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



public class LessonStep38 : MonoBehaviour
{
    // Start is called before the first frame update
    public Button btnLogin;
    public Button btnSend;
    public InputField ifMsg;

    void Start()
    {




        btnLogin.onClick.AddListener(() =>
        {

            NetMgr38.Ins.Connect("127.0.0.1", 8088);
        });



        btnSend.onClick.AddListener(() =>
        {

            if (ifMsg.text.Length > 0)
            {

                PlayData data = new PlayData();
                data.isMan = true;
                data.age = 12;
                data.name = ifMsg.text;//"张望历代";
                data.id = 33;
                byte[] arr = data.ToByte();
                NetMgr38.Ins.Send(arr);


            }

        });





    }

    // Update is called once per frame
    void Update()
    {



        NetMgr38.Ins.ReceiveMsg();
    }
}
