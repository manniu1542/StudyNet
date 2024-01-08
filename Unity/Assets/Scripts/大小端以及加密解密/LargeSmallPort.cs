using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class LargeSmallPort : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        byte[] arr = BitConverter.GetBytes(43);

        //获取一个 网络数据 。先判定 当前 大小端。


        var data = BitConverter.ToInt32(arr, 0);
        //一般 c#数据 为小端数据， 网络数据为大端数据。需要转换下 
        if (BitConverter.IsLittleEndian)
        {
            var result = IPAddress.NetworkToHostOrder(data);

            var byteTmp = BitConverter.GetBytes(data);
            ChangLargeSmallFun(ref byteTmp);

            Debug.Log(result);
        }
        Debug.Log(IPAddress.HostToNetworkOrder(721420288));


        //小端 转大端模式 并传输 数据到服务器 

        int sendData = 43;

        if (BitConverter.IsLittleEndian)
        {
            sendData = IPAddress.HostToNetworkOrder(sendData);


            Debug.Log(sendData);
        }


        //反转 数组。得出的 跟 大小端转换 是一致的。

        var byteTmp22 = BitConverter.GetBytes(43);
        ChangLargeSmallFun(ref byteTmp22);
        ;
        Debug.Log(BitConverter.ToInt32(byteTmp22, 0));
    }
    /// <summary>
    /// 转换 大小端 数据。，因为 大小端 就是 人跟机器的 读取顺序 不同造成的。 倒叙就解决了 问题。
    /// </summary>
    /// <param name="arr"></param>
    public void ChangLargeSmallFun(ref byte[] arr)
    {
        Array.Reverse(arr);
    }


}
