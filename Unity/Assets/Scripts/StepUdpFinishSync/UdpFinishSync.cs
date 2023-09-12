using StepUdpFinishSync;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

//写udp 服务端 （1.区分消息类型 ，2.能够接收多个客户端消息，3.能够发送广播 给 接收过的 客户端，4.主动记录上一次收到
// 客户端消息的时间，如果上时间没有收到该客户端消息，要移除次客户端消息）

//写udp 客户端 （1.区分消息类型， 2. 发送消息， 3 接收消息， 4.判断如果不是客户端发送的消息不处理）

public class UdpFinishSync : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        var arr = Encoding.UTF8.GetBytes("hello , i'm back");

        s.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080));
        s.SendTo(arr, 0, arr.Length, SocketFlags.None, new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8088));

        Debug.LogError("客户端发送消息完成！");
        //PlayerDataMsg pdm = new PlayerDataMsg();
        //pdm.msg = new PlayerData()
        //{
        //    age = 14,
        //    name = "s是的",
        //    isMan = true
        //};
        //var arr2 = pdm.ToByte();

        //s.SendTo(arr2, 0, arr2.Length, SocketFlags.None, new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8088));



        byte[] tmp = new byte[512];
        EndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
        s.ReceiveFrom(tmp, 0, tmp.Length, SocketFlags.None, ref anyIP);


        Debug.LogError("收到消息：" + Encoding.UTF8.GetString(tmp));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
