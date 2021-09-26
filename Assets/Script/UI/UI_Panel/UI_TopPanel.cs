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
    // Start is called before the first frame update
    void Start()
    {
        setbtn.onClick.AddListener(OnBtnSwSetPanel);
        ResetTop();
    }

    private void OnBtnSwSetPanel()
    {
        UIManager.Inst.OnBtnSetSw();
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
        SetNowScore(nownum);
    }
    public void SetNowScore(int score)
    {
        nownum += score;
        strnownum.text = nownum.ToString();
    }
    public bool IsTopScore()
    {
        if (nownum > GameGloab.Topscore)
        {
            GameGloab.Topscore=nownum;
            return true;
        }
        return false;
    }
}
