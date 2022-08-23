using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UiWndArt : ArtBase
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
    public UiWndArt(string pkName, System.Action cv, bool _unClear)
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
#if UNITY_EDITOR
        UnityEngine.Object pkgobj=null;
        var Dirpath = StaticTools.CombStr("Assets/Art/UIWnds/", packageName, "/");
        //Debug.LogError("LoadEditor   "+packageName);
        if (Directory.Exists(Dirpath))
        {
            DirectoryInfo direction = new DirectoryInfo(Dirpath);
            FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
           // Debug.Log(files.Length);
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Name.EndsWith(".meta"))
                {
                    continue;
                }
                //Debug.Log("Name:" + files[i].Name + "  |   " + files[i].FullName);
                var _fullname = files[i].FullName;
                var _name = files[i].Name;
                if (_name.EndsWith(".png"))
                {
                    string _path = _fullname.Substring(_fullname.IndexOf("Assets"));
                    UnityEngine.Object[] sprites = UnityEditor.AssetDatabase.LoadAllAssetsAtPath(_path);
                    foreach (var item in sprites)
                    {
                        //Debug.Log("__sprite allcount:" + sprites.Length+ " __sprite name:" + item.name ); 
                        AddAllAssets(item.name, item);
                    }
                }
                else if (_name.EndsWith(".prefab"))
                {
                    string _path = _fullname.Substring(_fullname.IndexOf("Assets"));
                    var obj = ObjectMgr.LoadMainAssetAtPath(_path);
                    if (obj.name == packageName)
                    {
                        pkgobj = obj;
                        //Debug.Log("AddLoadPackage  " + obj.name);
                    }
                    //Debug.Log("__prefab allcount:1  __prefab name:" + obj.name);
                    AddAllAssets(obj.name, obj);
                }
            }
        }
        PackageMgr.AddLoadPackage(packageName, pkgobj);
#endif
    }

	public void AddCb(Action cbv)
	{
		Cb += cbv;
	}
	public UnityEngine.Object GetRes(string objname)
    {
        if (AllAssets.ContainsKey(objname))
        {
            return AllAssets[objname];
        }
        Debug.LogError("找不到   " + objname);
        return null;
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
            AddAllAssets(objs[j].name, objs[j]);
        }
        UseArt(objs);
        yield return 0;
        ab.Unload(false);
    }
    public void AddAllAssets(string objname, UnityEngine.Object obj)
    {
        if (AllAssets.ContainsKey(objname))
        {
            AllAssets[objname] = obj; Debug.Log("重复资源     " + packageName + "     " + objname);
        }
        else
            AllAssets.Add(objname, obj);
    }
    public override void UseArt(object[] objs)
    {
        AllAssets.TryGetValue(packageName, out UnityEngine.Object obj);
        PackageMgr.AddLoadPackage(packageName, obj);
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
