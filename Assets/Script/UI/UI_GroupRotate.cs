using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GroupRotate : UIEventListenBase
{
    public Toggle RotateBnt;
    public Text GoldNum;
    public Button BtnAddGlog;
    List<GameObject> RotateImgs = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        UIMgr.Inst.RotateGoldAddPos = GoldNum.transform.position;
        AddRotateImg();
        //GameGloab.GoldCount=0;
        AddRotateGoldCount(GameGloab.GoldCount);
        ChangeRotate(false);
        RotateBnt.onValueChanged.AddListener(ChangeRotate);
        BtnAddGlog.onClick.AddListener(OpenAddRotatePanel);
    }

    private void OpenAddRotatePanel()
    {
        AudioMgr.Inst.ButtonClick();
        SendEventMgr.GSendMsg((ushort)UIMainListenID.SwPanel_AddRotate);
    }

    public void AddRotateGoldCount(int v=0)
    {
        GameGloab.GoldCount += v;
        GoldNum.text = GameGloab.GoldCount.ToString();
        if (GameGloab.GoldCount <= 0)
        {
            OffChangeRotate();// UIMgr.Inst.OffChangeRotate();
        }
    }
    public void OffChangeRotate()
    {
        RotateBnt.isOn = false;
    }
    private void ChangeRotate(bool arg0)
    {
        if (arg0)
        {
            if (GameGloab.GoldCount<=0)
            {
                DebugMgr.LogError("金币不足不能开启");
                //RotateBnt.SetIsOnWithoutNotify(false);
                
                RotateBnt.isOn = false;
                OpenAddRotatePanel();
            }
            else
            {
                SwitchRotateState(true);
            }
        }
        else
        {
            SwitchRotateState(false);
        }
        DebugMgr.Log(RotateBnt.isOn);
    }

    /// <summary>
    /// 是否可旋转的状态开关
    /// </summary>
    public void SwitchRotateState(bool v)
    {
        //还原待用的组的旋转
        GridGroupMgr.Inst.BackRotate();
        UIMgr.Inst.IsRotateState = v;
        SWRotates(v);
       
    }
    void SWRotates(bool v)
    {
        for (int i = 0; i < 3; i++)
        {
            if (v)
            {
                SWRotate(i, GridGroupMgr.Inst.IsCantUsePrep(i));
            }
            else
            {
                SWRotate(i, v);
            }
        }
    }
    public void SWRotate(int i,bool v)
    {
        if (RotateImgs[i]!=null)
        {
            RotateImgs[i].SetActive(v);
            if (v)
            {
                //开始旋转
            }
        }
    }
    void AddRotateImg()
    {
        for (int i = 0; i < 3; i++)
        {
            Vector2 pos = new Vector2((i - 1) * (6 * UIMgr.Inst.wh_2), 0);
            var obj = ObjectMgr.InstantiateGameObj(ObjectMgr.LoadResource("Prefab/addrotateimg") as GameObject);
            obj.transform.SetParent(transform);
            obj.transform.localPosition = pos;
            obj.transform.localScale = Vector2.one;
            RotateImgs.Add( obj);
        }
        SWRotates(false);
    }
    public override void InitEventListen()
    {
        messageIds = new ushort[]
       {
            (ushort)UIGroupRotateListenID.Up,
            (ushort)UIGroupRotateListenID.OffRotate,
            (ushort)UIGroupRotateListenID.SwOne,
            (ushort)UIGroupRotateListenID.HideOne,
            (ushort)UIGroupRotateListenID.AddRotateGold,
       };
        RegistEventListen(this, messageIds);
    }
    public override void ProcessEvent(MessageBase tmpMsg)
    {
        switch (tmpMsg.messageId)
        {
            case (ushort)UIGroupRotateListenID.OffRotate:
                OffChangeRotate();//关闭
                break;
            case (ushort)UIGroupRotateListenID.OnRotate:
                //开启
                break;
            case (ushort)UIGroupRotateListenID.HideOne:
                SWRotate(((Message)tmpMsg).num, false);
                break;
            case (ushort)UIGroupRotateListenID.SwOne:
                SWRotate(((Message)tmpMsg).num, true);
                break;
            case (ushort)UIGroupRotateListenID.AddRotateGold:
                AddRotateGoldCount(((Message)tmpMsg).num);
                break;
            default:
                break;
        }
        base.ProcessEvent(tmpMsg);
    }
}
