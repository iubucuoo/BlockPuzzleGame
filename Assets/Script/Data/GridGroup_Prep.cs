﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGroup_Prep : GridGroup
{
    public IPoolsType PoolsType = IPoolsType.GridGroup_Prep;
    public GridGroup_Prep()
    {
        G_width = GameGloab._width;
        G_height = GameGloab._height;
        ResName = "Prefab/blockdrag";//拖动出来的格子
    }

    public override IPoolsType GroupType { get { return IPoolsType.GridGroup_Prep; } }
    public override IPoolsType GridType =>  IPoolsType.GridDataPrep;
}
