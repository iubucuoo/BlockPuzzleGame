using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GamePanel_GroupRotate
{
    public Toggle RotateBnt;
    public Text GoldNum;
    public Button BtnAddGlog;
    List<GameObject> RotateImgs = new List<GameObject>();
    TimeEvent TE;
    UI_GamePanelJob paneljob;
    public UI_GamePanel_GroupRotate(Transform wnd, UI_GamePanelJob job)
    {
        paneljob = job;
        var BtnAddRotate = wnd.Find("BtnAddRotate");
        BtnAddGlog = BtnAddRotate.GetComponent<Button>();
        GoldNum = BtnAddRotate.Find("Text").GetComponent<Text>();
        RotateBnt = wnd.Find("rotateBtn").GetComponent<Toggle>();


        MainC.Inst.RotateGoldAddPos = GoldNum.transform.position;
        AddRotateImg();
        //GameGloab.GoldCount=0;
        AddRotateGoldCount();
        ChangeRotate(false);
        RotateBnt.onValueChanged.AddListener(ChangeRotate);
        BtnAddGlog.onClick.AddListener(OpenAddRotatePanel);
    }
 
    private void OpenAddRotatePanel()
    {
        AudioMgr.Inst.ButtonClick();
        AllUIPanelManager.Inst.Show(IPoolsType.UI_AddRotatePanel);
    }

    public void AddRotateGoldCount(int v = 0)
    {
        GameGloab.GoldCount += v;
        GoldNum.text = GameGloab.GoldCount.ToString();
        if (GameGloab.GoldCount <= 0)
        {
            OffChangeRotate();
        }
    }
    public void OffChangeRotate()
    {
        RotateBnt.isOn = false;
    }
    private void ChangeRotate(bool arg0)
    {
        if (arg0)
        {
            if (GameGloab.GoldCount <= 0)
            {
                DebugMgr.LogError("金币不足不能开启");
                //RotateBnt.SetIsOnWithoutNotify(false);

                RotateBnt.isOn = false;
                OpenAddRotatePanel();
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
        DebugMgr.Log(RotateBnt.isOn);
    }

    /// <summary>
    /// 是否可旋转的状态开关
    /// </summary>
    void SwitchRotateState(bool v)
    {
        //还原待用的组的旋转
        GridGroupMgr.Inst.BackRotate();
        MainC.Inst.IsRotateState = v;
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
    public void SWRotate(int i, bool v)
    {
        if (RotateImgs[i] != null)
        {
            if (v)
            {
                RotateImgs[i].transform.eulerAngles = rotatenum; //开始旋转
            }
            RotateImgs[i].SetActive(v);
        }
    }
    void AddRotateImg()
    {
        for (int i = 0; i < 3; i++)
        {
            Vector2 pos = new Vector2((i - 1) * (6 * MainC.wh_2), 0);
            var obj = ObjectMgr.InsResource("Prefab/addrotateimg");
            obj.transform.SetParent(paneljob.ROTATEROOT);
            obj.transform.localPosition = pos;
            obj.transform.localScale = Vector2.one;
            RotateImgs.Add(obj);
        }
        SWRotates(false);
        TE = TimeMgr.Instance.AddIntervelEvent(TimeCheck, 60, 0, -1);
    }
    Vector3 rotatenum;
    private void TimeCheck(int arg1, float arg2)
    {
        if (!RotateBnt.isOn)
        {
            return;
        }
        rotatenum.z--;
        if (rotatenum.z <= -180)
        {
            rotatenum.z = 0;
        }
        for (int i = 0; i < RotateImgs.Count; i++)
        {
            var obj = RotateImgs[i];
            if (obj && obj.activeInHierarchy)
            {
                obj.transform.eulerAngles = rotatenum;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
