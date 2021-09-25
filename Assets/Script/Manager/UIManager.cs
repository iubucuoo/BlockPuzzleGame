using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Inst;
    UI_TopPanel TopPanel;
    UI_GameOverPanel GameOverPanel;
    private void Awake()
    {
        Inst = this;
        TopPanel = toppanel.GetComponent<UI_TopPanel>();
        GameOverPanel = gameoverpanel.GetComponent<UI_GameOverPanel>();
    }
    public GameObject setpanel;
    public GameObject gameoverpanel;
    public GameObject toppanel;
    
    public void ResetTop()
    {
        TopPanel.ResetTop();
    }
    public void ResetNowScore()
    {
        TopPanel.ResetNowScore();
    }
    public bool IsTopScore()
    {
       return TopPanel.IsTopScore();
    }
    public void SetNowScore(int score)
    {
        TopPanel.SetNowScore(score);
    }
    public void OpenGameOverPanel()
    {
        GameOverPanel.ShowGameOver();
    }
    public void OnBtnSetHide()
    {
        AudioManager.Inst.ButtonClick();
        setpanel.SetActive(false);
    }
    public void OnBtnSetSw()
    {
        AudioManager.Inst.ButtonClick();
        setpanel.SetActive(true);
    }
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Inst.isPlaying_Music = GameGloab.MusicOnOff == 0;
        AudioManager.Inst.isPlaying_Sound = GameGloab.SoundIsOnOff == 0;
    }
     
}
