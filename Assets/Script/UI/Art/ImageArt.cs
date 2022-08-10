using System;
using System.Collections.Generic;
using UnityEngine;

public class ImageArt : ArtBase
{
	string packageName;
	string _LowPkgName;
	private float markTime;
	private int index = 1;
	private bool unClear;
	public System.Action Cb;

	public override int _ModelID => (int)RES_MODEL_INDEX.uiwnds;

	public override ResSort _Sort => ResSort.UI;
	public override bool _CanCacheAb => false;
    private Dictionary<string, UnityEngine.Object> AllAssets = new Dictionary<string, UnityEngine.Object>();
    public ImageArt(string pkName, System.Action cv, bool _unClear)
	{
		packageName = pkName;
		unClear = _unClear;
		Cb = cv;
		LoadRes();
	}

	private void LoadRes()
	{
		if (StaticTools.LoadArtIsAb)
		{
			_LowPkgName = packageName.ToLower();
			MsgSend.GetRes(this);
		}
		else
		{
			LoadEditor(packageName);
		}
	}

	protected void LoadEditor(string packageName)
	{
		var path = StaticTools.CombStr("Assets/Art/UIWnds/", packageName, "/", packageName);
		Cb();
	}

	public void AddCb(Action cbv)
	{
		Cb += cbv;
	}
	public void Mark()
	{
		index++;
	}

	public void ClearMark()
	{
		index--;
		markTime = Time.time;
	}
	public bool NeedClear()
	{
		if (unClear)
		{
			return false;
		}
		return index <= 0 && Time.time - markTime > 60;
	}



	public override string AbSingleName()
	{
		return _LowPkgName;
	}


    public override IEnumerator<float> Loading(AssetBundle ab)
    {
        var objs = ab.LoadAllAssets();
        for (int j = 0; j < objs.Length; j++)
        {
            string objname = objs[j].name;
            //Debug.LogError(objname);
            if (AllAssets.ContainsKey(objname))
            { AllAssets[objname] = objs[j];Debug.Log("�ظ���Դ     "+packageName +  "     " + objname);
            }
            else
                AllAssets.Add(objs[j].name, objs[j]);
        }
        UseArt(objs);
        yield return 0;
        ab.Unload(false);
    }

    public override void UseArt(object[] objs)
    {
        if (AllAssets.ContainsKey(packageName))
        {
            PackageMgr.AddLoadPackage(packageName, AllAssets[packageName]);
        }
        else
        {
            PackageMgr.AddLoadPackage(packageName, null);
        }
        Cb();
	}

	public override void FailArt()
	{

	}

	internal void RemoveData()
	{
        PackageMgr.RemoveLoadPackage(packageName);
    }
}
