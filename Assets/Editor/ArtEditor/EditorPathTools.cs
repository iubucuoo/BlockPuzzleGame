using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using WUtils;

public class EditorPathTools
{
	//#if UNITY_EDITOR
	public static string PROJECT_ROOT = RemoveDot(Application.dataPath + "/../");
	public static string PROJECT_LUA = Application.dataPath + "/Lua";
	public static string PROJECT_ART = Application.dataPath + "/Art";
	public static string PROJECT_ARTs = PROJECT_ART + "/";
	public static string PROJECT_SHADERS = Application.dataPath + "/Art/NewShaders";
	public static string PROJECT_SCINFO = Application.dataPath + "/Art/Thr/scInfo";
	public static string PROJECT_TABLES = Application.dataPath + "/Art/Tables";
	public static string PROJECT_MAPS = Application.dataPath + "/Art/Thr/Maps";	
	public static string PROJECT_COOLDATA = Application.dataPath + @"\Art\Tables\CoolData";

	public static string BUILDER_DLL_DIR = PROJECT_ROOT + "ITools/BuilderDLL/";
	public static string VERSION_BUILDERLOG_PATH = PROJECT_ROOT + "/ITools/VersionMgr/BuilderLog/";
	public static string VERSION_LOG_PATH = PROJECT_ROOT + "/ITools/VersionMgr/VersionLog";
	public static string SVN_RES_ROOT = RemoveDot(PROJECT_ROOT + "/../NewBuildRes/" +PathTools.PLATFORM);
	public static string SVN_VERSION = SVN_RES_ROOT + "/Version.bytes";	
	public static string SVN_LUA = SVN_RES_ROOT + "/lua/";
	public static string SVN_MANIFEST = SVN_RES_ROOT + "/ManifestData.bytes";
	public static string SVN_ASSETBUNDLE_MANIFEST = SVN_RES_ROOT + "/" + PathTools.PLATFORM;
	public static string SVN_EXPORT_APK = PROJECT_ROOT + "/../ExportApk";
	
	public static string EFFECT_COLLECTION_PATH = Application.dataPath + "/Art/NewShaders/Effect/effect_collection.shadervariants";
	public static string EFFECT_COLLECTION_DIR = "Assets/Art/NewShaders/Effect/effect_collection.shadervariants";

	public static string CHARACTOR_COLLECTION_PATH = Application.dataPath + "/Art/NewShaders/Charactor/charactor_collection.shadervariants";
	public static string CHARACTOR_COLLECTION_DIR = "Assets/Art/NewShaders/Charactor/charactor_collection.shadervariants";

	public static string SCENE_COLLECTION_PATH = Application.dataPath + "/Art/NewShaders/Scene/scene_collection.shadervariants";
	public static string SCENE_COLLECTION_DIR = "Assets/Art/NewShaders/Scene/scene_collection.shadervariants";

	public static string OLD_CHARACTOR_COLLECTION_PATH = Application.dataPath + "/Art/Shaders/Character/character_collection.shadervariants";
	public static string OLD_CHARACTOR_COLLECTION_DIR = "Assets/Art/Shaders/Character/character_collection.shadervariants";

	public static string OLD_SCENE_COLLECTION_PATH = Application.dataPath + "/Art/Shaders/scene/scene_collection.shadervariants";
	public static string OLD_SCENE_COLLECTION_DIR = "Assets/Art/Shaders/scene/scene_collection.shadervariants";

	public static string START_PATH = @"\StarEdit\StarEdit\bin\Debug\data\";
	public const string CS_VERSION = "CVersion|{0}";

	public static bool CanUseMap(string path)
	{
		return path.IndexOf("Thr/Maps/") >= 0 && path.IndexOf(".unity") >= 0&&path.IndexOf("ignore")<0;
	}

	static string RemoveDot(string file)
	{
		var t = (new DirectoryInfo(file)).FullName;
		t = FixedWindowsPath(t);
		return t;
	}
	public static string FixedWindowsPath(string tmpPath)
	{
		tmpPath = tmpPath.Replace("\\", "/");
		return tmpPath;
	}
}
