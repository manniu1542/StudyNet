using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class LessonStep22 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Lesson22();
        Lesson24();
    }
    public void Lesson22()
    {
        Debug.Log("�γ�22:��ַ �˿ں� �Ľ��");
        //��ַ
        IPAddress ip = IPAddress.Parse("127.0.0.1");
        print(ip.ToString());
        //��ַ+�˿ں�
        IPEndPoint ipep = new IPEndPoint(ip, 80);
        print(ipep.ToString());
    }
    public async void Lesson24()
    {
        Debug.Log("�γ�24��dns ����ת����ַ  �Ľ��");
        Debug.Log(Dns.GetHostName());
       
        var value = await Dns.GetHostEntryAsync("www.baidu.com");
        //��ȡ dnsӳ��� ��ַ �б� 
        for (int i = 0; i < value.AddressList.Length; i++)
        {
            Debug.Log(value.AddressList[i]);
        }
        //��ȡ dnsӳ��� ��ַ �б� �ı���
        for (int i = 0; i < value.Aliases.Length; i++)
        {
            Debug.Log(value.Aliases[i]);
        }
        //��ȡ ������ ���ڵ���������
        Debug.Log(value.HostName);
    }


    // Update is called once per frame
    void Update()
    {

    }
}
