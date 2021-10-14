using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMgr
{
    public static Object InstantiateObj(Object obj)
    {
//#if _CHECK_OPTIMIZE
//		System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
//		stopwatch.Start();
//#endif
        var newobj = Object.Instantiate(obj);
//#if _CHECK_OPTIMIZE
//		stopwatch.Stop();
//		if (stopwatch.ElapsedMilliseconds > 5)
//			if(DebugMgr.CanLog()) DebugMgr.Log(string.Format("InstantiateObj {0} costtime = {1}", obj.name, stopwatch.ElapsedMilliseconds));
//#endif
        return newobj;
    }

    public static GameObject InstantiateGameObj(GameObject obj)
    {
//#if _CHECK_OPTIMIZE
//		System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
//		stopwatch.Start();
//#endif
        var newobj = GameObject.Instantiate(obj);
//#if _CHECK_OPTIMIZE
//		stopwatch.Stop();
//		if (stopwatch.ElapsedMilliseconds > 5)
//			if(DebugMgr.CanLog()) DebugMgr.Log(string.Format("InstantiateGameObj {0} costtime = {1}", obj.name, stopwatch.ElapsedMilliseconds));
//#endif
        return newobj;
    }

    public static Object LoadResource(string path)
    {
        //#if _CHECK_OPTIMIZE
        //		System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        //		stopwatch.Start();
        //#endif
        var obj = ResourceMgr.Inst.LoadRes<GameObject>(path);
        //var obj = Resources.Load(path);
//#if _CHECK_OPTIMIZE
//		stopwatch.Stop();
//		if (stopwatch.ElapsedMilliseconds > 5)
//			if(DebugMgr.CanLog()) DebugMgr.Log(string.Format("LoadResource {0} costtime = {1}", obj.name, stopwatch.ElapsedMilliseconds));
//#endif
        return obj;
    }

//    public static Object LoadMainAssetAtPath(string path)
//    {
//#if UNITY_EDITOR
////#if _CHECK_OPTIMIZE
////		System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
////		stopwatch.Start();
////#endif
//        var obj = UnityEditor.AssetDatabase.LoadMainAssetAtPath(path);
////#if _CHECK_OPTIMIZE
////		stopwatch.Stop();
////		if (stopwatch.ElapsedMilliseconds > 5)
////			if (DebugMgr.CanLog()) DebugMgr.Log(string.Format("LoadMainAssetAtPath {0} costtime = {1}", obj.name, stopwatch.ElapsedMilliseconds));
////#endif
//        return obj;
//#endif
//        return null;
//    }
}
