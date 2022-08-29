﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppParam
{
    public static int _EditorEffectModelNum;
    public static int _EditorEffectNum;

    public static bool _LuaLoadError;
    public static ResLoadModel _ResLoadModel;
    public static bool LoadArtIsAb { get { return _ResLoadModel == ResLoadModel.ONLINE; } }
    public static byte _NetStatus;
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
        //Debug.LogWarning("当前无网络环境");
        return 0;
    }
}
