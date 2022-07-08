using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SetPanelJob : UIEventListenBase
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
    private void OnBtnResetGame()
    {
        SendEventMgr.GSendMsg((ushort)UIMainListenID.AdAndRefreshGame);
        //DebugMgr.LogError("OnBtnResetGame");
        OnBtnHide();//弹出广告直接隐藏
    }

    private void OnBtnAllBg()
    {
        OnBtnHide();
    }
    void OnBtnHide()
    {
        AudioMgr.Inst.ButtonClick();
        AllUIPanelManager.Inst.Hide(IPoolsType.UI_SetPanel);
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
