using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SetPanel : UIPanelBase
{
    public Toggle MusicToggle;
    public Toggle SoundToggle;

    public GameObject AllBg;
    public GameObject BtnResetGame;
    public GameObject BtnBackGame;
    public GameObject BtnBackGame1;
    void Start()
    {
        BtnBackGame.GetComponent<Button>().onClick.AddListener(OnBtnAllBg);
        BtnBackGame1.GetComponent<Button>().onClick.AddListener(OnBtnAllBg);
        AllBg.GetComponent<Button>().onClick.AddListener(OnBtnAllBg);
        BtnResetGame.GetComponent<Button>().onClick.AddListener(OnBtnResetGame);
        MusicToggle.onValueChanged.AddListener(ChangeMusicIsOn);
        SoundToggle.onValueChanged.AddListener(ChangeSoundIsOn);
        MusicToggle.isOn = GameGloab.MusicOnOff == 0;
        SoundToggle.isOn = GameGloab.SoundIsOnOff == 0;
    }

    void YesCallBack()
    {
        //Debug.Log("点击重开游戏----广告播放返回");
        AudioMgr.Inst.PlayGameOpen();
        SendEventMgr.GSendMsg((ushort)UITopPanelListenID.ResetTop); //UIMgr.Inst.ResetTop();
        GridGroupMgr.Inst.GameReset();//重新启动游戏
    }
    private void OnBtnResetGame()
    {
        AudioMgr.Inst.ButtonClick();
        //Confirm.SetActive(true);
        HideFinish();// gameObject.SetActive(false);//弹出广告直接隐藏
        GoogleAdMgr.Inst.SWAd(YesCallBack);
        //confirmNo.onClick.AddListener(OnBtnConfirmNo);
        //confirmYes.onClick.AddListener(OnBtnConfirmYes);
    }

    private void OnBtnAllBg()
    {
        AudioMgr.Inst.ButtonClick();
        HideBox();
    }

    void ChangeSoundIsOn(bool ison)
    {
        AudioMgr.Inst.ButtonClick();
        GameGloab.SoundIsOnOff = ison ? 0 : 1;
        AudioMgr.Inst.isPlaying_Sound = ison;
       
    }
    void ChangeMusicIsOn(bool ison)
    {
        AudioMgr.Inst.ButtonClick();
        GameGloab.MusicOnOff = ison ? 0 : 1;
        AudioMgr.Inst.isPlaying_Music = ison;
        if (ison)
        {
            AudioMgr.Inst.PlayBGMusic();
        }
        else
        {
            AudioMgr.Inst.StopBGMusic();
        }
    }
}
