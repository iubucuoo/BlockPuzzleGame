using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGroup_Ground : GridGroup
{
    public GridGroup_Ground()
    {
        G_width = GameStatic._width;
        G_height = GameStatic._height;
        ResName = "Prefab/blockdef";//默认的背景格子
    }

    public override IPoolsType PoolType { get { return IPoolsType.GridGroup_Ground; } }
    public override IPoolsType GridType =>  IPoolsType.GridDataDef;
}
