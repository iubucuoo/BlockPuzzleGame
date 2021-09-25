using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGroup_MinPrep : GridGroup,IPoolable
{
    public GridGroup_MinPrep()
    {
        G_width = GameGloab.wh/2;
        G_height = GameGloab.wh/2;
        ResName = "Prefab/blockmin";//min的格子
    }

    public override IPoolsType GroupType { get { return IPoolsType.GridGroup_MinPrep; } }
    public override IPoolsType GridType =>  IPoolsType.GridDataMin;
}
