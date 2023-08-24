using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace StepTcpFinishAsync
{


    public abstract class BaseData
    {
      
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
                else if (fi[i].FieldType == typeof(short))
                {
                    len += sizeof(short);
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
                    BaseData bd = fi[i].GetValue(this) as BaseData;
                    if (bd == null)
                        bd = Activator.CreateInstance(fi[i].FieldType) as BaseData;

                    len += bd.GetLen();
                }
                else
                {
                    Debug.LogError("漏掉序列化的属性：" + fi[i].Name);
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
                else if (fi[i].FieldType == typeof(short))
                {
                    BitConverter.GetBytes((short)o).CopyTo(arrByte, curIdx);
                    curIdx += sizeof(short);
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
                else if (fi[i].FieldType.IsSubclassOf(typeof(BaseData)))
                {
                    BaseData bd = fi[i].GetValue(this) as BaseData;
                    if (bd == null)
                        bd = Activator.CreateInstance(fi[i].FieldType) as BaseData;

                    bd.ToByte().CopyTo(arrByte, curIdx);
                    curIdx += bd.GetLen();

                }


            }

            return arrByte;
        }

        public virtual int DataByByte(byte[] arrByte, int curIdx = 0)
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
                else if (fi[i].FieldType == typeof(short))
                {
                    short vc = BitConverter.ToInt16(arrByte, curIdx);
                    fi[i].SetValue(this, vc);

                    curIdx += sizeof(short);
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
                else if (fi[i].FieldType.IsSubclassOf(typeof(BaseData)))
                {
                    BaseData bd = fi[i].GetValue(this) as BaseData;
                    if (bd == null)
                    {
                        bd = Activator.CreateInstance(fi[i].FieldType) as BaseData;
                        fi[i].SetValue(this, bd);
                    }

                    curIdx = bd.DataByByte(arrByte, curIdx);

                }


            }
            return curIdx;

        }

        public override string ToString()
        {

            return JsonUtility.ToJson(this); ;
        }

    }

}
