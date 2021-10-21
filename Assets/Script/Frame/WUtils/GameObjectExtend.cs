using UnityEngine;
namespace WUtils
{
    public static class GameObjectExtend
    {
        public static T GetOrCreatComponent<T>(this GameObject obj) where T : Behaviour
        {
            T val = obj.GetComponent<T>();
            if ((Object)val == (Object)null)
            {
                val = obj.AddComponent<T>();
            }
            return val;
        }
    }
}