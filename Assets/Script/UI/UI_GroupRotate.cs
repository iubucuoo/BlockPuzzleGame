using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GroupRotate : MonoBehaviour
{
    public Toggle RotateBnt;
    public bool IsRotateState { get; private set; }
    List<GameObject> RotateImgs = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        //GameGloab.GoldCount = 200;
        RotateBnt.onValueChanged.AddListener(ChangeRotate);
        AddRotateImg();
    }

    private void ChangeRotate(bool arg0)
    {
        if (arg0)
        {
            if (GameGloab.GoldCount<=0)
            {
                Debug.LogError("金币不足不能开启");
                RotateBnt.isOn = false;
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
            RotateImgs[i].SetActive(v);
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
