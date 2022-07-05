using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGroup_Ground : GridGroup,IPoolable
{
    public GridGroup_Ground()
    {
        G_width = MainC._width;
        G_height = MainC._height;
        ResName = "Prefab/blockdef";//默认的背景格子
    }

    public override IPoolsType GroupType { get { return IPoolsType.GridGroup_Ground; } }
    public override IPoolsType GridType =>  IPoolsType.GridDataDef;
}
