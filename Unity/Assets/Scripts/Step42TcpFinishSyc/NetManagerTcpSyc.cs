using LessonStep40;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

namespace TcpFinishSyc
{

    public class NetManagerTcpSyc
    {

        public static readonly NetManagerTcpSyc Ins = new NetManagerTcpSyc();
        public Socket socket;
        public Queue<BaseMsgData> queueSend = new Queue<BaseMsgData>();
        public Queue<BaseMsgData> queueReceive = new Queue<BaseMsgData>();
        public bool IsConnect => socket != null && socket.Connected;
        //连入 的方法
        public void Connect(string ip, int port)
        {

            try
            {
                if (IsConnect)
                {
                    Debug.Log("重复登录！");
                    return;
                }

                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                socket.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
                Debug.Log($"登录成功：{socket.LocalEndPoint}！");
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

                        int handlerType = BitConverter.ToInt32(buffer, 4);

                        switch (handlerType)
                        {
                            case HandlerCode.Handler_EnterRoom:
                                PlayerDataMsg data = new PlayerDataMsg();
                                data.DataByByte(buffer);
                                queueReceive.Enqueue(data);

                                break;
                            case HandlerCode.Handler_QuitGame:
                                QuitRoomMsg data1 = new QuitRoomMsg();
                                data1.DataByByte(buffer);
                                queueReceive.Enqueue(data1);

                                break;


                            default:
                                Debug.LogError("没有找到消息id：" + handlerType);
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

                    Debug.LogError($"收到{socket.RemoteEndPoint} QuitRoomMsg消息：玩家的 名字：{data.msg.name},年龄{data.msg.age}");
                }
                else if (obj is PlayerDataMsg)
                {
                    var data = obj as PlayerDataMsg;

                    Debug.LogError($"收到{socket.RemoteEndPoint} PlayerDataMsg消息：玩家的 名字：{data.msg.name},年龄{data.msg.age}");
                }


            }


        }
        //发送 消息的方法
        public void Send(BaseMsgData md)
        {
            if (socket == null) return;
            queueSend.Enqueue(md);

        }
        public void SendFinishAnlyze(BaseMsgData md)
        {
            if (md is QuitGameMsg)
            {
                MainThreadClose();
                Debug.Log("主动退出消息：");
            }
            else if (md is HeartMsg)
            {
                //Debug.Log("心跳消息");
            }
            else
            {
                Debug.Log("普通数据消息：");
            }
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

                        var data = queueSend.Dequeue();


                        socket.Send(data.ToByte());
                        SendFinishAnlyze(data);


                    }

                }
                catch (SocketException e)
                {
                    MainThreadClose();


                    Debug.LogWarning(e.Message);
                }


            }


        }

        public void MainThreadClose()
        {
            MainSynchronizationContext.Instance.Post((a) =>
            {
                Close();
            }, null);
        }
        //断开连接 并且 关闭 socket 套接字。

        public void Close()
        {
            if (socket == null) return;
            try
            {



                if (IsConnect)
                {
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Disconnect(false);
                    socket.Close();
                    Debug.Log("关闭sokect");
                }
                else
                {
                    Debug.LogError("服务器可能挂掉了！");
                }

                socket = null;

            }
            catch (SocketException e)
            {
                Debug.LogError(e.Message);

            }

        }

    }


}