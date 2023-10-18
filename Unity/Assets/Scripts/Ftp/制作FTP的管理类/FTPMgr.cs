using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class FTPMgr
{

    public static readonly FTPMgr Instance = new FTPMgr();

    private FTPMgr() { }


    private string ftpUrl = "ftp://127.0.0.1";
    private NetworkCredential Credentials = new NetworkCredential("serverZxw", "123456");
    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="farFileNameWithPostfix"></param>
    /// <param name="copyFilePath"></param>
    /// <param name="cbUploadFinish"></param>
    /// <returns></returns>
    public async Task UploadFile(string farFileNameWithPostfix, string copyFilePath, UnityAction cbUploadFinish = null)
    {
        await Task.Run(() =>
        {
            try
            {


                FtpWebRequest req = FtpWebRequest.Create(new Uri(ftpUrl + "/" + farFileNameWithPostfix)) as FtpWebRequest;


                req.Credentials = Credentials;

                req.KeepAlive = false;
                req.Method = WebRequestMethods.Ftp.UploadFile;
                req.UseBinary = true;
                req.Proxy = null;

                using (FileStream fileStream = File.Open(copyFilePath, FileMode.Open))
                {
                    var arr = new byte[1024];
                    Stream reqS = req.GetRequestStream();
                    int len = fileStream.Read(arr, 0, arr.Length);
                    while (len > 0)
                    {
                        reqS.Write(arr, 0, len);
                        len = fileStream.Read(arr, 0, arr.Length);
                    }

                    reqS.Close();
                    fileStream.Close();

                }
                cbUploadFinish?.Invoke();





            }
            catch (Exception e)
            {

                Debug.LogError(e);
            }
        });


    }
    /// <summary>
    /// 下载文件
    /// </summary>
    /// <param name="farFileNameWithPostfix"></param>
    /// <param name="downFilePath"></param>
    /// <param name="cbDownloadFinish"></param>
    /// <returns></returns>
    public async Task DownloadFile(string farFileNameWithPostfix, string downFilePath, UnityAction cbDownloadFinish = null)
    {
        await Task.Run(() =>
        {
            try
            {

                FtpWebRequest req = FtpWebRequest.Create(new Uri(ftpUrl + "/" + farFileNameWithPostfix)) as FtpWebRequest;

                req.Credentials = Credentials;
                req.KeepAlive = false;
                req.UseBinary = true;
                req.Proxy = null;
                req.Method = WebRequestMethods.Ftp.DownloadFile;

                FtpWebResponse res = req.GetResponse() as FtpWebResponse;
                var resS = res.GetResponseStream();

                using (FileStream s = File.Create(downFilePath))
                {
                    var arr = new byte[1024];
                    int len = resS.Read(arr, 0, arr.Length);
                    while (len != 0)
                    {
                        s.Write(arr, 0, len);
                        len = resS.Read(arr, 0, arr.Length);
                    }

                    s.Close();
                    resS.Close();
                }

                res.Close();
                cbDownloadFinish?.Invoke();

            }
            catch (Exception e)
            {

                Debug.LogError(e);
            }
        });

    }


    /// <summary>
    /// 删除文件
    /// </summary>
    /// <param name="downFilePath"></param>
    public async Task DeleteFile(string farFileNameWithPostfix, UnityAction cbDownloadFinish = null)
    {
        await Task.Run(() =>
        {
            try
            {

                FtpWebRequest ftp = FtpWebRequest.Create(new Uri(ftpUrl + "/" + farFileNameWithPostfix)) as FtpWebRequest;
                ftp.Credentials = Credentials;
                ftp.KeepAlive = false;
                ftp.Method = WebRequestMethods.Ftp.DeleteFile;
                ftp.Proxy = null;
                FtpWebResponse ftpRes = ftp.GetResponse() as FtpWebResponse;
                ftpRes.Close();

                cbDownloadFinish?.Invoke();

            }
            catch (Exception e)
            {

                Debug.LogError(e);
            }
        });

    }

    /// <summary>
    /// 获取文件大小
    /// </summary>
    /// <param name="downFilePath"></param>
    public async Task GetFileSize(string farFileNameWithPostfix, UnityAction<long> cbDownloadFinish = null)
    {
        await Task.Run(() =>
        {
            try
            {
                FtpWebRequest ftp = FtpWebRequest.Create(new Uri(ftpUrl + "/" + farFileNameWithPostfix)) as FtpWebRequest;
                ftp.Credentials = Credentials;
                ftp.KeepAlive = false;
                ftp.Method = WebRequestMethods.Ftp.GetFileSize;
                ftp.Proxy = null;
                FtpWebResponse ftpRes = ftp.GetResponse() as FtpWebResponse;
                cbDownloadFinish?.Invoke(ftpRes.ContentLength);

                ftpRes.Close();


            }
            catch (Exception e)
            {

                Debug.LogError(e);
            }

        });
    }
    /// <summary>
    /// 生成文件夹
    /// </summary>
    /// <param name="downFilePath"></param>
    public async Task CreateFileDir(string farDirName, UnityAction<bool> cbDownloadFinish = null)
    {
        await Task.Run(() =>
        {
            try
            {
                FtpWebRequest ftp = FtpWebRequest.Create(new Uri(ftpUrl + "/" + farDirName)) as FtpWebRequest;
                ftp.Credentials = Credentials;
                ftp.KeepAlive = false;
                ftp.Method = WebRequestMethods.Ftp.MakeDirectory;
                ftp.Proxy = null;
                FtpWebResponse ftpRes = ftp.GetResponse() as FtpWebResponse;


                ftpRes.Close();

                cbDownloadFinish?.Invoke(true);
            }
            catch (Exception e)
            {

                cbDownloadFinish?.Invoke(false);
                Debug.LogError(e);
            }

        });
    }

    /// <summary>
    /// 获取文件目录 (只能得到 指定目录下的 文件路径。不能 获得 该目录下 子目录的文件 路径信息)
    /// </summary>
    /// <param name="downFilePath"></param>
    public async Task GetFarDirAllFilePath(string farDirName, UnityAction<List<string>> cbDownloadFinish = null)
    {
        await Task.Run(() =>
        {
            try
            {
                FtpWebRequest ftp = FtpWebRequest.Create(new Uri(ftpUrl + "/" + farDirName+(string.IsNullOrEmpty(farDirName)?"":"/"))) as FtpWebRequest;
                ftp.Credentials = Credentials;
                ftp.KeepAlive = false;
                ftp.Method = WebRequestMethods.Ftp.ListDirectory;
                ftp.Proxy = null;
                ftp.UseBinary = true;
                FtpWebResponse ftpRes = ftp.GetResponse() as FtpWebResponse;
                Stream s = ftpRes.GetResponseStream();
                //把下载的信息流 转成StreamReader 就成一行一行的执行
                StreamReader streamReader = new StreamReader(s);
                var arrDir = new List<string>();
                string dir = streamReader.ReadLine();
                while (!string.IsNullOrEmpty(dir))
                {
                    arrDir.Add(dir);
                    dir = streamReader.ReadLine();
                }
                s.Close();
                cbDownloadFinish?.Invoke(arrDir);

                ftpRes.Close();


            }
            catch (Exception e)
            {
                cbDownloadFinish?.Invoke(null);
                Debug.LogError(e);
            }
        });

    }
}

