using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace UnityNet.UnityWebRequset
{


    class UnityWebRequsetMgr
    {
        const int TimeOut = 2000;
        // 请求txt 文件
        public static void GetTxtFile(string url, UnityAction<string, byte[]> fun)
        {
            UnityWebRequest req = UnityWebRequest.Get(url);

            var res = req.SendWebRequest();

            res.completed += (asy) =>
            {
                if (req.result == UnityWebRequest.Result.Success)
                {

                    fun(string.Empty, req.downloadHandler.data);
                }
                else
                {

                    fun("错误信息：" + req.error + " ,result" + req.result + " ,responseCode" + req.responseCode, null);
                }

            };

        }
        // 请求txt 文件
        public static void GetImgFile(string url, UnityAction<string, Texture2D> fun)
        {
            UnityWebRequest req = UnityWebRequestTexture.GetTexture(url);

            var res = req.SendWebRequest();

            res.completed += (asy) =>
            {
                if (req.result == UnityWebRequest.Result.Success)
                {

                    fun(string.Empty, (req.downloadHandler as DownloadHandlerTexture).texture);
                }
                else
                {

                    fun("错误信息：" + req.error + " ,result" + req.result + " ,responseCode" + req.responseCode, null);
                }

            };

        }
        // 请求ab 文件

        public static void GetAssetBundleFile(string url, UnityAction<string, AssetBundle> fun)
        {
            UnityWebRequest req = UnityWebRequestAssetBundle.GetAssetBundle(url);

            var res = req.SendWebRequest();

            res.completed += (asy) =>
            {
                if (req.result == UnityWebRequest.Result.Success)
                {
                    fun(string.Empty, (req.downloadHandler as DownloadHandlerAssetBundle).assetBundle);
                }
                else
                {

                    fun("错误信息：" + req.error + " ,result" + req.result + " ,responseCode" + req.responseCode, null);
                }

            };

        }


        /// <summary>
        /// 获取 下载 信息 支持类型 byte[] ，File，AudioClip，Texture2D，AssetBundle(crc =0默认不检查，有值得话就可以检查 ab包的完整性)，自定义下载类型 BaseMsgData
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void GetDownLoadInfo<T>(MonoBehaviour monoB,string url, UnityAction<T> complateFun = null, UnityAction<ulong, ulong> progressFun = null, string downLoadLoaclPath = "", AudioType audioType = AudioType.WAV, uint abCRC = 0, Type baseMsgDataT = null) where T : class
        {
        
            IEnumerator ietor = null;
            if (typeof(T) == typeof(byte[]))
            {
                DownloadHandlerBuffer downloadHandlerBuffer = new DownloadHandlerBuffer();
                ietor = GetFile(url, downloadHandlerBuffer, (err) =>
                {
                    complateFun?.Invoke(string.IsNullOrEmpty(err) ? downloadHandlerBuffer.data as T : null);
                }, progressFun);
            }
            else if (typeof(T) == typeof(FileStream))
            {
                DownloadHandlerFile downloadHandlerFile = new DownloadHandlerFile(downLoadLoaclPath);
                ietor = GetFile(url, downloadHandlerFile, (err) =>
                {
                    complateFun?.Invoke(string.IsNullOrEmpty(err) ? File.OpenRead(downLoadLoaclPath) as T : null);
                }, progressFun);
            }
            else if (typeof(T) == typeof(AudioClip))
            {
                DownloadHandlerAudioClip downloadHandlerBuffer = new DownloadHandlerAudioClip(url, audioType);
                ietor = GetFile(url, downloadHandlerBuffer, (err) =>
                {
                    complateFun?.Invoke(string.IsNullOrEmpty(err) ? downloadHandlerBuffer.audioClip as T : null);
                }, progressFun);
            }
            else if (typeof(T) == typeof(Texture2D))
            {
                DownloadHandlerTexture downloadHandlerBuffer = new DownloadHandlerTexture();
                ietor = GetFile(url, downloadHandlerBuffer, (err) =>
                {
                    complateFun?.Invoke(string.IsNullOrEmpty(err) ? downloadHandlerBuffer.texture as T : null);
                }, progressFun);
            }
            else if (typeof(T) == typeof(AssetBundle))
            {
                DownloadHandlerAssetBundle downloadHandlerBuffer = new DownloadHandlerAssetBundle(url, abCRC);
                ietor = GetFile(url, downloadHandlerBuffer, (err) =>
                {
                    complateFun?.Invoke(string.IsNullOrEmpty(err) ? downloadHandlerBuffer.assetBundle as T : null);
                }, progressFun);
            }
            else if (typeof(T) == typeof(BaseMsgData))
            {
                DownloadHandlerCustomMsg downloadHandlerBuffer = new DownloadHandlerCustomMsg();
                ietor = GetFile(url, downloadHandlerBuffer, (err) =>
                {
                    complateFun?.Invoke(string.IsNullOrEmpty(err) ? downloadHandlerBuffer.GetMsgData(baseMsgDataT) as T : null);
                }, progressFun);
            }
            else
            {
                Debug.LogError("找不到该类型：" + typeof(T));
            }
            if (ietor !=null)
            {
                monoB.StartCoroutine(ietor);
           
            }
        }

        public static IEnumerator GetFile<T>(string url, T download, UnityAction<string> complateFun, UnityAction<ulong, ulong> progressFun = null) where T : DownloadHandler
        {
       
            UnityWebRequest req = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET);
            req.downloadHandler = download;
            req.timeout = TimeOut;
            var res = req.SendWebRequest();
            ulong allLoadedBytes = 0;

            while (!res.isDone)
            {
                progressFun?.Invoke(req.downloadedBytes, allLoadedBytes);
                if (allLoadedBytes == 0 && req.downloadedBytes != 0)
                {
                    allLoadedBytes = ulong.Parse(req.GetResponseHeader("Content-Length"));
                }
                yield return null;
            }
            if (allLoadedBytes == 0)
            {
                allLoadedBytes = ulong.Parse(req.GetResponseHeader("Content-Length"));
            }
            if (!url.StartsWith("file://"))//本地下载会调用两次 完成！
            {
                progressFun?.Invoke(req.downloadedBytes, allLoadedBytes);
            }

            if (req.result == UnityWebRequest.Result.Success)
            {

                complateFun(string.Empty);

            }
            else
            {
                complateFun("错误信息：" + req.error + " ,result" + req.result + " ,responseCode" + req.responseCode);

            }





        }







        //上传多个任意 文件
        public static void PostFile(List<IMultipartFormSection> listMFSection, string url, UnityAction<string> fun)
        {
            UnityWebRequest req = UnityWebRequest.Post(url, listMFSection);

            var res = req.SendWebRequest();

            res.completed += (asy) =>
            {
                if (req.result == UnityWebRequest.Result.Success)
                {
                    fun(string.Empty);
                }
                else
                {

                    fun("错误信息：" + req.error + " ,result" + req.result + " ,responseCode" + req.responseCode);
                }

            };



        }

        public static void UploadFile(string severFileName, string localPath, UnityAction<string> fun)
        {

            List<IMultipartFormSection> sections = new List<IMultipartFormSection>();
            sections.Add(new MultipartFormFileSection("key", File.ReadAllBytes(localPath), severFileName, "application/octet-stream"));
            UnityWebRequsetMgr.PostFile(sections, "http://192.168.0.104:8080/HttpServer/", fun);

        }
        public static void UploadFile(string localFilePath, string fileContenType, string url, UnityAction<string> fun)
        {
            UnityWebRequest req = new UnityWebRequest(url);

            //UploadHandlerFile  只不过就是 把 所需的文件类型  转成byte[]数据  ,雷系   UploadHandlerRaw  
            var uh = new UploadHandlerFile(localFilePath);
            //    类型/细分类型
            uh.contentType = fileContenType;

            req.uploadHandler = uh;

            var res = req.SendWebRequest();
            res.completed += (ray) =>
            {
                if (req.result == UnityWebRequest.Result.Success)
                {
                    fun(string.Empty);
                }
                else
                {
                    fun("错误信息：" + req.error + " ,result" + req.result + " ,responseCode" + req.responseCode);
                }


            };

        }

        public static void UploadData(byte[] data, string url, UnityAction<string> fun)
        {
            UnityWebRequest req = new UnityWebRequest(url);
            var uh = new UploadHandlerRaw(data);
            //    类型/细分类型
            uh.contentType = "application/octet-stream";//默认类型  
            req.uploadHandler = uh;

            var res = req.SendWebRequest();
            res.completed += (ray) =>
            {
                if (req.result == UnityWebRequest.Result.Success)
                {
                    fun(string.Empty);
                }
                else
                {
                    fun("错误信息：" + req.error + " ,result" + req.result + " ,responseCode" + req.responseCode);
                }


            };

        }

    }
}
