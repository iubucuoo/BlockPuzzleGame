using MEC;
using System.Collections.Generic;
using UnityEngine;

public class CommonArt : ArtBase, IPoolable
{
	public override int _ModelID => _ID;

	public IPoolsType PoolType => IPoolsType.CommonArt;

	public bool IsRecycled { get; set; }

	public override ResSort _Sort => ResSort.Config;

	public override bool _CanCacheAb { get { return _ModelID != (int)RES_MODEL_INDEX.lua; } }

	string _AbName;
	string _ObjName;
	int _ID;
	System.Action<Object> _CB = null;
	System.Action<Object[]> _CBs = null;

	System.Action _CB2 = null;

	public override string AbSingleName()
	{
		return _AbName;
	}

	public override string ArtName()
	{
		return _ObjName;
	}
	public CommonArt SetValue(int modelID, string abName, string objName, System.Action<Object> cb = null, System.Action cb2 = null)
	{
		_ID = modelID;
		_AbName = abName;
		_ObjName = objName;
		_CB = cb;
		_CB2 = cb2;
		return this;
	}
	public CommonArt SetValue(int modelID, string abName, string objName, System.Action<Object[]> cb = null, System.Action cb2 = null)
	{
		_ID = modelID;
		_AbName = abName;
		_ObjName = objName;
		_CBs = cb;
		_CB2 = cb2;
		return this;
	}



	public void Dispose()
	{
		throw new System.NotImplementedException();
	}


	public override IEnumerator<float> Loading(AssetBundle ab)
	{
		if (string.IsNullOrEmpty(ArtName()))
		{
			if (_CBs == null)
			{
				UseArt(ab);
			}
			else
			{
				var temp = ab.LoadAllAssetsAsync();
				yield return Timing.WaitUntilDone(temp);
				UseArt(temp.allAssets);
			}
		}
		else
		{
			var temp = ab.LoadAssetAsync(ArtName());
			yield return Timing.WaitUntilDone(temp);
			UseArt(temp.asset);
		}
	}

	public void OnRecycled()
	{
		throw new System.NotImplementedException();
	}

	public override void UseArt(object obj)
	{
        _CB?.Invoke(obj as Object);
    }
	public override void UseArt(object[] objs)
	{
        _CBs?.Invoke(objs as Object[]);
    }

	public override void FailArt()
	{
        _CB2?.Invoke();
    }
}
