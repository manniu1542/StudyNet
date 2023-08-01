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
        Debug.Log("课程22:地址 端口号 的解答！");
        //地址
        IPAddress ip = IPAddress.Parse("127.0.0.1");
        print(ip.ToString());
        //地址+端口号
        IPEndPoint ipep = new IPEndPoint(ip, 80);
        print(ipep.ToString());
    }
    public async void Lesson24()
    {
        Debug.Log("课程24：dns 域名转换地址  的解答！");
        Debug.Log(Dns.GetHostName());
       
        var value = await Dns.GetHostEntryAsync("www.baidu.com");
        //获取 dns映射的 地址 列表 
        for (int i = 0; i < value.AddressList.Length; i++)
        {
            Debug.Log(value.AddressList[i]);
        }
        //获取 dns映射的 地址 列表 的别名
        for (int i = 0; i < value.Aliases.Length; i++)
        {
            Debug.Log(value.Aliases[i]);
        }
        //获取 该域名 所在的主机名。
        Debug.Log(value.HostName);
    }


    // Update is called once per frame
    void Update()
    {

    }
}
