using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace IUBCOFrame
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        public static T instance
        {
            get
            {
                CheckInstance();
                return m_Instance;
            }
        }

        public static void CheckInstance()
        {
            if (m_Instance == null)
            {
                m_Instance = FindObjectOfType<T>();
                if (m_Instance == null)
                {
                    GameObject gameObject = new GameObject(typeof(T).Name);
                    DontDestroyOnLoad(gameObject);
                    m_Instance = gameObject.AddComponent<T>();
                }
            }
        }

        public static bool hasInstance
        {
            get
            {
                return m_Instance != null;
            }
        }
        protected virtual void Awake()
        {
            if (!m_Instance)
            {
                m_Instance = (T)this;
                if (transform.parent == null)
                {
                    DontDestroyOnLoad(this);
                    return;
                }
            }
            else if (m_Instance != this)
            {
                Debug.LogErrorFormat("Instance of {0} already exist, this destroyed!", new object[]
                {
                    name
                });
                Destroy(this);
            }
        }
        protected static T m_Instance;
    }
}