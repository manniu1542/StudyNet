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
    public const int Handler_EnterRoom = 1000;
    public const int Handler_QuitRoom = 1002;
    public const int Handler_QuitGame = 1003;

    public static int TypeToHandlerId(Type type)
    {
        if (type == typeof(PlayData))
        {
            return Handler_EnterRoom;
        }
        else if (type == typeof(PlayData))
        {

            return Handler_QuitRoom;
        }
        else if (type == typeof(LessonStep40.QuitGameData))
        {

            return Handler_QuitGame;
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




}



public class NetMgr38
{
    public static readonly NetMgr38 Ins = new NetMgr38();
    public Socket socket;
    public Queue<BaseMsgData> queueSend = new Queue<BaseMsgData>();
    public Queue<BaseMsgData> queueReceive = new Queue<BaseMsgData>();
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
                    var receiveNum = socket.Receive(buffer);

                    int handlerType = BitConverter.ToInt32(buffer, 0);

                    switch (handlerType)
                    {
                        case HandlerCode.Handler_EnterRoom:
                            PlayData data = new PlayData();
                            data.DataByByte(buffer, 4);
                            queueReceive.Enqueue(data);

                            break;

                        default:
                            break;
                    }



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
            var obj = queueReceive.Dequeue();



            if (obj is PlayData)
            {
                var data = obj as PlayData;
                Debug.Log($"收到{socket.RemoteEndPoint}消息：玩家的 id{data.id},名字：{data.name},年龄{data.age}，性别：{data.isMan}");
            }


        }


    }
    //发送 消息的方法
    public void Send(BaseMsgData md)
    {
        if (socket == null) return;
        queueSend.Enqueue(md);

    }
    private void SendServer(object o)
    {

        while (true)
        {

            try
            {
                if (queueSend.Count > 0)
                {
                    Debug.Log("上传消息：");
                    socket.Send(queueSend.Dequeue().ToByte());

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

                NetMgr38.Ins.Send(data);
                Debug.Log("点击发送了 消息！");

            }

        });





    }

    // Update is called once per frame
    void Update()
    {



        NetMgr38.Ins.ReceiveMsg();
    }
}
