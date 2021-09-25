using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridDataPrep : GridData
{
    public override IPoolsType GroupType => IPoolsType.GridDataPrep;

    public override void OnRecycled()
    {
        base.OnRecycled();
    }
}
