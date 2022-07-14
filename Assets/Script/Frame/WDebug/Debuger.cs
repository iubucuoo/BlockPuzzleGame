using WDebug;
using System;
using UnityEngine;

public class DebugMgr
{
    public static bool EnableLog = true;

    public static int filter;

    public static string DebugIP = "192.168.1.107";

    public static ushort DebugPort = 7788;

    public static bool IsUdpLog;

    private static DebugControl _debugControl;

    private static event Action<string> udpSocket;

    private static DebugControl GetControl()
    {
        if (_debugControl == null)
        {
            _debugControl = new DebugControl(DebugMgr.udpSocket);
        }
        return _debugControl;
    }

    public static void AddSocketLogCallBack(Action<string> _event)
    {
        DebugMgr.udpSocket = _event;
    }

    public static void Log(object message)
    {
        Log(message, null, null);
    }

    public static void Log(object message, object _filter, UnityEngine.Object context = null)
    {
        GetControl().Log(message, _filter, context);
    }

    public static void LogWarning(object message)
    {
        LogWarning(message, null, null);
    }

    public static void LogWarning(object message, object _filter, UnityEngine.Object context = null)
    {
        GetControl().LogWarning(message, _filter, context);
    }

    public static void LogError(object message, object _filter, UnityEngine.Object context = null)
    {
        GetControl().LogError(message, _filter, context);
    }
    public static bool CanLog()
    {
        return EnableLog;
    }
    public static bool CanLogWarning()
    {
        return EnableLog;
    }
    public static bool CanLogError()
    {
        return EnableLog ;
    }
    public static void LogError(object message)
    {
        LogError(message, null, null);
    }

    static DebugMgr()
    {
        DebugMgr.udpSocket = delegate (string message)
        {
            Debug.Log($"UDP方法没有代理出去:{message}");
        };
        IsUdpLog = false;
    }
}
