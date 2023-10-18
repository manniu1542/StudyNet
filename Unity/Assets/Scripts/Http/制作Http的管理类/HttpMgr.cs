using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HttpStudy
{
    public class HttpMgr
    {
        public static readonly HttpMgr Instance = new HttpMgr();
        HttpMgr() { }

        string url = "http://192.168.0.104:8080/HttpServer/";

        int timeout = 2000;


        /// <summary>
        /// 下载 文件 
        /// </summary>
        /// <param name="farDownFileName"></param>
        /// <param name="loadFilePath"></param>
        /// <param name="fun"></param>
        public async Task DownLoadFile(string farDownFileName, string loadFilePath, Action<HttpStatusCode> fun)
        {
            HttpStatusCode hscode = HttpStatusCode.OK; ;
            await Task.Run(() =>
            {
                try
                {
                    HttpWebRequest req = HttpWebRequest.Create(new Uri(url + farDownFileName)) as HttpWebRequest;


                    req.Method = WebRequestMethods.Http.Get;

                    req.Timeout = timeout;


                    HttpWebResponse res = req.GetResponse() as HttpWebResponse;

                    if (res.StatusCode == HttpStatusCode.OK)
                    {
                        Stream resS = res.GetResponseStream();
                        byte[] arr = new byte[1024];
                        using (FileStream fs = File.Create(loadFilePath))
                        {

                            int len = resS.Read(arr, 0, arr.Length);
                            while (len != 0)
                            {
                                fs.Write(arr, 0, len);
                                len = resS.Read(arr, 0, arr.Length);
                            }
                            resS.Close();

                            fs.Close();

                        }

                        Debug.LogError($"文件类型{res.ContentType} , 文件大小{res.ContentLength}");

                        Debug.Log("下载文件完毕！");
                    }
                    else
                    {
                        hscode = res.StatusCode;
                        Debug.LogError(res.StatusDescription);
                    }
                    res.Close();

                }
                catch (Exception e)
                {
                    hscode = HttpStatusCode.NotAcceptable;
                    Debug.LogError(e);
                }
            });

            fun?.Invoke(hscode);

        }

        /// <summary>
        /// 查询 文件存不存在。
        /// </summary>
        /// <param name="needReqUri"></param>
        /// <param name="fun"></param>
        public async Task Head(string needReqUri, Action<HttpStatusCode> fun)
        {
            HttpStatusCode hscode = HttpStatusCode.OK; ;
            await Task.Run(() =>
            {
                try
                {
                    HttpWebRequest req = HttpWebRequest.Create(new Uri(url + needReqUri)) as HttpWebRequest;


                    req.Method = WebRequestMethods.Http.Head;

                    req.Timeout = timeout;


                    HttpWebResponse res = req.GetResponse() as HttpWebResponse;


                    hscode = res.StatusCode;
                    res.Close();

                }
                catch (Exception e)
                {
                    hscode = HttpStatusCode.NotAcceptable;
                    Debug.LogError(e);
                }

            });
            fun?.Invoke(hscode);

        }
        /// <summary>
        /// 查询 文件存不存在。
        /// </summary>
        /// <param name="needReqUri"></param>
        /// <param name="fun"></param>
        public async Task Post(string updataXwww, Action<HttpStatusCode> fun)
        {
            HttpStatusCode hscode = HttpStatusCode.OK; ;
            await Task.Run(() =>
            {
                try
                {
                    HttpWebRequest req = HttpWebRequest.Create(new Uri(url)) as HttpWebRequest;


                    req.Method = WebRequestMethods.Http.Post;

                    req.Timeout = timeout;
                    req.ContentType = "application/x-www-form-urlencoded";
                    byte[] arr = Encoding.UTF8.GetBytes(updataXwww);
                    req.ContentLength = arr.Length;

                    Stream s = req.GetRequestStream();
                    s.Write(arr, 0, arr.Length);
                    s.Close();
   

                    HttpWebResponse res = req.GetResponse() as HttpWebResponse;


                    hscode = res.StatusCode;
                    res.Close();

                }
                catch (Exception e)
                {
                    hscode = HttpStatusCode.NotAcceptable;
                    Debug.LogError(e);
                }

            });
            fun?.Invoke(hscode);

        }

    }


}


