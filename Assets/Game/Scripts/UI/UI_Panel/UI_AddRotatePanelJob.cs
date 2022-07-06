using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_AddRotatePanelJob : UIEventListenBase
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

    private void OnBtnResetGame()
    {
        OnBtnHide();//弹出广告直接隐藏
        SendEventMgr.GSendMsg((ushort)UIMainListenID.AdAndRefreshGame);
    }
    private void OnBtnAllBg()
    {
        OnBtnHide();
    }

    private void OnBtnSwAd()
    {
        OnBtnHide();
        GoogleAdMgr.Inst.SWAd(AddRotateGold,NoOkAd);
    }
    void OnBtnHide()
    {
        AudioMgr.Inst.ButtonClick();
        AllUIPanelManager.Inst.Hide(IPoolsType.UI_AddRotatePanel);
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
