using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class StaticTools
{
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


    public static int SoundIsOnOff
    {
        get { return PlayerPrefs.GetInt("SoundIsOn", 0); }
        set
        {
            if (PlayerPrefs.GetInt("SoundIsOn", 0) != value)
            {
                PlayerPrefs.SetInt("SoundIsOn", value);
            }
        }
    }
    public static int MusicOnOff
    {
        get { return PlayerPrefs.GetInt("MusicIsOn", 0); }
        set
        {
            if (PlayerPrefs.GetInt("MusicIsOn", 0) != value)
            {
                PlayerPrefs.SetInt("MusicIsOn", value);
            }
        }
    }
}
