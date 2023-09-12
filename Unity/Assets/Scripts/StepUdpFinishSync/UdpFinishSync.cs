using StepUdpFinishSync;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

//дudp ����� ��1.������Ϣ���� ��2.�ܹ����ն���ͻ�����Ϣ��3.�ܹ����͹㲥 �� ���չ��� �ͻ��ˣ�4.������¼��һ���յ�
// �ͻ�����Ϣ��ʱ�䣬�����ʱ��û���յ��ÿͻ�����Ϣ��Ҫ�Ƴ��οͻ�����Ϣ��

//дudp �ͻ��� ��1.������Ϣ���ͣ� 2. ������Ϣ�� 3 ������Ϣ�� 4.�ж�������ǿͻ��˷��͵���Ϣ������

public class UdpFinishSync : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        s.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), Random.Range(8000, 9000)));
        //var arr = Encoding.UTF8.GetBytes("hello , i'm back");
        //s.SendTo(arr, 0, arr.Length, SocketFlags.None, new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8088));

        Debug.LogError("�ͻ��˷�����Ϣ��ɣ�" + s.LocalEndPoint);
        //QuitGameMsg pdm = new QuitGameMsg();
        PlayerDataMsg pdm = new PlayerDataMsg();
        pdm.msg = new PlayerData()
        {
            age = 14,
            name = "s�ǵ�",
            isMan = true
        };
        var arr2 = pdm.ToByte();

        s.SendTo(arr2, 0, arr2.Length, SocketFlags.None, new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8088));



        byte[] tmp = new byte[512];
        EndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
        s.ReceiveFrom(tmp, 0, tmp.Length, SocketFlags.None, ref anyIP);


        Debug.LogError($"�յ� ip {anyIP}��Ϣ��" + Encoding.UTF8.GetString(tmp));



        byte[] tmp2 = new byte[512];
        EndPoint anyIP2 = new IPEndPoint(IPAddress.Any, 0);
        s.ReceiveFrom(tmp2, 0, tmp2.Length, SocketFlags.None, ref anyIP);


        Debug.LogError($"�յ� ip�㲥 {anyIP2}��Ϣ��" + Encoding.UTF8.GetString(tmp2));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
