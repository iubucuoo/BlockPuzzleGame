using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMenu : MonoBehaviour
{
    Canvas canvas;
    RectTransform rectTr_canvas;
    RectTransform rectTr_bg;
    public Button btn_start;
    
    public GameObject homebg;

    public GameObject panelbg;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponent<Canvas>();
        rectTr_bg = GameGloab.root_bg.GetComponent<RectTransform>();
        rectTr_canvas = gameObject.GetComponent<RectTransform>();
        btn_start.onClick.AddListener(OnBtnStart);
    }
    public void OnBtnChinese()
    {
        LanguageManger.Inst.ChangeLanguage(LanguageList.Cn);
    }
    public void OnBtnEnglish()
    {
        LanguageManger.Inst.ChangeLanguage(LanguageList.En);
    }
    void OnBtnStart()
    {
        AudioManager.Inst.ButtonClick();
        Debug.Log("开始游戏");
        panelbg.SetActive(true);
        homebg.SetActive(false);
        btn_start.gameObject.SetActive(false);
        GridGroupMgr.Inst.GameStart();
        if (AudioManager.Inst.isPlaying_Music)
        {
            AudioManager.Inst.PlayBGMusic();
        }
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
            if (Time.frameCount % 10 == 0 && DragingGridMgr.Inst.IsDrag)//隔10针检测一次
            {
                PosCheck();
            }
        }else if (Input.GetMouseButtonUp(0))
        {
            OldDragPos = Vector2.zero;
            DragPos = GameGloab.OutScreenV2;
            //Debug.LogError("GetMouseButtonUp------    " + DragingGridMgr.Inst.IsDrag);
        }
#endif
        //手机端 检测touch
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                if (/*Time.frameCount % 5 == 0 && */DragingGridMgr.Inst.IsDrag)
                {
                    PosCheck();
                }
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                OldDragPos = Vector2.zero;//放置同一个位置点击的时候不处理位置改动
                DragPos = GameGloab.OutScreenV2;//防止残留的位置是上次的位置导致显示闪一下
            }
        }
    }
    void PosCheck()
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTr_canvas, Input.mousePosition, canvas.worldCamera, out Vector2 pos))
        {
            DragPos = pos + GameGloab.DragUp;//拖动位置用来显示
        }
        if ((oldmousepos - Input.mousePosition).sqrMagnitude > 90)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTr_bg, Input.mousePosition, canvas.worldCamera, out Vector2 pos1))
            {
                //Debug.Log("鼠标相对于bgroot的ui位置" + pos1 + (oldmousepos - Input.mousePosition).sqrMagnitude);
                GridGroupMgr.Inst.CheckAvailable(pos1 + GameGloab.DragUp);//位置检测 用来判断能否放置
            }
            oldmousepos = Input.mousePosition;
        }
    }
    Vector2 DragPos;
    Vector2 OldDragPos;
    void FixedUpdate()
    {
        if (DragingGridMgr.Inst.IsDrag)
        {
            if (DragPos != OldDragPos)
            {
                DragingGridMgr.Inst.DragRoot.localPosition = DragPos;
                OldDragPos = DragPos;
            }
        }
    }
}
