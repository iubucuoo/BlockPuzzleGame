using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ToolsEditor : Editor
{
    [MenuItem("Tools/清理缓存")]
    public static void ClearCache()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("Clear Success");
    }

}
