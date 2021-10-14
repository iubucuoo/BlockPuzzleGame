using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceMgr
{
    static ResourceMgr _instance;
    public static ResourceMgr Inst
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ResourceMgr();
            }
            return _instance;
        }
    }
    Dictionary<string, object> m_res = new Dictionary<string, object>();
    public T LoadRes<T>(string respath)where T : Object
    {
        if (m_res.ContainsKey(respath))
        {
            return m_res[respath] as T;
        }
        T t = Resources.Load<T>(respath);
        m_res[respath] = t;
        return t;
    }
}
