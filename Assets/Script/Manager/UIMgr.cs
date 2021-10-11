﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMgr : MonoBehaviour
{
    public static UIMgr Inst;

    public Transform UIRoot { get; private set; }
    public Transform ADDROOT { get; private set; }
    public Transform BGROOT { get; private set; }
    public Transform CanvasRoot { get; private set; }
    public RectTransform CanvasRect { get; private set; }
    RectTransform BGROOTRect;
    Canvas Canvas;
    Transform DragRoot;

    UI_TopPanel UI_TopPanel;
    UI_GameOverPanel UI_GameOverPanel;
    UI_SetPanel UI_SetPanel;
    UI_GroupRotate UI_GroupRotate;
    UI_AddRotatePanel UI_AddRotatePanel;

    public Dictionary<string, Sprite> Sprites;
    private void Awake()
    {
        Inst = this;
#if UNITY_EDITOR
        DebugMgr.EnableLog = true;
#endif
        CanvasRoot = GameObject.Find("Canvas").transform;
        UIRoot = CanvasRoot.Find("Root");
        Sprites = new Dictionary<string, Sprite>();
        Canvas = CanvasRoot.GetComponent<Canvas>();
        BGROOT = UIRoot.Find("BGROOT");
        ADDROOT = UIRoot.Find("ADDROOT");
        DragRoot = UIRoot.Find("DragRoot");
        CanvasRect = CanvasRoot.GetComponent<RectTransform>();
        BGROOTRect = BGROOT.GetComponent<RectTransform>();

        UI_GroupRotate = UIRoot.Find("ROTATEROOT").GetComponent<UI_GroupRotate>();
        UI_TopPanel = UIRoot.Find("gamebg/PanelTop").GetComponent<UI_TopPanel>();
        UI_GameOverPanel = UIRoot.Find("gameoverPanel").GetComponent<UI_GameOverPanel>();
        UI_SetPanel = UIRoot.Find("SetPanel").GetComponent<UI_SetPanel>();
        UI_AddRotatePanel = UIRoot.Find("AddRotatePanel").GetComponent<UI_AddRotatePanel>();

        DragingGridMgr.Inst.SetDrag(DragRoot);
    }

    public bool IsRotateState { get { return UI_GroupRotate.IsRotateState; }}

    public void AddRotateGoldCount(int v)
    {
        UI_GroupRotate.AddRotateGoldCount(v);
    }
    public void OffChangeRotate()
    {
        UI_GroupRotate.OffChangeRotate();
    }
    public void SWRotate(int i, bool v)
    {
        UI_GroupRotate.SWRotate(i, v);
    }
    public bool GetLocalPoint_BgRoot(out Vector2 pos)
    {
        return RectTransformUtility.ScreenPointToLocalPointInRectangle(BGROOTRect, Input.mousePosition, Canvas.worldCamera, out pos);
    }
    public bool GetLocalPoint_Canv(out Vector2 pos)
    {
        return RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvasRect, Input.mousePosition, Canvas.worldCamera, out pos);
    }

    public void ResetTop()
    {
        UI_TopPanel.ResetTop();
    }
    public void ResetNowScore()
    {
        UI_TopPanel.ResetNowScore();
    }
    public void WriteTopScore()
    {
        UI_TopPanel.WriteTopScore();
    }
    public bool IsTopScore()
    {
       return UI_TopPanel.IsTopScore();
    }
    public void SetNowScore(int score)
    {
        UI_TopPanel.SetNowScore(score);
    }
    public void OpenGameOverPanel()
    {
        UI_GameOverPanel.ShowGameOver();
    }
    public void OnBtnAddRotateSW()
    {
        AudioMgr.Inst.ButtonClick();
        UI_AddRotatePanel.ShowBoxX();
    }
    public void OnBtnSetSw()
    {
        AudioMgr.Inst.ButtonClick();
        UI_SetPanel.ShowBoxX();
    }
    // Start is called before the first frame update
    void Start()
    {
        AudioMgr.Inst.isPlaying_Music = GameGloab.MusicOnOff == 0;
        AudioMgr.Inst.isPlaying_Sound = GameGloab.SoundIsOnOff == 0;
    }
     
}
