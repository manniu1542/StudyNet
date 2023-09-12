using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;


//SocketAsyncEventArgs  ����  �滻 begin  end  socket ������ ��
public class StepRealy32Asyc2 : MonoBehaviour
{
    // Start is called before the first frame update
    Socket socket;
    byte[] arrReceive = new byte[1024];

    void Start()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        var s = new SocketAsyncEventArgs();
        s.Completed += (_socket, args) =>
        {
            if (args.SocketError == SocketError.Success)
            {
                Debug.Log("�ɹ����룺");
                var soc = _socket as Socket;


                AsyncRecevie(soc);


            }
            else
            {
                Debug.LogError("����ʧ�ܣ�" + args.SocketError);
            }

        };

        s.RemoteEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8088);
        socket.ConnectAsync(s);




    }

    void AsyncRecevie(Socket s)
    {

        var args = new SocketAsyncEventArgs();
        args.SetBuffer(arrReceive, 0, arrReceive.Length);
        args.Completed += (_socket, _args) =>
        {
            if (_args.SocketError == SocketError.Success)
            {

                var soc = _socket as Socket;


                string str = Encoding.UTF8.GetString(_args.Buffer, 0, _args.BytesTransferred);

                Debug.Log("�յ���Ϣ��" + str);
                soc.ReceiveAsync(args);
            }
            else
            {
                Debug.LogError("�յ���Ϣʧ�ܣ�" + _args.SocketError);
            }

        };

        s.ReceiveAsync(args);

    }
    // Update is called once per frame
    void Update()
    {
        if (socket != null && socket.Connected && Input.GetKeyDown(KeyCode.A))
        {
            string str = "hello Async";
            byte[] tmp = Encoding.UTF8.GetBytes(str);

            var args = new SocketAsyncEventArgs();
            args.SetBuffer(tmp, 0, tmp.Length);
            args.Completed += (_socket, _args) =>
            {
                if (_args.SocketError == SocketError.Success)
                {

                    Debug.Log("������Ϣ��ɣ�" + str);

                }
                else
                {
                    Debug.LogError("������Ϣʧ�ܣ�" + _args.SocketError);
                }

            };
            socket.SendAsync(args);

        }

    }
}
