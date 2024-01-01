using Google.Protobuf;
using MySutyProto;
using System;
using System.IO;
using UnityEngine;




public class UseProtobuf : MonoBehaviour
{





    private void Start()
    {
        // ���� ��ֵ protobuf �����
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
        msg.Msg1 = new MySutyProto2.TestMsg();  //�������� ���� �������� ��Ҳ����ζĬ����null

        // д�뱾�� �ı��ֽ���

        using (FileStream fs = File.Create(Application.dataPath + "/Scripts/Protobuf/data/test.dta"))
        {
            msg.WriteTo(fs);
            Debug.Log("д���ֽ�����ɣ�");
        }



        // ���� ��ȡ�ֽ��� 
        using (FileStream fs = File.OpenRead(Application.dataPath + "/Scripts/Protobuf/data/test.dta"))
        {
            Test2Msg msg2 = Test2Msg.Parser.ParseFrom(fs);

            Debug.Log("�ڴ��ж���:" + msg2 + "�ֽ��ַ��� ��" + msg2.Arr.ToStringUtf8());
        }


        //protobuf ���� д��  �ֽ�����




        using (MemoryStream fs = new MemoryStream())
        {
            msg.WriteTo(fs);


            Debug.Log("�ڴ���д��:" + msg);
        }


        //protobuf ���� ���� �ֽ�����


        byte[] tmpArr = msg.ToByteArray();
        using (MemoryStream fs = new MemoryStream(tmpArr))
        {
            msg = Test2Msg.Parser.ParseFrom(fs);
            Debug.Log("�ڴ��ж���:" + msg + "�ֽ��ַ��� ��" + msg.Arr.ToStringUtf8());
        }


        //�Զ��� ���л�
        msg = null;
        msg = msg.SerProto(tmpArr);
        Debug.Log("�ڴ��ж���:" + msg + "�ֽ��ַ��� ��" + msg.Arr.ToStringUtf8());

    }




}
