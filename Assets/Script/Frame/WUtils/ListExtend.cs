using System;
using System.Collections.Generic;
namespace WUtils
{
    public static class ListExtend
    {
        public static bool GHaveAdd<T>(this List<T> tmpList, T obj)
        {
            if (!tmpList.Contains(obj))
            {
                tmpList.Add(obj);
                return false;
            }
            return true;
        }
    }
}