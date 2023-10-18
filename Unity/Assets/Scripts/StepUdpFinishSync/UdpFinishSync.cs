using StepTcpFinishAsync;
using StepUdpFinishSync;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using PlayerDataMsg = StepUdpFinishSync.PlayerDataMsg;

//写udp 服务端 （1.区分消息类型 ，2.能够接收多个客户端消息，3.能够发送广播 给 接收过的 客户端，4.主动记录上一次收到
// 客户端消息的时间，如果上时间没有收到该客户端消息，要移除次客户端消息）

//写udp 客户端 （1.区分消息类型， 2. 发送消息， 3 接收消息， 4.判断如果不是客户端发送的消息不处理）

public class UdpFinishSync : MonoBehaviour
{
    // Start is called before the first frame update

    public Button btnLogin;
    public Button btnSend;
    public Button btnClose;
    public InputField ifMsg;
    void Start()
    {




        btnClose.onClick.AddListener(() =>
        {

            ClientServer.Instance.Close();
        });

        btnLogin.onClick.AddListener(() =>
        {

            ClientServer.Instance.Start();
            Debug.Log($"客户端{ClientServer.Instance.socket.LocalEndPoint}登录！");
        });



        btnSend.onClick.AddListener(() =>
        {

            if (ifMsg.text.Length > 0)
            {

                if (ifMsg.text == "1")//退出游戏
                {


                    ClientServer.Instance.AddSendMsg(new StepUdpFinishSync.QuitGameMsg());

                }
                else
                {

                    PlayerDataMsg data = new PlayerDataMsg();
                    data.msg = new StepUdpFinishSync.PlayerData();
                    data.msg.age = 12;
                    data.msg.name = ifMsg.text;//"张望历代";

                    ClientServer.Instance.AddSendMsg(data);
                }

            }

        });


    }

    // Update is called once per frame
    void Update()
    {
        ClientServer.Instance.SendHeardMsg(Time.deltaTime);
    }
}
