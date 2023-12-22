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
        //���� �ķ���
        public void Connect(string ip, int port)
        {

            try
            {
                if (IsConnect)
                {
                    Debug.Log("�ظ���¼��");
                    return;
                }

                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                socket.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
                Debug.Log($"��¼�ɹ���{socket.LocalEndPoint}��");
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
                                Debug.LogError("û���ҵ���Ϣid��" + handlerType);
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