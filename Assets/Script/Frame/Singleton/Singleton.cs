using System;

public class Singleton<T> where T : new()
{
    protected Singleton()
    {
    }

    public static T instance
    {
        get
        {
            if (s_Instance == null)
            {
                object obj = s_Lock;
                lock (obj)
                {
                    if (s_Instance == null)
                    {
                        s_Instance = Activator.CreateInstance<T>();
                    }
                }
            }
            return s_Instance;
        }
    }


    private static T s_Instance = default;


    private static object s_Lock = new object();
}
