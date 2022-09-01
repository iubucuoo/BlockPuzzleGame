using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

public class EasyToolsEditor : Editor
{
    #region 测试打印 【优先级1】
    [MenuItem("EasyTools/Test~~~~~~~Debug", false, 1)]
    public static void TestDebug()
    {
        UnityEngine.Debug.Log(WUtils.PathTools.WWW_CACHE_VERSION);
    }
    #endregion

    #region 资源工具 【优先级100】
    [MenuItem("EasyTools/资源工具/BuildAB&拷贝到SA", false, 100)]
    public static void BuildABEditor()
    {
        DiffBuilder.MRealBuildABEditor();
        AssetDatabase.Refresh();
        BuildApkTools.CopyToProject();
    }

    [MenuItem("EasyTools/资源工具/生成BuilderAB.txt 本地资源结构", false, 101)]
    public static void BuilderABTxt()
    {
        NewResBuilder.BuilderABTxt();
    }
    #endregion

    #region 打包工具 【优先级101】

    [MenuItem("EasyTools/打包工具/打Release_Apk", false,101)]
    public static void BuildReleaseApk()
    {
        BuildApkTools.BuildAPK(BuildOptions.None, "WGameProject_release.apk");
    }
    [MenuItem("EasyTools/打包工具/打Debug_Apk", false,102)]
    public static void BuildDebugApk()
    {
        BuildApkTools.BuildAPK(BuildOptions.AllowDebugging | BuildOptions.Development, "3DProject_debug.apk");
    }
    #endregion

    #region Editor 【优先级102】
    /// <summary>
    /// 打开编辑器安装路径
    /// </summary>
    [MenuItem("EasyTools/Editor/Open Installation Path", false, 102)]
    private static void OpenInstallationPath()
    {
        string path = EditorApplication.applicationPath.Substring(0, EditorApplication.applicationPath.LastIndexOf("/"));
        ProcessStartInfo psi = new ProcessStartInfo(path);
        Process.Start(psi);
    }

    /// <summary>
    /// 打开DataPath文件夹
    /// </summary>
    [MenuItem("EasyTools/Editor/Open DataPath Folder", false, 103)]
    private static void OpenDataPathFolder()
    {
        string path = Application.dataPath;
        ProcessStartInfo psi = new ProcessStartInfo(path);
        Process.Start(psi);
    }

    /// <summary>
    /// 打开StreamingAssets文件夹
    /// </summary>
    [MenuItem("EasyTools/Editor/Open StreamingAssets Folder", false, 104)]
    private static void OpenStreamingAssetsFolder()
    {
        string path = Application.streamingAssetsPath;
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            AssetDatabase.Refresh();
        }
        ProcessStartInfo psi = new ProcessStartInfo(path);
        Process.Start(psi);
    }

    /// <summary>
    /// 打开PersistentData文件夹
    /// </summary>
    [MenuItem("EasyTools/Editor/Open PersistentData Folder", false, 105)]
    private static void OpenPersistentDataFolder()
    {
        string path = Application.persistentDataPath;
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        ProcessStartInfo psi = new ProcessStartInfo(path);
        Process.Start(psi);
    }
    #endregion

    #region 二进制表 【优先级103】
    [MenuItem("EasyTools/导出二进制表",false,103)]
    public static void BuildChinese()
    {
        BuildTables.BuildChinese();
    }
    #endregion

    #region Other 【优先级200】
    [MenuItem("EasyTools/清理缓存",false,200)]
    public static void ClearCache()
    {
        PlayerPrefs.DeleteAll();
        Directory.Delete(Application.persistentDataPath, true);
        DebugMgr.Log("Clear Success");
    }

    [MenuItem("EasyTools/对象池状态", false, 201)]
    static void Name1() { PoolsStatus.Init(); }

    [MenuItem("EasyTools/创建图片字体")]
    static void XFCreateImageFont()
    {
        CreatFontTool.XFCreateImageFont();
    }
    [MenuItem("EasyTools/SpritesPacker/TexturePacker")]
    public static void BuildTexturePacker()
    {
        MySpritesPacker.BuildTexturePacker();
    }
    [MenuItem("EasyTools/SpritesPacker/CommandBuild")]
    public static void CommandBuildTexturePacker()
    {
        CommandBuild.BuildTexturePacker();
    }
    [MenuItem("EasyTools/Language Json Data")]
    public static void LanguageJsonData()
    {
        LanguageEditor.Init();
    }
    
    #endregion
}
