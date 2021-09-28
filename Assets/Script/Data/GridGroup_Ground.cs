using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGroup_Ground : GridGroup,IPoolable
{
    public GridGroup_Ground()
    {
        G_width = GameGloab.wh;
        G_height = GameGloab.wh;
        ResName = "Prefab/blockdef";//默认的背景格子
        DataArray = new int[,]{
            { 0, 0, 0, 0, 0 , 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0 , 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0 , 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0 , 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0 , 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0 , 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0 , 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0 , 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0 , 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0 , 0, 0, 0, 0, 0 }
        };
        SetData(DataArray, UIManager.Inst.BGROOT);
    }

    public override IPoolsType GroupType { get { return IPoolsType.GridGroup_Ground; } }
    public override IPoolsType GridType =>  IPoolsType.GridDataDef;
}
