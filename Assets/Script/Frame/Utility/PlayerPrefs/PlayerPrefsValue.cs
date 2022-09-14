using UnityEngine;
public class PlayerPrefsValue
{
    public static void BeginUpdate()
    {
        m_Save++;
    }

    public static void EndUpdate()
    {
        if (--m_Save == 0 && m_Dirty)
        {
            PlayerPrefs.Save();
        }
    }

    public static void Save()
    {
        if (m_Save == 0 && m_Dirty)
        {
            PlayerPrefs.Save();
        }
    }

    protected static int m_Save = 0;

    protected static bool m_Dirty = false;
}
