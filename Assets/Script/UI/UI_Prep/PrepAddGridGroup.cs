using System;
using UnityEngine;

public class PrepAddGridGroup : MonoBehaviour
{
    public Transform Root;
    public bool IsUse;
    bool canuse=true;
    public bool IsCanUse { get { return canuse; } set {
            if (canuse!=value)
            {
                Debug.Log("cant use   " + transform.name);
                minPrepGroup.SetCanUseStatus(value);//设置表现
            }
            canuse = value;
        } }
    [SerializeField]
    public GridGroup_MinPrep minPrepGroup { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        EventTriggerListener.Get(gameObject).onDown = OnPointerDown;
        EventTriggerListener.Get(gameObject).onUp = OnPointerUp;
        EventTriggerListener.Get(gameObject).onClick = OnClickGroup;
    }

    private void OnClickGroup(GameObject obj)
    {
        Debug.Log("OnClick   " + transform.name);
        //如果当前是 旋转的状态 
        //点击后 执行 旋转minPrepGroup数据  之后再执行能不能放置
        //IsGameOver();
    }

    void UsePrepGridGroup()
    {
        IsUse = true;
        Recycle();
        //三个格子都用完了，刷新三个待放入的格子
        if (GridGroupMgr.Inst.IsCantUseAllPrep())
        {
            GridGroupMgr.Inst.RefreshPrepGridGroup();
        }
        //就算三个用完重新刷新 也需要判断能不能放置
        IsGameOver();
    }
    void IsGameOver()
    {
        if (!GridGroupMgr.Inst.IsCanPrepNext())
        {
            Debug.LogError("游戏结束");
            UIManager.Inst.OpenGameOverPanel();
        }
    }
    public void SetGridData(GridGroup_MinPrep v)
    {
        minPrepGroup = v;
    }
    void SetChildActive(bool sw)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(sw);
        }
    }
    public void OnPointerUp(GameObject eventData)
    {
        Debug.Log("OnPointerUp   " + transform.name);
        if (IsUse|| !IsCanUse)//旋转的状态也不执行这里
        {
            return;
        }
        DragingGridMgr.Inst.SetDragUp(this);
        if (GridGroupMgr.Inst.RefreshMainGrid())//如果当前可以放置 刷新主面板显示
        {
            AudioManager.Inst.PlayPlace();
            UsePrepGridGroup();//设置当前待放入的group为使用过了
        }
        else
        {
            AudioManager.Inst.PlayReturn();
            SetChildActive(true);//使用失败 跑一个回到原始位置的动画
        }
    }
    public void OnPointerDown(GameObject eventData)
    {
        Debug.Log("OnPointerDown   " + transform.name);
        if (IsUse || !IsCanUse)//旋转的状态也不执行这里
        {
            return;
        }
        AudioManager.Inst.PlayPick();
        DragingGridMgr.Inst.SetDragDown(minPrepGroup);
        SetChildActive(false);
    }
    void Recycle()
    {
        if (minPrepGroup != null && !minPrepGroup.IsRecycled)
        {
            PoolMgr.Recycle(minPrepGroup);
            minPrepGroup = null;
        }
    }
    public void Reset()
    {
        Recycle();
        IsUse = false;
        canuse = true;
    }
}
