using MEC;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
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
}

public class EffectModel : IEffectModel, IArt
{
    System.Action _Destory;
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
	void Init( string pkgName, string resName, bool islink = false)
	{
		_PkgName = pkgName;
		_ResName = resName;
        AppParam._EditorEffectModelNum++;
        AppParam._EditorEffectNum++;
		if (!_IsLoaded)
		{
			_IsLoaded = true;
			_CacheTransform = new GameObject().transform;
            MsgSend.GetRes(this);
		}
	}
	public void SetValue(System.Action _destory,string _PkgName, string _ResName, EffectBaseData basedata, float _Multiple)
	{
        _Destory = _destory;
        Init(_PkgName, _ResName);
		InitPos(basedata);						
		var temp = 1 / lossyScale.x;
		localScale = new Vector3(_Multiple * temp, temp, _Multiple * temp);
		ReleaseSys(true);
	}
	void ReleaseSys(bool b)
	{
		if (_HaveSys && _Sys != null)
		{
			_Sys.ReleaseSkillsSerialize(b);
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
			else
            {
                _Over?.Invoke();
            }
        }
		ReleaseSys(true);
        TimeMgr.Instance.AddIntervelEvent((x, y) => {
            _Destory();
        }, (int)(_Sys._LongTime*1000), 1);

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
	void InitPos(EffectBaseData basedata)
	{
        SetParent(_CacheTransform, ModelPools.pool);
        _CacheTransform.localEulerAngles = Vector3.zero;
        _CacheTransform.position = new Vector3(basedata.offsetx, basedata.offsety, basedata.offsetz);
        
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
				else
                {
                    _Over?.Invoke();
                }
            }
			else
			{
				readyOver = true;
				if(_Sys == null && _Over != null)
					_Over();
			}
		}
		catch (System.Exception e)
		{//增加异常捕捉，之前陈文强出现了一次 报错
			Log.Error(string.Format("资源回收出现异常pkgName={0}，ResName={1},{2}", _PkgName, _ResName, e.ToString()));
		}
	}
}
