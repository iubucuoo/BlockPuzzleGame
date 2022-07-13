using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GridDataMin : GridData
{
    public override IPoolsType PoolType => IPoolsType.GridDataMin;

    public override void OnRecycled()
    {
        base.OnRecycled();
    }
}