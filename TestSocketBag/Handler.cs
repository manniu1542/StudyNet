using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TestThreadServerClientSocket
{

    public class BaseData
    {
        ////枚举转 byte
        //private byte[] StructToByte<H>(H t) where H : struct
        //{


        //}
        //继承的类转byte  类对象 的值怎么 获取 
        public virtual int GetLen()
        {
            Type t = this.GetType();
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
                else
                {
                    Console.Write("漏掉序列化的属性：" + fi[i].Name);
                }



            }
            return len;
        }

        public virtual byte[] ToByte()
        {
            //获取所有子级


            Type t = this.GetType();

            int len = GetLen();
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

        public virtual void DataByByte(byte[] arrByte, int curIdx = 0)
        {
            //检查字节是否 是所需要的。


            Type type = this.GetType();

            FieldInfo[] fi = type.GetFields();

            for (int i = 0; i < fi.Length; i++)
            {

                if (fi[i].FieldType == typeof(int))
                {

                    int v = BitConverter.ToInt32(arrByte, curIdx);
                    fi[i].SetValue(this, v);

                    curIdx += sizeof(int);


                }
                else if (fi[i].FieldType == typeof(long))
                {
                    long vc = BitConverter.ToInt64(arrByte, curIdx);
                    fi[i].SetValue(this, vc);

                    curIdx += sizeof(long);

                }
                else if (fi[i].FieldType == typeof(string))
                {

                    int v = BitConverter.ToInt32(arrByte, curIdx);

                    curIdx += sizeof(int);


                    string str = Encoding.UTF8.GetString(arrByte, curIdx, v);
                    fi[i].SetValue(this, str);

                    curIdx += Encoding.UTF8.GetByteCount(str);
                }
                else if (fi[i].FieldType == typeof(bool))
                {
                    bool v2 = BitConverter.ToBoolean(arrByte, curIdx);
                    fi[i].SetValue(this, v2);

                    curIdx += sizeof(bool);

                }


            }


        }
    }

    public static class HandlerCode
    {
        public const int Handler_EnterRoom = 1000;
        public const int Handler_EnterRoom2 = 1002;


        public static int TypeToHandlerId(Type type)
        {
            if (type == typeof(PlayData))
            {
                return Handler_EnterRoom;
            }
            else if (type == typeof(PlayData))
            {

                return Handler_EnterRoom2;
            }
            else
            {
                return -1;
            }
        }

    }

    public class PlayData : BaseMsgData
    {
        public string name;
        public int age;
        public int id;
        public bool isMan;
    }
    public class BaseMsgData : BaseData
    {



        public override byte[] ToByte()
        {
            byte[] tmp = base.ToByte();
            int handlerId = HandlerCode.TypeToHandlerId(this.GetType());

            byte[] arrMsg = new byte[tmp.Length + 4];
            BitConverter.GetBytes(handlerId).CopyTo(arrMsg, 0);

            tmp.CopyTo(arrMsg, 4);

            return arrMsg;
        }




    }
}
