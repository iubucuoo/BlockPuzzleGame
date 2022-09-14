using System;
using UnityEngine;
public class PlayerPrefsFloat : PlayerPrefsValueT<float>
{
    public PlayerPrefsFloat(string key, float @default) : base(key, @default)
    {
    }
    protected override float DoGet(float @default)
    {
        return PlayerPrefs.GetFloat(this.key, @default);
    }
    protected override void DoSet()
    {
        PlayerPrefs.SetFloat(key, value);
    }
    public static implicit operator float(PlayerPrefsFloat value)
    {
        return value.value;
    }
}

