using System.Collections.Generic;
using UnityEngine;

public class PackageMgr
{
	static PackageMgr instance;
	static PackageMgr inst { get { return instance ?? (instance = new PackageMgr()); } }
	static readonly Dictionary<string, ImageArt> _ResourceDatas = new Dictionary<string, ImageArt>();
	//string this[int packageNum] { get { return LuaUtils.GetPackageName(packageNum); } }
	PackageMgr()
	{
		TimeMgr.Instance.AddIntervelEvent(TimerClear, 10000, -1);
	}

	

	/// <summary>
	/// 回调CallBack
	/// </summary>
	/// <param name="packageName"></param>
	/// <param name="cbv"></param>
	/// <param name="unClear"></param>
	public static void LoadObject(string packageName, System.Action cbv, bool unClear = false)
	{
		if (string.IsNullOrEmpty(packageName))
		{
			DebugMgr.LogError("资源包名BUG");
			return;
		}
		if (IsLoaded(packageName))
		{
			MarkResource(packageName);
			cbv();
		}
		else
		{
			if (_ResourceDatas.TryGetValue(packageName, out ImageArt art))
			{
				art.AddCb(cbv);
			}
			else
			{
				_ResourceDatas.Add(packageName, new ImageArt(packageName, cbv, unClear));
			}
		}
	}
	//public static GObject CreateObject(int packageNum, string resName)
	//{
	//	return CreateObject(inst[packageNum], resName);
	//}

	//internal static GObject CreateObject(string pakName, string resName)
	//{
	//	if (IsLoaded(pakName))
	//	{
	//		return UIPackage.CreateObject(pakName, resName).asCom;
	//	}
	//	else
	//	{
	//		if(DebugMgr.CanLogError()) DebugMgr.LogError("资源包未加载 + " + pakName);
	//		return null;
	//	}
	//}

	public static void RemoveAll()
	{
		var e = _ResourceDatas.GetEnumerator();
		//while (e.MoveNext())
		//{
		//	if (IsLoaded(e.Current.Key))
		//		UIPackage.RemovePackage(e.Current.Key);
		//}
		_ResourceDatas.Clear();
		e.Dispose();
	}

	public static void RemovePackage(int packageNum)
	{
		//RemovePackage(inst[packageNum]);
	}

	static bool IsLoaded(string wnd)
	{
		return false;
	}

	static void RemovePackage(string packageName)
	{
		inst.ClearMark(packageName);
	}
	/// <summary>
	/// 加载一次标记
	/// </summary>
	/// <param name="packageName"></param>
	/// <param name="onlyLoad"></param>
	static void MarkResource(string packageName)
	{
		if (_ResourceDatas.TryGetValue(packageName, out ImageArt temp))
			temp.Mark();
	}
	/// <summary>
	/// 卸载一次标记
	/// </summary>
	/// <param name="packageName"></param>
	void ClearMark(string packageName)
	{
		if (_ResourceDatas.TryGetValue(packageName, out ImageArt temp))
			temp.ClearMark();
	}

	/// <summary>
	/// 计时器卸载闲置资源
	/// </summary>
	/// <param name="t1"></param>
	/// <param name="t2"></param>
	void TimerClear(int t1, float t2)
	{
		var e = _ResourceDatas.GetEnumerator();
		while (e.MoveNext())
		{
			var value = e.Current.Value;
			if (!value.NeedClear()) continue;
			var key = e.Current.Key;
			if (IsLoaded(key))
			{
				value.RemoveData();
				_ResourceDatas.Remove(key);
				break;
			}
		}
		e.Dispose();
	}
}
