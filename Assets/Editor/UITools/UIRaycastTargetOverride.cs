using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Reflection;

[InitializeOnLoad]
static internal class UIRaycastTargetOverride
{
    //The reflected dafault methods.
    private static MethodInfo m_miGetDefaultResource = null;
    private static MethodInfo m_miPlaceUIElementRoot = null;

    static UIRaycastTargetOverride()
    {
        Initialize();
    }

    private static void Initialize()
    {
        //Get all loaded assemblies.
        Assembly[] allAssemblies = AppDomain.CurrentDomain.GetAssemblies();
        Assembly uiEditorAssembly = null;
        foreach (Assembly assembly in allAssemblies)
        {
            AssemblyName assemblyName = assembly.GetName();
            if ("UnityEditor.UI" == assemblyName.Name)
            {
                uiEditorAssembly = assembly;
                break;
            }
        }

        //Check if we find ui assembly.
        if (null == uiEditorAssembly)
        {
            Debug.LogError("Can not find assembly: UnityEditor.UI.dll");
            return;
        }

        //Get things we need.
        Type menuOptionType = uiEditorAssembly.GetType("UnityEditor.UI.MenuOptions");
        m_miGetDefaultResource = menuOptionType.GetMethod("GetStandardResources", BindingFlags.NonPublic | BindingFlags.Static);
        m_miPlaceUIElementRoot = menuOptionType.GetMethod("PlaceUIElementRoot", BindingFlags.NonPublic | BindingFlags.Static);
    }

    [MenuItem("GameObject/UI/Text", false, 2000)]
    static public void AddText(MenuCommand menuCommand)
    {
        GameObject go = DefaultControls.CreateText((DefaultControls.Resources)m_miGetDefaultResource.Invoke(null, null));
        m_miPlaceUIElementRoot.Invoke(null, new object[] { go, menuCommand });

        //Remove raycast target.
        Text text = go.GetComponent<Text>();
        text.raycastTarget = false;
    }

    [MenuItem("GameObject/UI/Image", false, 2000)]
    static public void AddImage(MenuCommand menuCommand)
    {
        GameObject go = DefaultControls.CreateImage((DefaultControls.Resources)m_miGetDefaultResource.Invoke(null, null));
        m_miPlaceUIElementRoot.Invoke(null, new object[] { go, menuCommand });

        //Remove raycast target.
        Image image = go.GetComponent<Image>();
        image.raycastTarget = false;
    }

    [MenuItem("GameObject/UI/Raw Image", false, 2000)]
    static public void AddRawImage(MenuCommand menuCommand)
    {
        GameObject go = DefaultControls.CreateRawImage((DefaultControls.Resources)m_miGetDefaultResource.Invoke(null, null));
        m_miPlaceUIElementRoot.Invoke(null, new object[] { go, menuCommand });

        //Remove raycast target.
        RawImage rawImage = go.GetComponent<RawImage>();
        rawImage.raycastTarget = false;
    }
    [MenuItem("GameObject/UI/NoDrawGraphic", false, 2000)]
    static public void AddNoDrawGraphic(MenuCommand menuCommand)
    {
        GameObject go = DefaultControls.CreateImage((DefaultControls.Resources)m_miGetDefaultResource.Invoke(null, null));
        m_miPlaceUIElementRoot.Invoke(null, new object[] { go, menuCommand });

        //不知道咋加新的 先这样写这
        GameObject.DestroyImmediate(go.GetComponent<Image>());
        go.AddComponent<NoDrawGraphic>();
        go.name = "NoDrawGraphic";

    }
}