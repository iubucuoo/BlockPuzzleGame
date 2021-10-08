using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Inst;

    public Transform UIRoot { get; private set; }
    public Transform ADDROOT { get; private set; }
    public Transform BGROOT { get; private set; }
    public RectTransform CanvasRect { get; private set; }
    RectTransform BGROOTRect;
    Canvas Canvas;
    Transform DragRoot;
    Transform CanvasObj;

    UI_TopPanel UI_TopPanel;
    UI_GameOverPanel UI_GameOverPanel;
    UI_SetPanel UI_SetPanel;

    public Dictionary<string, Sprite> Sprites;
    private void Awake()
    {
        Inst = this;
#if UNITY_EDITOR
        DebugMgr.EnableLog = true;
#endif
        CanvasObj = GameObject.Find("Canvas").transform;
        UIRoot = CanvasObj.Find("Root");
        Sprites = new Dictionary<string, Sprite>();
        Canvas = CanvasObj.GetComponent<Canvas>();
        BGROOT = UIRoot.Find("BGROOT");
        ADDROOT = UIRoot.Find("ADDROOT");
        DragRoot = UIRoot.Find("DragRoot");

        CanvasRect = CanvasObj.GetComponent<RectTransform>();
        BGROOTRect = BGROOT.GetComponent<RectTransform>();

        UI_TopPanel = UIRoot.Find("gamebg/PanelTop").GetComponent<UI_TopPanel>();
        UI_GameOverPanel = UIRoot.Find("gameoverPanel").GetComponent<UI_GameOverPanel>();
        UI_SetPanel = UIRoot.Find("SetPanel").GetComponent<UI_SetPanel>();

        DragingGridMgr.Inst.SetDrag(DragRoot);
    }

    public bool GetLocalPoint_BgRoot(out Vector2 pos)
    {
        return RectTransformUtility.ScreenPointToLocalPointInRectangle(BGROOTRect, Input.mousePosition, Canvas.worldCamera, out pos);
    }
    public bool GetLocalPoint_Canv(out Vector2 pos)
    {
        return RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvasRect, Input.mousePosition, Canvas.worldCamera, out pos);
    }

    public void ResetTop()
    {
        UI_TopPanel.ResetTop();
    }
    public void ResetNowScore()
    {
        UI_TopPanel.ResetNowScore();
    }
    public void WriteTopScore()
    {
        UI_TopPanel.WriteTopScore();
    }
    public bool IsTopScore()
    {
       return UI_TopPanel.IsTopScore();
    }
    public void SetNowScore(int score)
    {
        UI_TopPanel.SetNowScore(score);
    }
    public void OpenGameOverPanel()
    {
        UI_GameOverPanel.ShowGameOver();
    }
    public void OnBtnSetSw()
    {
        AudioManager.Inst.ButtonClick();
        UI_SetPanel.ShowBoxX();
    }
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Inst.isPlaying_Music = GameGloab.MusicOnOff == 0;
        AudioManager.Inst.isPlaying_Sound = GameGloab.SoundIsOnOff == 0;
    }
     
}
