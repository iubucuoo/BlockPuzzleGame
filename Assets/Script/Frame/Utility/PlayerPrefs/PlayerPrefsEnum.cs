using System;
using UnityEngine;

public class PlayerPrefsEnum<T> : PlayerPrefsValueT<T> where T : struct
{
    public PlayerPrefsEnum(string key, T @default) : base(key, @default)
    {
    }
    protected override T DoGet(T @default)
    {
        T result;
        if (PlayerPrefs.HasKey(key))
        {
            result = (T)Enum.Parse(typeof(T), PlayerPrefs.GetString(key));
        }
        else
        {
            result = @default;
        }
        return result;
    }
    protected override void DoSet()
    {
        PlayerPrefs.SetString(key, value.ToString());
    }
}

