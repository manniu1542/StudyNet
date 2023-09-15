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

//дudp ����� ��1.������Ϣ���� ��2.�ܹ����ն���ͻ�����Ϣ��3.�ܹ����͹㲥 �� ���չ��� �ͻ��ˣ�4.������¼��һ���յ�
// �ͻ�����Ϣ��ʱ�䣬�����ʱ��û���յ��ÿͻ�����Ϣ��Ҫ�Ƴ��οͻ�����Ϣ��

//дudp �ͻ��� ��1.������Ϣ���ͣ� 2. ������Ϣ�� 3 ������Ϣ�� 4.�ж�������ǿͻ��˷��͵���Ϣ������

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
            Debug.Log($"�ͻ���{ClientServer.Instance.socket.LocalEndPoint}��¼��");
        });



        btnSend.onClick.AddListener(() =>
        {

            if (ifMsg.text.Length > 0)
            {

                if (ifMsg.text == "1")//�˳���Ϸ
                {


                    ClientServer.Instance.AddSendMsg(new StepUdpFinishSync.QuitGameMsg());

                }
                else
                {

                    PlayerDataMsg data = new PlayerDataMsg();
                    data.msg = new StepUdpFinishSync.PlayerData();
                    data.msg.age = 12;
                    data.msg.name = ifMsg.text;//"��������";

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
