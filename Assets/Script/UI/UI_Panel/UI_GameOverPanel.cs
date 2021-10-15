using UnityEngine;
using UnityEngine.UI;

public class UI_GameOverPanel : UIPanelBase
{
    public GameObject gameover;
    public GameObject newrecord;
    public Text newrecordtxt;
    public Button btnRefresh;
    // Start is called before the first frame update
    void Start()
    {
        btnRefresh.onClick.AddListener(OnBtnRefresh);
    }

    void ShowFinish()
    {
        bool isnewrecord = UIMgr.Inst.IsTopScore;
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
    private void OnBtnRefresh()
    {
        AudioMgr.Inst.ButtonClick();
        HideFinish();
        GoogleAdMgr.Inst.SWAd(RefreshGame);
    }
}
