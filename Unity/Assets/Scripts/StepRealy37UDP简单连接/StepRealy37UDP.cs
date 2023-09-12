using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
//练习udp  ,发 收

public class StepRealy37UDP : MonoBehaviour
{
    int udpLimteByteNum = 512;
    void Start()
    {

        Socket socketUdp = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        socketUdp.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8088));
        Debug.Log($"启动简易udp客户端");
        //发消息
        byte[] sendMsg = Encoding.UTF8.GetBytes("客户端登录！");
        if (sendMsg.Length <= udpLimteByteNum)
        {
            IPEndPoint serverPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8089);
            socketUdp.SendTo(sendMsg, serverPoint);
            Debug.Log($"发送客户端{socketUdp.LocalEndPoint}消息,给{serverPoint}");
        }

        //收消息
        byte[] receiveByte = new byte[udpLimteByteNum];
        EndPoint remPoint = new IPEndPoint(IPAddress.Any, 0);
        int rcNum = socketUdp.ReceiveFrom(receiveByte, 0, udpLimteByteNum, SocketFlags.None, ref remPoint);
        Debug.Log($"收到{remPoint}消息：" + Encoding.UTF8.GetString(receiveByte, 0, rcNum));

    }

    // Update is called once per frame
    void Update()
    {

    }
}
