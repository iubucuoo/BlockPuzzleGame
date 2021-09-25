using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GridData : IPoolable
{
    public virtual IPoolsType GroupType { get { return IPoolsType.GridData; } }
    public bool IsRecycled { get; set; }
    public bool IsUse;//判断是否存放了格子
    public Transform Parent;
    public Text Text { get; private set; }
    GameObject GridObj;
    Image DefImage;
    Image DesImage;//将要删除的展示图;
    Image PrepImage;//将要放置的展示图;
    string resName;

    int _turestaus;
    public int TrueStatus { get { return _turestaus; } set { _turestaus = value; _TempStatus = _turestaus; } }//配置里的值，主面板的可修改
    public int _TempStatus;//临时存放的truestatus，如可以放置或着可销毁的时候临时用的

    public Vector3 Position { get { return GridObj ? GridObj.transform.position : (Vector3)GameGloab.OutScreenV2; } }

    void SetSprites()
    {
        if (TrueStatus == 0)
        { DefImage.sprite = GameGloab.Sprites["Dark_BG_BS"]; }
        else if (TrueStatus == 1)
        { DefImage.sprite = GameGloab.Sprites["Block_Wood"]; }
        else if (TrueStatus == 2)
        { DefImage.sprite = GameGloab.Sprites["swgrid"]; }
        else if (TrueStatus == 3)
        { DefImage.sprite = GameGloab.Sprites["BlueBubble"]; }
        else if (TrueStatus == 4)
        { DefImage.sprite = GameGloab.Sprites["ColorBubble"]; }
    }


    public void Revert()
    {
        TrueStatus = _TempStatus;
        if (GroupType == IPoolsType.GridDataDef)
        {
            DesImage.gameObject.SetActive(false);
            PrepImage.gameObject.SetActive(false);
        }
        SetSprites();
    }
    /// <summary>
    /// 将要删除的展示
    /// </summary>
    public void swClear()
    {
        if (GroupType == IPoolsType.GridDataDef)
        {
            DesImage.gameObject.SetActive(true);
            PrepImage.gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// 将要删除的展示还原
    /// </summary>
    public void swClearRevert()
    {
        if (GroupType == IPoolsType.GridDataDef)
            DesImage.gameObject.SetActive(false);
    }

    /// <summary>
    /// 将要放入的展示
    /// </summary>
    public void swPrep()
    {
        if (GroupType == IPoolsType.GridDataDef)
            PrepImage.gameObject.SetActive(true);
    }
    /// <summary>
    /// 还原将要放入的展示
    /// </summary>
    public void swPrepRevert()
    {
        if (GroupType == IPoolsType.GridDataDef)
            PrepImage.gameObject.SetActive(false);
        _TempStatus = TrueStatus;
    }
    /// <summary>
    /// 设置待放入的minprep 能不能放的状态 false时不能使用显示外灰色
    /// </summary>
    public void swPrepGray(bool can)
    {
        DefImage.color = can?Color.white:Color.gray;
    }

    public void CreatObj(Transform _parent, Vector2 _Pos, string res)
    {
        resName = res;
        if (GridObj == null)
        {
            GridObj = ObjectMgr.InstantiateGameObj(ResourceMgr.Inst.LoadRes<GameObject>(res));
            DefImage = GridObj.transform.Find("def").GetComponent<Image>();
            if (GroupType == IPoolsType.GridDataDef)
            {
                DesImage = GridObj.transform.Find("des").GetComponent<Image>();
                PrepImage = GridObj.transform.Find("prep").GetComponent<Image>();
            }
            if (GroupType == IPoolsType.GridDataPrep)
            {
                DefImage.rectTransform.sizeDelta *= 0.8f;
            }
#if UNITY_EDITOR
            GridObj.name = resName;
            //Text = gridobj.transform.Find("Text").GetComponent<Text>();
#endif
        }
        GridObj.transform.SetParent(_parent);
        GridObj.transform.localPosition = _Pos;
        GridObj.SetActive(true);
        Revert();
    }
    public virtual void OnRecycled()
    {
        IsUse = false;
        if (GridObj)
        {
            GridObj.transform.SetParent(null);
            DefImage.color = Color.white;
            GridObj.SetActive(false);
        }
        TrueStatus = 0;
        resName = "";
    }
}
