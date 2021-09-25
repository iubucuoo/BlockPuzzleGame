using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridGroup : IPoolable
{
    public virtual IPoolsType GroupType { get { return IPoolsType.GridGroup; } }
    public virtual IPoolsType GridType { get { return IPoolsType.GridData; } }
    public bool IsRecycled { get; set; }
    protected int G_width;
    protected int G_height;
    public string ResName { get; protected set; }
    public int W_count { get; private set; }
    public int H_count { get; private set; }
    public GridData[,] Grid{ get; protected set; }
    public int[,] DataArray { get; protected set; }
    Transform Root;
    Vector3 Pos = new Vector3(0, 0);

    public void SetData(int[,] _data, Transform _root)
    {
        Root = _root;
        DataArray = _data;
    }
    public void CreatGrids()
    {
        W_count = DataArray.GetLength(1);
        H_count = DataArray.GetLength(0);
        Grid = new GridData[H_count, W_count];
        int h_1 = H_count - 1;
        for (int i = 0; i < H_count; i++)
        {
            for (int j = 0; j < W_count; j++)
            {
                if (DataArray[i, j] >0 || GroupType == IPoolsType.GridGroup_Ground)
                {
                    if (Grid[i, j] == null)
                    {
                        Grid[i, j] = PoolMgr.Allocate(GridType) as GridData;
                    }
                    Grid[i, j].IsUse = DataArray[i, j] >0 ;
                    Grid[i, j].TrueStatus = DataArray[i, j];
                    Pos.x = (j - W_count * 0.5f + 0.5f) * G_width;
                    Pos.y = (h_1 - i - H_count * 0.5f + 0.5f) * G_height;
                    Grid[i, j].CreatObj(Root, Pos, ResName);
#if UNITY_EDITOR
                    //if (Grid[i, j].IsUse)
                    //    Grid[i, j].Text.text = i + ":" + j;
                    //else
                    //    Grid[i, j].Text.text = "";
#endif
                }
            }
        }
    }
    public void SetCanUseStatus(bool st)
    {
        foreach (var v in Grid)
        {
            if (v!=null && !v.IsRecycled)
            {
                v.swPrepGray(st);
            }
        }
    }
    public void OnRecycled()
    {
        foreach (var v in Grid)
        {
            if (v!=null)
            {
                PoolMgr.Recycle(v);
            }
        }
    }
}
