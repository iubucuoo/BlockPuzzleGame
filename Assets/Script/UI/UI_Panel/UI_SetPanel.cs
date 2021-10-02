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
    //public GameObject Confirm;
    //public Button confirmYes;
    //public Button confirmNo;
    // Start is called before the first frame update
    void Start()
    {
        BtnBackGame.GetComponent<Button>().onClick.AddListener(OnBtnAllBg);
        AllBg.GetComponent<Button>().onClick.AddListener(OnBtnAllBg);
        BtnResetGame.GetComponent<Button>().onClick.AddListener(OnBtnResetGame);
        MusicToggle.onValueChanged.AddListener(ChangeMusicIsOn);
        SoundToggle.onValueChanged.AddListener(ChangeSoundIsOn);
        MusicToggle.isOn = GameGloab.MusicOnOff == 0;
        SoundToggle.isOn = GameGloab.SoundIsOnOff == 0;
        //Confirm.GetComponent<Button>().onClick.AddListener(OnBtnConfirmNo);
        //Confirm.SetActive(false);
    }

    //private void OnBtnConfirmNo()
    //{
    //    AudioManager.Inst.ButtonClick();
    //    //confirmNo.onClick.RemoveListener(OnBtnConfirmNo);
    //    //confirmYes.onClick.RemoveListener(OnBtnConfirmYes);
    //    Confirm.SetActive(false);
    //}

    //private void OnBtnConfirmYes()
    //{
    //    AudioManager.Inst.ButtonClick();
    //    //展示广告  广告完毕 执行以下操作
    //    //Debug.Log("点击重开游戏");
    //    //confirmNo.onClick.RemoveListener(OnBtnConfirmNo);
    //    //confirmYes.onClick.RemoveListener(OnBtnConfirmYes);
    //    Confirm.SetActive(false);
    //    gameObject.SetActive(false);//弹出广告直接隐藏
    //    GoogleAdManager.Inst.GameOver(YesCallBack);
    //}
    void YesCallBack()
    {
        //Debug.Log("点击重开游戏----广告播放返回");
        AudioManager.Inst.PlayGameOpen();
        UIManager.Inst.ResetTop();
        GridGroupMgr.Inst.GameReset();//重新启动游戏
    }
    private void OnBtnResetGame()
    {
        AudioManager.Inst.ButtonClick();
        //Confirm.SetActive(true);
        gameObject.SetActive(false);//弹出广告直接隐藏
        GoogleAdManager.Inst.GameOver(YesCallBack);
        //confirmNo.onClick.AddListener(OnBtnConfirmNo);
        //confirmYes.onClick.AddListener(OnBtnConfirmYes);
    }

    private void OnBtnAllBg()
    {
        AudioManager.Inst.ButtonClick();
        HideBox();
    }

    public void ChangeSoundIsOn(bool ison)
    {
        AudioManager.Inst.ButtonClick();
        GameGloab.SoundIsOnOff = ison ? 0 : 1;
        AudioManager.Inst.isPlaying_Sound = ison;
       
    }
    public void ChangeMusicIsOn(bool ison)
    {
        AudioManager.Inst.ButtonClick();
        GameGloab.MusicOnOff = ison ? 0 : 1;
        AudioManager.Inst.isPlaying_Music = ison;
        if (ison)
        {
            AudioManager.Inst.PlayBGMusic();
        }
        else
        {
            AudioManager.Inst.StopBGMusic();
        }
    }
    
}
