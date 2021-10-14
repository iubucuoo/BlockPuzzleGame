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
        HideFinish();// gameObject.SetActive(false);//弹出广告直接隐藏
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
        FreeSendEvent.GSendMsg((ushort)UISwTextEffectListenID.SwEffect, _pos, "+2");
        //UIMgr.Inst.PlayTextTip(UIMgr.Inst.RotateGoldAddPos, "+2");
        FreeSendEvent.GSendMsg((ushort)UIGroupRotateListenID.AddRotateGold, 2);//UIMgr.Inst.AddRotateGoldCount(2);

        //Vector3 end = UIMgr.Inst.RotateGoldAddPos();
        //Vector3 start = end;
        //start.x += 1080;
        //start.y += 100;
        //EffectPool.Inst.PlayFlowEffect(start,UIMgr.Inst.RotateGoldAddPos(),EffectCb);
    }

    private void EffectCb()
    {
        FreeSendEvent.GSendMsg((ushort)UIGroupRotateListenID.AddRotateGold, 2);//UIMgr.Inst.AddRotateGoldCount(2);
    }
    //public override void InitEventListen()
    //{
    //    messageIds = new ushort[]{
    //        (ushort)UIAddRotateListenID.SwPanel,
    //    };
    //    RegistEventListen(this, messageIds);
    //}
    //public override void ProcessEvent(MessageBase tmpMsg)
    //{
    //    switch (tmpMsg.messageId)
    //    {
    //        case (ushort)UIAddRotateListenID.SwPanel:
    //            ShowBoxX();
    //            break;
    //        default:
    //            break;
    //    }
    //    base.ProcessEvent(tmpMsg);
    //}
}
