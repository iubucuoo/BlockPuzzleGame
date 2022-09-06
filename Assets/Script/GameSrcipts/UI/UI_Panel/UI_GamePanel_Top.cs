using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GamePanel_Top 
{
    Text strtopnum;
    Text strnownum;
    Button setbtn;
    public int nownum = 0;
    int nowgametop;
    Transform Wnd;

    int swnum = 0;
    public UI_GamePanel_Top(Transform wnd)
    {
        Wnd = wnd;
        strtopnum = wnd.Find("top/Text").GetComponent<Text>();
        strnownum = wnd.Find("now/Text").GetComponent<Text>();
        setbtn = wnd.Find("SetBtn").GetComponent<Button>();

        SetNowGameTop(GameStatic.Topscore);//初始化top
        setbtn.onClick.AddListener(OnBtnSwSetPanel);
        ResetTop();
    }
    float speet = .05f;
    float timer = .05f;
    public void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0 && swnum < nownum)
        {
            SWScore();
            timer = speet;
        }
    }
    void SWScore()
    {
        swnum++;
        strnownum.text = swnum.ToString();
    }
    private void OnBtnSwSetPanel()
    {
        AudioMgr.Inst.ButtonClick();
        AllUIPanelManager.Inst.Show(IPoolsType.UI_SetPanel,true);
    }
    public void ResetTop()
    {
        ResetTopScore();
        ResetNowScore();
    }
    void ResetTopScore()
    {
        strtopnum.text = GameStatic.Topscore.ToString();
    }
    void ResetNowScore()
    {
        SetNowNum(0);
        swnum = 0;
        strnownum.text = "0";
    }
    public void SetNowScore(int score)
    {
        SetNowNum(nownum + score);
        if (nownum > GameStatic.Topscore)
        {
            GameStatic.Topscore = nownum;
            ResetTopScore();
        }
        GameStatic.IsTopScore = IsTopScore();
    }
    public void WriteTopScore()
    {
        SetNowGameTop(nownum);
    }
    void SetNowNum(int v)
    {
        nownum = v;
        GameStatic.IsTopScore = IsTopScore();
    }
    void SetNowGameTop(int v)
    {
        nowgametop = v;
        GameStatic.IsTopScore = IsTopScore();
    }
    bool IsTopScore()
    {
        return nownum > nowgametop;
    }
}
