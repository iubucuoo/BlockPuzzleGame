using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameOverPanel : UIPanelBase
{
    public GameObject gameover;
    public GameObject newrecord;
    public Text newrecordtxt;
    public Button btnRefresh;
    // Start is called before the first frame update
    void Start()
    {
        btnRefresh.onClick.AddListener(OnBtnRefresh);
    }

    void ShowFinish()
    {
        bool isnewrecord = UIManager.Inst.IsTopScore();
        gameover.SetActive(!isnewrecord);
        newrecord.SetActive(isnewrecord);
        if (isnewrecord)
        {
            UIManager.Inst.WriteTopScore();
            AudioManager.Inst.PlayNewRecord();//播放 新记录音乐UI
            newrecordtxt.text = GameGloab.Topscore.ToString();
        }
        else
        {
            AudioManager.Inst.PlayGameOver();
        }
    }
    public void ShowGameOver()
    {
        //先来一个屏幕变暗动作 跳出没有可放的位置
        //再弹出游戏结束面板
        ShowBoxY(ShowFinish);       
    }
    private void OnBtnRefresh()
    {
        AudioManager.Inst.ButtonClick();
        gameObject.SetActive(false);
        GoogleAdManager.Inst.GameOver(RefreshCallBack);
    }
    void RefreshCallBack()
    {
        AudioManager.Inst.PlayGameOpen();
        UIManager.Inst.ResetTop();
        GridGroupMgr.Inst.GameReset();//重新启动游戏
    }
}
