using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class LessonStep34 : MonoBehaviour
{
    // Start is called before the first frame update
    public Button btnLogin;
    public Button btnSend;
    public InputField ifMsg;

    public Socket socketClient;
    public bool isStart;
    void Start()
    {
        btnLogin.onClick.AddListener(() =>
        {

            socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8088);
            try
            {
                socketClient.Connect(ipep);
            }
            catch (SocketException se)
            {

                Debug.Log($"加入网路{ipep}失败码{se.ErrorCode},{se.Message}");
                
                return;
            }
            isStart = true;
            Debug.Log("登录成功！");
            
        });



        btnSend.onClick.AddListener(() =>
        {

            if (ifMsg.text.Length > 0 && socketClient != null)
            {


                print("发送消息给服务器！");
                try
                {
                    socketClient.Send(Encoding.UTF8.GetBytes(ifMsg.text));
                }
                catch (SocketException se)
                {

                    Debug.Log($"失败码{se.ErrorCode},{se.Message}");

                    socketClient.Close();
                    isStart = false;
                    print("关闭客户端连入！");

                }



            }

        });

        Thread threadReceiveInfo = new Thread(OnReceive);

        threadReceiveInfo.Start();
    }
    private void OnReceive()
    {
        byte[] data = new byte[1024 * 1024];
        int receiveNum = 0;
        while (true)
        {
            if (!isStart) continue;

            if (socketClient != null && socketClient.Available > 0)
            {
                receiveNum = socketClient.Receive(data);

                ThreadPool.QueueUserWorkItem(HandleMsg, (socketClient, Encoding.UTF8.GetString(data, 0, receiveNum)));
            }

        }
    }

    private void HandleMsg(object obj)
    {
        (Socket s, string str) info = ((Socket s, string str))obj;
        Debug.Log($"接收到服务器{info.s.RemoteEndPoint}消息：{info.str}");


    }

}
