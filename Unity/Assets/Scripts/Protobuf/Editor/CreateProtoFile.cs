using NUnit.Framework.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class CreateProtoFile
{

    [MenuItem("Tool/����C# protobuf")]
    public static void CreateCsharp()
    {


        string fileDir = Application.dataPath.Replace("Assets", "CreateProto");
 

        var arrDirCsharp = Directory.GetFiles(fileDir, "*.proto");
        if (arrDirCsharp.Length <= 0) return;
        string csharpFileDir = Application.dataPath + "/Scripts/Protobuf/Csharp";

        string[] files = Directory.GetFiles(csharpFileDir);
        foreach (string file in files)
        {
            File.Delete(file);
        }

        string cmd = $"{fileDir}/protoc.exe -I={fileDir} --csharp_out={csharpFileDir} ";
        for (int i = 0; i < arrDirCsharp.Length; i++)
        {
            cmd += $" {arrDirCsharp[i]}";

        }
        cmd = cmd.Replace("\\", "/");

        var startInfo = new ProcessStartInfo()
        {
            FileName = "cmd.exe",
            Arguments = $"/c {cmd}", // /c �������� cmd.exe ִ���������ر�
            RedirectStandardOutput = true,
            RedirectStandardError = true,//������־
            UseShellExecute = false,//�������
            CreateNoWindow = true

        };
        using (Process process = new Process { StartInfo = startInfo })
        {

            //ִ������
            process.Start();



            // ��ȡ��׼���
            string output = process.StandardOutput.ReadToEnd();
            if (!string.IsNullOrEmpty(output))
            {
                UnityEngine.Debug.Log(output);
            }
            // ��ȡ�������
            string errorOutput = process.StandardError.ReadToEnd();
            if (!string.IsNullOrEmpty(errorOutput))
            {
                UnityEngine.Debug.LogError(errorOutput);
            }
            else
            {
                UnityEngine.Debug.Log("���C#��protobuf��");
            }

            // �ȴ������˳�
            process.WaitForExit();






        }

        AssetDatabase.Refresh();
 
    }
}