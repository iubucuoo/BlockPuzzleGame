using System.IO;
using System.Text;
using UnityEngine;
using WUtils;

[ProtoBuf.ProtoContract]
public class NewResAb
{
	[ProtoBuf.ProtoMember(1)]
	public int _ID;//ID先通过旧配置进行赋值，然后新的配置从新生成。生成的规则1.必须保存唯一性，
	[ProtoBuf.ProtoMember(2)]
	public string _AbName;
	[ProtoBuf.ProtoMember(3)]
	public string _AbPath;
	[ProtoBuf.ProtoMember(4)]
	public NewResUnit[] _Data;
	[ProtoBuf.ProtoMember(5)]
	public int[] _Dependencies;
	[ProtoBuf.ProtoMember(6)]
	public int _Size;//ABSize
	[ProtoBuf.ProtoMember(7)]
	public int _VersionNum;
	[ProtoBuf.ProtoMember(8)]
	public int _DownloadID;//出包的时候根据表来确定，1.表中有数据，在包外。2.表中无数据，在包内。默认为【0】，这个字段 只是用来确认章节，-1代表需要修复[-1代表补丁]
	[ProtoBuf.ProtoMember(9)]
	public bool _IsFinish;
	public int _CurSize;
	public int _curSize { get { if (_IsFinish) { return _Size; } return _CurSize; } }
	public int _Count { get { return _Data == null ? 0 : _Data.Length; } }
	public NewResUnit this[int index] { get { return _Data == null ? null : _Data[index]; } set { _Data[index] = value; } }

	public int _MapID;//请求资源的时候 所在的MapID



	string _InPkgStr;
	string _ExtPkgStr;
	int _LoadTime;
	public LoadStatus _LoadStatus { get; set; }
	public AssetBundle _AssetBundle;
	public const int _RemoveTime = 60000;

	public float _StartLoadTime;

	public void SetAb(AssetBundle _ab)
	{
		_AssetBundle = _ab;
		_LoadTime = (int)(Time.time * 1000);
	}
	public bool GetNeedDownloadOnLogin()
	{
		return _DownloadID == -1 && _IsFinish == false;
	}
	public string GetPath(bool isInPkg)
	{
		if (isInPkg)
		{
			if (string.IsNullOrEmpty(_InPkgStr))
			{
				_InPkgStr = PathTools.STREAM_RES_PATH + AbPathAndSuffix(_AbPath);
			}
		}
		else
		{
			if (string.IsNullOrEmpty(_ExtPkgStr))
			{
				_ExtPkgStr = PathTools.CACHE_RES_ROOT + AbPathAndSuffix(_AbPath);
			}
		}
		return (isInPkg ? _InPkgStr : _ExtPkgStr);
	}
	public string GetPathBak()
	{
		return GetPath(false) + ".bak";
	}
	internal void Move()
	{
		var bak = GetPathBak();
		if (File.Exists(bak))
		{
			var desc = GetPath(false);
			if (File.Exists(desc))
			{
				File.Delete(desc);
			}
			File.Move(bak, GetPath(false));
		}
	}


	public bool GetLoadPath(out string path, int _tempMapID)
	{
		_MapID = _tempMapID;
		path = null;
		if (_DownloadID == 0)
		{
			path = GetPath(true);
			return true;
		}
		else if (_IsFinish)
		{
			path = GetPath(StaticTools._LuaLoadError);
			return true;
		}
		return false;//这种情况需要弹窗说需要下载资源
	}
	public string GetDir()
	{
		return PathTools.CACHE_RES_ROOT + this[0]._ModelName;
	}
	string AbPathAndSuffix(string _abpath)
	{
		if (string.IsNullOrEmpty(_abpath))
		{
			return _abpath;
		}
		if (_abpath.Contains(PathTools.DllABPath))
		{
			return _abpath + PathTools.Dot_Logic;
		}
		else
		{
			return _abpath + PathTools.DOT_LY;
		}
	}
	public string _NetUrl { get { return PathTools.GetNetUrlPath + AbPathAndSuffix(_AbPath); } }

	public NewResAb()
	{

	}

	public NewResAb(string name)
	{
		_AbName = name;
	}
	internal void PushUnit(NewResUnit unit)
	{
		for (int i = 0; i < _Count; i++)
		{
			var temp = this[i];
			if (temp._ObjName == unit._ObjName)
			{
				this[i] = unit;
				return;
			}
		}
		var _TempDatas = new NewResUnit[_Count + 1];
		for (int i = 0; i < _Count; i++)
		{
			_TempDatas[i] = this[i];
		}
		_TempDatas[_Count] = unit;
		_Data = _TempDatas;
	}

	internal string ToDebug()
	{
		StringBuilder sb = new StringBuilder();
		sb.Append("AbID=" + _ID + ",Version=" + _VersionNum + ",_Size=" + _Size);
		if (_Dependencies != null)
		{
			sb.Append(",_Dependencies=" + string.Join(",", _Dependencies) + ",");
		}
		sb.Append(",AbPath=" + ((_AbPath == null) ? "" : _AbPath));
		for (int i = 0; i < _Count; i++)
		{
			sb.Append(this[i].ToDebug());
			sb.Append("\n");
		}
		return sb.ToString();
	}

	internal bool GetObj(IArt art)
	{
		if (art.IsWaitArt())
		{
			for (int i = 0; i < _Count; i++)
			{
				var temp = this[i];
				if (temp._ObjName == art.ArtName())
				{
					art.UseArt(GetNative(temp._Path));
					return true;
				}
			}
		}

		//Debug.LogError("未找到" + _AbName + "," + art.ArtName());
		if(art is ArtBase)
		{
			(art as ArtBase).FailArt();
		}
		return false;
	}
	internal bool GetObj(string artName, out NewResUnit unit)
	{
		unit = null;
		for (int i = 0; i < _Count; i++)
		{
			var temp = this[i];
			if (temp._ObjName == artName)
			{
				unit = temp;
				return true;
			}
		}
		//Debug.LogError("未找到" + _AbName + "," + art.ArtName());
		return false;
	}

	Object GetNative(string path)
	{
#if UNITY_EDITOR
		return ObjectMgr.LoadMainAssetAtPath(path);
#else
        return null;
#endif
	}

	internal bool FindResUnit(string path, out NewResUnit temp)
	{
		temp = null;
		for (int i = 0; i < _Count; i++)
		{
			temp = this[i];
			if (temp.FindResUnit(path))
			{
				return true;
			}
		}
		return false;
	}
	internal void CopyAbParams(NewResAb other)
	{
		_Dependencies = other._Dependencies;//1.先拷贝相关的数据，2.以覆盖的形式处理依赖数据
	}
	public void SetDependencies(int[] depens)
	{
		_Dependencies = depens;
	}
	public void UnloadAbAndObj()
	{
		var time = (int)(Time.time * 1000);
		if (_LoadStatus == LoadStatus.Done && time - _LoadTime > _RemoveTime && _MapID != StaticTools.CurrentMapID)
		{
			_LoadStatus = LoadStatus.None;
			try
			{
				_AssetBundle.Unload(false);
			}
			catch
			{
                DebugMgr.LogError(string.Format( "AssetBundle 回收异常处理, AbPath={0}", _AbPath));
			}
		}

		for (int i = 0; i < _Count; i++)
		{
			var unit = this[i];
			if ((unit._LoadTime - _LoadTime) > _RemoveTime)
			{
				unit._Obj = null;
			}
		}
	}
}

