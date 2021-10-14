using UnityEngine;
using System;

/// <summary>
/// 调试日志模块
/// </summary>
/// 
public enum LogLevel
{
    Error,
    Warning,
    Info,
}
public class DebugMgr
{
    public static LogLevel LogLevel = LogLevel.Info;
    public static bool EnableLog = true;

    #region log
    public static bool CanLog()
    {
        return EnableLog && LogLevel >= LogLevel.Info;
    }

    public static void Log(string str)
    {
        Debug.Log(str);
    }

    public static void Log(object obj)
    {
        Debug.Log(obj);
    }

    public static void Log(string str, object obj1)
    {
        Debug.Log(string.Format(str, obj1));
    }

    public static void Log(string str, object obj1, object obj2)
    {
        Debug.Log(string.Format(str, obj1, obj2));
    }

    public static void Log(string str, object obj1, object obj2, object obj3)
    {
        Debug.Log(string.Format(str, obj1, obj2, obj3));
    }

    public static void Log(string str, params object[] args)
    {
        Debug.LogFormat(str, args);
    }

    public static void Log(UnityEngine.Object obj, string str, params object[] args)
    {
        Debug.LogFormat(obj, str, args);
    }
    #endregion

    #region logwarning
    public static bool CanLogWarning()
    {
        return EnableLog && LogLevel >= LogLevel.Warning;
    }

    public static void LogWarning(string str)
    {
        Debug.LogWarning(str);
    }

    public static void LogWarning(string str, object obj1)
    {
        Debug.LogWarning(string.Format(str, obj1));
    }

    public static void LogWarning(string str, object obj1, object obj2)
    {
        Debug.LogWarning(string.Format(str, obj1, obj2));
    }

    public static void LogWarning(string str, object obj1, object obj2, object obj3)
    {
        Debug.LogWarning(string.Format(str, obj1, obj2, obj3));
    }

    public static void LogWarning(string str, params object[] args)
    {
        Debug.LogWarningFormat(str, args);
    }
    #endregion

    #region logerror
    public static bool CanLogError()
    {
        return EnableLog && LogLevel >= LogLevel.Error;
    }

    public static void LogError(string str)
    {
        Debug.LogError(str);
    }

    public static void LogError(object obj)
    {
        Debug.LogError(obj);
    }

    public static void LogError(string str, object obj1)
    {
        Debug.LogError(string.Format(str, obj1));
    }

    public static void LogError(string str, object obj1, object obj2)
    {
        Debug.LogError(string.Format(str, obj1, obj2));
    }

    public static void LogError(string str, object obj1, object obj2, object obj3)
    {
        Debug.LogError(string.Format(str, obj1, obj2, obj3));
    }

    public static void LogError(string str, params object[] args)
    {
        Debug.LogErrorFormat(str, args);
    }
    #endregion

    #region logexpection
    public static bool CanLogException()
    {
        return EnableLog && LogLevel >= LogLevel.Error;
    }
    public static void LogException(Exception ex)
    {
        Debug.LogException(ex);
    }
    #endregion logexpection

}
