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
        gameObject.SetActive(false);//弹出广告直接隐藏
        GoogleAdMgr.Inst.GameOver(YesCallBack);
    }
    void YesCallBack()
    {
        AudioMgr.Inst.PlayGameOpen();
        UIMgr.Inst.ResetTop();
        GridGroupMgr.Inst.GameReset();//重新启动游戏
    }
    private void OnBtnAllBg()
    {
        AudioMgr.Inst.ButtonClick();
        HideBox();
    }

    private void OnBtnSwAd()
    {
        Debug.LogError("展示广告");//显示广告
    }

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
}
