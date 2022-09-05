using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS : MonoSingleton<FPS>
{
    float windowScale
    {
        get { return Screen.height / 720; }
    }
    FPSCounter m_FPSConter = new FPSCounter(1);
    void Start()
    {
    }

    void Update()
    {
        m_FPSConter.Update();
        
    }

    void OnGUI()
    {
        var scale = windowScale;
        GUI.matrix = Matrix4x4.Scale(new Vector3(scale, scale, scale));
        if (GUILayout.Button(("FPS: " + m_FPSConter.FPS), GUILayout.ExpandWidth(true), GUILayout.Height(80))){

        }
  
    }
}
