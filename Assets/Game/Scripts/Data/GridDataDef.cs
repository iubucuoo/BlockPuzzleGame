using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GridDataDef : GridData
{
    public override IPoolsType PoolType => IPoolsType.GridDataDef;

   

    public override void OnRecycled()
    {
        base.OnRecycled();
    }

}