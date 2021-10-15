using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_AddRotatePanel : UIPanelBase
{
    public Button BtnAd;
    public Button AllBg;
    public Button BtnResetGame;
    public Button BtnBackGame;
    
    // Start is called before the first frame update
    void Start()
    {
        BtnAd.onClick.AddListener(OnBtnSwAd);
        BtnBackGame.onClick.AddListener(OnBtnAllBg);
        AllBg.onClick.AddListener(OnBtnAllBg);
        BtnResetGame.onClick.AddListener(OnBtnResetGame);
    }

    private void OnBtnResetGame()
    {
        AudioMgr.Inst.ButtonClick();
        HideFinish();//弹出广告直接隐藏
        GoogleAdMgr.Inst.SWAd(RefreshGame);
    }
    private void OnBtnAllBg()
    {
        AudioMgr.Inst.ButtonClick();
        HideBox();
    }

    private void OnBtnSwAd()
    {
        AudioMgr.Inst.ButtonClick();
        HideFinish();
        GoogleAdMgr.Inst.SWAd(AddRotateGold);
    }
    void AddRotateGold()
    {
        float[] _pos = new float[] { UIMgr.Inst.RotateGoldAddPos.x, UIMgr.Inst.RotateGoldAddPos.y };
        SendEventMgr.GSendMsg((ushort)UISwTextEffectListenID.SwEffect, _pos, "+2");
        SendEventMgr.GSendMsg((ushort)UIGroupRotateListenID.AddRotateGold, 2);
    }

    private void EffectCb()
    {
        SendEventMgr.GSendMsg((ushort)UIGroupRotateListenID.AddRotateGold, 2);
    }
}
