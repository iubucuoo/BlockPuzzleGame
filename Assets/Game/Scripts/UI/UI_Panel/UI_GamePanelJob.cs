﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GamePanelJob : UIEventListenBase
{
    UI_GamePanel gamepanel;
    // Start is called before the first frame update
    public void SetPanel(UI_GamePanel ui)
    {
        gamepanel = ui;
    }
    RectTransform BGROOTRect;
    RectTransform CanvasRect;
    Canvas Canvas;
    UI_GamePanel_Top toppanel;
    UI_GamePanel_GroupRotate rotatepanel;
    public Transform ROTATEROOT;
    private void Awake()
    {
        Canvas = GetComponent<Canvas>();
        CanvasRect=GetComponent<RectTransform>();
        BGROOTRect = transform.Find("BGROOT").GetComponent<RectTransform>();
        gameObject.AddComponent<GridGroupMgr>();
        GridGroupMgr.Inst.ADDROOT = transform.Find("ADDROOT");
        GridGroupMgr.Inst.BGROOT = transform.Find("BGROOT");
        ROTATEROOT = transform.Find("ROTATEROOT");
        DragingGridMgr.Inst.SetInit(transform.Find("DragRoot"),this);
    }
    private void Start()
    {
        toppanel = new UI_GamePanel_Top(transform.Find("gamebg/PanelTop"));
        rotatepanel = new UI_GamePanel_GroupRotate(transform.Find("gamebg"),this);
        AudioMgr.Inst.ButtonClick();
        AudioMgr.Inst.PlayBGMusic();
        GridGroupMgr.Inst.GameStart();
    }

    Vector3 oldmousepos;
    private void Update()
    {
        toppanel.Update();
        //if (Input.GetMouseButtonDown(0))
        //{
        //    if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bgrectTransform, Input.mousePosition, canvas.worldCamera, out Vector2 pos1))
        //    {
        //        int posx = GridGroupMgr.OutGridPos(pos1.x);
        //        int posy = GridGroupMgr.OutGridPos(pos1.y);
        //        if (GridGroupMgr.Inst.Postox.ContainsKey(posx) && GridGroupMgr.Inst.Postoy.ContainsKey(posy))
        //        {
        //            Debug.Log("鼠标相对于bgroot的ui位置" + pos1 + "     " +  posy + "   " + posx + "     " + GridGroupMgr.Inst.Postoy[posy] + "   " + GridGroupMgr.Inst.Postox[posx]);
        //        }
        //    }
        //}
#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            if (DragingGridMgr.Inst.IsDrag)
            {
                if (Time.frameCount % 10 == 0)//隔10针检测一次
                {
                    PosCheck();
                }
                PosSet();
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            DragPos = GameGloab.OutScreenV2;
            SetDragRootPos();
            //Debug.LogError("GetMouseButtonUp------    " + DragingGridMgr.Inst.IsDrag);
        }
#else

        //手机端 检测touch
        if (Input.touchCount > 0)
        {
            if ( Input.GetTouch(0).phase == TouchPhase.Moved ||  Input.GetTouch(0).phase == TouchPhase.Began)
            {
                if (DragingGridMgr.Inst.IsDrag)
                {
                    if (Time.frameCount % 5 == 0)
                    { PosCheck(); }
                    PosSet();
                }
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                OldDragPos = Vector2.zero;//放置同一个位置点击的时候不处理位置改动
                DragPos = GameGloab.OutScreenV2;//防止残留的位置是上次的位置导致显示闪一下
                SetDragRootPos();
            }
        }
#endif
    }
    void PosSet()
    {
        if (GetLocalPoint_Canv(out Vector2 pos))
        {
            DragPos = pos + MainC.DragUp;//拖动位置用来显示
            SetDragRootPos();
        }
    }
    public bool GetLocalPoint_Canv(out Vector2 pos)
    {
        return RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvasRect, Input.mousePosition, Canvas.worldCamera, out pos);
    }
    public bool GetLocalPoint_BgRoot(out Vector2 pos)
    {
        return RectTransformUtility.ScreenPointToLocalPointInRectangle(BGROOTRect, Input.mousePosition, Canvas.worldCamera, out pos);
    }
    void PosCheck()
    {
        if ((oldmousepos - Input.mousePosition).sqrMagnitude > 90)
        {
            if (GetLocalPoint_BgRoot(out Vector2 pos1))
            {
                //Debug.Log("鼠标相对于bgroot的ui位置" + pos1 + (oldmousepos - Input.mousePosition).sqrMagnitude);
                GridGroupMgr.Inst.CheckAvailable(pos1 + MainC.DragUp);//位置检测 用来判断能否放置
            }
            oldmousepos = Input.mousePosition;
        }
    }
    void SetDragRootPos()
    {
        DragingGridMgr.Inst.DragRoot.localPosition = DragPos;
    }
    Vector2 DragPos;

    public override void InitEventListen()
    {
        messageIds = new ushort[]
       {
            (ushort)GamePanelListenID.Test1,
            (ushort)GamePanelListenID.Test2,
            (ushort)GamePanelListenID.Test3,
            (ushort)GamePanelListenID.Test4,
            (ushort)UITopPanelListenID.ResetTop,
            (ushort)UITopPanelListenID.WriteTopScore,
            (ushort)UITopPanelListenID.SetNowScore,

            (ushort)UIMainListenID.AdAndRefreshGame,
            (ushort)UIGroupRotateListenID.OffRotate,
            (ushort)UIGroupRotateListenID.SwOne,
            (ushort)UIGroupRotateListenID.HideOne,
            (ushort)UIGroupRotateListenID.AddRotateGold,
       };
        RegistEventListen(this, messageIds);
    }



    public override void ProcessEvent(MessageBase tmpMsg)
    {
        if (gamepanel==null || toppanel == null ||!gamepanel.visible)
        {
            DebugMgr.LogError("startpanel or  toppanel 未显示");
            return;
        }
        switch (tmpMsg.messageId)
        {
            case (ushort)GamePanelListenID.Test1:
                DebugMgr.LogError("----Test1----");
                break;
            case (ushort)GamePanelListenID.Test2:
                DebugMgr.LogError("----Test2----");
                break;
            case (ushort)GamePanelListenID.Test3:
                DebugMgr.LogError("----Test3----");
                break;
            case (ushort)GamePanelListenID.Test4:
                DebugMgr.LogError("----Test4----");
                break;
            case (ushort)UIMainListenID.AdAndRefreshGame:
                GoogleAdMgr.Inst.SWAd(RefreshGame);
                break;
            case (ushort)UITopPanelListenID.ResetTop:
                toppanel.ResetTop();
                break;
            case (ushort)UITopPanelListenID.WriteTopScore:
                toppanel.WriteTopScore();
                break;
            case (ushort)UITopPanelListenID.SetNowScore:
                Message msg = (Message)tmpMsg;
                toppanel.SetNowScore(msg.num);
                break;
            case (ushort)UIGroupRotateListenID.OffRotate:
               rotatepanel.OffChangeRotate();//关闭
                break;
            case (ushort)UIGroupRotateListenID.HideOne:
                rotatepanel.SWRotate(((Message)tmpMsg).num, false);
                break;
            case (ushort)UIGroupRotateListenID.SwOne:
                rotatepanel.SWRotate(((Message)tmpMsg).num, true);
                break;
            case (ushort)UIGroupRotateListenID.AddRotateGold:
                rotatepanel.AddRotateGoldCount(((Message)tmpMsg).num);
                break;
            default:
                break;
        }
    }
    void RefreshGame()
    {
        toppanel.ResetTop();
        GridGroupMgr.Inst.GameReset();//重新启动游戏
        AudioMgr.Inst.PlayGameOpen();
    }
    public void UnRegistEvents()
    {
        UnRegistEventListen(this, messageIds);
    }
}
