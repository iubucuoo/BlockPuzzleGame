using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TopPanel : UIEventListenBase
{
    public Text strtopnum;
    public Text strnownum;
    public Button setbtn;
    public int nownum = 0;
    int nowgametop;
    // Start is called before the first frame update
    void Start()
    {
        SetNowGameTop(GameGloab.Topscore);//初始化top
        setbtn.onClick.AddListener(OnBtnSwSetPanel);
        ResetTop();
    }
    float speet = .05f;
    float timer = .05f;
    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0 && swnum < nownum)
        {
            SWScore();
            timer = speet;
        }
    }
    int swnum = 0;
    void SWScore()
    {
        swnum++;
        strnownum.text = swnum.ToString();
    }

    private void OnBtnSwSetPanel()
    {
        AudioMgr.Inst.ButtonClick();
        SendEventMgr.GSendMsg((ushort)UIMainListenID.SwPanel_Set);
    }

    void ResetTop()
    {
        ResetTopScore();
        ResetNowScore();
    }
    void ResetTopScore()
    {
        strtopnum.text = GameGloab.Topscore.ToString();
    }
    void ResetNowScore()
    {
        SetNowNum(0);
        swnum = 0;
        strnownum.text = "0";
    }
    void SetNowScore(int score)
    {
        SetNowNum(nownum + score);
        if (nownum > GameGloab.Topscore)
        {
            GameGloab.Topscore = nownum;
            ResetTopScore();
        }
        MainC.Inst.IsTopScore = IsTopScore();
    }

    void WriteTopScore()
    {
        SetNowGameTop(nownum);
    }
    void SetNowNum(int v)
    {
        nownum = v;
        MainC.Inst.IsTopScore = IsTopScore();
    }
    void SetNowGameTop(int v)
    {
        nowgametop = v;
        MainC.Inst.IsTopScore = IsTopScore();
    }
    bool IsTopScore()
    {
        if (nownum > nowgametop)
        {
            return true;
        }
        return false;
    }

    public override void ProcessEvent(MessageBase tmpMsg)
    {
        switch (tmpMsg.messageId)
        {
            case (ushort)UITopPanelListenID.SetNowScore:
                Message msg = (Message)tmpMsg;
                SetNowScore(msg.num);
                break;
            case (ushort)UITopPanelListenID.ResetTop:
                ResetTop();
                break;
            case (ushort)UITopPanelListenID.WriteTopScore:
                WriteTopScore();
                break;
            default:
                break;
        }
        base.ProcessEvent(tmpMsg);
    }

    public override void InitEventListen()
    {
        messageIds = new ushort[]
        {
            (ushort)UITopPanelListenID.SetNowScore,
            (ushort)UITopPanelListenID.WriteTopScore,
            (ushort)UITopPanelListenID.ResetTop,
        };
        RegistEventListen(this, messageIds);
    }
}
