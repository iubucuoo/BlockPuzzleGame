using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class M_math
{
    /// <summary>
    /// 绝对值
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public static float Abs(float v)
    {
        if (v < 0) { return -v; }
        return v;
    }
    /// <summary>
    /// 绝对值
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public static int Abs(int v)
    {
        if (v < 0) { return -v; }
        return v;
    }

    /// <summary>
    /// 判断是偶数
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public static bool Even(int v)
    {
        return (v & 1) == 0;
    }
    /// <summary>
    /// 判断是奇数
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public static bool Odd(int v)
    {
        return (v & 1) == 1;
    }
}
