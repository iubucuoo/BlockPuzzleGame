using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StartPanel : UIBase
{ 
    public override string WndName => IPoolsType.UI_StartPanel.ToString();
    public override bool isFull => true;
    public override IPoolsType PoolType =>  IPoolsType.UI_StartPanel;
 
    public override void OnDestroy_m()
    {
        startbtn.onClick.RemoveListener(OnBtnStart);
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
