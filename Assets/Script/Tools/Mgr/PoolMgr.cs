using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoolMgr
{
    [SerializeField]
    public Dictionary<int, Pool> dic;
    private static PoolMgr _inst;
    public static PoolMgr Inst
    {
        get {
            if (_inst ==null)
            {
                _inst = new PoolMgr();
                _inst.dic = new Dictionary<int, Pool>();
            }
            return _inst;
        }
    }

 
    IPoolable AllocateV(IPoolsType _type)
    {
        if (!dic.TryGetValue((int)_type,out Pool pool))
        {
            pool = new ObjectPool();
            dic.Add((int)_type, pool);
        }
        return pool.Allocate(_type);
    }
    void RecycleV(IPoolable pool)
    {
        int typeint = (int)pool.GroupType;
        if (dic.TryGetValue(typeint,out Pool v))
        {
            v.Recycle(pool);
        }
        else
        {
            Debug.LogError("字典中不存在的类型" + typeint);//字典中不存在的类型
        }
    }
    public static void Recycle(IPoolable pool)
    {
        Inst.RecycleV(pool);
    }
    public static IPoolable Allocate(IPoolsType _type)
    {
        return Inst.AllocateV(_type);
    }
}
