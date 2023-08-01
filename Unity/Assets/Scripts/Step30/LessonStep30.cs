using Newtonsoft.Json.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEditor;
using UnityEngine;


public class BaseData
{
    ////ö��ת byte
    //private byte[] StructToByte<H>(H t) where H : struct
    //{


    //}
    //�̳е���תbyte  ����� ��ֵ��ô ��ȡ 
    public int GetLen(Type t)
    {

        FieldInfo[] fi = t.GetFields();

        int len = 0;
        for (int i = 0; i < fi.Length; i++)
        {

            if (fi[i].FieldType == typeof(int))
            {
                len += sizeof(int);
            }
            else if (fi[i].FieldType == typeof(long))
            {
                len += sizeof(long);
            }
            else if (fi[i].FieldType == typeof(string))
            {
                string tt = fi[i].GetValue(this) as string;
                len += sizeof(int);
                len += Encoding.UTF8.GetByteCount(tt);
            }
            else if (fi[i].FieldType == typeof(bool))
            {
                len += sizeof(bool);
            }
            else if (fi[i].FieldType.IsSubclassOf(typeof(BaseData)))
            {
                len += GetLen(fi[i].FieldType);
            }
            else
            {
                Debug.LogError("©�����л������ԣ�" + fi[i].Name);
            }



        }
        return len;
    }
    public int GetLen<T>() where T : BaseData
    {

        Type t = typeof(T);
        return GetLen(t);
    }
    public byte[] ToByte<T>() where T : BaseData
    {
        //��ȡ�����Ӽ�


        Type t = typeof(T);

        int len = GetLen<T>();
        byte[] arrByte = new byte[len];


        FieldInfo[] fi = t.GetFields();
        int curIdx = 0;
        for (int i = 0; i < fi.Length; i++)
        {
            object o = fi[i].GetValue(this);
            if (fi[i].FieldType == typeof(int))
            {

                BitConverter.GetBytes((int)o).CopyTo(arrByte, curIdx);
                curIdx += sizeof(int);
            }
            else if (fi[i].FieldType == typeof(long))
            {

                BitConverter.GetBytes((long)o).CopyTo(arrByte, curIdx);
                curIdx += sizeof(long);
            }
            else if (fi[i].FieldType == typeof(string))
            {
                string tt = fi[i].GetValue(this) as string;
                int count = Encoding.UTF8.GetByteCount(tt);


                BitConverter.GetBytes(count).CopyTo(arrByte, curIdx);
                curIdx += sizeof(int);



                Encoding.UTF8.GetBytes(tt).CopyTo(arrByte, curIdx);

                curIdx += count;


            }
            else if (fi[i].FieldType == typeof(bool))
            {

                BitConverter.GetBytes((bool)o).CopyTo(arrByte, curIdx);
                curIdx += sizeof(bool);
            }



        }

        return arrByte;
    }

    public object ToDataByByte(Type type, byte[] arrByte)
    {
        //����ֽ��Ƿ� ������Ҫ�ġ�



        object obj = Activator.CreateInstance(type);
        FieldInfo[] fi = type.GetFields();

        int curIdx = 0;
        for (int i = 0; i < fi.Length; i++)
        {

            if (fi[i].FieldType == typeof(int))
            {

                int v = BitConverter.ToInt32(arrByte, curIdx);
                fi[i].SetValue(obj, v);

                curIdx += sizeof(int);


            }
            else if (fi[i].FieldType == typeof(long))
            {
                long vc = BitConverter.ToInt64(arrByte, curIdx);
                fi[i].SetValue(obj, vc);

                curIdx += sizeof(long);

            }
            else if (fi[i].FieldType == typeof(string))
            {

                int v = BitConverter.ToInt32(arrByte, curIdx);

                curIdx += sizeof(int);


                string str = Encoding.UTF8.GetString(arrByte, curIdx, v);
                fi[i].SetValue(obj, str);

                curIdx += Encoding.UTF8.GetByteCount(str);
            }
            else if (fi[i].FieldType == typeof(bool))
            {
                bool v2 = BitConverter.ToBoolean(arrByte, curIdx);
                fi[i].SetValue(obj, v2);

                curIdx += sizeof(bool);

            }



        }
        return obj;

    }
}



[Serializable]
public class Test : BaseData
{
    public Test ggg;
    public int a;
    public string b;
    public bool c;
    public long d;
}
//�������ݸ�ʽ 
public class LessonStep30 : MonoBehaviour
{
    // Start is called before the first frame update
    Test test;
    void Start()
    {
        test = new Test();
        test.a = 1;
        test.b = "�ǰ���1as";
        test.c = false;
        test.d = 555;
        //Lesson30_1();

        Lesson30_2();
    }
    public void Lesson30_1()
    {
        Debug.Log("�γ�30:���л� �Ľ��");



        byte[] arrbyte;
        //���л� 
        MemoryStream stream = new MemoryStream();

        BinaryFormatter bc = new BinaryFormatter();
        bc.Serialize(stream, test);
        arrbyte = stream.ToArray();

        stream.Close();

        //�����л� 
        MemoryStream stream2 = new MemoryStream(arrbyte);

        BinaryFormatter bc2 = new BinaryFormatter();

        var test2 = bc2.Deserialize(stream2) as Test;


        stream2.Close();

        Debug.Log(test2.a);
        Debug.Log(test2.b);
        Debug.Log(test2.c);
        Debug.Log(test2.d);

    }

    public void Lesson30_2()
    {
        var arr = test.ToByte<Test>();

        Test tt = new Test();
        var gg = tt.ToDataByByte(typeof(Test), arr) as Test;

        Debug.Log(gg.a);
        Debug.Log(gg.b);
        Debug.Log(gg.c);
        Debug.Log(gg.d);

    }
    // Update is called once per frame
    void Update()
    {

    }
}
