using UnityEngine;
/// <summary>
/// 日志工具箱
/// </summary>
public static class Log
{
#if UNITY_EDITOR
    private const string InfoPrefix = "<b><color=cyan>[Info]</color></b> ";
    private const string WarningPrefix = "<b><color=yellow>[Warning]</color></b> ";
    private const string ErrorPrefix = "<b><color=red>[Error]</color></b> ";
#endif
    public static bool EnableLog = true;
    /// <summary>
    /// 打印信息日志
    /// </summary>
    /// <param name="content">日志内容</param>
    public static void Info(this string content)
    {
#if UNITY_EDITOR
        Debug.Log(InfoPrefix + content);
#else
            if (EnableLog)
            {
                Debug.Log(content);
            }
#endif
    }
    /// <summary>
    /// 打印警告日志
    /// </summary>
    /// <param name="content">日志内容</param>
    public static void Warning(this string content)
    {
#if UNITY_EDITOR
        Debug.LogWarning(WarningPrefix + content);
#else
            if (EnableLog)
            {
                Debug.LogWarning(content);
            }
#endif
    }
    /// <summary>
    /// 打印错误日志
    /// </summary>
    /// <param name="content">日志内容</param>
    public static void Error(this string content)
    {
#if UNITY_EDITOR
        Debug.LogError(ErrorPrefix + content);
#else
            if (EnableLog)
            {
                Debug.LogError(content);
            }
#endif
    }
}