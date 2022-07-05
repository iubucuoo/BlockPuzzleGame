using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIBase
{
    Canvas canvas;
    public abstract string WndName{ get; }
    public abstract UIHideType hideType { get;}
    public abstract UIHideFunc hideFunc { get;}
    public abstract int layer { get; set; }
    public abstract bool isFull { get; }
    int orderInLayer = 0;
    public bool visible = false;
    bool loaded = false;
    //bool bBaseUI = true;
    public GameObject WndRoot { get; private set; }

    Transform parent;
    private float rootSavedX;
    private float rootSavedY;
    void SetUIVisible(bool _visible)
    {
        if (hideFunc == UIHideFunc.MoveOutOfScreen)
        {
            if (visible)
            {
                if (rootSavedX!=0 && rootSavedY!=0)
                {
                    WndRoot.GetComponent<RectTransform>().anchoredPosition = new Vector2(rootSavedX, rootSavedY);
                }
            }
            else
            {
                var root_rect = WndRoot.GetComponent<RectTransform>();
                rootSavedX = root_rect.anchoredPosition.x;
                rootSavedY = root_rect.anchoredPosition.y;
                root_rect.anchoredPosition = new Vector2(100000000, 100000000);
            }
        }
        else
        {
            WndRoot.SetActive(_visible);
        }
    }
   
    void SetVisible(bool _visible)
    {
        if (visible == _visible)
            return;
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
    public void Hide()
    {
        SetVisible(false);
    }
    public void Show()
    {
        SetVisible(true);
    }
    void DoShow()
    {
        CheckParent();
        OnShow();
    }

    void DoHide()
    { 
        OnHide();
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
        canvas = AddMissingCom<Canvas>();
        canvas.overrideSorting = true;
        AddMissingCom<GraphicRaycaster>();
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
        SetVisible(false);
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
            parent = MainC.Inst.UIRoot;
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
    public T AddMissingCom<T>() where T : Component
    {
        T _canvas = WndRoot.GetComponent<T>();
        if (_canvas == null)
        {
            _canvas = WndRoot.AddComponent<T>();
        }
        return _canvas;
    }
}
