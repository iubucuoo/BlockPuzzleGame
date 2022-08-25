using MEC;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
public enum BINDTYPE
{
    NONE,//NONE
    ROTATE,//受父级坐标/旋转影响
    COORDINATE,//受父级坐标影响
    SUPER_ROTATE,//超类旋转影响
}
public struct EffectBaseData
{
	public float offsetx;
	public float offsety;
	public float offsetz;
}

//针对单个特效定义结构
//通过创建一个空的GameObject 去做相应的空间逻辑
//什么形式播
//播什么特效，在什么时间，什么地点播，什么时候回收
public abstract class IEffectModel
{
	public string _PkgName;
	public string _ResName;
	protected GameObject _Obj;
	protected bool _IsLoaded;
	protected ParticalSystemData _Sys;
	protected bool _HaveSys;
	protected System.Action _Over;
	Transform _CacheTrans;
	protected Transform _CacheTransform { get { return _CacheTrans; } set { if (_IsLoaded) _CacheTrans = value; } }
	public Vector3 position { get { return _IsLoaded ? _CacheTransform.position : Vector3.zero; } set { if (_IsLoaded) _CacheTransform.position = value; } }
	public Vector3 localScale { get { return _CacheTransform.localScale; } set { if (_IsLoaded) _CacheTransform.localScale = value; } }
	public Vector3 lossyScale { get { return _CacheTransform.lossyScale; } }
	public Transform Parent { get { return _CacheTransform.parent; } set { if (_IsLoaded) _CacheTransform.parent = value; } }

	public string _Name
	{
		set
		{
#if UNITY_EDITOR
			if (_IsLoaded) { _CacheTransform.name = value; }
#endif
		}
	}

}

public class EffectModel : IEffectModel, IArt
{
	Vector3 _Diff;
	bool isLink;
	bool isBindBone;
	bool readyOver;
	public EffectModel()
	{
		_Over = Over;
		_IsLoaded = false;
	}

	public int _ModelID => (int)RES_MODEL_INDEX.effects;
	public ResSort _Sort => ResSort.Other;

	public NewResAb GetNewResAb { get { ResCenter.inst._ResMgr.GetAB(_ModelID, AbSingleName(), out NewResAb ab); return ab; } }
	public NewResUnit GetNewResUnit { get { ResCenter.inst._ResMgr.GetObj(_ModelID, ArtName(), out NewResUnit unit); return unit; } }

	public bool _CanCacheObj => true;

	public bool _CanCacheAb => true;

	public int _MapID { get => 0; set => throw new System.NotImplementedException(); }

	public int AbSingleID()
	{
		throw new System.NotImplementedException();
	}
	public bool IsWaitArt(int key)
	{
		return true;
	}
	public string AbSingleName()
	{
		return _PkgName;
	}

	public string ArtName()
	{
		return _ResName;
	}

	public bool ComportRes(string abName, string artName)
	{
		throw new System.NotImplementedException();
	}

	public IEnumerator<float> Loading(AssetBundle ab)
	{
		var unit = GetNewResUnit;
		var objs = ab.LoadAssetAsync(ArtName());
		yield return Timing.WaitUntilDone(objs);
		var result = objs.asset;
		unit.SetObj(result);
		UseArt(result);
	}
	int Init( string pkgName, string resName, bool islink = false)
	{
		_PkgName = pkgName;
		_ResName = resName;
        AppParam._EditorEffectModelNum++;
        AppParam._EditorEffectNum++;
		//if (!AppParam._IsUpdateFrame)
		//{
		//	return -1;
		//}
		if (!_IsLoaded)
		{
			_IsLoaded = true;
			_CacheTransform = new GameObject().transform;
			isLink = islink;
			CreateEffect(_PkgName, _ResName);
			return 0;
		}
		return 1;
	}
	public EffectModel SetValue(ref EffectBaseData basedata, string _PkgName, string _ResName, float _Multiple)
	{
		if (Init( _PkgName, _ResName) != -1)
		{
			InitPos( ref basedata);
			//#GLZ 因为骨骼的原因，需要x100,后面再看有没有更好的方式						
			if (!isBindBone)
			{
				localScale = new Vector3(_Multiple, 1, _Multiple);
			}
			else
			{
				localScale = Vector3.one;
				var temp = 1 / lossyScale.x;
				localScale = new Vector3(_Multiple * temp, temp, _Multiple * temp);
			}
			ReleaseSys(true);
		}
		return this;
	}

	public EffectModel SetValue(string pkgName, string resName,  Vector3 diff)
	{
 
		_Diff = diff;
		var b = Init(pkgName, resName, true);
		if (b == 1)
		{
			SetLink();
			ReleaseSys(true);
		}
		return this;
	}
	void ReleaseSys(bool b)
	{
		if (_HaveSys && _Sys != null)
		{
			_Sys.ReleaseSkillsSerialize(b);
		}
	}
	void SetLink()
	{
		if (isLink)
		{
			if (_HaveSys && _Sys != null)
			{
				_Sys.SetLineAB(_Diff);
			}
		}
	}
    public void ResetParent(Transform parent, Transform go)
    {
        go.parent = parent;
        go.localEulerAngles = Vector3.zero;
        go.localPosition = Vector3.zero;
        go.localScale = Vector3.one;
    }
    public void UseArt(object obj)
	{
		var eff = (ObjectMgr.InstantiateObj((Object)obj) as GameObject);
		var eff_trans = eff.transform;
		ResetParent(_CacheTransform, eff_trans);
        _Sys = eff_trans.GetComponentInChildren<ParticalSystemData>(true);
		if(_Sys != null)
			_HaveSys = true;
		if (readyOver)
		{
			if(_Sys != null)
				_Sys.Reset(_Over);
			else if(_Over != null)
			{
				_Over();
			}
				
		}
		SetLink();
		ReleaseSys(true);
	}
	public void UseArt(object[] objs)
	{
		throw new System.NotImplementedException();
	}

	//实例化之后，直接跟父节点一致
	void CreateEffect(string pkgName, string resName)
	{
		_PkgName = pkgName;
		_ResName = resName;
		//随着动作结束而结束
		MsgSend.GetRes(this);
	}
	void InitPos( ref EffectBaseData _Data)
	{
        var offsetx = _Data.offsetx;
        var offsety = _Data.offsety;
        var offsetz = _Data.offsetz;
        //如果是被动释放并且是要绑定旋转的，需要设置成绑定坐标
        SetParent(_CacheTransform, ModelPools.pool);
        _CacheTransform.localEulerAngles = Vector3.zero;
        float sin = 0;
        float cos = 0;
        float locationX = sin * offsetz + cos * offsetx;
        float locationZ = cos * offsetz - sin * offsetx;
        _CacheTransform.localPosition = new Vector3(locationX, offsety, locationZ);
        
    }
    public void SetParent(Transform A, Transform B)
    {
        A.parent = B;
    }
    //销毁逻辑进行封装
    public void Over()
	{
		Parent = ModelPools.pool;
		position = Vector3.one * -5000;
		//需要延迟处理特效
		ModelPools.Push(this);
        AppParam._EditorEffectModelNum--;
        if (_IsLoaded)
		{
            AppParam._EditorEffectNum--;
        }
    }
	public void Destroy()
	{
		try
		{
			if (_HaveSys)
			{
				if(_Sys != null)
					_Sys.Reset(_Over);
				else if(_Over != null)
				{
					_Over();
				}

			}
			else
			{
				readyOver = true;
				//这里直接调用over不妥
				if(_Sys == null && _Over != null)//如果Sys为空，那么复用的时候也没法回收，所以这里直接回收了
					_Over();
			}
		}
		catch (System.Exception e)
		{//增加异常捕捉，之前陈文强出现了一次 报错
			DebugMgr.LogError(string.Format("资源回收出现异常pkgName={0}，ResName={1},{2}", _PkgName, _ResName, e.ToString()));
		}
	}
}
