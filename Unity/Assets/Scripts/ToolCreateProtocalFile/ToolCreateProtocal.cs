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
        //ѡ������ ��ʽ���������� xml�� json��unity�е�scriptObject��excel�� ����Э�� 



        //ָ�����ù��� ��Ŀ�� ����ȡ �������ֶ����ͣ�




        //��ȡ������Ϣ


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
            print("ö�����֣�" + enumTmp.Attributes["name"].Value);
            print("ö�������ռ䣺" + enumTmp.Attributes["namespace"].Value);
            string strEnumFiled = "";
            var list = enumTmp.SelectNodes("field");
            foreach (XmlNode item in list)
            {
                strEnumFiled += item.Attributes["name"].Value + "=" + (string.IsNullOrEmpty(item.InnerText) ? "0" : item.InnerText) + "\n";

            }
            print("ö�� Ԫ��" + strEnumFiled);
        }
        //
        var dataList = root.SelectNodes("data");
        foreach (XmlNode enumTmp in dataList)
        {
            print("���ݽṹ�����֣�" + enumTmp.Attributes["name"].Value);
            print("���ݽṹ�Ŀռ䣺" + enumTmp.Attributes["namespace"].Value);
            string strEnumFiled = "";
            var list = enumTmp.SelectNodes("field");
            foreach (XmlNode item in list)
            {
                strEnumFiled += item.Attributes["type"].Value + "  " + item.Attributes["name"].Value + "\n";

            }
            print("���ݽṹ�� �ֶ�" + strEnumFiled);
        }


        var messageList = root.SelectNodes("message");
        foreach (XmlNode enumTmp in messageList)
        {
            print("��Ϣ�����֣�" + enumTmp.Attributes["name"].Value);
            print("��Ϣ�Ŀռ䣺" + enumTmp.Attributes["namespace"].Value);
            print("��Ϣ��ID��" + enumTmp.Attributes["id"].Value);

            string strEnumFiled = "";
            var list = enumTmp.SelectNodes("field");
            foreach (XmlNode item in list)
            {
                strEnumFiled += item.Attributes["type"].Value + "\n";

            }
            print("��Ϣ�� �ֶ�" + strEnumFiled);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
