﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainC : MonoBehaviour
{
    public Button btn_start;

    public GameObject homebg;

    public GameObject panelbg;
    private void Awake()
    {
        Application.targetFrameRate = 60;
        gameObject.AddComponent<MEC.Timing>();
        gameObject.AddComponent<TimeMgr>();
        gameObject.AddComponent<GridGroupMgr>();
        gameObject.AddComponent<UIMgr>();
        GoogleAdMgr.CheckInstance();
        FPS.CheckInstance();
    }
    public List<Sprite> sprites = new List<Sprite>();
    

    bool errorOver = true;
    void ForegroundErrorOver(string title)
    {
        //需要在主线程中处理数据
        if (!errorOver)
            return;
        TimeMgr.Instance.AddIntervelEvent((int i, float f) =>
        {
            if (errorOver)
            {
                errorOver = !errorOver;
                string sw = title == null ? "网络不给力哦！ 点击重试。" : title;
                //弹出框 提示网络不行重新链接
                //TipWnd.inst.SwTipCom(title == null ? "网络不给力哦！ 点击重试。" : title, "提示", callback: () =>
                //{
                //    SaveFile();
                //    StaticTools.ReStartGame();
                //    errorOver = true;
                //}, btn_name: "重试", isMaskBtn: false);
            }
        }, 16, 0, 1);
    }
    DownloadThread thread;
    void Start()
    {

        MEC.Timing.RunCoroutine(NetStatus());
        thread = new DownloadThread();
        thread.ForegroundErrorOver = ForegroundErrorOver;

        //先载入数据文件
        //LoadLanguageData();
        //



        foreach (var v in sprites)
        {
            UIMgr.Inst.Sprites[v.name] = v;
        }
        homebg.SetActive(true);
        btn_start.onClick.AddListener(OnBtnStart);
    }
    IEnumerator<float> NetStatus()
    {
        while (true)
        {
            StaticTools._NetStatus = StaticTools.GetNetState();
            //通知 lua侧 网络状态
            //var objs = new object[] { GlobalData._NetStatus };
            //FreeSendEvent.GSendMsg((ushort)LMainUIListenID.NetStatus, null, objs);
            yield return MEC.Timing.WaitForSeconds(1);
        }
    }
    void LoadLanguageData()
    {
        var ab = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/language.ly");
        StartCoroutine(Loading(ab));
    }
    IEnumerator<float> Loading(AssetBundle ab)
    {
        var objs = ab.LoadAllAssets();
        UseArt(objs);
        yield return 0;
        ab.Unload(false);
    }
    void UseArt(object[] objs)
    {
        foreach (var str in objs)
        {
            var languagedata = JsonUtility.FromJson<LanguageData>((str as TextAsset).text);
            foreach (var v in languagedata.datas)
            {
                if (!LanguageManger.Inst.languagedic.ContainsKey(v.LanguageList))
                {
                    Dictionary<string, string> ky = new Dictionary<string, string>();
                    foreach (var vv in v.ky)
                    {
                        ky.Add(vv.key, vv.value);
                    }
                    LanguageManger.Inst.languagedic.Add(v.LanguageList, ky);
                }
            }
        }
    }
  

    void OnBtnStart()
    {
        AudioMgr.Inst.ButtonClick();
        DebugMgr.Log("开始游戏");
        panelbg.SetActive(true);
        homebg.SetActive(false);
        btn_start.gameObject.SetActive(false);
        AudioMgr.Inst.PlayBGMusic();
        GridGroupMgr.Inst.GameStart();
    }

    Vector3 oldmousepos;
    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bgrectTransform, Input.mousePosition, canvas.worldCamera, out Vector2 pos1))
        //    {
        //        int posx = GridGroupMgr.OutGridPos(pos1.x);
        //        int posy = GridGroupMgr.OutGridPos(pos1.y);
        //        if (GridGroupMgr.Inst.Postox.ContainsKey(posx) && GridGroupMgr.Inst.Postoy.ContainsKey(posy))
        //        {
        //            Debug.Log("鼠标相对于bgroot的ui位置" + pos1 + "     " +  posy + "   " + posx + "     " + GridGroupMgr.Inst.Postoy[posy] + "   " + GridGroupMgr.Inst.Postox[posx]);
        //        }
        //    }
        //}
#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            if (DragingGridMgr.Inst.IsDrag)
            {
                if (Time.frameCount % 10 == 0)//隔10针检测一次
                {
                    PosCheck();
                }
                PosSet();
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            OldDragPos = Vector2.zero;
            DragPos = GameGloab.OutScreenV2;
            SetDragRootPos();
            //Debug.LogError("GetMouseButtonUp------    " + DragingGridMgr.Inst.IsDrag);
        }
#else

        //手机端 检测touch
        if (Input.touchCount > 0)
        {
            if ( Input.GetTouch(0).phase == TouchPhase.Moved ||  Input.GetTouch(0).phase == TouchPhase.Began)
            {
                if (DragingGridMgr.Inst.IsDrag)
                {
                    if (Time.frameCount % 5 == 0)
                    { PosCheck(); }
                    PosSet();
                }
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                OldDragPos = Vector2.zero;//放置同一个位置点击的时候不处理位置改动
                DragPos = GameGloab.OutScreenV2;//防止残留的位置是上次的位置导致显示闪一下
                SetDragRootPos();
            }
        }
#endif
    }

    //设置拖动位置 不限帧
    void PosSet()
    {
        if (UIMgr.Inst.GetLocalPoint_Canv(out Vector2 pos))
        {
            DragPos = pos + UIMgr.DragUp;//拖动位置用来显示
            SetDragRootPos();
        }
    }
    void PosCheck()
    {
        if ((oldmousepos - Input.mousePosition).sqrMagnitude > 90)
        {
            if (UIMgr.Inst.GetLocalPoint_BgRoot(out Vector2 pos1))
            {
                //Debug.Log("鼠标相对于bgroot的ui位置" + pos1 + (oldmousepos - Input.mousePosition).sqrMagnitude);
                GridGroupMgr.Inst.CheckAvailable(pos1 + UIMgr.DragUp);//位置检测 用来判断能否放置
            }
            oldmousepos = Input.mousePosition;
        }
    }
    void SetDragRootPos()
    {
        DragingGridMgr.Inst.DragRoot.localPosition = DragPos;
    }
    Vector2 DragPos;
    Vector2 OldDragPos;
    void FixedUpdate2()
    {
        if (DragingGridMgr.Inst.IsDrag)
        {
            //if (DragPos != OldDragPos)
            //{
            DragingGridMgr.Inst.DragRoot.localPosition = DragPos;
            //OldDragPos = DragPos;
            //}
        }
    }
}