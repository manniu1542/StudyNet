using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;



public class GenerateCSharp
{
    public string GeneratePath = Application.dataPath + "/Scripts/ToolCreateProtocalFile/";

    public void GenerateEnum(XmlNodeList xmlNodes)
    {


        foreach (XmlNode enumTmp in xmlNodes)
        {
            string enumName = enumTmp.Attributes["name"].Value;
            string namespaceName = enumTmp.Attributes["namespace"].Value;

            string strEnumFiled = "";
            var list = enumTmp.SelectNodes("field");
            foreach (XmlNode item in list)
            {
                strEnumFiled += "\t\t" + item.Attributes["name"].Value + " = " + (string.IsNullOrEmpty(item.InnerText) ? "0" : item.InnerText) + ",\r\n";

            }
            string content = "namespace " + namespaceName + "\r\n";
            content += "{\r\n";
            content += "\tpublic enum " + enumName + "\r\n";
            content += "\t{\r\n";
            content += strEnumFiled;
            content += "\t}\r\n";
            content += "}\r\n";
            string savePath = GeneratePath + "Protocol/" + namespaceName;
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);

            }
            File.WriteAllText(savePath + $"/{enumName}.cs", content);


        }
   

    }

    public void GenerateData(XmlNodeList xmlNodes)
    {


        foreach (XmlNode enumTmp in xmlNodes)
        {
            string enumName = enumTmp.Attributes["name"].Value;
            string namespaceName = enumTmp.Attributes["namespace"].Value;

            string strEnumFiled = "";
            var list = enumTmp.SelectNodes("field");
            foreach (XmlNode item in list)
            {
                strEnumFiled += "\t\t" + item.Attributes["type"].Value + " " + item.Attributes["name"].Value + ";\r\n";
            }
            string content = "namespace " + namespaceName + "\r\n";
            content += "{\r\n";
            content += "\tpublic class " + enumName + ":BaseData\r\n";
            content += "\t{\r\n";
            content += strEnumFiled;
            content += "\t}\r\n";
            content += "}\r\n";
            string savePath = GeneratePath + "Protocol/" + namespaceName;
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);

            }
            File.WriteAllText(savePath + $"/{enumName}.cs", content);


        }
   

    }
    public void GenerateMessage(XmlNodeList xmlNodes)
    {


        foreach (XmlNode enumTmp in xmlNodes)
        {
            string enumName = enumTmp.Attributes["name"].Value;
            string namespaceName = enumTmp.Attributes["namespace"].Value;
            string msgId = enumTmp.Attributes["id"].Value;
            string strEnumFiled = "";
            var list = enumTmp.SelectNodes("field");
            foreach (XmlNode item in list)
            {
                strEnumFiled += "\t\t" + item.Attributes["type"].Value + " " + item.Attributes["name"].Value + ";\r\n";
            }
            string content = "namespace " + namespaceName + "\r\n";
            content += "{\r\n";
            content += "\tpublic class " + enumName + ":BaseMsgData\r\n";
            content += "\t{\r\n";
            content += strEnumFiled;
            content += "\tpublic override int GetID()\r\n\t{\r\n \t\t return " + msgId + ";\r\n\t}";
            content += "\t}\r\n";
            content += "}\r\n";
            string savePath = GeneratePath + "Protocol/" + namespaceName;
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);

            }
            File.WriteAllText(savePath + $"/{enumName}.cs", content);


        }
  

    }
}
