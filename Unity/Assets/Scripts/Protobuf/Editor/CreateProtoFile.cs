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

        //获取proto 文件的 目录。
        string fileDir = Application.dataPath.Replace("Assets", "CreateProto");
        //获取所有proto 文件
        string[] protoFiles = Directory.GetFiles(fileDir, "*.proto");
        //生成C#的 脚本 文件目录
        string csharpFileDir = Application.dataPath + "/Scripts/Protobuf";

        if (protoFiles.Length > 0)
        {
            UnityEngine.Debug.LogError($"该目录{fileDir}下没有proto文件生成！");
            return;
        }
        //命令行 
        string cmd = $"{fileDir}/protoc.exe -I={fileDir} --csharp_out={csharpFileDir}";
        foreach (var item in protoFiles)
        {
            cmd += $" {fileDir}/{item}.proto";
        }


        var startInfo = new ProcessStartInfo()
        {
            FileName = "cmd.exe",
            Arguments = $"/c {cmd}", // /c 参数告诉 cmd.exe 执行完命令后关闭
            RedirectStandardOutput = true,// 获取窗口一般输出
            RedirectStandardError = true, // 获取窗口错误输出
            UseShellExecute = false,// 可以 获取输出log
            CreateNoWindow = true//没有窗口

        };
        bool isFinish = true;
        using (Process process = new Process { StartInfo = startInfo })
        {

            //执行命令
            process.Start();

            // 异步处理标准输出
            process.OutputDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    UnityEngine.Debug.Log($"Output: {e.Data}");
                }
            };

            // 异步处理标准错误
            process.ErrorDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    isFinish = false;
                    UnityEngine.Debug.LogError($"Error: {e.Data}");
                }
            };

            process.BeginOutputReadLine(); // 开始异步读取标准输出
            process.BeginErrorReadLine();  // 开始异步读取标准错误

            process.WaitForExit();


        }

        AssetDatabase.Refresh();
        if (isFinish)
            UnityEngine.Debug.Log($"protobuf构建完成！");
    }
}