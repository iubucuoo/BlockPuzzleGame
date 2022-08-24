using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameStatic
{
    public static int _height = 90;// 60;
    public static int _width = 90;// 60;
    public static int wh_2 = 45;// 30;
    public static Vector2 DragUp = new Vector2(0, _width * 4);//y高度 对应的倍数

    public static bool IsTopScore { get; set; }
    public static bool IsRotateState { get; set; }
    public static Vector3 RotateGoldAddPos { get; set; }



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
