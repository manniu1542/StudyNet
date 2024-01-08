using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class EncryptionAndDecryption : MonoBehaviour
{
    public string mingwen = "hello encry";
    public byte miyao = 127;

    byte[] miwen;
    // Start is called before the first frame update
    public void Encryption()
    {
        miwen = Encoding.UTF8.GetBytes(mingwen);

        for (int i = 0; i < miwen.Length; i++)
        {
            miwen[i] ^= miyao;
        }


        Debug.Log("加密完成");
    }

    // Update is called once per frame
    public void Decryption()
    {
        for (int i = 0; i < miwen.Length; i++)
        {
            miwen[i] ^= miyao;
        }

        Debug.Log("解密结果：" + Encoding.UTF8.GetString(miwen, 0, miwen.Length));
    }
}
