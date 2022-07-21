using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableMgr : ArtBase
{
    static TableMgr _inst;
    public static TableMgr Inst { get { return _inst = (_inst ?? new TableMgr()); } }
    public override int _ModelID => (int)RES_MODEL_INDEX.tables;
    public override ResSort _Sort => ResSort.Config;

    public Action _Cb;

    Dictionary<string, byte[]> _dic = new Dictionary<string, byte[]>();

    public override string AbSingleName()
    {
        return "chinese";
    }

    public override IEnumerator<float> Loading(AssetBundle ab)
    {
        var objs = ab.LoadAllAssets();
        UseArt(objs);
        yield return 0;
    }

    public byte[] GetTable(string tableName)
    {
        if (_dic.TryGetValue(tableName, out byte[] value))
        {
            _dic.Remove(tableName);
            return value;
        }
        return null;
    }

    public override void UseArt(object[] objs)
    {
        _dic.Clear();
        for (int i = 0; i < objs.Length; i++)
        {
            var txt = (objs[i] as TextAsset);
            var key = txt.name;
            var value = txt.bytes;
            _dic.Add(key, value);
        }
        _Cb();
    }

    public override void FailArt()
    {

    }
}