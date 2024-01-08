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

        string url = "http://192.168.0.104:8080/httpShare/";

        int timeout = 2000;

        private NetworkCredential Credentials = new NetworkCredential("zxw", "123456");
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
        /// <summary>
        /// 上传文件  (HFS这个软件 的资源服务器 如果 上传一次超过一定限制，就会上传失败!)
        /// </summary>
        /// <param name="needReqUri"></param>
        /// <param name="fun"></param>
        public async Task PostFile(string[] filePath, Action<HttpStatusCode> fun)
        {
            HttpStatusCode hscode = HttpStatusCode.OK; ;
            await Task.Run(() =>
            {
                try
                {
                    HttpWebRequest req = HttpWebRequest.Create(new Uri(url)) as HttpWebRequest;


                    req.Method = WebRequestMethods.Http.Post;
                    string boundary = "zhangxianwen";
                    req.ContentType = $"multipart/form-data;boundary={boundary}";
                    req.Timeout = timeout * 5;

                    //如果 服务器设置 了上传限制 （需要 设置使用凭证在链接 。设置凭证 账号密码）
                    req.PreAuthenticate = true;
                    req.Credentials = Credentials;
                   

                    Stream reqS = req.GetRequestStream();
                    for (int i = 0; i < filePath.Length; i++)
                    {
                        string fileName = Path.GetFileName(filePath[i]);
                        //name=\"file\";filename=\"{fileName}  不同的服务器 不一样的 表单 定义
                        string head = $"--{boundary}\r\nContent-Disposition: form-data;name=\"file\";filename=\"{fileName}\"\r\n";
                        head += "Content-Type:application/octet-stream \r\n";
                        byte[] arrhead = Encoding.UTF8.GetBytes(head);
                        string end = $"\r\n--{boundary}--\r\n\r\n";
                        byte[] arrend = Encoding.UTF8.GetBytes(end);


                        reqS.Write(arrhead, 0, arrhead.Length);
                        using (FileStream fs = File.Open(filePath[0], FileMode.Open))
                        {
                            byte[] arrTemp = new byte[1024];
                            int len = fs.Read(arrTemp, 0, arrTemp.Length);
                            while (len != 0)
                            {
                                reqS.Write(arrTemp, 0, len);
                                len = fs.Read(arrTemp, 0, arrTemp.Length);
                            }
                            fs.Close();

                        }

                        reqS.Write(arrend, 0, arrend.Length);



                    }
                    reqS.Close();

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


