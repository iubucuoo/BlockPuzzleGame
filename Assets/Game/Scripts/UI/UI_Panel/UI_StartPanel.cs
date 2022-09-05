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
        Log.Info("RemoveListener");
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
        Log.Info("Init");
    }
    void InitEvent()
    {
        startbtn.onClick.AddListener(OnBtnStart);
        Log.Info("InitEvent");
    }
    void OnBtnStart()
    {
        Log.Info("开始游戏");
        if (AppParam.LoadArtIsAb)
        {
            new TableArt(StartGame);
        }
        else
        {
            StartGame();
        }
    }
    void StartGame()
    {
        AllUIPanelManager.Inst.Show(IPoolsType.UI_GamePanel);
    }
}
