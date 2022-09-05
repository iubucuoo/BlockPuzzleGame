using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEditor;
using UnityEngine;
using WUtils;

public class CreateMD5 : Editor
{
	const char SIGN = PathTools.SIGN;

	/// <summary>
	/// 拿本地的路径资源与 svn 上的做对比  本地资源找不到的会对svn路径的资源进行删除 并写入Version  提交svn
	/// </summary>
	/// <param name="isRemove"></param>
	public static void BuilderMD5Record(bool isRemove = false)
	{
		AssetDatabase.Refresh();//操作前先刷新一下
		string path = EditorPathTools.SVN_RES_ROOT;
		var _VersionPath = EditorPathTools.SVN_VERSION;// path + "/Version.bytes";
		var _OldResMgr = new NewResMgr(File.Exists(_VersionPath) ? File.ReadAllBytes(_VersionPath) : null);//需要从旧的版本配置中获得依赖关系配置

        var list = FindFileSize.GetDic();

		var _NewResMgr = new NewEditorLoad().BuilderResData();//本地的资源---这里忽略 Environment 下的资源
		foreach (var item in list)
		{
			//过滤无用资源监测
			if (item.IndexOf(".manifest") < 0 && item.IndexOf("Version.bytes") < 0)
			{
				var sp = item.Split('\t');
				var ab_file = PathTools.GetSubPath(sp[0], 1);
				var _AbPath = ab_file.Split('.');
				var model_ab = _AbPath[0].Split('/');
				if (_NewResMgr.GetAB(GetModelName(model_ab), GetAbName(model_ab), out NewResAb ab))
				{
                    //Debug.Log("" + sp);
					ab._VersionNum = int.Parse(sp[1]);
					ab._Size = int.Parse(sp[2]);
					ab._AbPath = _AbPath[0];
				}
				else
				{
					if (isRemove)
					{
						var filePath = path + "/" + ab_file;
						File.Delete(filePath);

						var manifest = filePath + ".manifest";
						if (File.Exists(manifest))
						{
							File.Delete(manifest);
						}

					}
					Log.Error(string.Format("NewResData 没有找到相关的 Modelname={0},AbName={1}，极有可能是不属于svn数据", GetModelName(model_ab), GetAbName(model_ab)));
				}
			}
		}

		//1.先拷贝旧的
		//2.新的AB资源进行ID赋值
		//3.拷贝依赖
		//4.写入
		_NewResMgr.CopyAbParams(_OldResMgr);
		BuilderID(_NewResMgr._Data);
		GetDependencies(_NewResMgr);
		SetDownloadID(_NewResMgr);
		_NewResMgr.ToFile(_VersionPath);
		if (isRemove)
		{
			//SVNUpdate.Ctrl(SVNTYPE.COMMIT, EditorPathTools.SVN_RES_ROOT);
		}
	}
	static void SetDownloadID(NewResMgr _NewResMgr)
	{
		//var datas = ResDownloadManager.GetData();
		//for (int i = 0; i < datas.Length; i++)
		//{
		//	var temp = datas[i];
		//	if (temp.model_id == (int)RES_MODEL_INDEX.scinfo)
		//	{
		//		var list = ExportScene.GetScAbList(temp.obj_name);
		//		for (int z = 0; z < list.Count; z++)
		//		{
		//			var abName = list[z];
		//			if (_NewResMgr.GetAB(temp.model_id, abName, out NewResAb ab))
		//			{
		//				ab._DownloadID = temp.id;
		//			}
		//		}
		//	}
		//	else
		//	{
		//		if (_NewResMgr.GetAB(temp.model_id, temp.obj_name, out NewResAb ab))
		//		{
		//			ab._DownloadID = temp.id;
		//		}
		//	}
		//}
	}

	static string GetModelName(string[] sp)
	{
		return sp.Length == 1 ? "" : sp[0];
	}
	static string GetAbName(string[] sp)
	{
		return sp[sp.Length == 1 ? 0 : 1];
	}
	static void GetDependencies(NewResMgr _NewResMgr)
	{
		AssetBundle manifesetLoader = AssetBundle.LoadFromFile(EditorPathTools.SVN_ASSETBUNDLE_MANIFEST);
		AssetBundleManifest abManifest = manifesetLoader.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
		string[] allbundle = abManifest.GetAllAssetBundles();

		foreach (var item in allbundle)
		{
			string[] depens = abManifest.GetAllDependencies(item);
			var sp = item.Split('.')[0].Split('/');
			var depensint = _NewResMgr.GetAbs(depens);

			if (_NewResMgr.GetAB(GetModelName(sp), GetAbName(sp), out NewResAb ab))
			{
				ab.SetDependencies(depensint);
			}
		}
		manifesetLoader.Unload(true);
	}



	public static void BuilderVersion()
	{
		string path = EditorPathTools.SVN_VERSION;
		if (!File.Exists(path))
		{
			path = PathTools.STREAM_VERSION;
			if (!File.Exists(path))
			{
				Log.Error("读取上一个版本号失败，请检查文件是否存在" + path);
				return;
			}
		}

		string str = PathTools.GetLineVersion(path, 1);
		string str_CVersion = PathTools.GetLineVersion(path, 2);

		string[] keys = str.Split(SIGN);
		float newVersion = 0;
		if (keys[0].Equals("Version"))
		{
			newVersion = int.Parse(keys[1]) + 1;
		}
		else
		{
			newVersion = 1;
			Log.Error("读取版本号格式错误,重0开始");
		}

		//格式
		//Version_ _xxxx
		//CVersion_ _xxxxx
		File.WriteAllText(EditorPathTools.SVN_VERSION, "Version" + SIGN + newVersion + "\n" + str_CVersion);
	}

	public static void ClearFilesCache()
	{
		string Path = Application.dataPath;
		Path = Path.Substring(0, Path.LastIndexOf('/'));
		Path = Path.Substring(0, Path.LastIndexOf('/')) + "/" + Application.productName + "_Cache";
		if (Directory.Exists(Path))
		{
			DeleteDir(Path);
			Log.Info("清理本地文件 Success");
		}
	} 

	public static void DeleteDir(string file)
	{
		try
		{
			//去除文件夹和子文件的只读属性
			//去除文件夹的只读属性
			DirectoryInfo fileInfo = new DirectoryInfo(file);
			fileInfo.Attributes = FileAttributes.Normal & FileAttributes.Directory;

			//去除文件的只读属性
			File.SetAttributes(file, FileAttributes.Normal);

			//判断文件夹是否还存在
			if (Directory.Exists(file))
			{
				foreach (string f in Directory.GetFileSystemEntries(file))
				{
					if (File.Exists(f))
					{
						//如果有子文件删除文件
						File.Delete(f);
					}
					else
					{
						//循环递归删除子文件夹
						DeleteDir(f);
					}
				}

				//删除空文件夹
				Directory.Delete(file);
			}

		}
		catch (Exception ex) // 异常处理
		{
			Console.WriteLine(ex.Message.ToString());// 异常信息
		}
	}


    //[MenuItem("ITools/Proto消息字段提示生成")]
    public static void ProtoTipMake()
	{
		String protoPath = Application.dataPath;
		protoPath = protoPath.Substring(0, protoPath.LastIndexOf("/Assets")) + "/ITools/BuilderProtobuf/client/";
		String outPath = Application.dataPath + "/Lua/Protol/";
		DirectoryInfo dir = new DirectoryInfo(protoPath);
		FileSystemInfo[] info = dir.GetFileSystemInfos();
		string fileOnlyName;
		string moduleName;
		for (int i = 0; i < info.Length; i++)
		{
			FileInfo file = info[i] as FileInfo;
			if (file.Extension == ".proto")
			{
				fileOnlyName = file.Name.Substring(0, file.Name.LastIndexOf(".proto"));
				moduleName = "";
				if (fileOnlyName.IndexOf("ClientMsg") != -1)
					moduleName = "pb";
				else if (fileOnlyName.IndexOf("ServerMsg") != -1)
					moduleName = "spb";
				if (moduleName.Length > 0)
					EmmyProtoBufExport.ExportApi(file.FullName, outPath, moduleName);
			}
		}

		Log.Info("ProtoTipMake Success");

	}

	public static void BuilderID(NewResData data)
	{
		var dic = ExternalTools.MD5KEY(out int MAXKEY);
		for (int i = 0; i < data._Count; i++)
		{
			var model = data._Data[i];
			for (int j = 0; j < model._Count; j++)
			{
				var ab = model[j];
				var _AbPath = ab._AbPath;
				if (!string.IsNullOrEmpty(_AbPath))
				{
					var md5 = MD5.Create();
					byte[] byteArray = Encoding.ASCII.GetBytes(_AbPath);
					var b = md5.ComputeHash(byteArray);
					StringBuilder sb = new StringBuilder();
					for (int z = 0; z < b.Length; z++)
					{
						sb.Append(b[z].ToString("x2"));
					}
					var key = sb.ToString();
					if (dic.TryGetValue(key, out int v))
					{
						ab._ID = v;
					}
					else
					{
						ab._ID = MAXKEY;
						dic.Add(key, MAXKEY);
						MAXKEY++;
					}
				}
				else
				{
					Log.Error("GK:BUG AbPath=null _AbName=" + ab._AbName);
				}
			}
		}
		ExternalTools.SaveMD5Key(dic);
	}
}


