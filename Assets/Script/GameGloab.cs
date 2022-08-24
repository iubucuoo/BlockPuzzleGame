using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameGloab
{
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
}
