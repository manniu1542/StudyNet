using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
//��ϰudp  ,�� ��

public class StepRealy37UDP : MonoBehaviour
{
    int udpLimteByteNum = 512;
    void Start()
    {

        Socket socketUdp = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        socketUdp.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8088));
        Debug.Log($"��������udp�ͻ���");
        //����Ϣ
        byte[] sendMsg = Encoding.UTF8.GetBytes("�ͻ��˵�¼��");
        if (sendMsg.Length <= udpLimteByteNum)
        {
            IPEndPoint serverPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8089);
            socketUdp.SendTo(sendMsg, serverPoint);
            Debug.Log($"���Ϳͻ���{socketUdp.LocalEndPoint}��Ϣ,��{serverPoint}");
        }

        //����Ϣ
        byte[] receiveByte = new byte[udpLimteByteNum];
        EndPoint remPoint = new IPEndPoint(IPAddress.Any, 0);
        int rcNum = socketUdp.ReceiveFrom(receiveByte, 0, udpLimteByteNum, SocketFlags.None, ref remPoint);
        Debug.Log($"�յ�{remPoint}��Ϣ��" + Encoding.UTF8.GetString(receiveByte, 0, rcNum));

    }

    // Update is called once per frame
    void Update()
    {

    }
}
