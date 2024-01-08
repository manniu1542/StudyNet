using Google.Protobuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class ProtobufHelp
{
    // Start is called before the first frame update
    //���л����ֽ�ת ��
    public static T SerProto<T>(this T self,  byte[] arr) where T :class,  IMessage<T>, new()
    {
        //MessageParser<T> msgp = new MessageParser<T>(factory);
      
        

        //return msgp.ParseFrom(arr); ;

        if(self ==null)
            self = new T();

        self.MergeFrom(arr);
        return self;
    }

    //��ת�ֽ�
    public static void UnSerProto(IMessage msg, out byte[] arr)
    {
        //using (MemoryStream fs = new MemoryStream())
        //{
        //    msg.WriteTo(fs);
        //    arr= fs.ToArray();
          
        //}
        arr = msg.ToByteArray();
    }
}
