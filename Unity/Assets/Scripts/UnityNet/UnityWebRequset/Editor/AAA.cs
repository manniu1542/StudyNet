using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AAA
{
    [MenuItem("Tool/BuildAB _F5")]
    // Start is called before the first frame update
    public static void AAAt()
    {
        BuildPipeline.BuildAssetBundles("Assets/AssetBundles", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
        AssetDatabase.Refresh();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
