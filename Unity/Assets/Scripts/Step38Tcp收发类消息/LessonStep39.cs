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

using BaseMsgData = LessonStep39.BaseMsgData;

namespace LessonStep39
{

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
                            case HandlerCode.Handler_QuitRoom:
                                QuitRoomMsg data = new QuitRoomMsg();
                                data.DataByByte(buffer, 0);
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


                if (obj is QuitRoomMsg)
                {
                    var data = obj as QuitRoomMsg;

                    Debug.Log($"收到{socket.RemoteEndPoint}消息：玩家的 名字：{data.msg.name},年龄{data.msg.age}");
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

                        socket.Send(data.ToByte());

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
        public int age;
        public string name;
    }

    public abstract class BaseMsgData : BaseData
    {
        public int id;
        public abstract int GetID();



        public override byte[] ToByte()
        {
            int len = 4 + 4 + base.GetLen();
            int handlerId = this.GetID();
            byte[] tmp = base.ToByte();
            byte[] arrMsg = new byte[len];



            BitConverter.GetBytes(len).CopyTo(arrMsg, 0);
            BitConverter.GetBytes(handlerId).CopyTo(arrMsg, 4);

            tmp.CopyTo(arrMsg, 8);









            return arrMsg;
        }


        public override int DataByByte(byte[] arrByte, int curIdx = 0)
        {

            id = BitConverter.ToInt32(arrByte, curIdx + 4);


            curIdx = base.DataByByte(arrByte, curIdx + 8);


            return curIdx;
        }



    }


    public class QuitRoomMsg : BaseMsgData
    {
        public QuitRoomData msg;
        public override int GetID()
        {
            return HandlerCode.Handler_QuitRoom;
        }


    }




    public class LessonStep39 : MonoBehaviour
    {
        // Start is called before the first frame update
        public Button btnLogin;
        public Button btnSend;
        public InputField ifMsg;

        void Start()
        {

            NetMgr39.Ins.socket.Shutdown(SocketShutdown.Both);

            btnLogin.onClick.AddListener(() =>
               {

                   NetMgr39.Ins.Connect("127.0.0.1", 8088);
               });



            btnSend.onClick.AddListener(() =>
            {

                if (ifMsg.text.Length > 0)
                {





                    //测试 分包 粘包的发送 情况    

                    //1.两个 粘包 （2个playdata数据 ）
                    {



                        //QuitRoomData data = new QuitRoomData();

                        //data.age = 12;
                        //data.name = ifMsg.text;//"张望历代";
                        //QuitRoomMsg mm = new QuitRoomMsg() { msg = data };


                        //QuitRoomData data2 = new QuitRoomData();

                        //data2.age = 88;
                        //data2.name = ifMsg.text + " copy";//"张望历代";
                        //QuitRoomMsg cc = new QuitRoomMsg() { msg = data2 };


                        //int len = 4 + 4 + mm.GetLen();
                        //int len2 = 4 + 4 + cc.GetLen();
                        //byte[] arrTmp = new byte[len + len2];

                        //mm.ToByte().CopyTo(arrTmp, 0);

                        //cc.ToByte().CopyTo(arrTmp, len);

                        //NetMgr39.Ins.socket.Send(arrTmp);
                        //Debug.Log((len * 2) + "点击发送了 消息！ 发送了 多少字节：");
                    }


                    //2.分包 （ 先发 一些  playdata数据 ，再发一部分）

                    {

                        //QuitRoomData data = new QuitRoomData();

                        //data.age = 12;
                        //data.name = ifMsg.text;//"张望历代";
                        //QuitRoomMsg mm = new QuitRoomMsg() { msg = data };
                        //int len = mm.GetLen();

                        //byte[] bytes = mm.ToByte();
                        ////这条信息的总长度 

                        //int firstSendCount = 10;
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

                        QuitRoomData data = new QuitRoomData();

                        data.age = 8;
                        data.name = ifMsg.text;//"张望历代";
                        QuitRoomMsg mm = new QuitRoomMsg() { msg = data };


                        QuitRoomData data2 = new QuitRoomData();

                        data2.age = 9;
                        data2.name = ifMsg.text + " copy";//"张望历代";
                        QuitRoomMsg cc = new QuitRoomMsg() { msg = data2 };


                        int len = 4 + 4 + mm.GetLen();
                        int len2 = 4 + 4 + cc.GetLen();
                        byte[] arrTmp = new byte[len + len2];

                        mm.ToByte().CopyTo(arrTmp, 0);

                        cc.ToByte().CopyTo(arrTmp, len);


                        int firstSendCount = 10;
                        var first = new byte[arrTmp.Length / 2 + firstSendCount];
                        Array.Copy(arrTmp, 0, first, 0, first.Length);
                        NetMgr39.Ins.socket.Send(first);

                        Debug.Log(first.Length + "先发了一部分！");
                        Task.Run(() =>
                        {
                            Thread.Sleep(4000);

                            byte[] tmp2 = new byte[arrTmp.Length - arrTmp.Length / 2 - firstSendCount];
                            Array.Copy(arrTmp, arrTmp.Length / 2 + firstSendCount, tmp2, 0, tmp2.Length);

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


}