using UnityEngine;
public class PlayerPrefsBool : PlayerPrefsValueT<bool>
{
    public PlayerPrefsBool(string key, bool @default) : base(key, @default)
    {
    }
    protected override bool DoGet(bool @default)
    {
        return PlayerPrefs.GetInt(this.key, @default ? 1 : 0) != 0;
    }
    protected override void DoSet()
    {
        PlayerPrefs.SetInt(key, value ? 1 : 0);
    }
    public static implicit operator bool(PlayerPrefsBool value)
    {
        return value.value;
    }
}

