using HttpStudy;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;

public class StudyKnowHttp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        //HttpMgr.Instance.Head("66.txt", (state) =>
        //{
        //     Debug.Log($"�ı��Ƿ���ڵ���� ��{state}");
        //}).ContinueWith((a) =>
        //{

        //});
        //HttpMgr.Instance.DownLoadFile("66.txt", "C:\\Users\\Administrator\\Desktop\\77.txt", (state) =>
        //{
        //    Debug.Log($"������� ��{state}");
        //}).ContinueWith((a) =>
        //{

        //});
        //TestHead();


        HttpMgr.Instance.Post("hello world", (state) =>
        {
            Debug.Log($"�ϴ���� ��{state}");
        }).ContinueWith((a) =>
        {

        });

    }

    void TestHead()
    {
        //http   Get �����ļ� ���� 
        try
        {
            HttpWebRequest req = HttpWebRequest.Create(new Uri("http://192.168.0.104:8080/HttpServer/66.txt")) as HttpWebRequest;


            req.Method = WebRequestMethods.Http.Head;

            req.Timeout = 2000;


            HttpWebResponse res = req.GetResponse() as HttpWebResponse;

            if (res.StatusCode == HttpStatusCode.OK)
            {

                Debug.LogError($"�ļ�����{res.ContentType} , �ļ���С{res.ContentLength}");
                Debug.Log("�ļ����ڣ�");
            }
            else
            {
                Debug.LogError(res.StatusDescription);
            }


        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }

    }
    void TestGet()
    {
        //http   Get �����ļ� ���� 
        try
        {
            HttpWebRequest req = HttpWebRequest.Create(new Uri("http://192.168.0.104:8080/HttpServer/66.txt")) as HttpWebRequest;


            req.Method = WebRequestMethods.Http.Get;

            req.Timeout = 2000;


            HttpWebResponse res = req.GetResponse() as HttpWebResponse;

            if (res.StatusCode == HttpStatusCode.OK)
            {
                Stream resS = res.GetResponseStream();

                using (FileStream fs = File.Create("C:\\Users\\Administrator\\Desktop\\77.txt"))
                {
                    byte[] arr = new byte[1024];
                    int len = resS.Read(arr, 0, arr.Length);
                    while (len != 0)
                    {
                        fs.Write(arr, 0, len);
                        len = resS.Read(arr, 0, arr.Length);
                    }
                    resS.Close();
                    res.Close();
                    fs.Close();

                }

                Debug.LogError($"�ļ�����{res.ContentType} , �ļ���С{res.ContentLength}");

                Debug.Log("�����ļ���ϣ�");
            }
            else
            {
                Debug.LogError(res.StatusDescription);
            }


        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
