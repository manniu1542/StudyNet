using System;
using System.IO;
using System.Net;
using UnityEngine;

public class FTP_Step1 : MonoBehaviour
{

    void Start()
    {
        //Debug.Log("�ϴ���ʼ��");
        // FTPMgr.Instance.UploadFile("sss.txt", Application.streamingAssetsPath + "/aa.txt", () =>
        //{

        //    Debug.Log("�ϴ���ɣ�");
        //}).ContinueWith(task => { Debug.Log("�ϴ� ִ�����"); });

        // Debug.Log("���ؿ�ʼ��");
        // FTPMgr.Instance.DownloadFile("yyy.txt", Application.persistentDataPath + "/download.txt", () =>
        // {

        //     Debug.Log("������ɣ�");
        // }).ContinueWith(task => { Debug.Log("���� ִ�����"); }); ;

        //Debug.Log("ɾ����ʼ��");
        //FTPMgr.Instance.DeleteFile("yyy.txt", () =>
        //{
        //    Debug.Log("ɾ����ɣ�");
        //}).ContinueWith(task => { Debug.Log("���� ִ�����"); }); ;;

        //Debug.Log("��ȡ�ļ���С��ʼ��");
        //FTPMgr.Instance.GetFileSize("sss.txt", (len) =>
        //{
        //    Debug.Log("��ȡ�ļ���С��ɣ�" + len);
        //}).ContinueWith(task => { Debug.Log("��ȡ�ļ���С ִ�����"); }); ; ;


        //Debug.Log("�����ļ��п�ʼ��");
        //FTPMgr.Instance.CreateFileDir("hhh", (len) =>
        //{
        //    Debug.Log("�����ļ�����ɣ�" + len);
        //}).ContinueWith(task => { Debug.Log("�����ļ��� ִ�����"); }); ; ;
        Debug.Log("��ȡ�ļ�Ŀ¼��ʼ��");
        FTPMgr.Instance.GetFarDirAllFilePath("hhh", (arr) =>
        {
            Debug.Log("�ļ�Ŀ¼:" );
            foreach (var item in arr)
            {
                Debug.Log("" + item);
            }
          
        }).ContinueWith(task => { Debug.Log("��ȡ�ļ�Ŀ¼ ִ�����"); }); ; ;


        //SendFile();
    }

    void SendFile()
    {
        Debug.Log("�ϴ���ʼ��");
        FtpWebRequest ftpReq = FtpWebRequest.Create(new Uri("ftp://127.0.0.1/yyy.txt")) as FtpWebRequest;

        NetworkCredential networkCredential = new NetworkCredential("serverZxw", "123456");
        ftpReq.Credentials = networkCredential;

        ftpReq.KeepAlive = false;
        ftpReq.Method = WebRequestMethods.Ftp.UploadFile;
        ftpReq.UseBinary = true;
        //����ص� ������ http ������Э�� ����ftp ���������ͻ��
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
                    //FileStream.Read   ���ڲ�ָ�롣ָ���ϴ�ֹͣ��λ�á�������ȡ ���� ֱ�����
                    lenth = file.Read(arr, 0, arr.Length);

                }

                file.Close();
                sReq.Close();
            }

            Debug.Log("�ϴ���ϣ�");


        }
        catch (Exception e)
        {
            Debug.LogError(e);

        }




    }

    // Start is called before the first frame update
    void Test()
    {
        //ftp ƾ֤ ���˺����룩
        NetworkCredential nc = new NetworkCredential("serverZxw", "123456");



        //ftp ���׽��ֶ��� 
        FtpWebRequest ftpReq = FtpWebRequest.Create(new Uri("ftp://127.0.0.1/22.txt")) as FtpWebRequest;

        //�� ƾ֤
        ftpReq.Credentials = nc;

        //���� ��ftp�׽��ֵ����� �� ��ʲô�õ�
        ftpReq.Method = WebRequestMethods.Ftp.DownloadFile;
        //�ر� ftp�Բ��� ��ַ����� �ϴ����� ���� �����õı���
        ftpReq.Abort();

        ////�ϴ������ļ�
        Stream sReq = ftpReq.GetRequestStream();

        ////���ص� ftp��Ӧ
        //FtpWebResponse ftpRes = ftpReq.GetResponse() as FtpWebResponse;

        //ftp�Ļ�����2��tcp���ɡ�  tcp����Ҫ �ر����ӵġ�  ��ʾ���һ��ftp ���ϴ����ء�false���Զ��ر��ˡ�  true�Ǳ�ʾ ������״�� 
        ftpReq.KeepAlive = false;

        //�Ƿ�ʹ��2���ƴ��䡣 Ĭ�� ʹ��ascll  ���䡣
        ftpReq.UseBinary = true;
        //������
        ftpReq.RenameTo = "44.txt";



        //����  

        FtpWebResponse ftpRes = ftpReq.GetResponse() as FtpWebResponse;


        //�ر����ص�ftp
        ftpRes.Close();
        //��ȡ ftp ���ص���
        Stream resload = ftpRes.GetResponseStream();

        //�������ݳ���
        print(ftpRes.ContentLength);
        //�õ��������ݵ�����
        print(ftpRes.ContentType);

        //���µ�״̬��
        print(ftpRes.StatusCode);
        //���µ�״̬�� ����
        print(ftpRes.StatusDescription);
        //��½ǰ����Ϣ
        print(ftpRes.BannerMessage);
        //�뿪�����Ϣ
        print(ftpRes.ExitMessage);
        //�ϴ��޸ĵ� ��Ϣ
        print(ftpRes.LastModified);



        Debug.Log("���ftp�Ĳ�����");

    }

    // Update is called once per frame
    void Update()
    {

    }
}
