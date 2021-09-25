using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GridDataDef : GridData
{
    public override IPoolsType GroupType => IPoolsType.GridDataDef;

   

    public override void OnRecycled()
    {
        base.OnRecycled();
    }

}