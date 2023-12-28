using StepTcpFinishAsync;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class ToolCreateProtocal : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //选择哪种 格式（配置语言 xml， json，unity中的scriptObject，excel） 配置协议 



        //指定配置规则 （目的 ，获取 类名，字段类型）




        //读取配置信息


        ReadXml();

    }

    void ReadXml()
    {
        XmlDocument xml = new XmlDocument();
        xml.Load(Application.dataPath + "/Scripts/ToolCreateProtocalFile/temp.xml");

        var root = xml.SelectSingleNode("messages");


        var enumList = root.SelectNodes("enum");
        foreach (XmlNode enumTmp in enumList)
        {
            print("枚举名字：" + enumTmp.Attributes["name"].Value);
            print("枚举命名空间：" + enumTmp.Attributes["namespace"].Value);
            string strEnumFiled = "";
            var list = enumTmp.SelectNodes("field");
            foreach (XmlNode item in list)
            {
                strEnumFiled += item.Attributes["name"].Value + "=" + (string.IsNullOrEmpty(item.InnerText) ? "0" : item.InnerText) + "\n";

            }
            print("枚举 元素" + strEnumFiled);
        }
        //
        var dataList = root.SelectNodes("data");
        foreach (XmlNode enumTmp in dataList)
        {
            print("数据结构的名字：" + enumTmp.Attributes["name"].Value);
            print("数据结构的空间：" + enumTmp.Attributes["namespace"].Value);
            string strEnumFiled = "";
            var list = enumTmp.SelectNodes("field");
            foreach (XmlNode item in list)
            {
                strEnumFiled += item.Attributes["type"].Value + "  " + item.Attributes["name"].Value + "\n";

            }
            print("数据结构的 字段" + strEnumFiled);
        }


        var messageList = root.SelectNodes("message");
        foreach (XmlNode enumTmp in messageList)
        {
            print("消息的名字：" + enumTmp.Attributes["name"].Value);
            print("消息的空间：" + enumTmp.Attributes["namespace"].Value);
            print("消息的ID：" + enumTmp.Attributes["id"].Value);

            string strEnumFiled = "";
            var list = enumTmp.SelectNodes("field");
            foreach (XmlNode item in list)
            {
                strEnumFiled += item.Attributes["type"].Value + "\n";

            }
            print("消息的 字段" + strEnumFiled);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
