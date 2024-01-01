using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEditor;
using UnityEngine;

public class CreateProtocol
{
    private static string ProtocolPath = Application.dataPath + "/Scripts/ToolCreateProtocalFile/temp.xml";
    private static XmlNode root;
    public static void SetXmlRoot()
    {
        XmlDocument xml = new XmlDocument();
        xml.Load(ProtocolPath);

        root = xml.SelectSingleNode("messages");

    }

    static GenerateCSharp scrGenerateCSharp = new GenerateCSharp();
    [MenuItem("Tool/GenerateCSharpSelf")]
    public static void GenerateCSharp()
    {

        SetXmlRoot();

        scrGenerateCSharp.GenerateEnum(root.SelectNodes("enum"));
        scrGenerateCSharp.GenerateData(root.SelectNodes("data"));
        scrGenerateCSharp.GenerateMessage(root.SelectNodes("message"));
        AssetDatabase.Refresh();

    }
    [MenuItem("GenerateJava")]
    public void GenerateJava()
    {


    }

}
