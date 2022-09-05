using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using WUtils;

[Serializable]
[ProtoContract]

public class Cite
{
    [ProtoMember(1)]
    public List<Cite> unit;//所有依赖本文件的资源
    [ProtoMember(2)]
    public string path;
    public Cite()
    {

    }
    public Cite(string _path)
    {
        path = _path;
        unit = new List<Cite>();
    }
    public void AddCite(Cite cite, Dictionary<string, Cite> all)
    {
        if (!Check(cite, all))
        {
            unit.Add(cite);
        }
    }

    /// <summary>
    /// 检查是否添加过，all内没有的也需要删除
    /// </summary>
    /// <param name="cite"></param>
    /// <param name="all"></param>
    /// <returns></returns>
    public bool Check(Cite cite, Dictionary<string, Cite> all)
    {
        if (unit == null)
        {
            unit = new List<Cite>();
        }
        for (int i = unit.Count - 1; i >= 0; i--)
        {
            if (!all.ContainsKey(unit[i].path))
            {
                //因为移除操作无法进行，所以只能在这里操作
                //Debug.LogWarning("移除 sub：" + unit[i].path);
                unit.RemoveAt(i);
            }
            else
            {
                if (unit[i].path == cite.path)
                {
                    return true;
                }
            }
        }
        return false;
    }
}

[ProtoContract]
public class Cites
{
    [ProtoMember(1)]
    public Dictionary<string, Cite> dic;

    public Cites()
    {

    }
    public Cites(int l)
    {
        dic = new Dictionary<string, Cite>();
    }
}

public class CiteMgr
{
    /// <summary>
    /// 获得合法资源的关系 并 序列化到client\ITools\VersionMgr\VersionLog\CiteMsg.txt文件
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static Cites AddAll()
    {
        string[] data = AssetDatabase.GetAllAssetPaths();
        var tar = new Cites(1);
        AddCite(data, tar);
        SaveCiteLog(tar);
        return tar;
    }

    /// <summary>
    /// 路径中所带以下的字符  会被忽略
    /// </summary>
    static string[] words = new string[]
    {
        "Lightmap",
        "LightingData",
        "NavMesh",
        ".unity",
        "Thr/Maps/",
    };
    /// <summary>
    /// 判断是否忽略->除带有Art的并且不带有word组内字符串的路径外 其他都ignore
    /// </summary>
    /// <param name="temp"></param>
    /// <returns></returns>
    public static bool Ignore(string temp)
    {
        if (temp.IndexOf("/Art/") >= 0)
        {
            foreach (var item in words)
            {
                if (temp.IndexOf(item) >= 0)
                {
                    return true;
                }
            }
            return false;
        }
        return true;

    }

    /// <summary>
    /// 找到所有合法的资源 及其依赖关系
    /// </summary>
    /// <param name="data"></param>
    /// <param name="tar"></param>
    public static void AddCite(string[] data, Cites tar)
    {
        for (int i = 0; i < data.Length; i++)
        {
            var resPath = data[i];
            if (resPath != null && !Ignore(resPath))
            {
                if (!tar.dic.TryGetValue(resPath, out Cite _asset))
                {
                    _asset = new Cite(resPath);
                    tar.dic.Add(_asset.path, _asset);
                }

                var depends = AssetDatabase.GetDependencies(resPath, false);

                for (int j = depends.Length - 1; j >= 0; j--)
                {
                    var item = depends[j];
                    if (resPath != item && !Ignore(item))
                    {
                        if (!tar.dic.TryGetValue(item, out Cite cite))
                        {
                            cite = new Cite(item);
                            tar.dic.Add(item, cite);
                        }
                        //哪些资源需要依赖他
                        cite.AddCite(_asset, tar.dic);//asset需要cite
                    }
                    else
                    {
                        depends[j] = null;
                    }
                }
                AddCite(depends, tar);
            }
        }
    }

    public static Cites GetCiteLog()
    {
        string dir = "";//EditorPathTools.VERSION_LOG_PATH;
        string file = dir + "/CiteMsg.txt";
        if (!Directory.Exists(dir) || !File.Exists(file))
        {
            Directory.CreateDirectory(dir);
            return new Cites(1);
        }
        Cites cites = ProtobufTools.Deserialize<Cites>(File.ReadAllBytes(file)) as Cites;
        return cites;
    }

    /// <summary>
    /// 序列化到client\ITools\VersionMgr\VersionLog\CiteMsg.txt文件内
    /// </summary>
    /// <param name="data"></param>
    static void SaveCiteLog(Cites data)
    {
        string dir = EditorPathTools.VERSION_LOG_PATH;
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        ProtobufTools.SerializeToFile(dir + "/CiteMsg.txt", data);
    }

}

public class DiffBuilder
{
	static Dictionary<string, string> _DeleteData = new Dictionary<string, string>();
	static Dictionary<string, int> _UpdateData = new Dictionary<string, int>();
	static Dictionary<string, int> _AddData = new Dictionary<string, int>();

	static Dictionary<string, int> _NowFileSvn = new Dictionary<string, int>();
	static Dictionary<string, int> _BeforeFileSvn = new Dictionary<string, int>();

	static Dictionary<string, int> _ChangedDependFile = new Dictionary<string, int>();//SignFile

	static Cites _CitesMgr;
	static bool _StopBuilderForError;
    public static void MRealBuildABEditor()
    {
        AssetDatabase.Refresh();
        Bind();
        BuildTables.BuildChinese();
        AssetDatabase.Refresh();
        ClearCacheData();
        ClearABName();
        FindAllSignFile();//添加所有可标记的
        FindSameAb();
        SetSign();//设置ab标记
        BuilderLog();

        if (_StopBuilderForError)
        {
            //只要保证Newlog.txt是对的其他就不会出错
            Log.Error("停止资源更新 报错了，重置数据");
            return;
        }
        BuilderAB();

        //ClearABName();
        CreateMD5.BuilderMD5Record(true);
    }
    public static void RealBuildABEditor()
    {
        Bind();

        AssetDatabase.Refresh();
        BuildTables.BuildChinese();

        //0.清空标记
        AssetDatabase.Refresh();
        ClearCacheData();
        ClearABName();
        FindAllSignFile();//添加所有可标记的
        FindSameAb();
        SetSign();//设置ab标记
        BuilderLog();

        if (_StopBuilderForError)
        {
            //只要保证Newlog.txt是对的其他就不会出错
            Log.Error("停止资源更新 报错了，重置数据");
            return;
        }
        BuilderAB();

        ClearABName();
        CreateMD5.BuilderMD5Record(true);

    }
    static void BuilderLog()
    {
        string dir = EditorPathTools.VERSION_BUILDERLOG_PATH;
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        File.WriteAllText(dir + GetTime(),
            LitJson.JsonMapper.ToJson(_ChangedDependFile));//生成log
    }
    //全部查询设置标记
    private static void FindAllSignFile()
    {
        var _ResPaths = AssetDatabase.GetAllAssetPaths();
        foreach (var item in _ResPaths)
        {
            var data = NewResBuilder.CreateSimulateUnit(item);
            if (data != null)
            {
                PushData(data._Path, -1);//需要查询是否被依赖 查找同AB	
            }
        }
    }
    /*
     *  0.清除标记
        1.当前的文件svn版本和本地存储的版本比对差异 确定Add，Update,del,文件
        2.比对之后 如果发现有些资源本身不标记，但依赖他的资源会标记，那么先放到资源待定了，
        3.引用待定的资源挑选出来放到pending 并且已经确认 add，update集合里没有这个资源
        4.查找需要标记的同级资源
        5.查找add,update,pending,samefiles 依赖的资源，如果需要标记需要标记上
        6.本次操作生成日志 用于检验
        7.上标记
        8.拷贝本次svn记录
        9.清除标记
    */
    //[MenuItem("Builder/现有资源差异检查")]
	static void SignDiffFile()
	{
		//0.清空标记
		AssetDatabase.Refresh();
		ClearCacheData();
		ClearABName();
		_CitesMgr = CiteMgr.AddAll();//获得资源的关系图
		_NowFileSvn = ExternalTools.GetCurSvnLog();//获得当前工程需要处理资源版本信息
		_BeforeFileSvn = ExternalTools.GetLogDic();//上一次的__NowFileSvn
		FindDiff();//当前与上个版本对比  (_AddData)(_UpdateData)(_DeleteData)字典添加数据
		FindSignFile();//筛选(_AddData)(_UpdateData)(_DeleteData)按规则添加要标记数据
		FindSameAb();
		SetSign();//设置ab标记
		BuildLog();//生成log		
				   //9.清除标记
	}

    //[MenuItem("Builder/Real.BuildAB")]
	public static void BuildABEditor()
	{
		Bind();
		UpdateOther(false);
		BuildAB(false);
		CommitResRoot(false);
		Log.Error("Real.BuildAB  Over");
	}
   
    /// <summary>
    /// 保证资源处于最新状态（ 用SVN还原并更新资源 再重新处理资源 再上传到SVN ）
    /// </summary>
    /// <param name="isJenkins"></param>
    public static void UpdateOther(bool isJenkins)
	{
		//if (!isJenkins)
		//{
		//	SVNUpdate.Ctrl(SVNTYPE.REVERT, EditorPathTools.PROJECT_ROOT, EditorPathTools.PROJECT_LUA, EditorPathTools.SVN_RES_ROOT);//clear up
		//	SVNUpdate.Ctrl(SVNTYPE.UPDATE, EditorPathTools.PROJECT_ROOT, EditorPathTools.PROJECT_LUA, EditorPathTools.SVN_RES_ROOT);//clear up
		//}
		AssetDatabase.Refresh();
		ClearABName();
        //AndroidBuilder.BuilderDLL();
        BuildTables.BuildChinese();//更新表
		//ExportScene.ExportPrefabs();//更新场景
		//ShaderCollectionMgr.Collection();//收集shader keyword		
		BuilderLua();//更新lua

		//if (!isJenkins)
		//{
		//	SVNUpdate.Ctrl(SVNTYPE.COMMIT, EditorPathTools.PROJECT_MAPS, EditorPathTools.PROJECT_SCINFO, EditorPathTools.PROJECT_TABLES, EditorPathTools.PROJECT_SHADERS);
		//}
	}

    //[MenuItem("Builder/CollectionShader")]
    //public static void CollectionShader()
    //{
    //	ShaderCollectionMgr.Collection();//收集shader keyword
    //}


    //[MenuItem("Builder/只标记Lua数据")]
    static void BuilderLua()
    {
        //ToLuaMenu.BuildNotJitBundles();//更新lua
    }
    static void Bind()
	{
		_StopBuilderForError = false;
		Application.logMessageReceived -= HandleMessage;
		Application.logMessageReceived += HandleMessage;
	}
	static void BuildAB(bool isJenkins)
	{
        SignDiffFile();
        //BuilderLua();
        if (_StopBuilderForError)
		{
			//只要保证Newlog.txt是对的其他就不会出错
			Log.Error("停止资源更新 报错了，重置数据");
			return;
		}

		BuilderAB();

		#region//这里添加非压缩资源的打资源
		ClearABName();//清理标记
		SetSign(false);//添加不压缩标记
		BuilderAB(BuildAssetBundleOptions.UncompressedAssetBundle);
		#endregion
		ExternalTools.SaveAndCommitLog(_NowFileSvn);
		//SVNUpdate.Ctrl(SVNTYPE.COMMIT, EditorPathTools.SVN_RES_ROOT);//打完ab提交一次  后续有svn与本地的比较
	}

	static void CommitResRoot(bool isJenkins)
	{
		ClearABName();
		CreateMD5.BuilderMD5Record(true);
		if (!isJenkins)
		{
			//SVNUpdate.Ctrl(SVNTYPE.COMMIT, EditorPathTools.SVN_RES_ROOT);//把version 提交
		}
	}

	static void HandleMessage(string condition, string stackTrace, LogType type)
	{
		if (type == LogType.Error || type == LogType.Exception)
		{
			_StopBuilderForError = true;
		}
	}
    #region Jenkins
    public static void BuilderAbForJenkins()
	{
		Bind();
		//jenkins 更新svn
		UpdateOther(true);
		//jenskin 提交		

	}
	public static void BuilderAbForJenkins2()
	{
		Bind();
		BuildAB(true);
		//jenskin 提交
	}
	public static void BuilderAbForJenkins3()
	{
		CommitResRoot(true);
	}
    #endregion

    //shader变动以后，默认的增量打包很容易出问题，修改为强制重新打包
    static void BuilderAB(BuildAssetBundleOptions options = BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.ForceRebuildAssetBundle)
	{
		float _Time = Time.realtimeSinceStartup;
		var path = EditorPathTools.SVN_RES_ROOT;
        if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}
        var isIOS = EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS;
        BuildPipeline.BuildAssetBundles(path,
            options,
            isIOS ? EditorUserBuildSettings.activeBuildTarget : BuildTarget.Android);
        Log.Info("[BuilderAB ]花费时间=" + (Time.realtimeSinceStartup - _Time));
    }

	static void SetSign(bool compressed = true)
	{
		foreach (var item in _ChangedDependFile)
		{
            bool isuncompre = item.Key.Contains("Uncompressed");
            if ((compressed && isuncompre) || (!compressed && !isuncompre))
            {
                continue;
            }
            //最后添加标记的时候 在Environment下不标记
            if (!item.Key.Contains(NewEditorLoad._Environment))
			{
				var abPath = NewResBuilder.GetAbKeyBuilder(item.Key);
				if (!string.IsNullOrEmpty(abPath))
				{
					var importer = AssetImporter.GetAtPath(item.Key);
					if (importer != null)
					{
						importer.SetAssetBundleNameAndVariant(abPath, PathTools.SUFFIX);
					}
					else
					{
						//这种情况就是这个资源被干掉了
					}
				}
			}
		}
	}
	static void BuildLog()
	{
		Dictionary<string, string> log = new Dictionary<string, string>();
		log.Add("增加", LitJson.JsonMapper.ToJson(_AddData));
		log.Add("更新", LitJson.JsonMapper.ToJson(_UpdateData));
		log.Add("删除文件", LitJson.JsonMapper.ToJson(_DeleteData));
		log.Add("被标记AB", LitJson.JsonMapper.ToJson(_ChangedDependFile));
		string dir = EditorPathTools.VERSION_BUILDERLOG_PATH;
		if (!Directory.Exists(dir))
		{
			Directory.CreateDirectory(dir);
		}
		File.WriteAllText(dir + GetTime(),
			LitJson.JsonMapper.ToJson(log));
	}
	static string GetTime()
	{
		//获取当前时间
		var hour = DateTime.Now.Hour;
		var minute = DateTime.Now.Minute;
		var second = DateTime.Now.Second;
		var year = DateTime.Now.Year;
		var month = DateTime.Now.Month;
		var day = DateTime.Now.Day;
		//格式化显示当前时间
		return string.Format("{0:D2}{1:D2}{2:D2}_{3:D2}_{4:D2}_{5:D2}", year, month, day, hour, minute, second);
	}

	/// <summary>
	/// 通过已经标记的资源 找到该资源所在AB下的所有资源一起添加到标记
	/// </summary>
	static void FindSameAb()
	{
		List<string> _Data = new List<string>();
		foreach (var item in _ChangedDependFile)
		{
			var ab = NewResBuilder.GetNewResAb(item.Key);
			if (ab != null)//==null 这种情况，说明被干掉的资源，彻底没有相关的ab了
			{
				for (int i = 0; i < ab._Count; i++)
				{
					_Data.Add(ab[i]._Path);
				}
			}
		}
		for (int i = 0; i < _Data.Count; i++)
		{
			PushData(_Data[i], 2);
		}

		//============================所有资源在查询一遍
		//clear
		_Data.Clear();
		foreach (var item in _ChangedDependFile)
		{
			var ab = NewResBuilder.GetNewResAb(item.Key);
			if (ab != null)//==null 这种情况，说明被干掉的资源，彻底没有相关的ab了
			{
				for (int i = 0; i < ab._Count; i++)
				{
					_Data.Add(ab[i]._Path);
				}
			}
		}
		for (int i = 0; i < _Data.Count; i++)
		{
			PushData(_Data[i], 2);
		}
	}
	static void ClearCacheData()
	{
		_AddData.Clear();
		_DeleteData.Clear();
		_UpdateData.Clear();
		_ChangedDependFile.Clear();
		_NowFileSvn.Clear();
		_BeforeFileSvn.Clear();
	}

	/// <summary>
	/// 前版本与现版本对比  增加(_AddData)  删除(_DeleteData)  差异(_UpdateData)
	/// </summary>
	static void FindDiff()
	{
		foreach (var item in _NowFileSvn)
		{
			if (!_BeforeFileSvn.TryGetValue(item.Key, out int _BeforeV))
			{
				_AddData.Add(item.Key, item.Value);
			}
			else
			{//如果版本一致就不要添加
				if (item.Value != _BeforeV)
				{
					_UpdateData.Add(item.Key, item.Value);
				}
			}
		}
		foreach (var item in _BeforeFileSvn)
		{
			if (!_NowFileSvn.TryGetValue(item.Key, out int v))
			{
				_DeleteData.Add(item.Key, "");
			}
		}
	}

	/// <summary>
	/// 添加标记 并对path依赖的所有资源按规则决定是否添加标记
	/// </summary>
	/// <param name="path"></param>
	/// <param name="isAdd"></param>
	static void PushData(string path, int isAdd = 1)
	{
		if (!_ChangedDependFile.ContainsKey(path))
		{
			_ChangedDependFile.Add(path, isAdd);
			DependRes(path);
		}
	}
	/// <summary>
	/// 对path依赖的所有资源按规则决定是否添加标记
	/// </summary>
	/// <param name="path"></param>
	static void DependRes(string path)
	{
		//查看这个资源依赖的资源需不需要标记
		var datas = AssetDatabase.GetDependencies(path, false);
		if (datas != null)
		{
			for (int i = 0; i < datas.Length; i++)
			{
				var temp = datas[i];
				if (NewResBuilder.NeedAbSign(temp, out NewResUnit _ResUnit))
				{
					PushData(temp);
				}
			}
		}
	}

	/// <summary>
	/// 检查被依赖的资源是否添加标记 若通过则以被依赖项起无限向下检查依赖项是否添加标记
	/// </summary>
	/// <param name="v"></param>
	static void SignFileDepend(Cite v)
	{
		for (int i = 0; i < v.unit.Count; i++)
		{
			var temp = v.unit[i];
			SignFileDepend(temp);
			if (NewResBuilder.NeedAbSign(temp.path, out NewResUnit _ResUnit))
			{
				PushData(temp.path);
			}
		}
	}
	static void FindAddUpdateSignFile(Dictionary<string, int> _Data)
	{
		//1.先通过【增/改】中是否有需要标记的，先加入到标记行列中
		//2.删 需要查找之前属于哪个资源		
		foreach (var item in _Data)
		{
			if (NewResBuilder.NeedAbSign(item.Key, out NewResUnit _ResUnit))
			{
				//if(DebugMgr.CanLog()) DebugMgr.Log("需要被标记=" + item.Key);
				PushData(item.Key);
			}
			//  即使被标记了也需要查是否被依赖
			if (_CitesMgr.dic.TryGetValue(item.Key, out Cite v))
			{
				SignFileDepend(v);
			}
		}
	}

	/// <summary>
	/// 对(_AddData)(_UpdateData)(_DeleteData)按规则添加标记  这里依赖查询不忽略Environment
	/// </summary>
	static void FindSignFile()
	{
		FindAddUpdateSignFile(_AddData);
		FindAddUpdateSignFile(_UpdateData);

		foreach (var item in _DeleteData)
		{
			var data = NewResBuilder.CreateSimulateUnit(item.Key);
			if (data != null)
			{
				//需要查询是否被依赖 查找同AB			
				PushData(data._Path, -1);

				//,出现被删除的资源不属于标记,那么这个资源如果属于被依赖的资源，被标记的资源一定会修改 不然肯定出错。 这个地方需要测试一下#GLZ
				Log.Warning("可能需要被依赖=" + data._Path);
			}
		}
	}

    public static void ClearABName()
	{
		float _Time = Time.realtimeSinceStartup;
		HashSet<string> hash = new HashSet<string>();
		string[] assetBundleNames = AssetDatabase.GetAllAssetBundleNames();

		for (int i = 0; i < assetBundleNames.Length; i++)
		{
			string[] aFiles = AssetDatabase.GetAssetPathsFromAssetBundle(assetBundleNames[i]);
			foreach (var item in aFiles)
			{
				if (!hash.Contains(item))
				{
					hash.Add(item);
				}
			}
		}
		foreach (var item in hash)
		{
			AssetImporter.GetAtPath(item).SetAssetBundleNameAndVariant("", "");
		}
		AssetDatabase.RemoveUnusedAssetBundleNames();
		Log.Info("[ClearABName3 ]花费时间=" + (Time.realtimeSinceStartup - _Time));
	}
}
