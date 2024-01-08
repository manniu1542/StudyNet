using Google.Protobuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class ProtobufHelp
{
    // Start is called before the first frame update
    //序列化，字节转 类
    public static T SerProto<T>(this T self,  byte[] arr) where T :class,  IMessage<T>, new()
    {
        //MessageParser<T> msgp = new MessageParser<T>(factory);
      
        

        //return msgp.ParseFrom(arr); ;

        if(self ==null)
            self = new T();

        self.MergeFrom(arr);
        return self;
    }

    //类转字节
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
