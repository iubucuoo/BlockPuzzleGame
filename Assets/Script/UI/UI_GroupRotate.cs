using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GroupRotate : MonoBehaviour
{
    public Toggle RotateBnt;
    public Text GoldNum;
    public bool IsRotateState { get; private set; }
    List<GameObject> RotateImgs = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        AddRotateImg();
        GameGloab.GoldCount=0;
        AddRotateGoldCount(0);
        ChangeRotate(false);
        RotateBnt.onValueChanged.AddListener(ChangeRotate);
    }

    public void AddRotateGoldCount(int v=0)
    {
        GameGloab.GoldCount += v;
        GoldNum.text = GameGloab.GoldCount.ToString();
    }
    public void OffChangeRotate()
    {
        RotateBnt.isOn = false;
    }
    private void ChangeRotate(bool arg0)
    {
        if (arg0)
        {
            if (GameGloab.GoldCount<=0)
            {
                Debug.LogError("金币不足不能开启");
                //RotateBnt.SetIsOnWithoutNotify(false);
                
                RotateBnt.isOn = false;
                UIMgr.Inst.OnBtnAddRotateSW();
            }
            else
            {
                SwitchRotateState(true);
            }
        }
        else
        {
            SwitchRotateState(false);
        }
        Debug.Log(RotateBnt.isOn);
    }

    /// <summary>
    /// 是否可旋转的状态开关
    /// </summary>
    public void SwitchRotateState(bool v)
    {
        //还原待用的组的旋转
        GridGroupMgr.Inst.BackRotate();
        IsRotateState = v;
        SWRotates(v);
       
    }
    void SWRotates(bool v)
    {
        for (int i = 0; i < 3; i++)
        {
            if (v)
            {
                SWRotate(i, GridGroupMgr.Inst.IsCantUsePrep(i));
            }
            else
            {
                SWRotate(i, v);
            }
        }
    }
    public void SWRotate(int i,bool v)
    {
        if (RotateImgs[i]!=null)
        {
            RotateImgs[i].SetActive(v);
            if (v)
            {
                //开始旋转
            }
        }
    }
    void AddRotateImg()
    {
        for (int i = 0; i < 3; i++)
        {
            Vector2 pos = new Vector2((i - 1) * (6 * GameGloab.wh_2), 0);
            var obj = ObjectMgr.InstantiateGameObj(ObjectMgr.LoadResource("Prefab/addrotateimg") as GameObject);
            obj.transform.SetParent(transform);
            obj.transform.localPosition = pos;
            obj.transform.localScale = Vector2.one;
            RotateImgs.Add( obj);
        }
        SWRotates(false);
    }
    
}
