using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public Transform DragRoot { get; set; }

    public GridGroup_Prep prepData;

    public void AddDragGroup(GridGroup_Prep v)
    {
        v.CreatGrids();
    }

    public void SetDragDown(GridGroup v)
    {
        prepData = PoolMgr.Allocate(IPoolsType.GridGroup_Prep)as GridGroup_Prep;
        prepData.SetData(v.DataArray, DragRoot);
        AddDragGroup(prepData);
        //生成组 跑一个动画  然后跟随手拖动位置
        IsDrag = true;
    }

    public void SetDragUp(PrepAddGridGroup v)
    {
        //放手
        DragRoot.localPosition = GameGloab.OutScreenV2;
        PoolMgr.Recycle(prepData);
        IsDrag = false;
    }
}
