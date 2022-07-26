
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using Google.Protobuf.WellKnownTypes;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class PoolsStatus : EditorWindow
{
	private Vector2 _pos;
	[FormerlySerializedAs("_loadUnit")]
	[SerializeField]
	private bool loadUnit = false;


	public static void Init()
	{
		GetWindow(typeof(PoolsStatus));
	}


	private void OnGUI()
	{
		loadUnit = GUILayout.Toggle(loadUnit, "loadUnit");
		if (!loadUnit) return;
		_pos = GUILayout.BeginScrollView(_pos);


		foreach (IPoolsType item in System.Enum.GetValues(typeof(IPoolsType)))
		{
			int index = (int)item;
			if (!PoolMgr.Inst.Dic.TryGetValue(index, out Pool queue)) continue;
			EditorGUILayout.BeginHorizontal(); //开始水平布局   
			GUI.color = Color.white;
			GUILayout.Label((item).ToString(), GUILayout.Width(150));

			GUI.color = Color.white;
			GUILayout.Label("总量 " + queue.AllCreate, GUILayout.Width(150));

			GUI.color = Color.red;
			GUILayout.Label("激活 " + queue.CountActive, GUILayout.Width(150));

			GUI.color = Color.green;
			GUILayout.Label("未激活 " + queue.CountInactive, GUILayout.Width(150));

			EditorGUILayout.EndHorizontal(); //结束水平布局 
		}
		GUILayout.EndScrollView();

	}

	void OnInspectorUpdate()
	{
		this.Repaint();  //重新画窗口  
	}
}
