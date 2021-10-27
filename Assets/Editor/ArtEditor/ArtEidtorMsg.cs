using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ArtEidtorMsg : Editor
{
    static string SetPath()
    {
#if UNITY_ANDROID
        return Application.dataPath + "/../LyRes" + "/Android_Res";
#elif UNITY_IPHONE
        return Application.dataPath + "/../LyRes" + "/IOS_Res";
#else
        return Application.dataPath + "/../LyRes" + "/PC_Res";
#endif

    }
    static string outPath
    {
        get
        {
            return SetPath();
        }
    }
    [MenuItem("Tools/PullAB[根据当前标记的资源导出Ab]")]
    public static void PullAB()
    {
        if (!Directory.Exists(outPath))
        {
            Directory.CreateDirectory(outPath);
        }
        var isIOS = EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS;
        long start_time = System.DateTime.Now.Ticks;
        AssetDatabase.RemoveUnusedAssetBundleNames();
        BuildPipeline.BuildAssetBundles(outPath, //JenkinsTools.GetBuildMap(),
                 BuildAssetBundleOptions.ChunkBasedCompression
                 | BuildAssetBundleOptions.DeterministicAssetBundle
                 , isIOS ? EditorUserBuildSettings.activeBuildTarget : BuildTarget.Android);
        Debug.Log("pullAB Time[end] :" + (System.DateTime.Now.Ticks - start_time) / 10000000 + "s");
        CopyToSetramAssets();
    }
    static bool IsFilter(FileInfo file)
    {
        if (file.Extension == ".ly")
        {
            return false;
        }
        return true;
    }
    public static void CopyToSetramAssets()
    {
        DirectoryInfo root_dir = new DirectoryInfo(outPath);

        var fileinfos = root_dir.GetFileSystemInfos();
        for (int i = 0; i < fileinfos.Length; i++)
        {
            var file = fileinfos[i] as FileInfo;
            Debug.Log(file);
            if (!IsFilter(file))
            {
                string OutPath = Path.GetFullPath(outPath);
                string url = file.FullName.Replace(OutPath, "").Substring(1);
                url = LoadLocalTable.PathCutOff(url, Format.Change_Z_NotCut);
                var streamingAssetsPath = Application.streamingAssetsPath + "/" + url;
                string path = Directory.GetParent(streamingAssetsPath).FullName;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if (File.Exists(streamingAssetsPath))
                {
                    //Debug.Log("delete=" + Application.streamingAssetsPath + "/" + url);
                    File.Delete(streamingAssetsPath);
                }
                File.Copy(file.FullName, streamingAssetsPath);
            }
        }
       
    }
   

}
