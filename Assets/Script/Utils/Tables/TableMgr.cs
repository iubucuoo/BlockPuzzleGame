using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableMgr
{
    public static TableMgr inst
    {
        get
        {
            if (_inst == null)
            {
                _inst = new TableMgr();
            }
            return _inst;
        }
    }
    public static TableMgr _inst;

    TableArt data;

    public void Load(AssetBundle ab,Action cb)
    {
        data = new TableArt("chinese", cb);
        MEC.Timing.RunCoroutine(data.Loading(ab));//这里加载资源，加载完成后回调
    }

    internal TextAsset GetTable(string tableName)
    {
        var txt = data.dic[tableName] as TextAsset;
        data.dic.Remove(tableName);
        return txt;
    }
}
class TableArt
{
    public Dictionary<string, UnityEngine.Object> dic;
    protected Action cb;
    public TableArt(string _AbName, Action _cb) 
    {
        cb = _cb;
    }
 
    public  IEnumerator<float> Loading(AssetBundle ab)
    {
        var result = ab.LoadAllAssetsAsync();
        yield return MEC.Timing.WaitUntilDone(result);
        UnityEngine.Object[] objs = result.allAssets;
        if (dic == null)
        {
            dic = new Dictionary<string, UnityEngine.Object>(objs.Length);
        }
        for (int i = 0; i < objs.Length; i++)
        {
            var tmp = objs[i];
            var key_str = tmp.name;
            if (!dic.ContainsKey(key_str))
            {
                dic.Add(key_str, tmp);
            }
        }
        ab.Unload(false);
        cb();
    }
}
