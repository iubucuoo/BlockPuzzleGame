using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTool : MonoSingleton<DebugTool>
{
    static readonly Rect DRAG_RECT = new Rect(0, 0, float.MaxValue, 25);
    const float DEFAULT_ICON_X = 10;
    const float DEFAULT_ICON_Y = 10;
    const float ICON_LINE_HEIGHT = 20;
    const float WINDOW_HEIGHT = 720;
    public PlayerPrefsFloat iconX;
    public PlayerPrefsFloat iconY;
    float windowScale
    {
        get { return Screen.height / WINDOW_HEIGHT; }
    }
    Rect windowRect
    {
        get
        {
            float width = Screen.width / windowScale;
            return new Rect(0, 0, width, WINDOW_HEIGHT);
        }
    }
    Rect iconRect
    {
        get
        {
            return new Rect(iconX, iconY, 150, 90);
        }
        set
        {
            PlayerPrefsValue.BeginUpdate();
            iconX.Set(value.x);
            iconY.Set(value.y);
            PlayerPrefsValue.EndUpdate();
        }
    }
    FPSCounter m_FPSConter = new FPSCounter(1);
    void Start()
    {
        iconX = new PlayerPrefsFloat("DebugWindow.iconX", DEFAULT_ICON_X);
        iconY = new PlayerPrefsFloat("DebugWindow.iconY", DEFAULT_ICON_Y);
        iconRect = new Rect(DEFAULT_ICON_X, DEFAULT_ICON_Y, 0, 0);
    }

    void Update()
    {
        m_FPSConter.Update();
        
    }
    void DrawWindow(int windowID)
    {
        GUI.DragWindow(DRAG_RECT);
        GUILayout.Space(5);
        GUILayout.Button(("FPS: " + m_FPSConter.FPS), GUILayout.ExpandWidth(true), GUILayout.Height(80));
    }
    void OnGUI()
    {
        var scale = windowScale;
        GUI.matrix = Matrix4x4.Scale(new Vector3(scale, scale, scale));
        //GUI.Window(0, windowRect, DrawWindow, "<b>DEBUG TOOL</b>");

        iconRect = GUI.Window(0, iconRect, DrawWindow, "<b>DEBUG TOOL</b>");

    }
}
