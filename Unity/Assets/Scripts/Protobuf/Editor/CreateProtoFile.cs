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

    [MenuItem("Tool/生成C# protobuf")]
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
            Arguments = $"/c {cmd}", // /c 参数告诉 cmd.exe 执行完命令后关闭
            RedirectStandardOutput = true,
            RedirectStandardError = true,//错误日志
            UseShellExecute = false,//允许输出
            CreateNoWindow = true

        };
        using (Process process = new Process { StartInfo = startInfo })
        {

            //执行命令
            process.Start();



            // 读取标准输出
            string output = process.StandardOutput.ReadToEnd();
            if (!string.IsNullOrEmpty(output))
            {
                UnityEngine.Debug.Log(output);
            }
            // 读取错误输出
            string errorOutput = process.StandardError.ReadToEnd();
            if (!string.IsNullOrEmpty(errorOutput))
            {
                UnityEngine.Debug.LogError(errorOutput);
            }
            else
            {
                UnityEngine.Debug.Log("完成C#的protobuf！");
            }

            // 等待进程退出
            process.WaitForExit();






        }

        AssetDatabase.Refresh();
 
    }
}