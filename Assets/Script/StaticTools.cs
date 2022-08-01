using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class StaticTools
{
    public static bool _LuaLoadError;
    public static byte _NetStatus;
    public static ResLoadModel _ResLoadModel;
    public static bool LoadArtIsAb { get { return _ResLoadModel == ResLoadModel.ONLINE; } }
    public static int CurrentMapID { get; set; }
    public static byte GetNetState()
    {
        //当用户使用移动网络时
        if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
        {
            return 1;
        }

        //当用户使用WiFi时  
        if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            return 2;
        }
        Debug.LogWarning("当前无网络环境");
        return 0;
    }



    #region StringBuilder
    static StringBuilder sb = new StringBuilder(255);
    internal static StringBuilder ToString(string str)
    {
        sb.Append(str);
        return sb;
    }
    internal static string CombStr(string str1, string str2)
    {
        ClearStr();
        ToString(str1);
        ToString(str2);
        return ToEnd();
    }
    internal static string CombStr(string str1, string str2, string str3)
    {
        ClearStr();
        ToString(str1);
        ToString(str2);
        ToString(str3);
        return ToEnd();
    }
    internal static string CombStr(string str1, string str2, string str3, string str4)
    {
        ClearStr();
        ToString(str1);
        ToString(str2);
        ToString(str3);
        ToString(str4);
        return ToEnd();
    }
    internal static string ToEnd()
    {
        return sb.ToString();
    }

    internal static StringBuilder ClearStr()
    {
        sb.Length = 0;
        return sb;
    }
    #endregion
}
