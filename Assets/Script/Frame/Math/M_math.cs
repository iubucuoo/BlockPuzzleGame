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
        int max = hcount / 2;
        for (int layer = 0; layer < max; ++layer)
        {
            int first = layer;
            int last = hcount - 1 - layer;

            for (int i = first; i < last; ++i)
            {
                int offset = i - first;
                int top = matrix[first, i];
                matrix[first, i] = matrix[last - offset, first];
                matrix[last - offset, first] = matrix[last, last - offset];
                matrix[last, last - offset] = matrix[i, last];
                matrix[i, last] = top;
            }
        }
        return matrix;
    }
}
