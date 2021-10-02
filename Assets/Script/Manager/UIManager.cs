using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Inst;
    UI_TopPanel TopPanel;
    UI_GameOverPanel GameOverPanel;

    public Transform UIRoot;
    public Transform ADDROOT { get; private set; }
    public Transform BGROOT { get; private set; }
    public Transform DragRoot { get; private set; }
    public RectTransform BGROOTRect { get; private set; }

    public Dictionary<string, Sprite> Sprites;
    private void Awake()
    {
        Inst = this;
#if UNITY_EDITOR
        DebugMgr.EnableLog = true;
#endif
        Sprites = new Dictionary<string, Sprite>();
        Canvas = CanvasObj.GetComponent<Canvas>();
        BGROOT = UIRoot.transform.Find("BGROOT");
        ADDROOT = UIRoot.transform.Find("ADDROOT");
        DragRoot = UIRoot.transform.Find("DragRoot");
        CanvasRect = CanvasObj.GetComponent<RectTransform>();
        BGROOTRect = BGROOT.GetComponent<RectTransform>();


        TopPanel = toppanel.GetComponent<UI_TopPanel>();
        GameOverPanel = gameoverpanel.GetComponent<UI_GameOverPanel>();

        DragingGridMgr.Inst.SetDrag(DragRoot);
    }
    public GameObject setpanel;
    public GameObject gameoverpanel;
    public GameObject toppanel;
    public GameObject CanvasObj;
    //public Transform UICanvasObj;
    public Canvas Canvas { get; private set; }
    public RectTransform CanvasRect { get; private set; }

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
        TopPanel.ResetTop();
    }
    public void ResetNowScore()
    {
        TopPanel.ResetNowScore();
    }
    public void WriteTopScore()
    {
        TopPanel.WriteTopScore();
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
        setpanel.GetComponent<UI_SetPanel>().ShowBoxX();
    }
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Inst.isPlaying_Music = GameGloab.MusicOnOff == 0;
        AudioManager.Inst.isPlaying_Sound = GameGloab.SoundIsOnOff == 0;
    }
     
}
