using StepUdpFinishSync;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityNet.UnityWebRequset;

public class StudyUnityWebRequest : MonoBehaviour
{
    public Image img;
    public Text txtUI;
    // Start is called before the first frame update
    void Start()
    {
        //GetTxtContent2();
        //StartCoroutine(GetTxtContent());
        //StartCoroutine(GetImgContent());
        //StartCoroutine(GetAssetBundleContent());

        //PostStudy();
        //DownloadHandlerTexture download = new DownloadHandlerTexture();

        //StartCoroutine(UnityWebRequsetMgr.GetFile("http://192.168.0.104:8080/HttpServer/temp.png", download, (err) =>
        //{
        //    if (string.IsNullOrEmpty(err))
        //    {
        //        img.sprite = Sprite.Create(download.texture, new Rect(0, 0, download.texture.width, download.texture.height), Vector2.one * 0.5f);


        //    }
        //    else
        //        Debug.LogError(err);
        //}, (cur, all) =>
        //{
        //    Debug.Log(cur + "/" + all);
        //    txtUI.text = cur + "/" + all;
        //}));



        //UnityWebRequsetMgr.GetAssetBundleFile("http://192.168.0.104:8080/HttpServer/newTestCube.unity3d", (err, ab) =>
        //{
        //    if (string.IsNullOrEmpty(err))
        //    {


        //        var prefab = ab.LoadAsset<GameObject>("Cube");
        //        var go = GameObject.Instantiate(prefab);
        //        go.name = "ab���� Cube";
        //        go.transform.SetParent(transform);

        //    }
        //    else
        //        Debug.LogError(err);
        //});

        //UnityWebRequsetMgr.UploadData(Encoding.UTF8.GetBytes("hhh"), "http://192.168.0.104:8080/HttpServer/", (err) =>
        //{
        //    if (!string.IsNullOrEmpty(err))
        //    {
        //        Debug.Log(err);
        //    }
        //    else
        //    {
        //        Debug.Log("�ϴ��ɹ���");
        //    }
        //});
        //UnityWebRequsetMgr.UploadFile("temp.png", Application.streamingAssetsPath + "/tt.png", (err) =>
        //{
        //    if (!string.IsNullOrEmpty(err))
        //    {
        //        Debug.Log(err);
        //    }
        //    else
        //    {
        //        Debug.Log("�ϴ��ɹ���");
        //    }
        ////});
        //UnityWebRequsetMgr.GetDownLoadInfo<Texture2D>(this, "http://192.168.0.104:8080/HttpServer/temp.png", (texture) =>
        //{
        //    img.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
        //});
        UnityWebRequsetMgr.GetDownLoadInfo<FileStream>(this,"http://192.168.0.104:8080/HttpServer/temp.png", downLoadLoaclPath: Application.dataPath + "/../tt.png");

        //UnityWebRequsetMgr.UploadFile(Application.streamingAssetsPath + "/tt.png", "image/png", "http://192.168.0.104:8080/HttpServer/", (err) =>
        //{
        //    if (!string.IsNullOrEmpty(err))
        //    {
        //        Debug.Log(err);
        //    }
        //    else
        //    {
        //        Debug.Log("�ϴ��ɹ���");
        //    }
        //});
        //DownloadHandlerCustomABLoad download = new DownloadHandlerCustomABLoad("Cube");

        //StartCoroutine(UnityWebRequsetMgr.GetFile("http://192.168.0.104:8080/HttpServer/newTestCube.unity3d", download, (err) =>
        //{
        //    if (string.IsNullOrEmpty(err))
        //    {
        //        var go = GameObject.Instantiate((GameObject)download.Obj);
        //        go.name = "�Զ��� Cube";
        //        go.transform.SetParent(transform);


        //    }
        //    else
        //        Debug.LogError(err);
        //}, (cur, all) =>
        //{
        //    Debug.Log(cur + "/" + all);
        //    txtUI.text = cur + "/" + all;
        //}));




    }

    IEnumerator TestPostUrlMsg(string url, List<IMultipartFormSection> list, UnityAction<StepUdpFinishSync.BaseMsgData> complateFun, UnityAction<ulong, ulong> progressFun = null)
    {
        UnityWebRequest req = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);

        var dhcm = new DownloadHandlerCustomMsg();
        req.downloadHandler = dhcm;
        //List<IMultipartFormSection> ת�� �ֽ�����
        UploadHandler uploadHandler = new UploadHandlerRaw(new byte[1024]);
        uploadHandler.contentType = "multipart/form-data; boundary=zxw";
        req.uploadHandler = uploadHandler;


        req.timeout = 2000;
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
        if (!url.StartsWith("file://"))//�������ػ�������� ��ɣ�
        {
            progressFun?.Invoke(req.downloadedBytes, allLoadedBytes);
        }

        if (req.result == UnityWebRequest.Result.Success)
        {

            complateFun(dhcm.GetMsgData<PlayerDataMsg>());

        }
        else
        {
            complateFun(null);

        }

    }
    IEnumerator GetTxtContent()
    {
        UnityWebRequest req = UnityWebRequest.Get("http://192.168.0.104:8080/HttpServer/66.txt");
        var send = req.SendWebRequest();

        while (!send.isDone)
        {
            Debug.Log("��ǰ���ȣ�" + send.progress);
            yield return null;
        }

        yield return send;


        if (req.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("���� ���ݣ�" + Encoding.UTF8.GetString(req.downloadHandler.data));
        }
        else
        {
            Debug.LogError($"����ʧ��ԭ��result_{req.result},error_{req.error},responseCode_{req.responseCode}");
        }

    }

    void GetTxtContent2()
    {
        UnityWebRequest req = UnityWebRequest.Get("http://192.168.0.104:8080/HttpServer/66.txt");
        var send = req.SendWebRequest();

        send.completed += data =>
        {
            if (req.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("���� ���ݣ�" + Encoding.UTF8.GetString(req.downloadHandler.data));
            }
            else
            {
                Debug.LogError($"����ʧ��ԭ��result_{req.result},error_{req.error},responseCode_{req.responseCode}");
            }

        };


    }

    IEnumerator GetImgContent()
    {
        UnityWebRequest req = UnityWebRequestTexture.GetTexture("http://192.168.0.104:8080/HttpServer/tt.png");
        //UnityWebRequest req = UnityWebRequestTexture.GetTexture(Application.streamingAssetsPath+"/tt.png");
        var send = req.SendWebRequest();

        while (!send.isDone)
        {
            Debug.Log("��ǰ���ȣ�" + send.progress);
            yield return null;
        }

        yield return send;


        if (req.result == UnityWebRequest.Result.Success)
        {
            //var texture = DownloadHandlerTexture.GetContent(req);
            var texture = req.downloadHandler as DownloadHandlerTexture;
            var pic2d = texture.texture;
            img.sprite = Sprite.Create(pic2d, new Rect(0, 0, pic2d.width, pic2d.height), Vector2.one * 0.5f);


            img.SetNativeSize();
        }
        else
        {
            Debug.LogError($"����ʧ��ԭ��result_{req.result},error_{req.error},responseCode_{req.responseCode}");
        }

    }
    IEnumerator GetAssetBundleContent()
    {

        UnityWebRequest req = UnityWebRequestAssetBundle.GetAssetBundle("http://192.168.0.104:8080/HttpServer/abTest.unity3d");
        //UnityWebRequest req = UnityWebRequestAssetBundle.GetAssetBundle(Application.streamingAssetsPath + "/test.unity3d");
        var send = req.SendWebRequest();

        while (!send.isDone)
        {
            Debug.Log("��ǰ���ȣ�" + req.downloadProgress);
            Debug.Log("��ǰ�����ֽڣ�" + req.downloadedBytes);
            yield return null;
        }
        Debug.Log("��ǰ���ȣ�" + req.downloadProgress);
        Debug.Log("��ǰ�����ֽڣ�" + req.downloadedBytes);
        yield return send;


        if (req.result == UnityWebRequest.Result.Success)
        {

            var ab = DownloadHandlerAssetBundle.GetContent(req);


            var prefab = ab.LoadAsset<GameObject>("Cube");
            var go = GameObject.Instantiate(prefab);
            go.name = "ab���� Cube";
            go.transform.SetParent(transform);


        }
        else
        {
            Debug.LogError($"����ʧ��ԭ��result_{req.result},error_{req.error},responseCode_{req.responseCode}");
        }

    }


    //���� post �ϴ� �ļ� �����磺ab�� ��  


    void PostStudy()
    {

        var list = new List<IMultipartFormSection>();
        //�ϴ� һ�� txt�ļ�
        list.Add(new MultipartFormFileSection("file", "111", Encoding.UTF8, "aaa.txt"));
        //key value �ϴ� txt�ļ�
        list.Add(new MultipartFormFileSection("file", File.ReadAllBytes(Application.streamingAssetsPath + "/aa.txt"), "temp.txt", "text/plain"));
        //key value �ϴ� png �ļ�
        list.Add(new MultipartFormFileSection("file", File.ReadAllBytes(Application.streamingAssetsPath + "/tt.png"), "pic.png", "image/png"));
        //key value �ϴ� ab��
        list.Add(new MultipartFormFileSection("assetboundle", File.ReadAllBytes(Application.streamingAssetsPath + "/test.unity3d"), "abTest.unity3d", "application/octet-stream"));

        //key  value
        //�ϴ�����
        list.Add(new MultipartFormDataSection("name", "zhangxianwen"));
        list.Add(new MultipartFormDataSection("name2", Encoding.UTF8.GetBytes("a=helloworld"), "application/x-www-form-urlencode"));
        list.Add(new MultipartFormDataSection("name3", new byte[1024], "application/octet-stream"));



        UnityWebRequest req = UnityWebRequest.Post("http://192.168.0.104:8080/HttpServer/", list);

        var send = req.SendWebRequest();
        send.completed += (asy) =>
        {
            if (req.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("�ϴ���ɣ�");
            }
            else
            {
                Debug.Log("�ϴ�ʧ��:" + req.result + "  error:" + req.error + "  code:" + req.responseCode);
            }
        };
    }

    // Update is called once per frame
    void Update()
    {

    }
}
