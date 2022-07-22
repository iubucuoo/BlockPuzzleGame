using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ArtEidtorMsg : Editor
{
    [MenuItem("Tools/PullAB[根据当前标记的资源导出Ab]")]
    public static void PullAB()
    {
        if (!Directory.Exists(EditorPathTools.SVN_RES_ROOT))
        {
            Directory.CreateDirectory(EditorPathTools.SVN_RES_ROOT);
        }
        var isIOS = EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS;
        long start_time = System.DateTime.Now.Ticks;
        AssetDatabase.RemoveUnusedAssetBundleNames();
        BuildPipeline.BuildAssetBundles(EditorPathTools.SVN_RES_ROOT, //JenkinsTools.GetBuildMap(),
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
        DirectoryInfo root_dir = new DirectoryInfo(EditorPathTools.SVN_RES_ROOT);

        var fileinfos = root_dir.GetFileSystemInfos();
        for (int i = 0; i < fileinfos.Length; i++)
        {
            var file = fileinfos[i] as FileInfo;
            Debug.Log(file);
            if (!IsFilter(file))
            {
                string OutPath = Path.GetFullPath(EditorPathTools.SVN_RES_ROOT);
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
