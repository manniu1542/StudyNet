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
                    Debug.LogError("������������룺" + args.SocketError);
                }
            };
            return ag;
        }

        //���� �ķ���
        public void Connect(string ip, int port)
        {


            if (IsConnect)
            {
                Debug.Log("�ظ���¼��");
                return;
            }

            if (socket != null) return;

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var args = NewSocketAsyncEventArgs((funArgs) =>
            {

                Debug.Log($"��¼�ɹ���{socket.LocalEndPoint}��");

                Receive();
                SendServerInit();

            });
            args.RemoteEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8088);
            socket.ConnectAsync(args);



        }


        //���� ����ķ���

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
                        Debug.LogError("û���ҵ���Ϣid��" + handlerType);
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

                    Debug.LogError($"�յ�{socket.RemoteEndPoint} QuitRoomMsg��Ϣ����ҵ� ���֣�{data.msg.name},����{data.msg.age}");
                }
                else if (obj is PlayerDataMsg)
                {
                    var data = obj as PlayerDataMsg;

                    Debug.LogError($"�յ�{socket.RemoteEndPoint} PlayerDataMsg��Ϣ����ҵ� ���֣�{data.msg.name},����{data.msg.age}");
                }


            }


        }
        //���� ��Ϣ�ķ���
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
                Debug.Log("�����˳���Ϣ��");
            }
            else if (md is HeartMsg)
            {
                //Debug.Log("������Ϣ");
            }
            else
            {
                Debug.Log("��ͨ������Ϣ��");
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
        //�Ͽ����� ���� �ر� socket �׽��֡�

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
                    Debug.Log("�ر�sokect");
                }
                else
                {
                    Debug.LogError("���������ܹҵ��ˣ�");
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