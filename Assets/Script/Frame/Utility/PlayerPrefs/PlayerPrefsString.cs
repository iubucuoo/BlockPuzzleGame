using System;
using UnityEngine;
public class PlayerPrefsString : PlayerPrefsValueT<string>
{
    public PlayerPrefsString(string key, string @default) : base(key, @default)
    {
    }
    protected override string DoGet(string @default)
    {
        return PlayerPrefs.GetString(key, @default);
    }
    protected override void DoSet()
    {
        PlayerPrefs.SetString(key, value);
    }
    public static implicit operator string(PlayerPrefsString value)
    {
        return value.value;
    }
    public override string ToString()
    {
        return value;
    }
}

