using System;
using UnityEngine;
public class PlayerPrefsInt : PlayerPrefsValueT<int>
{
    public PlayerPrefsInt(string key, int @default) : base(key, @default)
    {
    }
    protected override int DoGet(int @default)
    {
        return PlayerPrefs.GetInt(this.key, @default);
    }
    protected override void DoSet()
    {
        PlayerPrefs.SetInt(key, value);
    }
    public static implicit operator int(PlayerPrefsInt value)
    {
        return value.value;
    }
}

