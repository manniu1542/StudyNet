using Google.Protobuf;
using MySutyProto;
using System;
using System.IO;
using UnityEngine;




public class UseProtobuf : MonoBehaviour
{





    private void Start()
    {
        // 生成 赋值 protobuf 类对象
        Test2Msg msg = new Test2Msg();

        msg.Aa = 1;
        msg.Tt = 2;
        msg.Gg = 3.14f;
        msg.Mm = "hha";
        msg.Bb = true;
        msg.Arr = ByteString.CopyFromUtf8("hello world");
        msg.TypeTest = MySutyProto2.EnuTestType.Two;
        msg.ArrTT.Add(5.21f);
        msg.ArrTT.Add(9.21f);
        msg.ArrTT.Add(6.21f);
        msg.At = 8;
        msg.TtMap.Add(2, 3.11f);
        msg.TtMap.Add(9, 0.11f);
        msg.Msg1 = new MySutyProto2.TestMsg();  //不生成类 数据 不会增加 。也就意味默认是null

        // 写入本地 文本字节流

        using (FileStream fs = File.Create(Application.dataPath + "/Scripts/Protobuf/data/test.dta"))
        {
            msg.WriteTo(fs);
            Debug.Log("写入字节流完成！");
        }



        // 本地 读取字节流 
        using (FileStream fs = File.OpenRead(Application.dataPath + "/Scripts/Protobuf/data/test.dta"))
        {
            Test2Msg msg2 = Test2Msg.Parser.ParseFrom(fs);

            Debug.Log("内存中读入:" + msg2 + "字节字符串 ：" + msg2.Arr.ToStringUtf8());
        }


        //protobuf 对象 写入  字节数组




        using (MemoryStream fs = new MemoryStream())
        {
            msg.WriteTo(fs);


            Debug.Log("内存中写入:" + msg);
        }


        //protobuf 对象 读出 字节数组


        byte[] tmpArr = msg.ToByteArray();
        using (MemoryStream fs = new MemoryStream(tmpArr))
        {
            msg = Test2Msg.Parser.ParseFrom(fs);
            Debug.Log("内存中读出:" + msg + "字节字符串 ：" + msg.Arr.ToStringUtf8());
        }


        //自定义 序列化
        msg = null;
        msg = msg.SerProto(tmpArr);
        Debug.Log("内存中读出:" + msg + "字节字符串 ：" + msg.Arr.ToStringUtf8());

    }




}
