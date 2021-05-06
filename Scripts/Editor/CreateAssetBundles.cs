using UnityEditor;
using System.IO;
using UnityEngine;

public class CreateAssetBundles
{
    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        string assetBundleDirectory = "AssetBundles";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory,
                                        BuildAssetBundleOptions.None,
                                        BuildTarget.Android);
    }

    [MenuItem("Assets/Create Bundel Main")]
    public static void creatBundleMain()
    {
        //获取选择的对象的路径
        Object[] os = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        bool isExist = Directory.Exists(Application.dataPath + "/StreamingAssets");
        if (!isExist)
        {
            Directory.CreateDirectory(Application.dataPath + "/StreamingAssets");
        }
        foreach (Object o in os)
        {
            string sourcePath = AssetDatabase.GetAssetPath(o);

            string targetPath = Application.dataPath + "/StreamingAssets/" + o.name + ".assetbundle";
            if (BuildPipeline.BuildAssetBundle(o, null, targetPath, BuildAssetBundleOptions.CollectDependencies, BuildTarget.Android))
            {
                Debug.Log("create bundle cuccess!");
            }
            else
            {
                Debug.Log("failure happen");
            }
            AssetDatabase.Refresh();
        }
    }
    [MenuItem("Assets/Create Bundle All")]
    public static void CreateBundleAll()
    {
        bool isExist = Directory.Exists(Application.dataPath + "/StreamingAssets");
        if (!isExist)
        {
            Directory.CreateDirectory(Application.dataPath + "/StreamingAssets");
        }
        Object[] os = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        if (os == null || os.Length == 0)
        {
            return;
        }
        string targetPath = Application.dataPath + "/StreamingAssets/" + "All.assetbundle";
        if (BuildPipeline.BuildAssetBundle(null, os, targetPath, BuildAssetBundleOptions.CollectDependencies, BuildTarget.Android))
        {
            Debug.Log("create bundle all cuccess");
        }
        else
        {
            Debug.Log("failure happen");
        }
        AssetDatabase.Refresh();
    }

}