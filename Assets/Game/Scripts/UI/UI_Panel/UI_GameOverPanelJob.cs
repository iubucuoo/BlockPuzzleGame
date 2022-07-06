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
        gameover =transform.Find("gameover").gameObject;
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
        ShowFinish();
        //先来一个屏幕变暗动作 跳出没有可放的位置
        //再弹出游戏结束面板
        //ShowBoxY(ShowFinish);       
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
       
}
