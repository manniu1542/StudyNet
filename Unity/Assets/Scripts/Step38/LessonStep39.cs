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
using System.Threading.Tasks;

public class NetMgr39
{
    public static readonly NetMgr39 Ins = new NetMgr39();
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
                    var data = queueSend.Dequeue();
                    int len = data.GetLen();

                    byte[] bytes = new byte[4 + 4 + len];
                    //这条信息的总长度 
                    BitConverter.GetBytes(bytes.Length).CopyTo(bytes, 0);

                    data.ToByte().CopyTo(bytes, 4);


                    socket.Send(bytes);

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


public class QuitRoomData : BaseData
{
    public int tt;
    public string dd;
}

public class QuitRoomMsg : BaseMsgData2
{
    public QuitRoomData pd;




}

public class BaseMsgData2 : BaseData
{
    public int id;

    //public override int GetLen()
    //{//长度 + id
    //    return 4 + 4 + base.GetLen();
    //}

    //public override byte[] ToByte()
    //{
    //    byte[] tmp = base.ToByte();
    //    int handlerId = HandlerCode.TypeToHandlerId(this.GetType());

    //    byte[] arrMsg = new byte[tmp.Length + 4];
    //    BitConverter.GetBytes(handlerId).CopyTo(arrMsg, 0);

    //    tmp.CopyTo(arrMsg, 4);

    //    return arrMsg;
    //}
    //public override int DataByByte(byte[] arrByte, int curIdx = 0)
    //{

    //    return 0;
    //     base.DataByByte(arrByte, curIdx);
    //}



}



//1.容器 优化。减少new 。记录 他的 index偏移即可

//2.消息类 修改 。 获取 他的 len，   获取 字节（长度+id），  字节转成data （解析 长度 +id）

public class LessonStep39 : MonoBehaviour
{
    // Start is called before the first frame update
    public Button btnLogin;
    public Button btnSend;
    public InputField ifMsg;

    void Start()
    {

        QuitRoomMsg tt = new QuitRoomMsg();

        tt.pd = new QuitRoomData();
        tt.pd.tt = 44;
        tt.pd.dd = "s是是";
        tt.id = 4441;
        var ttg = tt.ToByte();

        QuitRoomMsg ccc = new QuitRoomMsg();
        ccc.DataByByte(ttg);
        Debug.Log(ccc.ToString());

        btnLogin.onClick.AddListener(() =>
        {

            NetMgr39.Ins.Connect("127.0.0.1", 8088);
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

                //测试 分包 粘包的发送 情况    

                //1.两个 粘包 （2个playdata数据 ）
                {

                    //int len = data.GetLen();

                    //byte[] bytes = new byte[4 + 4 + len];
                    ////这条信息的总长度 
                    //BitConverter.GetBytes(bytes.Length).CopyTo(bytes, 0);

                    //data.ToByte().CopyTo(bytes, 4);

                    //byte[] bytes2 = new byte[bytes.Length * 2];
                    //bytes.CopyTo(bytes2, 0);
                    //for (int i = bytes.Length, j = 0; i < bytes2.Length; i++, j++)
                    //{
                    //    bytes2[i] = bytes[j];
                    //}

                    //NetMgr39.Ins.socket.Send(bytes2);
                    //Debug.Log((4 + 4 + len) + "点击发送了 消息！ 发送了 多少字节：");
                }


                //2.分包 （ 先发 一些  playdata数据 ，再发一部分）

                {

                    //int len = data.GetLen();

                    //byte[] bytes = new byte[4 + 4 + len];
                    ////这条信息的总长度 
                    //BitConverter.GetBytes(bytes.Length).CopyTo(bytes, 0);

                    //data.ToByte().CopyTo(bytes, 4);
                    //int firstSendCount = 2;
                    //byte[] tmp = new byte[firstSendCount];

                    //Array.Copy(bytes, 0, tmp, 0, firstSendCount);

                    //NetMgr39.Ins.socket.Send(tmp);
                    //Debug.Log(firstSendCount + "先发了一部分！");
                    //Task.Run(() =>
                    //{
                    //    Thread.Sleep(4000);

                    //    byte[] tmp2 = new byte[bytes.Length - firstSendCount];
                    //    Array.Copy(bytes, firstSendCount, tmp2, 0, bytes.Length - firstSendCount);

                    //    NetMgr39.Ins.socket.Send(tmp2);
                    //    Debug.Log((bytes.Length - firstSendCount) + "再发了一部分！");
                    //});

                }


                //3.粘包。 在分包。

                {

                    int len = data.GetLen();

                    byte[] bytes = new byte[4 + 4 + len];
                    //这条信息的总长度 
                    BitConverter.GetBytes(bytes.Length).CopyTo(bytes, 0);

                    data.ToByte().CopyTo(bytes, 4);

                    byte[] bytes2 = new byte[bytes.Length * 2];
                    bytes.CopyTo(bytes2, 0);
                    for (int i = bytes.Length, j = 0; i < bytes2.Length; i++, j++)
                    {
                        bytes2[i] = bytes[j];
                    }
                    int firstSendCount = 10;
                    var first = new byte[bytes2.Length / 2 + firstSendCount];
                    Array.Copy(bytes2, 0, first, 0, first.Length);
                    NetMgr39.Ins.socket.Send(first);
                    Debug.Log(first.Length + "先发了一部分！");
                    Task.Run(() =>
                    {
                        Thread.Sleep(4000);

                        byte[] tmp2 = new byte[bytes2.Length - bytes2.Length / 2 - firstSendCount];
                        Array.Copy(bytes2, bytes2.Length / 2 + firstSendCount, tmp2, 0, tmp2.Length);

                        NetMgr39.Ins.socket.Send(tmp2);
                        Debug.Log(tmp2.Length + "再发了一部分！");
                    });



                }
            }

        });





    }

    // Update is called once per frame
    void Update()
    {



        NetMgr39.Ins.ReceiveMsg();
    }
}
