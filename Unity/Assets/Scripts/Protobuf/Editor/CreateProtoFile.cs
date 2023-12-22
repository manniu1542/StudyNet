using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

public static class CreateProtoFile
{

    [MenuItem("Tool/����C# protobuf")]
    public static void CreateCsharp()
    {


        string fileDir = Application.dataPath.Replace("Assets", "CreateProto");
        string fileName = "testEnum";
        string csharpFileDir = Application.dataPath + "/Scripts/Protobuf";
        string cmd = $"{fileDir}/protoc.exe -I={fileDir} --csharp_out={csharpFileDir}  {fileDir}/{fileName}.proto";

        var startInfo = new ProcessStartInfo()
        {
            FileName = "cmd.exe",
            Arguments = $"/c {cmd}", // /c �������� cmd.exe ִ���������ر�
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true

        };
        using (Process process = new Process { StartInfo = startInfo })
        {

            //ִ������
            process.Start();

            // �ȴ������˳�
            process.WaitForExit();

            // ��ȡ��׼���
            string output = process.StandardOutput.ReadToEnd();


            UnityEngine.Debug.Log(cmd);

            UnityEngine.Debug.Log(output);


        }



    }
}