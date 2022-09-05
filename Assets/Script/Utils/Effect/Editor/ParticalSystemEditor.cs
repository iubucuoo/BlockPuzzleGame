using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ParticalSystemData))]
public class ParticalSystemDataEditor : Editor
{
    SerializedProperty particalSystems;
    SerializedProperty animators;
	SerializedProperty _Gos;

	SerializedProperty root;
    SerializedProperty fullPathHashes;
    SerializedProperty isLoop;
	SerializedProperty _LongTime;

	SerializedProperty _Emittings;
    SerializedProperty trailRenders;
    SerializedProperty lineRenders;
	SerializedProperty _ActionStopDispose;

	public void SetLongTime(ParticalSystemData myTarget,float t)
	{		
		if (t> myTarget._LongTime)
		{
			myTarget._LongTime = t;
		}
		if (t>50)//大于50s 特效师 希望是用来实现循环的
		{
			myTarget._LongTime = 0;
		}
	}

    public override void OnInspectorGUI()
    {
        //if(DebugMgr.CanLog()) DebugMgr.Log("ParticalSystemDataEditor.OnInspectorGUI");
        ParticalSystemData myTarget = (ParticalSystemData)target;
		

		myTarget.isCreateShow = EditorGUILayout.Toggle("创建直接显示特效", myTarget.isCreateShow);
		myTarget._ActionStopDispose = EditorGUILayout.Toggle("动作结束直接销毁", myTarget._ActionStopDispose);

		if (myTarget.root!= null)
        {
			myTarget._LongTime = 0;

			ParticleSystem[] _particleSystems = myTarget.root.GetComponentsInChildren<ParticleSystem>();
            myTarget.particalSystems = _particleSystems;

			var time= myTarget.root.gameObject.GetParticleDuration();
			myTarget.isLoop = time == -1;
			SetLongTime(myTarget, time);

			TrailRenderer[] _trailRenders = myTarget.root.GetComponentsInChildren<TrailRenderer>();
            myTarget.trailRenders = _trailRenders;
			myTarget._Emittings = new bool[myTarget.trailRenders.Length];
			for (int i = 0; i < myTarget.trailRenders.Length; i++)
			{
				var temp = myTarget.trailRenders[i];
				myTarget._Emittings[i] = temp.emitting;
				SetLongTime(myTarget, temp.time);
			}


			//if(DebugMgr.CanLog()) DebugMgr.Log("trailRenders.count  " + myTarget.trailRenders.Length);
			LineRenderer[] _LineRender = myTarget.root.GetComponentsInChildren<LineRenderer>();
            myTarget.lineRenders = _LineRender;
			

            Animator[] _animators = myTarget.root.GetComponentsInChildren<Animator>();
            myTarget.animators = _animators;

			var fullPathHash= new List<int>();
			float _MaxAniTime = 0;
            for (int i = 0, len = _animators.Length; i < len; i++)
            {
				var ani = _animators[i];
				var animators = ani.runtimeAnimatorController.animationClips;
				for (int j = 0; j < animators.Length; j++)
				{
					var clip_len = animators[j].length;
					if (_MaxAniTime < clip_len)
					{
						_MaxAniTime = clip_len;
					}
				}				
				fullPathHash.Add(_animators[i].GetCurrentAnimatorStateInfo(0).fullPathHash);
                _animators[i].cullingMode = AnimatorCullingMode.CullUpdateTransforms;
            }
			if (time<_MaxAniTime)
			{
				SetLongTime(myTarget, _MaxAniTime);
			}
			myTarget.fullPathHashes = fullPathHash.ToArray();

			Transform[] trans = myTarget.root.GetComponentsInChildren<Transform>();
			GameObject[] go = new GameObject[trans.Length];
			for (int i = 0; i < trans.Length; i++)
			{
				go[i] = trans[i].gameObject;
			}
			myTarget._Gos = go;
		}
        else
        {
            myTarget.root = myTarget.transform;
            Log.Info("ParticalSystemDataEditor.root " + myTarget.root.name);
        }
		EditorGUILayout.PropertyField(isLoop);
		EditorGUILayout.PropertyField(_LongTime);
		EditorGUILayout.PropertyField(particalSystems, true);
        EditorGUILayout.PropertyField(_Emittings, true);
        EditorGUILayout.PropertyField(trailRenders, true);
        EditorGUILayout.PropertyField(lineRenders, true);
        EditorGUILayout.PropertyField(root);
        EditorGUILayout.PropertyField(animators,true);
		EditorGUILayout.PropertyField(_Gos, true);

		serializedObject.ApplyModifiedProperties();        
        if (GUI.changed)
        {         
            EditorUtility.SetDirty(target);            
        }
		GUI.color = Color.green;
		if (GUILayout.Button("绑定数据"))
		{
			EditorUtility.SetDirty(target);
		}		
    }
	
    void OnEnable()
    {        
        particalSystems = serializedObject.FindProperty("particalSystems");
        animators = serializedObject.FindProperty("animators");
        root = serializedObject.FindProperty("root");
        isLoop = serializedObject.FindProperty("isLoop");
		_Emittings = serializedObject.FindProperty("_Emittings");        
        fullPathHashes = serializedObject.FindProperty("fullPathHashes");
        trailRenders = serializedObject.FindProperty("trailRenders");
        lineRenders = serializedObject.FindProperty("lineRenders");
		_LongTime = serializedObject.FindProperty("_LongTime");
		_ActionStopDispose = serializedObject.FindProperty("_ActionStopDispose");
		_Gos = serializedObject.FindProperty("_Gos");
	}
}
