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

using BaseMsgData = LessonStep40.BaseMsgData;

namespace LessonStep40
{

    public class NetMgr40
    {

        public static readonly NetMgr40 Ins = new NetMgr40();
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
                if (!socket.Connected)
                {
                    MainThreadClose();
                    break;
                }
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
                            case HandlerCode.Handler_QuitGame:
                                QuitRoomMsg data1 = new QuitRoomMsg();
                                data1.DataByByte(buffer, 0);
                                queueReceive.Enqueue(data1);

                                break;
                            default:
                                break;
                        }



                    }


                }
                catch (SocketException e)
                {
                    MainThreadClose();

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
                if (!socket.Connected)
                {
                    MainThreadClose();
                    break;
                }
                try
                {
                    if (queueSend.Count > 0)
                    {
                        Debug.Log("上传消息：");
                        var data = queueSend.Dequeue();


                        socket.Send(data.ToByte());
                        if (data is QuitGameMsg)
                        {
                            MainThreadClose();
                        }

                    }

                }
                catch (SocketException e)
                {
                    MainThreadClose();


                    Debug.LogError(e.Message);
                }


            }


        }

        public void MainThreadClose()
        {
            TcpFinishSyc.MainSynchronizationContext.Instance.Post((a) =>
            {
                Debug.Log(Thread.CurrentThread.ManagedThreadId + "线程调用！");
                Close();
            }, null);
        }
        //断开连接 并且 关闭 socket 套接字。

        public void Close()
        {
            if (socket == null) return;
            try
            {
                Debug.Log("关闭sokect");
                socket.Shutdown(SocketShutdown.Both);
                socket.Disconnect(false);
                socket.Close();
                socket = null;

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
    public class QuitGameData : BaseData
    {
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

    public class QuitGameMsg : BaseMsgData
    {
        public QuitGameData msg;
        public override int GetID()
        {
            return HandlerCode.Handler_QuitGame;
        }


    }



    public class LessonStep40 : MonoBehaviour
    {
        // Start is called before the first frame update
        public Button btnClose;
        public Button btnLogin;
        public Button btnSend;
        public InputField ifMsg;

        void Start()
        {
            TcpFinishSyc.MainSynchronizationContext.Instance.Init(Thread.CurrentThread.ManagedThreadId);
            btnClose.onClick.AddListener(() =>
            {

                NetMgr40.Ins.Close();
            });
            btnLogin.onClick.AddListener(() =>
            {

                NetMgr40.Ins.Connect("127.0.0.1", 8088);
            });



            btnSend.onClick.AddListener(() =>
            {

                if (ifMsg.text.Length > 0)
                {

                    if (ifMsg.text == "1")
                    {


                        NetMgr40.Ins.Send(new QuitGameMsg() { msg = new QuitGameData() });

                    }
                    else
                    {

                        QuitRoomData data = new QuitRoomData();

                        data.age = 12;
                        data.name = ifMsg.text;//"张望历代";
                        QuitRoomMsg mm = new QuitRoomMsg() { msg = data };
                        NetMgr40.Ins.Send(mm);
                    }











                }

            });





        }

        // Update is called once per frame
        void Update()
        {

            TcpFinishSyc.MainSynchronizationContext.Instance.Update();

            NetMgr40.Ins.ReceiveMsg();
        }
    }


}