using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TopPanel : MonoBehaviour
{
    public Text strtopnum;
    public Text strnownum;
    public Button setbtn;
    public int nownum=0;
    int nowgametop;
    // Start is called before the first frame update
    void Start()
    {
        nowgametop = GameGloab.Topscore;//初始化top
        setbtn.onClick.AddListener(OnBtnSwSetPanel);
        ResetTop();
    }
    float speet = .05f;
    float timer=.05f;
    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer<0&& swnum < nownum)
        {
            SWScore();
            timer = speet;
        }
    }
    int swnum=0;
    void SWScore()
    {
        swnum++;
        strnownum.text = swnum.ToString();
    }

    private void OnBtnSwSetPanel()
    {
        UIMgr.Inst.OnBtnSetSw();
    }

    public void ResetTop()
    {
        ResetTopScore();
        ResetNowScore();
    }
    void ResetTopScore()
    {
        strtopnum.text = GameGloab.Topscore.ToString();
    }
    public void ResetNowScore()
    {
        nownum = 0;
        swnum = 0;
        strnownum.text = "0";
    }
    public void SetNowScore(int score)
    {
        nownum += score;
        if (nownum>GameGloab.Topscore)
        {
            GameGloab.Topscore = nownum;
            ResetTopScore();
        }
    }

    public void WriteTopScore()
    {
        nowgametop = nownum;
    }
    public bool IsTopScore()
    {
        if (nownum > nowgametop)
        {
            return true;
        }
        return false;
    }
}
