using System;
using System.IO;
using System.Net;
using UnityEngine;

public class FTP_Step1 : MonoBehaviour
{

    void Start()
    {
        //Debug.Log("上传开始！");
        // FTPMgr.Instance.UploadFile("sss.txt", Application.streamingAssetsPath + "/aa.txt", () =>
        //{

        //    Debug.Log("上传完成！");
        //}).ContinueWith(task => { Debug.Log("上传 执行完成"); });

        // Debug.Log("下载开始！");
        // FTPMgr.Instance.DownloadFile("yyy.txt", Application.persistentDataPath + "/download.txt", () =>
        // {

        //     Debug.Log("下载完成！");
        // }).ContinueWith(task => { Debug.Log("下载 执行完成"); }); ;

        //Debug.Log("删除开始！");
        //FTPMgr.Instance.DeleteFile("yyy.txt", () =>
        //{
        //    Debug.Log("删除完成！");
        //}).ContinueWith(task => { Debug.Log("下载 执行完成"); }); ;;

        //Debug.Log("获取文件大小开始！");
        //FTPMgr.Instance.GetFileSize("sss.txt", (len) =>
        //{
        //    Debug.Log("获取文件大小完成！" + len);
        //}).ContinueWith(task => { Debug.Log("获取文件大小 执行完成"); }); ; ;


        //Debug.Log("生成文件夹开始！");
        //FTPMgr.Instance.CreateFileDir("hhh", (len) =>
        //{
        //    Debug.Log("生成文件夹完成！" + len);
        //}).ContinueWith(task => { Debug.Log("生成文件夹 执行完成"); }); ; ;
        Debug.Log("获取文件目录开始！");
        FTPMgr.Instance.GetFarDirAllFilePath("hhh", (arr) =>
        {
            Debug.Log("文件目录:" );
            foreach (var item in arr)
            {
                Debug.Log("" + item);
            }
          
        }).ContinueWith(task => { Debug.Log("获取文件目录 执行完成"); }); ; ;


        //SendFile();
    }

    void SendFile()
    {
        Debug.Log("上传开始！");
        FtpWebRequest ftpReq = FtpWebRequest.Create(new Uri("ftp://127.0.0.1/yyy.txt")) as FtpWebRequest;

        NetworkCredential networkCredential = new NetworkCredential("serverZxw", "123456");
        ftpReq.Credentials = networkCredential;

        ftpReq.KeepAlive = false;
        ftpReq.Method = WebRequestMethods.Ftp.UploadFile;
        ftpReq.UseBinary = true;
        //代理关掉 。避免 http 或其他协议 。跟ftp 请求产生冲突。
        ftpReq.Proxy = null;
        Stream sReq = ftpReq.GetRequestStream();
        try
        {
            using (FileStream file = File.Open(Application.streamingAssetsPath + "/aa.txt", FileMode.Open))
            {
                var arr = new byte[1024];

                int lenth = file.Read(arr, 0, arr.Length);

                while (lenth > 0)
                {

                    sReq.Write(arr, 0, lenth);
                    //FileStream.Read   有内部指针。指向上次停止的位置。继续读取 内容 直至最后
                    lenth = file.Read(arr, 0, arr.Length);

                }

                file.Close();
                sReq.Close();
            }

            Debug.Log("上传完毕！");


        }
        catch (Exception e)
        {
            Debug.LogError(e);

        }




    }

    // Start is called before the first frame update
    void Test()
    {
        //ftp 凭证 （账号密码）
        NetworkCredential nc = new NetworkCredential("serverZxw", "123456");



        //ftp 的套接字对象 
        FtpWebRequest ftpReq = FtpWebRequest.Create(new Uri("ftp://127.0.0.1/22.txt")) as FtpWebRequest;

        //绑定 凭证
        ftpReq.Credentials = nc;

        //设置 该ftp套接字的作用 是 干什么用的
        ftpReq.Method = WebRequestMethods.Ftp.DownloadFile;
        //关闭 ftp对操作 地址对象的 上传下载 避免 重引用的报错
        ftpReq.Abort();

        ////上传的流文件
        Stream sReq = ftpReq.GetRequestStream();

        ////下载的 ftp响应
        //FtpWebResponse ftpRes = ftpReq.GetResponse() as FtpWebResponse;

        //ftp的基础是2个tcp构成。  tcp是需要 关闭链接的。  表示完成一次ftp 的上传下载。false就自动关闭了。  true是表示 持续打开状况 
        ftpReq.KeepAlive = false;

        //是否使用2进制传输。 默认 使用ascll  传输。
        ftpReq.UseBinary = true;
        //重命名
        ftpReq.RenameTo = "44.txt";



        //下载  

        FtpWebResponse ftpRes = ftpReq.GetResponse() as FtpWebResponse;


        //关闭下载的ftp
        ftpRes.Close();
        //获取 ftp 下载的流
        Stream resload = ftpRes.GetResponseStream();

        //接收数据长度
        print(ftpRes.ContentLength);
        //得到接收数据的类型
        print(ftpRes.ContentType);

        //最新的状态码
        print(ftpRes.StatusCode);
        //最新的状态码 描述
        print(ftpRes.StatusDescription);
        //登陆前的消息
        print(ftpRes.BannerMessage);
        //离开后的消息
        print(ftpRes.ExitMessage);
        //上次修改的 信息
        print(ftpRes.LastModified);



        Debug.Log("完成ftp的操作！");

    }

    // Update is called once per frame
    void Update()
    {

    }
}
