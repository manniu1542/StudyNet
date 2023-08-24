using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

namespace StepTcpFinishAsync
{

    public class NetManagerTcpAsync
    {

        public static readonly NetManagerTcpAsync Ins = new NetManagerTcpAsync();
        public Socket socket;
        public Queue<BaseMsgData> queueSend = new Queue<BaseMsgData>();
        public Queue<BaseMsgData> queueReceive = new Queue<BaseMsgData>();
        public bool IsConnect => socket != null && socket.Connected;
        byte[] bufferChache = new byte[1024 * 4];

        SocketAsyncEventArgs sendArgs;
        bool isSendContinue;
        public SocketAsyncEventArgs NewSocketAsyncEventArgs(Action<SocketAsyncEventArgs> sucessFun, Action<SocketAsyncEventArgs> failFun = null)
        {
            var ag = new SocketAsyncEventArgs();
            ag.Completed += (socket, args) =>
            {
                if (!this.socket.Connected)
                {
                    MainThreadClose();
                    return;
                }
                if (args.SocketError == SocketError.Success)
                {
                    sucessFun?.Invoke(args);
                }
                else
                {
                    failFun?.Invoke(args);
                    Close();
                    Debug.LogError("网络出错，错误码：" + args.SocketError);
                }
            };
            return ag;
        }

        //连入 的方法
        public void Connect(string ip, int port)
        {


            if (IsConnect)
            {
                Debug.Log("重复登录！");
                return;
            }

            if (socket != null) return;

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var args = NewSocketAsyncEventArgs((funArgs) =>
            {

                Debug.Log($"登录成功：{socket.LocalEndPoint}！");

                Receive();
                SendServerInit();

            });
            args.RemoteEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8088);
            socket.ConnectAsync(args);



        }


        //监听 收入的方法

        private void Receive()
        {


            SocketAsyncEventArgs args = null;
            args = NewSocketAsyncEventArgs((funArgs) =>
            {


                if (funArgs.BytesTransferred <= 0) return;
                int handlerType = BitConverter.ToInt32(funArgs.Buffer, 4);

                switch (handlerType)
                {
                    case HandlerCode.Handler_EnterRoom:
                        PlayerDataMsg data = new PlayerDataMsg();
                        data.DataByByte(funArgs.Buffer);
                        queueReceive.Enqueue(data);

                        break;
                    case HandlerCode.Handler_QuitGame:
                        QuitRoomMsg data1 = new QuitRoomMsg();
                        data1.DataByByte(funArgs.Buffer);
                        queueReceive.Enqueue(data1);

                        break;


                    default:
                        Debug.LogError("没有找到消息id：" + handlerType);
                        break;
                }

                socket.ReceiveAsync(args);

            });
            args.SetBuffer(bufferChache, 0, bufferChache.Length);
            socket.ReceiveAsync(args);


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
        void SendServerInit()
        {
            isSendContinue = true;
            sendArgs = NewSocketAsyncEventArgs((funArgs) =>
            {
                isSendContinue = true;
            });


        }

        public void SendServer()
        {


            if (queueSend.Count > 0 && isSendContinue)
            {
                isSendContinue = false;
                var data = queueSend.Dequeue();

                var byteData = data.ToByte();

                sendArgs.SetBuffer(byteData, 0, byteData.Length);
                socket.SendAsync(sendArgs);

            }



        }

        public void MainThreadClose()
        {

            Close();

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