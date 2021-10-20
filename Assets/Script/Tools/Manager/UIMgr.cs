using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMgr : MonoBehaviour
{
    public static int _height = 90;// 60;
    public static int _width = 90;// 60;
    public static int wh_2 = 45;// 30;
    public static Vector2 DragUp = new Vector2(0, _width * 4);//y高度 对应的倍数
    public static UIMgr Inst;
    public Transform UIRoot { get; private set; }
    public Transform ADDROOT { get; private set; }
    public Transform BGROOT { get; private set; }
    public Transform CanvasRoot { get; private set; }
    public RectTransform CanvasRect { get; private set; }
    RectTransform BGROOTRect;
    Canvas Canvas;
    Transform DragRoot;
    public bool IsTopScore { get; set; }
    public bool IsRotateState { get; set; }
    public Vector3 RotateGoldAddPos { get; set; }

    public int ContinuousBoom = 0;//连续爆炸
    public Dictionary<string, Sprite> Sprites;
    private void Awake()
    {
        Inst = this;
#if UNITY_EDITOR
        DebugMgr.EnableLog = true;
#endif
        CanvasRoot = GameObject.Find("Canvas").transform;
        UIRoot = CanvasRoot.Find("Root");
        Sprites = new Dictionary<string, Sprite>();
        Canvas = CanvasRoot.GetComponent<Canvas>();
        BGROOT = UIRoot.Find("BGROOT");
        ADDROOT = UIRoot.Find("ADDROOT");
        DragRoot = UIRoot.Find("DragRoot");
        CanvasRect = CanvasRoot.GetComponent<RectTransform>();
        BGROOTRect = BGROOT.GetComponent<RectTransform>();
        DragingGridMgr.Inst.SetDrag(DragRoot);

        UIRoot.gameObject.AddComponent<UIPanelSwitch>();
        UIRoot.gameObject.AddComponent<CaneraShaker>();
    }

   

    public bool GetLocalPoint_BgRoot(out Vector2 pos)
    {
        return RectTransformUtility.ScreenPointToLocalPointInRectangle(BGROOTRect, Input.mousePosition, Canvas.worldCamera, out pos);
    }
    public bool GetLocalPoint_Canv(out Vector2 pos)
    {
        return RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvasRect, Input.mousePosition, Canvas.worldCamera, out pos);
    }
    // Start is called before the first frame update
    void Start()
    {
        AudioMgr.Inst.isPlaying_Music = GameGloab.MusicOnOff == 0;
        AudioMgr.Inst.isPlaying_Sound = GameGloab.SoundIsOnOff == 0;
    }    
}