using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_AddRotatePanelJob : UIPanelBase
{
    public Button BtnAd;
    public Button AllBg;
    public Button BtnResetGame;
    public Button BtnBackGame;
    
    // Start is called before the first frame update
    void Start()
    {
        AllBg = transform.Find("allbg").GetComponent<Button>();
        BtnAd = transform.Find("btnAD").GetComponent<Button>();
        BtnResetGame = transform.Find("btnRefresh").GetComponent<Button>();
        BtnBackGame = transform.Find("btngoon").GetComponent<Button>();

        BtnAd.onClick.AddListener(OnBtnSwAd);
        BtnBackGame.onClick.AddListener(OnBtnAllBg);
        AllBg.onClick.AddListener(OnBtnAllBg);
        BtnResetGame.onClick.AddListener(OnBtnResetGame);
    }
    public override void HideFinish()
    {
        DebugMgr.LogError("UI_AddRotatePanel----HideFinish");
        AllUIPanelManager.Inst.Hide(IPoolsType.UI_AddRotatePanel);
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
        HideFinish();
    }

    private void OnBtnSwAd()
    {
        AudioMgr.Inst.ButtonClick();
        HideFinish();
        GoogleAdMgr.Inst.SWAd(AddRotateGold,NoOkAd);
    }
    void NoOkAd()
    {
        SendEventMgr.GSendMsg((ushort)UISwTextEffectListenID.SwEffect, new float[] { 0, 0 }, "广告还没准备好");   //广告还未加载完成
    }
    void AddRotateGold()
    {
        float[] _pos = new float[] { MainC.Inst.RotateGoldAddPos.x, MainC.Inst.RotateGoldAddPos.y };
        SendEventMgr.GSendMsg((ushort)UISwTextEffectListenID.SwEffect, _pos, "+2");
        SendEventMgr.GSendMsg((ushort)UIGroupRotateListenID.AddRotateGold, 2);
    }

    private void EffectCb()
    {
        SendEventMgr.GSendMsg((ushort)UIGroupRotateListenID.AddRotateGold, 2);
    }
}
