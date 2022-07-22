using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class TableWindow : EditorWindow
{
	static string pathstr = "";
	static string Setpathstr = "";

	public static void CreateWindows()
	{
		Setpathstr = PlayerPrefs.GetString("星球编辑器url", "");
		TableWindow myWindow = (TableWindow)EditorWindow.GetWindow(typeof(TableWindow), false, "ITools/表处理/Asset->Cfg", true);//创建窗口
		myWindow.Show();//展示
	}
	void SetStarUrl(string pathstr)
	{
		if (!pathstr.Equals(string.Empty))
		{
			PlayerPrefs.SetString("星球编辑器url", pathstr);
		}
	}

	void OnGUI()
	{
		GUILayout.Label("如果使用的地址为空请先在底部设置地址");

		pathstr = PlayerPrefs.GetString("星球编辑器url", "");
		GUILayout.Label("星球编辑器地址(使用的): " + pathstr, EditorStyles.boldLabel);
		 
		GUILayout.Space(6);
		if (GUILayout.Button("ALL Cfg--->Txt&Lua&Cs"))
		{
			TableProcessing.CfgCreateTLC(pathstr);
		}
		GUILayout.Space(20);
		GUILayout.Label("如果使用的地址满足条件请忽视这里 (否者重新输入设置的地址)");
		Setpathstr = GUILayout.TextField(Setpathstr);
		if (GUILayout.Button("保存设置", GUILayout.ExpandWidth(false)))
		{
			if (Setpathstr == pathstr)
				DebugMgr.LogError("地址相同");
			else if (!Directory.Exists(Setpathstr))
				DebugMgr.LogError("地址不存在");
			else
			{
				SetStarUrl(Setpathstr);
				DebugMgr.Log("设置成功");
			}
		}
	}
}
