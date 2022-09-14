using System.Collections.Generic;
public static class DictionaryExtend
{
    public static void GHaveAdd<T>(this Dictionary<string, T> tmpDic, string tmpKey, T obj)
    {
        if (!tmpDic.ContainsKey(tmpKey))
        {
            tmpDic.Add(tmpKey, obj);
        }
        else
        {
            tmpDic[tmpKey] = obj;
        }
    }

    public static T GHaveGet<T>(this Dictionary<string, T> tmpDic, string tmpKey)
    {
        if (tmpDic.ContainsKey(tmpKey))
        {
            return tmpDic[tmpKey];
        }
        return default(T);
    }
}

