using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//写udp 服务端 （1.区分消息类型 ，2.能够接收多个客户端消息，3.能够发送广播 给 接收过的 客户端，4.主动记录上一次收到
// 客户端消息的时间，如果上时间没有收到该客户端消息，要移除次客户端消息）

//写udp 客户端 （1.区分消息类型， 2. 发送消息， 3 接收消息， 4.判断如果不是客户端发送的消息不处理）

public class StepUdpFinishSync : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
