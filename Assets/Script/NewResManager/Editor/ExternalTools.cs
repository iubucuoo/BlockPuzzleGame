using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ExternalTools
{
	static string _VersionLogPath = "";// EditorPathTools.VERSION_LOG_PATH;
    static string _AssetRoot = "";//EditorPathTools.PROJECT_ROOT;
    static string BIN = _AssetRoot + "ITools/VersionMgr/VersionMgr/VersionMgr/bin/Debug";

	public static Dictionary<string, int> GetCurSvnLog()
	{
		string batPath = BIN.Replace('/', '\\');
		System.Diagnostics.Process process = new System.Diagnostics.Process();
		process.StartInfo.WorkingDirectory = batPath;
		process.StartInfo.FileName = "VersionMgr.exe";
		process.Start();
		process.WaitForExit();
		return ReadResTxt(BIN + "/resLog.txt");
	}

	internal static List<string> GetFilesSize(List<string> list)
	{
		string _BIN = Application.dataPath + "/../ITools/VersionMgr/SVNMgr/GetFileSize/FilesSize/FilesSize/bin/Debug";
		string _itemPath = _BIN + "/dirPath.txt";
		string _resLog = _BIN + "/resLog.txt";
		if (File.Exists(_itemPath))
		{
			File.Delete(_itemPath);
		}
		File.CreateText(_itemPath).Dispose();

		FileStream fs = new FileStream(_itemPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
		StreamWriter sw = new StreamWriter(fs);
		foreach (var filePath in list)
		{
			sw.WriteLine(filePath);
		}
		sw.Close();

		string svnPath = (_BIN).Replace('/', '\\');
		System.Diagnostics.Process process = new System.Diagnostics.Process();
		process.StartInfo.WorkingDirectory = svnPath;
		process.StartInfo.FileName = "FilesSize.exe";
		process.Start();
		process.WaitForExit();
		return ReadResTxtList(_resLog);
	}


	public static Dictionary<string, int> GetLogDic()
	{
		//string dir = _VersionLogPath + "/NewResLog.txt";
		//if (File.Exists(dir))
		//{
		//	SVNUpdate.Ctrl(SVNTYPE.CLEAN_UP, dir);
		//	//提交一波资源
		//	SVNUpdate.Ctrl(SVNTYPE.UPDATE, dir);
		//	return LitJson.JsonMapper.ToObject<Dictionary<string, int>>(File.ReadAllText(dir));
		//}
		return new Dictionary<string, int>();
	}

	internal static void SVN_UPDATE_RESROOT()
	{
		//SVNUpdate.Ctrl(SVNTYPE.CLEAN_UP, EditorPathTools.SVN_RES_ROOT);
		////提交一波资源
		//SVNUpdate.Ctrl(SVNTYPE.UPDATE, EditorPathTools.SVN_RES_ROOT);
	}
	/// <summary>
	/// 保存ITools/VersionMgr/VersionLog/NewResLog.txt 并提交svn
	/// </summary>
	/// <param name="data"></param>
	public static void SaveAndCommitLog(Dictionary<string, int> data)
	{
		string dir = _VersionLogPath;
		if (!Directory.Exists(dir))
		{
			Directory.CreateDirectory(dir);
		}
		//会执行提交
		var file = dir + "/NewResLog.txt";
		File.WriteAllText(file, LitJson.JsonMapper.ToJson(data));
		//SVNUpdate.Ctrl(SVNTYPE.COMMIT, file);//提交svn		
	}

	static Dictionary<string, int> ReadResTxt(string _filePaht)
	{
		Dictionary<string, int> dic = new Dictionary<string, int>();
		if (File.Exists(_filePaht))
		{
			StreamReader _ss = File.OpenText(_filePaht);
			while (!_ss.EndOfStream)
			{
				var item = _ss.ReadLine();
				item = item.Trim('[', ']');
				var vv = item.Split(',');
				dic.Add(vv[0].Trim(), int.Parse(vv[1].Trim()));

			}
			_ss.Close();
		}
		return dic;
	}
	static List<string> ReadResTxtList(string _filePaht)
	{
		StreamReader _ss = null;
		List<string> _lis = new List<string>();
		if (File.Exists(_filePaht))
		{
			_ss = File.OpenText(_filePaht);

			while (!_ss.EndOfStream)
			{
				_lis.Add(_ss.ReadLine());
			}
			_ss.Close();
		}
		return _lis;
	}

	internal static Dictionary<string, int> MD5KEY(out int MAXKEY)
	{
		MAXKEY = 1;
		//string dir = _VersionLogPath + "/MD5KEY.txt";
		//if (File.Exists(dir))
		//{
		//	SVNUpdate.Ctrl(SVNTYPE.CLEAN_UP, dir);
		//	//提交一波资源
		//	SVNUpdate.Ctrl(SVNTYPE.UPDATE, dir);
		//	var dic = LitJson.JsonMapper.ToObject<Dictionary<string, int>>(File.ReadAllText(dir));
		//	foreach (var item in dic)
		//	{
		//		if (MAXKEY < item.Value)
		//		{
		//			MAXKEY = item.Value;
		//		}
		//	}
		//	MAXKEY += 1;
		//	return dic;
		//}
		return new Dictionary<string, int>();
	}

	internal static void SaveMD5Key(Dictionary<string, int> data)
	{
		string dir = _VersionLogPath;
		if (!Directory.Exists(dir))
		{
			Directory.CreateDirectory(dir);
		}
		//会执行提交
		string file = _VersionLogPath + "/MD5KEY.txt";

		//SVNUpdate.Ctrl(SVNTYPE.CLEAN_UP, dir);
		////提交一波资源
		//SVNUpdate.Ctrl(SVNTYPE.UPDATE, dir);

		File.WriteAllText(file, LitJson.JsonMapper.ToJson(data));
		//SVNUpdate.Ctrl(SVNTYPE.COMMIT, file);//提交svn		
	}
}
