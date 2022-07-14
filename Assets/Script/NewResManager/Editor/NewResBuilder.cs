
using System.IO;
using UnityEditor;
using UnityEngine;

public class NewResBuilder : Editor
{
	static NewResMgr _NewResMgr;
	//1.先生成当前的资源
	//2.资源比对【工程内的资源与上个版本进行比对，版本有差异，相应的资源需要处理】
	[MenuItem("Builder/生成BuilderAB.txt 用于查看")]
	public static void BuilderAB()
	{
		_NewResMgr = null;
		_NewResMgr = GetNewResMgr();

		File.WriteAllText(Application.dataPath + "/BuilderAB.txt", LitJson.JsonMapper.ToJson(_NewResMgr._Data));
	}
	public static bool NeedAbSign(string path, out NewResUnit temp)
	{
		GetNewResMgr();
		return _NewResMgr.FindResUnit(path, out temp);
	}
	public static NewResUnit CreateSimulateUnit(string path)
	{
		GetNewResMgr();
		return NewEditorLoad.CreateUnit(path);
	}
	public static string GetAbKeyBuilder(string path)
	{
		GetNewResMgr();
		if (_NewResMgr.FindResUnit(path, out NewResUnit temp))
		{
			return temp._ModelName + "/" + temp._AbName;
		}
		return null;
	}
	public static NewResAb GetNewResAb(string path)
	{
		GetNewResMgr();
		if (_NewResMgr.FindResUnit(path, out NewResUnit temp))
		{
			if (_NewResMgr.GetAB(temp._ModelID, temp._AbName, out NewResAb ab))
			{
				return ab;
			}
		}
		return null;
	}
	static NewResMgr GetNewResMgr()
	{
		if (_NewResMgr == null)
		{
			_NewResMgr = new NewEditorLoad().BuilderResData();
		}
		return _NewResMgr;
	}
	//[MenuItem("Builder/Version.bytes转Json")]
	//public static void VersionToJson()
	//{
	//	var version = new NewResMgr(File.Exists(IPathTools.CACHE_VERSION) ? File.ReadAllBytes(IPathTools.CACHE_VERSION) : null);
	//	File.WriteAllText(Application.dataPath + "/version.json", LitJson.JsonMapper.ToJson(version._Data));
	//	Debug.Log("over"+ File.Exists(IPathTools.CACHE_VERSION));
	//}
	//[MenuItem("Builder/按差异拷贝数据到StreamAsset")]
	//public static void CopyInPkg()
	//{
	//	BuildApkTools.CopyToProject();
	//}
}
