using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DragingGridMgr
{
    static DragingGridMgr _Instance;
    public static DragingGridMgr Inst
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new DragingGridMgr();
            }
            return _Instance;
        }
    }

    public bool IsDrag { get; private set; }
    public Transform DragRoot { get; private set; }
    public GridGroup_Prep prepData { get; private set; }

    Transform ChildParent;
    UI_GamePanelJob gamepaneljob;
    public void SetInit(Transform dr, UI_GamePanelJob ui)
    {
        DragRoot = dr;
        gamepaneljob = ui;
        ChildParent = DragRoot.GetChild(0);
    }
    public void AddDragGroup(GridGroup_Prep v)
    {
        v.CreatGrids();
    }
   Vector2 GetToPos()
    {
        if (gamepaneljob.GetLocalPoint_Canv(out Vector2 pos))
        {
            return pos + GameStatic.DragUp;//拖动位置用来显示
        }
        return GameGloab.OutScreenV2;
    }
    Vector2 topos;
    public void SetDragDown(PrepAddGridGroup v)
    {
        prepData = PoolMgr.Allocate(IPoolsType.GridGroup_Prep)as GridGroup_Prep;
        prepData.SetData(v.rotatePrep, ChildParent);
        AddDragGroup(prepData);
        //生成组 跑一个动画  然后跟随手拖动位置
        DragRoot.position = v.Root.position;
        topos = GetToPos();// + GameGloab.DragUp;
        DragRoot.DOLocalMove(topos, .15f);

    }
    public void SetDrag(bool v)
    {
        if (v)
        {
            DragRoot.DOKill();
        }
        IsDrag = v;
    }
    public void SetDragUp(PrepAddGridGroup v)
    {
        //放手
        DragRoot.localPosition = GameGloab.OutScreenV2;
        //ChildParent.localPosition = Vector2.zero;
        PoolMgr.Recycle(prepData);
        IsDrag = false;
    }
}
