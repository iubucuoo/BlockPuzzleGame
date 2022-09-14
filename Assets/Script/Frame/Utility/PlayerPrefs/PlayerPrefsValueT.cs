public abstract class PlayerPrefsValueT<T> : PlayerPrefsValue
{
    public PlayerPrefsValueT(string _key, T @default)
    {
        key = _key;
        value = DoGet(@default);
    }

    public void Set(T _value)
    {
        if (!Equals(value, _value))
        {
            value = _value;
            DoSet();
            m_Dirty = true;
            Save();
        }
    }
    protected abstract T DoGet(T @default);
    protected abstract void DoSet();
    public string key;
    public T value;
}
