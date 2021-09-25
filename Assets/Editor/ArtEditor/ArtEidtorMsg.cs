using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
public enum Format
{
    Change_Z_Cut,
    Change_F_Cut,
    Change_Not_Cut,
    Change_Z_NotCut,
    Change_F_NotCut,
    Change_Z_Cut_HaveHead,
    Change_F_Cut_HaveHead,
    Change_Not_Cut_HaveHead,
}
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
                url = PathCutOff(url, Format.Change_Z_NotCut);
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
    /// <summary>
    /// 路径，以及调整斜杠
    /// </summary>
    /// <param name="tmpPath">路径</param>
    /// <param name="isZFormat">是否换成正斜杠/</param>
    /// <returns></returns>
    internal static string PathCutOff(string tmpPath, Format isZFormat, string cutStr = "Assets")
    {
        string tmp = "";
        switch (isZFormat)
        {
            case Format.Change_F_Cut:
                tmp = FormatChange(PathCutOff(tmpPath, cutStr, false), false);
                break;
            case Format.Change_Z_Cut:
                tmp = FormatChange(PathCutOff(tmpPath, cutStr, false), true);
                break;
            case Format.Change_F_Cut_HaveHead:
                tmp = FormatChange(PathCutOff(tmpPath, cutStr, true), false);
                break;
            case Format.Change_Z_Cut_HaveHead:
                tmp = FormatChange(PathCutOff(tmpPath, cutStr, true), true);
                break;
            case Format.Change_F_NotCut:
                tmp = FormatChange(tmpPath, false);
                break;
            case Format.Change_Z_NotCut:
                tmp = FormatChange(tmpPath, true);
                break;
            case Format.Change_Not_Cut:
                tmp = PathCutOff(tmpPath, cutStr, false);
                break;
            case Format.Change_Not_Cut_HaveHead:
                tmp = PathCutOff(tmpPath, cutStr, true);
                break;
        }
        return tmp;
    }
    /// <summary>
    /// 只切割路径
    /// </summary>
    /// <param name="tmpPath"></param>
    /// <returns></returns>
    static string PathCutOff(string tmpPath, string CutStr, bool isHead)
    {
        int index = tmpPath.IndexOf(CutStr);
        if (isHead)
        {
            return tmpPath.Substring(0, index);
        }
        return tmpPath.Substring(index);
    }
    static string FormatChange(string tmpPath, bool isZFormat)
    {
        string startFormat = "\\";
        string endFormat = "/";

        if (!isZFormat)
        {
            startFormat = "/";
            endFormat = "\\";
        }
        return tmpPath.Replace(startFormat, endFormat);
    }

}
