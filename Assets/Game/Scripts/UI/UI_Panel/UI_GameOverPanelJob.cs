using UnityEngine;
using UnityEngine.UI;

public class UI_GameOverPanelJob : UIPanelBase
{
    GameObject gameover;
    GameObject newrecord;
    Text newrecordtxt;
    Button btnRefresh;
    // Start is called before the first frame update
    void Awake()
    {
        gameover = transform.Find("gameover").gameObject;
        newrecord = transform.Find("newrecord").gameObject;
        btnRefresh = transform.Find("btnRefresh").GetComponent<Button>();
        btnRefresh.onClick.AddListener(OnBtnRefresh);
    }

    void ShowFinish()
    {
        bool isnewrecord = MainC.Inst.IsTopScore;
        gameover.SetActive(!isnewrecord);
        newrecord.SetActive(isnewrecord);
        if (isnewrecord)
        {
            SendEventMgr.GSendMsg((ushort)UITopPanelListenID.WriteTopScore);
            AudioMgr.Inst.PlayNewRecord();//播放 新记录音乐UI
            newrecordtxt.text = GameGloab.Topscore.ToString();
        }
        else
        {
            AudioMgr.Inst.PlayGameOver();
        }
    }
    public void ShowGameOver()
    {
        //先来一个屏幕变暗动作 跳出没有可放的位置
        //再弹出游戏结束面板
        ShowBoxY(ShowFinish);       
    }
    public override void HideFinish()
    {
        gameover.SetActive(false);
        newrecord.SetActive(false);
        AllUIPanelManager.Inst.Hide(IPoolsType.UI_GameOverPanel);
    }
    private void OnBtnRefresh()
    {
        AudioMgr.Inst.ButtonClick();
        HideFinish();
        GoogleAdMgr.Inst.SWAd(RefreshGame);
    }

    public override void InitEventListen()
    {
        messageIds = new ushort[]
       {
            (ushort)GameOverPanelListenID.Test1,
            (ushort)GameOverPanelListenID.Test2,
            (ushort)GameOverPanelListenID.Test3,
            (ushort)GameOverPanelListenID.Test4,
       };
        RegistEventListen(this, messageIds);
    }
    public override void ProcessEvent(MessageBase tmpMsg)
    {
        if (!gameObject.activeInHierarchy)
        {
            DebugMgr.LogError("startpanel 未显示");
            return;
        }
        switch (tmpMsg.messageId)
        {
            case (ushort)GameOverPanelListenID.Test1:
                DebugMgr.LogError("--GameOverPanelListenID--Test1----");
                break;
            case (ushort)GameOverPanelListenID.Test2:
                DebugMgr.LogError("--GameOverPanelListenID--Test2----");
                break;
            case (ushort)GameOverPanelListenID.Test3:
                DebugMgr.LogError("--GameOverPanelListenID--Test3----");
                break;
            case (ushort)GameOverPanelListenID.Test4:
                DebugMgr.LogError("--GameOverPanelListenID--Test4----");
                break;
            default:
                break;
        }
    }
    public void UnRegistEvents()
    {
        UnRegistEventListen(this, messageIds);
    }
}
