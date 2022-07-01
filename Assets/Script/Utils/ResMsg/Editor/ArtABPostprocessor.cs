//============================================
//作 者:GK
//时 间:2017-05-29 14:51:35
//备 注:导入文件如果符合条件就会自动标记
//公 司:杭州白掌网络科技有限公司
//============================================
using UnityEngine;
using UnityEditor;

public class ArtABPostprocessor : AssetPostprocessor
{
    public static bool AUTO_SET_ASSETBUNDLE_NAME=true;
    public static string MODEL_PATH = "Assets/Art/";
    public static string MODEL_THR_PATH = "Assets/Art/Thr/";

    //使用AssetPostprocessor类定义的函数OnPostprocessAssetbundleNameChanged回调
    //当AssetBundle的名称发生变化时，编辑器会自动执行以下函数，返回变化信息
    public void OnPostprocessAssetbundleNameChanged(string assetPath, string previousAssetBundleName, string newAssetBundleName)
    {
        Debug.Log("Asset " + assetPath + " has been moved from assetBundle " + previousAssetBundleName + " to assetBundle " + newAssetBundleName);
    }
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {               
        MarkABName(importedAssets);


    }
    static void MarkABName(string[] importedAssets)
    {
        if (importedAssets.Length==0 || !AUTO_SET_ASSETBUNDLE_NAME)
        {
            return;
        }

        string[] extList = new[] { ".bytes", ".jpg", ".png",".mp3" };
        long start_time = System.DateTime.Now.Ticks;
        foreach (var item in importedAssets)
        {
                        
            if (item.IndexOf(".")!=-1 && item.IndexOf(MODEL_THR_PATH) !=-1)
            {
                PdSetThrAbName(extList, item);
            }
            else if(item.IndexOf(".") != -1 && item.IndexOf(MODEL_PATH) != -1)
            {
                PdSetAbName(extList, item);
            }            
        }
        Debug.LogWarning("MarkABName Time[end] :" + (System.DateTime.Now.Ticks - start_time) / 10000000 + "s");
    }
    static void PdSetAbName(string[] extList,string item)
    {
        for (int i = 0; i < extList.Length; i++)
        {
            if (item.IndexOf(extList[i]) != -1)
            {
                //Assets/Scripts/Utils/ResMsg/Editor/ArtABPostprocessor.cs
                int offset = MODEL_PATH.Length;
                string str = "";
                if (item.IndexOf("minimap")>=0)
                {
                    str = item.Substring(offset, item.LastIndexOf(".") - offset);
                }else
                {
                    str = item.Substring(offset, item.LastIndexOf("/") - offset);
                }

                if (item.IndexOf("@")>0)
                {
                    str += "@";
                }
                SetAssetBundleName(item, str);
                break;
            }
        }
    }
    static void PdSetThrAbName(string[] extList, string item)
    {
        for (int i = 0; i < extList.Length; i++)
        {
            if (item.IndexOf(extList[i]) != -1)
            {
                //Assets/Scripts/Utils/ResMsg/Editor/ArtABPostprocessor.cs
                int offset = MODEL_THR_PATH.Length;
                string str = "";
                if (item.IndexOf("minimap") >= 0)
                {
                    str = item.Substring(offset, item.LastIndexOf(".") - offset);
                }
                else
                {
                    str = item.Substring(offset, item.LastIndexOf("/") - offset);
                }

                if (item.IndexOf("@") > 0)
                {
                    str += "@";
                }
                SetAssetBundleName(item, str);
                break;
            }
        }
    }

    static void SetAssetBundleName(string path, string abName)
    {
        AssetImporter importer = AssetImporter.GetAtPath(path);
        if (abName != null)
        {
            if (importer.assetBundleName != abName)
            {
                importer.assetBundleName = abName;                
            }
            if (importer.assetBundleVariant != "ly")
            {
                importer.assetBundleVariant = "ly";
            }
        }
        else
        {
            importer.assetBundleName = null;
        }
    }
}
