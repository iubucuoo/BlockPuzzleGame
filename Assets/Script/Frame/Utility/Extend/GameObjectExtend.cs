using System;
using UnityEngine;
namespace WUtils
{
    public static class GameObjectExtend
    {
        public static T AddMissingComponent<T>(this GameObject obj) where T : Behaviour
        {
            T val = obj.GetComponent<T>();
            if (val == null)
            {
                val = obj.AddComponent<T>();
            }
            return val;
        }
        public static Component AddMissingComponent(this GameObject go, Type t)
        {
            Component component = go.GetComponent(t);
            if (component == null)
            {
                component = go.AddComponent(t);
            }
            return component;
        }
        public static GameObject Clone(this GameObject gameObject)
        {
            return UnityEngine.Object.Instantiate(gameObject, gameObject.transform.parent, false);
        }
        public static Material GetMaterial(this GameObject gameObject, int index = 0)
        {
            Renderer component = gameObject.GetComponent<Renderer>();
            Material result;
            if (!component)
            {
                result = null;
            }
            else if (index == 0)
            {
                result = component.material;
            }
            else
            {
                result = ((index < component.materials.Length) ? component.materials[index] : null);
            }
            return result;
        }
        public static Material GetSharedMaterial(this GameObject gameObject, int index = 0)
        {
            Renderer component = gameObject.GetComponent<Renderer>();
            Material result;
            if (!component)
            {
                result = null;
            }
            else if (index == 0)
            {
                result = component.sharedMaterial;
            }
            else
            {
                result = ((index < component.sharedMaterials.Length) ? component.materials[index] : null);
            }
            return result;
        }
        public static Component GetRootComponent(this GameObject gameObject, Type t)
        {
            Component result = null;
            Transform transform = gameObject.transform;
            while (transform != null)
            {
                Component component = transform.GetComponent(t);
                if (component)
                {
                    result = component;
                }
                transform = transform.parent;
            }
            return result;
        }
        public static T GetRootComponent<T>(this GameObject gameObject) where T : UnityEngine.Object
        {
            T result = default(T);
            Transform transform = gameObject.transform;
            while (transform != null)
            {
                T component = transform.GetComponent<T>();
                if (component)
                {
                    result = component;
                }
                transform = transform.parent;
            }
            return result;
        }
        public static bool HaveComponent(this GameObject go, Type t)
        {
            return go.GetComponent(t) != null;
        }
        public static bool HaveComponent<T>(this GameObject go)
        {
            return go.GetComponent(typeof(T)) != null;
        }
        public static bool RemoveComponent(this GameObject go, Type t)
        {
            Component component = go.GetComponent(t);
            bool result;
            if (component)
            {
                ObjectEx.Destroy(component);
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }
        
        public static void SetLayer(this GameObject go, int layer)
        {
            go.layer = layer;
            Transform transform = go.transform;
            int i = 0;
            int childCount = transform.childCount;
            while (i < childCount)
            {
                Transform child = transform.GetChild(i);
                child.gameObject.SetLayer(layer);
                i++;
            }
        }

    }

    public static class ObjectEx
    {
         
        public static void Destroy(UnityEngine.Object obj)
        {
            if (Application.isPlaying)
            {
                UnityEngine.Object.Destroy(obj);
            }
            else
            {
                UnityEngine.Object.DestroyImmediate(obj);
            }
        }

         
        public static bool IsNull(UnityEngine.Object obj)
        {
            return obj == null;
        }
    }
}