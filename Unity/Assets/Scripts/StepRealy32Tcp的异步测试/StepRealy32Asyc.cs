using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;


//begin end ��socket����
public class StepRealy32Asyc : MonoBehaviour
{
    // Start is called before the first frame update
    Socket socket;
    byte[] arrReceive = new byte[1024];
    void Start()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        Debug.Log("��ʼ���룡" + socket.Connected);
        socket.BeginConnect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8088), (result) =>
        {
            Debug.Log("222����ɹ���" + socket.Connected);
            socket.EndConnect(result);
            Debug.Log("����ɹ���" + socket.Connected);

            socket.BeginReceive(arrReceive, 0, arrReceive.Length, SocketFlags.None, AsyncRecevie, socket);


        }, null);
     
    }

    void AsyncRecevie(IAsyncResult reslut)
    {
        Socket s = reslut.AsyncState as Socket;
        var receiveNum = s.EndSend(reslut);
        string str = Encoding.UTF8.GetString(arrReceive, 0, receiveNum);
        Debug.Log("�յ���Ϣ��" + str);
        s.BeginReceive(arrReceive, 0, arrReceive.Length, SocketFlags.None, AsyncRecevie, s);

    }
    // Update is called once per frame
    void Update()
    {
        if (socket != null && socket.Connected && Input.GetKeyDown(KeyCode.A))
        {
            string str = "hello Async";
            byte[] tmp = Encoding.UTF8.GetBytes(str);
            socket.BeginSend(tmp, 0, tmp.Length, SocketFlags.None, (result) =>
            {
                socket.EndSend(result);
                Debug.Log("������ɣ�" + str);
            }, null);
        }

    }
}
