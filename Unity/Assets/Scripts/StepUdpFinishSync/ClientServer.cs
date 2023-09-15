using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace StepUdpFinishSync
{
    class ClientServer
    {
        public static readonly ClientServer Instance = new ClientServer();
        public Socket socket;

        IPEndPoint serverIP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8088);

        public float sendHeardMsgDT = 2f;
        bool isClose = true;
        Queue<BaseMsgData> queueSendMsg;
        ClientServer()
        {



        }


        public void Start()
        {
            isClose = false;
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            socket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), UnityEngine.Random.Range(8090, 9000)));

            Task.Run(Receive);
            Task.Run(SendTo);

        }
        public void SendHeardMsg(float dt)
        {
            if (!isClose)
            {
                sendHeardMsgDT -= dt;
                if (sendHeardMsgDT <= 0)
                {
                    sendHeardMsgDT = 2;
                    AddSendMsg(new HeartMsg());
                }
            }

        }
        public void Close()
        {
            isClose = true;
            if (socket != null)
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
                socket = null;

            }
        }
        public void AddSendMsg(BaseMsgData data)
        {
            queueSendMsg.Enqueue(data);
        }
        public void SendTo()
        {
            while (!isClose)
            {
                if (queueSendMsg.Count > 0)
                {
                    try
                    {

                        var arr = queueSendMsg.Dequeue().ToByte();
                        socket.SendTo(arr, 0, arr.Length, SocketFlags.None, serverIP);



                    }
                    catch (SocketException e)
                    {
                        Debug.LogError(e);
                        Close();

                    }
                }

            }


        }

        public void Receive()
        {
            while (!isClose)
            {

                if (socket.Available > 0)
                {
                    try
                    {

                        EndPoint tmp = new IPEndPoint(IPAddress.Any, 0);
                        byte[] arrReceive = new byte[512];
                        socket.ReceiveFrom(arrReceive, 0, 512, SocketFlags.None, ref tmp);

                        if (!tmp.Equals(serverIP))
                        {
                            Debug.Log("垃圾消息，可不做处理");
                            continue;
                        }

                        MsgHandle(ref arrReceive);



                    }
                    catch (SocketException e)
                    {
                        Debug.LogError(e);
                        Close();

                    }
                }



            }


        }
        public void MsgHandle(ref byte[] buffer)
        {

            int handlerType = BitConverter.ToInt32(buffer, 4);

            switch (handlerType)
            {
                case HandlerCode.Handler_EnterRoom:
                    PlayerDataMsg data = new PlayerDataMsg();
                    data.DataByByte(buffer);
                    Debug.Log($"收到服务器的消息：" +
                    $"名字:{data.msg.name} 年龄: {data.msg.age} 性别:{data.msg.isMan} ");


                    break;
                case HandlerCode.Handler_QuitGame:
                    Debug.Log("<Handler_QuitGame>");

                    break;


                default:
                    Debug.LogError("没有找到消息id：" + handlerType);
                    break;
            }



        }
    }
}
