using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LessonStep33 : MonoBehaviour
{

    public Button btnSend;
    public TMP_InputField ifMsg;
    // Start is called before the first frame update
    void Start()
    {
        btnSend.onClick.AddListener(() =>
        {

            if (ifMsg.text.Length > 0)
            {


                print("发送消息给服务器！");
                CreateClient(ifMsg.text);
            }

        });
    }

    void CreateClient(string msg)
    {
        Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8088);

        client.Connect(ipep);
        byte[] arr = Encoding.UTF8.GetBytes(msg);
        int senI = client.Send(arr);
        if (senI == arr.Length)
        {
            print("发送消息完整！");
        }
        else
        {
            print("发送消息有问题！0 发送失败，非0则是发送了部分消息" + senI);
        }
        client.Shutdown(SocketShutdown.Both);
        client.Close();
        print("断开连接！");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
