using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticTools
{
    public static ResLoadModel _ResLoadModel;
    public static bool _LuaLoadError;
    public static bool IsSwitchScene;
    public static bool AllowDownLoad;
    public static byte _NetStatus;
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
}
