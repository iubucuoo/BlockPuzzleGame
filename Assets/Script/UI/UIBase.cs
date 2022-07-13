using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WUtils;

public abstract class UIBase: IPoolable
{
    Canvas canvas;
    public abstract IPoolsType PoolType { get; }
    public bool IsRecycled { get; set; }
    public virtual void Dispose() { }
    public virtual void OnRecycled()
    {
         
    }
    public abstract string WndName{ get; }
    public virtual UIHideType hideType { get { return UIHideType.WaitDestroy; } }
    public virtual UIHideFunc hideFunc { get { return UIHideFunc.MoveOutOfScreen; } }
    public virtual int layer { get => (int)UILayer.Panel; set => layer = value; }
    public virtual bool isFull => false;
    public virtual int orderInLayer { get; set; }
    public bool visible = false;
    bool loaded = false;
    //bool bBaseUI = true;
    public GameObject WndRoot { get; private set; }
    RectTransform _wndrect;
    RectTransform Wndrect { get {
            if (_wndrect == null)
            {
                _wndrect = WndRoot.GetComponent<RectTransform>();
            }
            return _wndrect;
        } }

    Transform parent;
    private float rootSavedX = -100;
    private float rootSavedY = -100;
    void SetUIVisible(bool _visible)
    {
        if (hideFunc == UIHideFunc.MoveOutOfScreen)
        {
            if (visible)
            {
                if (rootSavedX!= -100 && rootSavedY!= -100)
                {
                    Wndrect.anchoredPosition = new Vector2(rootSavedX, rootSavedY);
                }
            }
            else
            {
                if (rootSavedX == -100 && rootSavedY == -100)
                {
                    rootSavedX = Wndrect.anchoredPosition.x + (ismove ? _x : 0);
                    rootSavedY = Wndrect.anchoredPosition.y;
                }
                Wndrect.anchoredPosition = new Vector2(100000000, 100000000);
            }
        }
        else
        {
            WndRoot.SetActive(_visible);
        }
    }

    readonly float _y = 2020;
    readonly float _x = 1480;
    readonly float swtime = .3f;
    readonly float hidetime = .2f;
    public void ShowBoxX(TweenCallback Finish = null)
    {
        Vector2 pos = Wndrect.anchoredPosition;
        Vector2 firstpos = Wndrect.anchoredPosition;
        firstpos.x += _x;
        WndRoot.transform.localPosition = firstpos;
        if (Finish != null)
        {
            WndRoot.transform.DOLocalMoveX(pos.x, swtime).SetEase(Ease.OutBack).OnComplete(Finish);
        }
        else
        {
            WndRoot.transform.DOLocalMoveX(pos.x, swtime).SetEase(Ease.OutBack);
        }
    }
    public void HideBox(TweenCallback HideFinish= null)
    {
        var endx = Wndrect.anchoredPosition.x - _x;
        WndRoot.transform.DOLocalMoveX( endx, hidetime).SetEase(Ease.InBack).OnComplete(HideFinish);
    }
    bool ismove = false;
    void SetVisible(bool _visible, bool Move = false)
    {
        if (visible == _visible)
            return;
        ismove = Move;
        visible = _visible;
        DebugMgr.Log(_visible ? "show" : "hide" + WndName);
        if (_visible)
        {
            if (_Time != null)
            {
                _Time.Destroy();
                _Time = null;//如果有删除倒计时 停止倒计时
            }
            if (loaded)
            {
                //DebugMgr.LogError(WndName + "  SetUIVisible   ");
                SetUIVisible(true);
                DoShow();
            }
            else
            {
                DoLoad();
            }
            //判断是否load  已经加载则显示  未加载则加载
        }
        else
        {
            if (loaded)
                DoHide();//判断是否load  已经加载则隐藏  未加载则不处理
        }
    }
    public virtual void OnDestroy_m()
    {

    }
    public virtual void OnCreate()
    {

    }
    public virtual void OnHide()
    {

    }
    public virtual void OnShow()
    {

    }
    public virtual void UnRegistEvents()
    {

    }
    public void Hide(bool Move = false)
    {
        SetVisible(false, Move);
    }
    public void Show(bool Move = false)
    {
        SetVisible(true, Move);
    }
    void DoShow()
    {
        CheckParent();
        OnShow();
        if (ismove)
            ShowBoxX();
    }

    void DoHide()
    { 
        OnHide();
        if (ismove)
            HideBox(HideType);
        else
            HideType();
    }
    void HideType()
    {
        if (hideType == UIHideType.Destroy)
            Destroy();
        else if (hideType == UIHideType.WaitDestroy)
        {
            SetUIVisible(false);
            _Time = TimeMgr.Instance.AddIntervelEvent((z, x) => {
                WaitDestroy();
            }, UIStatic.WAIT_DESTROY_TIME, 0, 1);
        }
        else
        {
            SetUIVisible(false);
        }

    }
    TimeEvent _Time;
    void WaitDestroy()
    {
        Destroy();
    }
    void DoLoad()
    {
        var go = ObjectMgr.InsResource("Panel/" + WndName);
        if (go==null)
            return;
        WndRoot = go;
        CheckParent();
        canvas = WndRoot.AddMissingComponent<Canvas>();
        canvas.overrideSorting = true;
        WndRoot.AddMissingComponent<GraphicRaycaster>();
        RefreshLayer();
        SetUIVisible(visible);
        loaded = true;
        OnCreate();
        //创建UI
        if (visible)
        {
            DoShow();
        }
    }
    void Destroy()
    {
        Hide();
        UnRegistEvents();
        OnDestroy_m();
        Object.Destroy(WndRoot);
        WndRoot = null;
        loaded = false;
        AllUIPanelManager.Inst.ReleaseUI(this);
    }
    void CheckParent()
    {
        if (parent == null)
        {
            parent = UIStatic.UIRoot_Canvas;
        }
        WndRoot.transform.SetParent(parent, false);
    }
    void SetLayer(int _layer)
    {
        if (_layer!=layer)
        {
            layer = _layer;
            RefreshLayer();
        }
    }
    void RefreshLayer()
    {
        canvas.sortingOrder = layer * 100 + orderInLayer;
    }
    void SetOrderInLayer(int _orderInLayer)
    {
        if (orderInLayer != _orderInLayer )
        {
            orderInLayer = _orderInLayer;
            RefreshLayer();
        }
    }
}
