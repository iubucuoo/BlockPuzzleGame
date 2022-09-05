using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using UnityEditor.SceneManagement;
using WUtils;

public class BuildApkTools : Editor
{
	public static void BuildAPK(BuildOptions options, string apkName)
	{
		CopyToProject();

        string outPath = EditorPathTools.SVN_EXPORT_APK;

		if (Directory.Exists(outPath))
		{
			Directory.Delete(outPath, true);
		}

		Directory.CreateDirectory(outPath);
		BuildTarget target = BuildTarget.Android;

		string[] outScenes = { "Assets/Game/Scenes/Main.unity"};
		BuildPipeline.BuildPlayer(outScenes, outPath + "/" + apkName, target, options);
		Log.Info("Build success");
	}
    
	static void DeleteUnUseFiles(string path)
	{
		DirectoryInfo dir = new DirectoryInfo(path);

		FileSystemInfo[] files = dir.GetFileSystemInfos();

		for (int i = 0; i < files.Length; ++i)
		{
			if (files[i] is FileInfo)
				File.Delete(files[i].FullName);
			else
				DeleteUnUseFiles(files[i].FullName);
		}

	}

	static void DeleteUnUseDirs(string path)
	{
		DirectoryInfo dir = new DirectoryInfo(path);

		DirectoryInfo[] dirs = dir.GetDirectories();
		for (int i = 0; i < dirs.Length; ++i)
		{
			if (dirs[i].GetDirectories().Length == 0)
			{
				Directory.Delete(dirs[i].FullName);
			}
			else
			{
				DeleteUnUseDirs(dirs[i].FullName);
			}
		}
		Directory.Delete(path);
	}

	public static void CopyToProject()//将首包资源拷贝回包内
    {
        if (Directory.Exists(PathTools.STREAM_RES_PATH))
        {
            Directory.Delete(PathTools.STREAM_RES_PATH, true);
        }
		var _CacheResMgr = new NewResMgr(File.ReadAllBytes(EditorPathTools.SVN_VERSION));
		for (int i = 0; i < _CacheResMgr._Data._Count; i++)
		{
			var tempModel = _CacheResMgr._Data[i];
			for (int j = 0; j < tempModel._Count; j++)
			{
				var tempAb = tempModel[j];
				if (tempAb._DownloadID == 0)
				{
					var _abPath = tempAb._AbPath;
					var fileName = _abPath + PathTools.DOT_LY;
					if (_abPath.Contains(PathTools.DllABPath))
					{
						fileName = _abPath + PathTools.Dot_Logic;
					}
					var sourcePath = EditorPathTools.SVN_RES_ROOT + "/" + fileName;
					var toPath = PathTools.STREAM_RES_PATH + fileName;
					var dir = PathTools.STREAM_RES_PATH + tempAb[0]._ModelName.ToLower();
					if (!Directory.Exists(dir))
					{
						Directory.CreateDirectory(dir);
					}
					File.Copy(sourcePath, toPath, true);
				}
			}
		}
		File.Copy(EditorPathTools.SVN_VERSION, PathTools.STREAM_VERSION, true);
		AssetDatabase.Refresh();
	}
    public static void AllCopyToSetramAssets()
    {
        DirectoryInfo root_dir = new DirectoryInfo(EditorPathTools.SVN_RES_ROOT);

        var fileinfos = root_dir.GetFileSystemInfos();
        for (int i = 0; i < fileinfos.Length; i++)
        {
            var file = fileinfos[i] as FileInfo;
            if (file.Extension == WUtils.PathTools.DOT_LY)
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
