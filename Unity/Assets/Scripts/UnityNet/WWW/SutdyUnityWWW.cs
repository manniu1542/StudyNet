using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SutdyUnityWWW : MonoBehaviour
{

    public RawImage img;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LocalLoadFile("tt.png"));
    }

    IEnumerator HttpLoadFile(string farFileName)
    {
        WWW www = new WWW("http://192.168.0.104:8080/httpShare/" + farFileName);
        while (!www.isDone)
        {
            print(www.bytesDownloaded);
            print(www.progress);
            yield return www;
        }

        if (www.error != null)
        {

            print(www.error);
            yield return null;
        }

        img.texture = www.texture;


    }

    IEnumerator FtpLoadFile(string farFileName)
    {
        //ftp的默认端口号是21.可省略
        WWW www = new WWW("ftp://127.0.0.1:21/" + farFileName);
        while (!www.isDone)
        {
            print(www.bytesDownloaded);
            print(www.progress);
            yield return www;
        }

        if (www.error != null)
        {

            print(www.error);
            yield return null;
        }

        img.texture = www.texture;


    }

    IEnumerator LocalLoadFile(string farFileName)
    {

        WWW www = new WWW("file://" + Application.streamingAssetsPath + "/" + farFileName);
        while (!www.isDone)
        {
            print(www.bytesDownloaded);
            print(www.progress);
            yield return www;
        }

        if (www.error != null)
        {

            print(www.error);
            yield return null;
        }

        img.texture = www.texture;


    }

    // Update is called once per frame
    void Update()
    {

    }
}
