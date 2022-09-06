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

    public static bool IsSameArrays(int[,] v1, int[,] v2)
    {
        int v1_hc = v1.GetLength(0);
        int v1_wc = v1.GetLength(1);

        int v2_hc = v2.GetLength(0);
        int v2_wc = v2.GetLength(1);
        if ( v1_hc==v2_hc && v1_wc==v2_wc)
        {
            for (int i = 0; i < v1_hc; i++)
            {
                for (int j = 0; j < v1_wc; j++)
                {
                    if (v1[i,j]!=v2[i,j])
                    {
                        return false;
                    }
                }
            }
        }
        else
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// 不规则的二维数组旋转
    /// </summary>
    /// <param name="matrix"></param>
    /// <returns></returns>
    static int[,] Rotate(int[,] matrix)
    {
        int hcount = matrix.GetLength(0);
        int wcount = matrix.GetLength(1);
        int[,] newv = new int[wcount, hcount];
        for (int i = 0; i < hcount; i++)
        {
            for (int j = 0; j < wcount; j++)
            {
                newv[j, hcount - 1 - i] = matrix[i, j];
            }
        }
        return newv;
    }

    /// <summary>
    /// 判断是否需要旋转，旋转前后都一样的组不需要旋转
    /// </summary>
    /// <param name="matrix"></param>
    /// <returns></returns>
    public static bool NeedToRotate(int[,] matrix)
    {
        int hcount = matrix.GetLength(0);
        int wcount = matrix.GetLength(1);
        if (hcount!=wcount)
        {
            return true;
        }
        for (int i = 0; i < hcount; i++)
        {
            for (int j = 0; j < wcount; j++)
            {
                if (matrix[i,j]==0)
                {
                    return true;
                }
            }
        }
        return false;
    }
    /// <summary>
    /// 长宽相等的二维数组旋转
    /// </summary>
    /// <param name="matrix"></param>
    /// <returns></returns>
    public static int[,] Rotate_90(int[,] matrix)
    {
        int hcount = matrix.GetLength(0);
        int wcount = matrix.GetLength(1);
        if (hcount != wcount)
        {
            return Rotate(matrix);
        }
        int[,] newv = new int[wcount, hcount];
        int max = hcount / 2;
        for (int layer = 0; layer < max; ++layer)
        {
            int first = layer;
            int last = hcount - 1 - layer;

            for (int i = first; i < last; ++i)
            {
                int offset = i - first;
                int top = matrix[first, i];
                newv[first, i] = matrix[last - offset, first];
                newv[last - offset, first] = matrix[last, last - offset];
                newv[last, last - offset] = matrix[i, last];
                newv[i, last] = top;
            }
        }
        return newv;
    }

    public static string getMemory(object o) // 获取引用类型的内存地址方法    
    {
        System.Runtime.InteropServices.GCHandle h = System.Runtime.InteropServices.GCHandle.Alloc(o, System.Runtime.InteropServices.GCHandleType.WeakTrackResurrection);

        System.IntPtr addr = System.Runtime.InteropServices.GCHandle.ToIntPtr(h);

        return "0x" + addr.ToString("X");
    }
}
