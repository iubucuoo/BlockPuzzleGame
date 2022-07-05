using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StartPanel : UIBase,IPoolable
{
    public override UIHideType hideType => UIHideType.WaitDestroy;
    public override UIHideFunc hideFunc => UIHideFunc.MoveOutOfScreen;
    public override int layer { get => (int)UILayer.Panel; set => layer = value; }
    public override string WndName => IPoolsType.UI_StartPanel.ToString();//"UI_StartPanel";
    public override bool isFull => true;
    public IPoolsType GroupType =>  IPoolsType.UI_StartPanel;

    public bool IsRecycled { get ; set; }
    public void OnRecycled()
    {

    }
  
    public override void OnDestroy_m()
    {
        startbtn.onClick.RemoveListener(OnBtnStart);
        PoolMgr.Recycle(this);


        DebugMgr.Log("RemoveListener");
    }
    public override void OnCreate()
    {
        Init();
        InitEvent();
    }
    Button startbtn;
    void Init()
    {
        startbtn = WndRoot.transform.Find("Button").GetComponent<Button>();
        DebugMgr.Log("Init");
    }
    void InitEvent()
    {
        startbtn.onClick.AddListener(OnBtnStart);
        DebugMgr.Log("InitEvent");
    }
    void OnBtnStart()
    {
        DebugMgr.Log("开始游戏");
        AllUIPanelManager.Inst.Show(IPoolsType.UI_GamePanel);
    }
}
