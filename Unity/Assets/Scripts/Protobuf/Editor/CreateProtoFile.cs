using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

public static class CreateProtoFile
{

    [MenuItem("Tool/生成C# protobuf")]
    public static void CreateCsharp()
    {


        string fileDir = Application.dataPath.Replace("Assets", "CreateProto");
        string fileName = "testEnum";
        string csharpFileDir = Application.dataPath + "/Scripts/Protobuf";
        string cmd = $"{fileDir}/protoc.exe -I={fileDir} --csharp_out={csharpFileDir}  {fileDir}/{fileName}.proto";

        var startInfo = new ProcessStartInfo()
        {
            FileName = "cmd.exe",
            Arguments = $"/c {cmd}", // /c 参数告诉 cmd.exe 执行完命令后关闭
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true

        };
        using (Process process = new Process { StartInfo = startInfo })
        {

            //执行命令
            process.Start();

            // 等待进程退出
            process.WaitForExit();

            // 读取标准输出
            string output = process.StandardOutput.ReadToEnd();


            UnityEngine.Debug.Log(cmd);

            UnityEngine.Debug.Log(output);


        }



    }
}