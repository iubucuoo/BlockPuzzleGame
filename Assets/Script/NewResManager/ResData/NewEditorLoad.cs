using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using WUtils;

public enum RES_MODEL_INDEX
{
	none,
	player = 1,
	monster,
	effects,
	drop,
	npc,
	trap,
	teleport,
	animation_texture,
	uiwnds,
	tables,
	lua,
	scinfo,
	other,
}

public class NewEditorLoad
{
	public static string[] _RootPathName = new string[] { "Art", "temp/Lua" };
	public static string[] _ObjPrefix = new string[] { ".prefab", ".anim", ".png", ".controller", ".bytes", ".json", ".txt", ".cginc", ".shader", ".overrideController",".wav", ".mp3",".xml", ".tpsheet" };
	public static string _Environment = "/Environment/";
	public static string _Ignore = "/ignore/";
	//定义默认默认ab
	public static string[] _DefaultABName = new string[] { "NewShaders", "Shaders", "PlayerAnims" };

	static Dictionary<string, int> _DefaultModelName;
	static Dictionary<int, string> _AppendModelName;

	NewResMgr _NewResMgr;
	static int _ModelIndex = 1000;

	public NewResMgr BuilderResData()
	{
		_NewResMgr = new NewResMgr();
#if UNITY_EDITOR
		//builder AB的时候 这个会报错
		AssetDatabase.Refresh();//刷新一下，确保全部资源都再
		Init();
		var _ResPaths = AssetDatabase.GetAllAssetPaths();
		for (int i = 0; i < _ResPaths.Length; i++)
		{
			var respath = _ResPaths[i];

			var v = CreateUnit(respath);
			if (v != null)
			{
				_NewResMgr.PushUnit(v);
			}
		}
		//_NewResMgr.PushUnit(AppendDllUnit());
#endif
		return _NewResMgr;
	}
	//模拟 数据 用于 dll
	//NewResUnit AppendDllUnit()
	//{
	//	var unit = new NewResUnit()
	//	{
	//		_ModelName = "dll",
	//		_ModelID = 13,
	//		_AbName = "main",
	//		_ObjName = "main",
	//		_Path = "no Assets path",
	//	};
	//	return unit;
	//}
	void Init()
	{
		_DefaultModelName = new Dictionary<string, int>()
		{
			//{@"Assets/Art/Thr/Models/PlayerAnims",(int)RES_MODEL_INDEX.player },
			//{@"Art/Thr/Models/weapon",(int)RES_MODEL_INDEX.player },
			//{@"Art/Thr/Models/Character",(int)RES_MODEL_INDEX.player },
			//{@"Assets/Art/Thr/Models/monster",(int)RES_MODEL_INDEX.monster },
			{@"Assets/Art/Effects",(int)RES_MODEL_INDEX.effects },
			{@"Assets/Art/Table",(int)RES_MODEL_INDEX.tables },
			//{@"Art/Thr/Models/drops",(int)RES_MODEL_INDEX.drop },
			//{@"Assets/Art/Thr/Models/npcs",(int)RES_MODEL_INDEX.npc },
			//{@"Art/Thr/Models/Trap",(int)RES_MODEL_INDEX.trap },
			//{@"Art/Thr/Models/teleport_door",(int)RES_MODEL_INDEX.teleport },
			//{@"Assets/Art/UIWnds",(int)RES_MODEL_INDEX.uiwnds },
			//{@"Assets/Art/Tables/AnimationTexture",(int)RES_MODEL_INDEX.animation_texture },
			//{@"Assets/temp/Lua",(int)RES_MODEL_INDEX.lua },
		};
		_ModelIndex = 1000;//初始化
		_AppendModelName = new Dictionary<int, string>();
	}

	public static NewResUnit CreateUnit(string respath)
	{
		if (respath.Contains(_Ignore) || respath.Contains(_Environment))
		{
            //Debug.Log("return null   "+respath);//不包含ignore与Environment
            return null;
		}
        //Art下 或者 temp/Lua下  
        if (respath.Contains(_RootPathName[0]) || respath.Contains(_RootPathName[1]))
		{
			//_ObjPrefix-->>>>>".prefab", ".anim", ".png", ".controller", ".bytes", ".json", ".txt", ".cginc", ".shader", ".overrideController" 
			for (int j = 0; j < _ObjPrefix.Length; j++)
			{
				var _Prefix = _ObjPrefix[j];
				if (respath.Contains(_Prefix))
                {
                    return CreateSigleUnit(respath, _Prefix);
				}
			}
		}
        //if (respath.Contains("Assets/Art"))
        //{
        //    Debug.Log("return null   " + respath);
        //}
        return null;
	}
	#region BuilderAB	
	static NewResUnit CreateSigleUnit(string path, string _Prefix)
	{
		var index = path.LastIndexOf("/");
		if (index < 0)
		{
			DebugMgr.LogError("CreateSigleUnit Data=error,Path=" + path  + ",_Prefix=" + _Prefix);
		}
		var objName = path.Substring(index + 1).Replace(_Prefix, "");
		var abPath = path.Substring(0, index);
		var modelName = GetModelName(abPath, out int _ModelID);
		var abName = GetAbName(abPath);
        //Debug.LogError(path + "  _abPath:  " + abPath + "  _abname:  " + abName + "  _objname:  " + objName + " _modelname  " + modelName + "   _modelid  " + _ModelID);
        var unit = new NewResUnit()
		{
			_ModelName = _ModelID > (int)RES_MODEL_INDEX.other ? modelName : ((RES_MODEL_INDEX)_ModelID).ToString(),
			_ModelID = _ModelID,
			_AbName = abName.ToLower(),
			_ObjName = objName,
			_Path = path,
		};
		return unit;
	}
	static string GetAbName(string abName)
	{
		for (int i = 0; i < _DefaultABName.Length; i++)
		{
			var temp = _DefaultABName[i];//默认模块
			if (abName.Contains(temp))
			{
				return temp;
			}
		}
		return PathTools.GetParentName(abName);
	}
	static string GetModelName(string abPath, out int id)
	{
		foreach (var item in _DefaultModelName)
		{
			//如果在追加的数据中，查找到就输出相关数据，查不到就必定在枚举中
			if (abPath.Contains(item.Key))
			{
				id = item.Value;
				if (_AppendModelName.TryGetValue(id, out string v))
				{
					return v;
				}
				else
				{
					return ((RES_MODEL_INDEX)id).ToString();
				}
			}
		}
		//新增数据
		var _ModelName = PathTools.GetParentName(abPath, 1);
		if (Enum.TryParse(_ModelName.ToLower(), out RES_MODEL_INDEX _ModelId))
		{
			id = (int)_ModelId;
		}
		else
		{
			id = (++_ModelIndex);
		}
		var Key = PathTools.GetParentPath(abPath, 1);
		_DefaultModelName.Add(Key, id);

		if (!_AppendModelName.ContainsKey(id))
		{
			_AppendModelName.Add(id, _ModelName);
		}
		return _ModelName;
	}
	#endregion
}
