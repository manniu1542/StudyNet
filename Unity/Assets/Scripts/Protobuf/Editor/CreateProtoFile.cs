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

        //��ȡproto �ļ��� Ŀ¼��
        string fileDir = Application.dataPath.Replace("Assets", "CreateProto");
        //��ȡ����proto �ļ�
        string[] protoFiles = Directory.GetFiles(fileDir, "*.proto");
        //����C#�� �ű� �ļ�Ŀ¼
        string csharpFileDir = Application.dataPath + "/Scripts/Protobuf";

        if (protoFiles.Length > 0)
        {
            UnityEngine.Debug.LogError($"��Ŀ¼{fileDir}��û��proto�ļ����ɣ�");
            return;
        }
        //������ 
        string cmd = $"{fileDir}/protoc.exe -I={fileDir} --csharp_out={csharpFileDir}";
        foreach (var item in protoFiles)
        {
            cmd += $" {fileDir}/{item}.proto";
        }


        var startInfo = new ProcessStartInfo()
        {
            FileName = "cmd.exe",
            Arguments = $"/c {cmd}", // /c �������� cmd.exe ִ���������ر�
            RedirectStandardOutput = true,// ��ȡ����һ�����
            RedirectStandardError = true, // ��ȡ���ڴ������
            UseShellExecute = false,// ���� ��ȡ���log
            CreateNoWindow = true//û�д���

        };
        bool isFinish = true;
        using (Process process = new Process { StartInfo = startInfo })
        {

            //ִ������
            process.Start();

            // �첽�����׼���
            process.OutputDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    UnityEngine.Debug.Log($"Output: {e.Data}");
                }
            };

            // �첽�����׼����
            process.ErrorDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    isFinish = false;
                    UnityEngine.Debug.LogError($"Error: {e.Data}");
                }
            };

            process.BeginOutputReadLine(); // ��ʼ�첽��ȡ��׼���
            process.BeginErrorReadLine();  // ��ʼ�첽��ȡ��׼����

            process.WaitForExit();


        }

        AssetDatabase.Refresh();
        if (isFinish)
            UnityEngine.Debug.Log($"protobuf������ɣ�");
    }
}