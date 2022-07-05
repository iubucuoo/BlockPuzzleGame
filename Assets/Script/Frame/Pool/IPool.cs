using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum IPoolsType
{
    GridGroup,
    GridGroup_Ground,
    GridGroup_MinPrep,
    GridGroup_Prep,
    GridData,
    GridDataMin,
    GridDataDef,
    GridDataPrep,

    UI_StartPanel,
    UI_AddRotatePanel,
    UI_GameOverPanel,
    UI_GamePanel,
    UI_SetPanel,


    Message,
    MessageBase,
}
public interface IPoolable
{
    IPoolsType GroupType { get; }
    void OnRecycled();//重置
    bool IsRecycled { get; set; }

}
public interface IPool
{
    IPoolable Allocate(IPoolsType _type);//分配
    bool Recycle(IPoolable obj);//回收
}
public interface IObjectFactory<T>
{
    T Create(IPoolsType _type);
}
