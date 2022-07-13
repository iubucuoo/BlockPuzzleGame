using System.IO;
using UnityEditor;
using UnityEngine;

public class ToolsEditor : Editor
{
    [MenuItem("Tools/清理缓存")]
    public static void ClearCache()
    {
        PlayerPrefs.DeleteAll();
        Directory.Delete(Application.persistentDataPath, true);
        Debug.Log("Clear Success");
    }

    [MenuItem("Tools/对象池状态")]
    static void Name1() { PoolsStatus.Init(); }
}
