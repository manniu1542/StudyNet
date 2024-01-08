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

        //��ȡһ�� �������� �����ж� ��ǰ ��С�ˡ�


        var data = BitConverter.ToInt32(arr, 0);
        //һ�� c#���� ΪС�����ݣ� ��������Ϊ������ݡ���Ҫת���� 
        if (BitConverter.IsLittleEndian)
        {
            var result = IPAddress.NetworkToHostOrder(data);

            var byteTmp = BitConverter.GetBytes(data);
            ChangLargeSmallFun(ref byteTmp);

            Debug.Log(result);
        }
        Debug.Log(IPAddress.HostToNetworkOrder(721420288));


        //С�� ת���ģʽ ������ ���ݵ������� 

        int sendData = 43;

        if (BitConverter.IsLittleEndian)
        {
            sendData = IPAddress.HostToNetworkOrder(sendData);


            Debug.Log(sendData);
        }


        //��ת ���顣�ó��� �� ��С��ת�� ��һ�µġ�

        var byteTmp22 = BitConverter.GetBytes(43);
        ChangLargeSmallFun(ref byteTmp22);
        ;
        Debug.Log(BitConverter.ToInt32(byteTmp22, 0));
    }
    /// <summary>
    /// ת�� ��С�� ���ݡ�����Ϊ ��С�� ���� �˸������� ��ȡ˳�� ��ͬ��ɵġ� ����ͽ���� ���⡣
    /// </summary>
    /// <param name="arr"></param>
    public void ChangLargeSmallFun(ref byte[] arr)
    {
        Array.Reverse(arr);
    }


}
