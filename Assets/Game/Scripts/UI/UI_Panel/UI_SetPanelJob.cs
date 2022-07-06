using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SetPanelJob : UIPanelBase
{
    public Toggle MusicToggle;
    public Toggle SoundToggle;

    public GameObject AllBg;
    public GameObject BtnResetGame;
    public GameObject BtnBackGame;
    public GameObject BtnBackGame1;
    void Awake()
    {
        MusicToggle = transform.Find("ToggleMusicSet").GetComponent<Toggle>();
        MusicToggle = transform.Find("ToggleSoundSet").GetComponent<Toggle>();
        AllBg = transform.Find("allbg").gameObject;
        BtnResetGame = transform.Find("BtnResetGame").gameObject;
        BtnBackGame = transform.Find("BtnBackGame").gameObject;
        BtnBackGame1 = transform.Find("BtnBackGame1").gameObject;

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
        GoogleAdMgr.Inst.SWAd(YesCallBack);
        HideFinish();//弹出广告直接隐藏
       
    }
    public override void HideFinish()
    {
        AllUIPanelManager.Inst.Hide(IPoolsType.UI_SetPanel);
    }
    private void OnBtnAllBg()
    {
        AudioMgr.Inst.ButtonClick();
        HideFinish();
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
