using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    public Canvas canvas;
    public Transform tf;

    public TMPro.TextMeshProUGUI txt;
    void Start()
    {

    }
    //两个相机 渲染的相对 坐标 ，转换。
    public void ShowTxt(string name = "2222")
    {
        if (tf == null)
        {
            txt.text = "";
            return;
        }

        //tf 转屏幕坐标。再转ui坐标
        Camera ca = GameObject.Find("Camera")?.GetComponent<Camera>();
        Vector3 v31 = ca.transform.TransformDirection(Vector3.forward);

        Vector3 v32 = transform.position - ca.transform.position;

        float dotProduct = Vector3.Dot(v31.normalized, v32.normalized);

        txt.gameObject.SetActive(dotProduct > 0);

        float angle = Mathf.Acos(dotProduct) * Mathf.Rad2Deg;
        Debug.Log("_____" + dotProduct + " _____ " + angle);


        Vector2 v2;
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(ca, tf.position);
        //屏幕坐标 到 ui坐标 
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), screenPos, canvas.worldCamera, out v2);


        var rtf = txt.GetComponent<RectTransform>();
        rtf.anchoredPosition = v2;

        txt.text = name;


    }
    // Update is called once per frame
    void Update()
    {


        ShowTxt();
    }
}
