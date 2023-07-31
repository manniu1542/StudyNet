using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Lesson1 : MonoBehaviour
{
    // Start is called before the first frame update
    async void Start()
    {
        Debug.Log(Dns.GetHostName());
        var value = await Dns.GetHostEntryAsync("www.baidu.com");
        for (int i = 0; i < value.AddressList.Length; i++)
        {
            Debug.Log(value.AddressList[i]);
        }
        for (int i = 0; i < value.Aliases.Length; i++)
        {
            Debug.Log(value.Aliases[i]);
        }

        Debug.Log(value.HostName);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
