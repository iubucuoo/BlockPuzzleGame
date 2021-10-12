using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameGloab
{
    public static Vector2 OutScreenV2 = new Vector2(5000, 5000);

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
        get {return PlayerPrefs.GetInt("MusicIsOn",0); }
        set
        {
            if (PlayerPrefs.GetInt("MusicIsOn",0)!=value)
            {
                PlayerPrefs.SetInt("MusicIsOn", value);
            }
        }
    }
    public static int GoldCount
    {
        get { return PlayerPrefs.GetInt("GoldCount", 0); }
        set
        {
            if (PlayerPrefs.GetInt("GoldCount", 0) != value)
            {
                PlayerPrefs.SetInt("GoldCount", value);
            }
        }
    }
    public static int Topscore
    {
        get { return PlayerPrefs.GetInt("Topscore", 0); }
        set
        {
            if (PlayerPrefs.GetInt("Topscore", 0) != value)
            {
                PlayerPrefs.SetInt("Topscore", value);
            }
        }
    }

}
