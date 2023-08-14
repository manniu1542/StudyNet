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
    //���� �ķ���
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


    //���� ����ķ���

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
                Debug.Log($"�յ�{socket.RemoteEndPoint}��Ϣ����ҵ� id{data.id},���֣�{data.name},����{data.age}���Ա�{data.isMan}");
            }


        }


    }
    //���� ��Ϣ�ķ���
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
                    Debug.Log("�ϴ���Ϣ��");
                    var data = queueSend.Dequeue();
                    int len = data.GetLen();

                    byte[] bytes = new byte[4 + 4 + len];
                    //������Ϣ���ܳ��� 
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

    //���� ��Ϣ�ķ���

    public void Close()
    {
        if (socket == null) return;
        try
        {

            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            Debug.Log("�ر�sokect");
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
    //{//���� + id
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



//1.���� �Ż�������new ����¼ ���� indexƫ�Ƽ���

//2.��Ϣ�� �޸� �� ��ȡ ���� len��   ��ȡ �ֽڣ�����+id����  �ֽ�ת��data ������ ���� +id��

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
        tt.pd.dd = "s����";
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
                data.name = ifMsg.text;//"��������";
                data.id = 33;

                //���� �ְ� ճ���ķ��� ���    

                //1.���� ճ�� ��2��playdata���� ��
                {

                    //int len = data.GetLen();

                    //byte[] bytes = new byte[4 + 4 + len];
                    ////������Ϣ���ܳ��� 
                    //BitConverter.GetBytes(bytes.Length).CopyTo(bytes, 0);

                    //data.ToByte().CopyTo(bytes, 4);

                    //byte[] bytes2 = new byte[bytes.Length * 2];
                    //bytes.CopyTo(bytes2, 0);
                    //for (int i = bytes.Length, j = 0; i < bytes2.Length; i++, j++)
                    //{
                    //    bytes2[i] = bytes[j];
                    //}

                    //NetMgr39.Ins.socket.Send(bytes2);
                    //Debug.Log((4 + 4 + len) + "��������� ��Ϣ�� ������ �����ֽڣ�");
                }


                //2.�ְ� �� �ȷ� һЩ  playdata���� ���ٷ�һ���֣�

                {

                    //int len = data.GetLen();

                    //byte[] bytes = new byte[4 + 4 + len];
                    ////������Ϣ���ܳ��� 
                    //BitConverter.GetBytes(bytes.Length).CopyTo(bytes, 0);

                    //data.ToByte().CopyTo(bytes, 4);
                    //int firstSendCount = 2;
                    //byte[] tmp = new byte[firstSendCount];

                    //Array.Copy(bytes, 0, tmp, 0, firstSendCount);

                    //NetMgr39.Ins.socket.Send(tmp);
                    //Debug.Log(firstSendCount + "�ȷ���һ���֣�");
                    //Task.Run(() =>
                    //{
                    //    Thread.Sleep(4000);

                    //    byte[] tmp2 = new byte[bytes.Length - firstSendCount];
                    //    Array.Copy(bytes, firstSendCount, tmp2, 0, bytes.Length - firstSendCount);

                    //    NetMgr39.Ins.socket.Send(tmp2);
                    //    Debug.Log((bytes.Length - firstSendCount) + "�ٷ���һ���֣�");
                    //});

                }


                //3.ճ���� �ڷְ���

                {

                    int len = data.GetLen();

                    byte[] bytes = new byte[4 + 4 + len];
                    //������Ϣ���ܳ��� 
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
                    Debug.Log(first.Length + "�ȷ���һ���֣�");
                    Task.Run(() =>
                    {
                        Thread.Sleep(4000);

                        byte[] tmp2 = new byte[bytes2.Length - bytes2.Length / 2 - firstSendCount];
                        Array.Copy(bytes2, bytes2.Length / 2 + firstSendCount, tmp2, 0, tmp2.Length);

                        NetMgr39.Ins.socket.Send(tmp2);
                        Debug.Log(tmp2.Length + "�ٷ���һ���֣�");
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
