using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridGroupMgr : MonoBehaviour
{
    public Dictionary<int, int> Postox { get; } = new Dictionary<int, int>()
    {
        [GameGloab.wh_2 * -9] = 0,
        [GameGloab.wh_2 * -7] = 1,
        [GameGloab.wh_2 * -5] = 2,
        [GameGloab.wh_2 * -3] = 3,
        [GameGloab.wh_2 * -1] = 4,
        [GameGloab.wh_2 * 1] = 5,
        [GameGloab.wh_2 * 3] = 6,
        [GameGloab.wh_2 * 5] = 7,
        [GameGloab.wh_2 * 7] = 8,
        [GameGloab.wh_2 * 9] = 9,
    };
    //int Pos_tox(int w)
    //{
    //    if (w < -270 || w > 270)
    //    {
    //        return -1;
    //    }
    //    int chu = 270 + w;
    //    if (chu <= 0)
    //    {
    //        return 0;
    //    }
    //    return chu / 60;
    //}
    public Dictionary<int, int> Postoy { get; } = new Dictionary<int, int>()
    {
        [GameGloab.wh_2 * -9] = 9,
        [GameGloab.wh_2 * -7] = 8,
        [GameGloab.wh_2 * -5] = 7,
        [GameGloab.wh_2 * -3] = 6,
        [GameGloab.wh_2 * -1] = 5,
        [GameGloab.wh_2 * 1] = 4,
        [GameGloab.wh_2 * 3] = 3,
        [GameGloab.wh_2 * 5] = 2,
        [GameGloab.wh_2 * 7] = 1,
        [GameGloab.wh_2 * 9] = 0,
    };
    //int Pos_toy(int h)
    //{
    //    if (h<-270 || h>270)
    //    {
    //        return -1;
    //    }
    //    int chu = 270 - h;
    //    if (chu<=0)
    //    {
    //        return 0;
    //    }
    //    return chu / 60;
    //}
    List<GridData> swPrepGridList = new List<GridData>();//临时展示在面包上的将要放入的格子
    List<GridData> swClearGridList = new List<GridData>();//临时展示在面包上的将要删除的格子
    public GridGroup_Ground gridGroup_Ground;//主面板数据
    public static GridGroupMgr Inst;
    private void Awake()
    {
        Inst = this;
    }
    List<int[,]> datalist;
    private void Start()
    {
        datalist = new List<int[,]>
        {
            new int[,]{
                { 1 },
            },
            new int[,]{
                { 1,1},
            },
            new int[,]{
                { 1},
                { 1},
            },
            new int[,]{
                { 1,1,1 },
            },
            new int[,]{
                { 1},
                { 1},
                { 1},
            },
            new int[,]{
                { 1,1 ,1,1},
            },
            new int[,]{
                { 1},
                { 1},
                { 1},
                { 1},
            },
            new int[,]{
                { 1,1 ,1,1,1},
            },
            new int[,]{
                { 1},
                { 1},
                { 1},
                { 1},
                { 1},
            },
            new int[,]{
                { 1,1 },
                { 0,1 },
            },
            new int[,]{
                { 0,1 },
                { 1,1 },
            },
            new int[,]{
                { 1,0 },
                { 1,1 },
            },
            new int[,]{
                { 1,1 },
                { 1,0 },
            },
            new int[,]{
                { 1,0,0 },
                { 1,0,0 },
                { 1,1,1 },
            },
            new int[,]{
                { 0,0,1 },
                { 0,0,1 },
                { 1,1,1 },
            },
            new int[,]{
                { 1,1,1 },
                { 1,0,0 },
                { 1,0,0 },
            },
            new int[,]{
                { 1,1,1 },
                { 0,0,1 },
                { 0,0,1 },
            },
            new int[,]{
                { 1,1,1 },
                { 1,1,1 },
                { 1,1,1 },
            }
        };
    }
    public void GameReset()
    {
        GoogleAdManager.Inst.RequestInterstitial();
        if (gridGroup_Ground!=null)
        {
            PoolMgr.Recycle(gridGroup_Ground);
            gridGroup_Ground = null;
        }
        gridGroup_Ground = PoolMgr.Allocate(IPoolsType.GridGroup_Ground) as GridGroup_Ground;
        gridGroup_Ground.CreatGrids();
        RefreshPrepGridGroup();
    }
    public void GameStart()
    {
        AddPrepGroupRoot();
        GameReset();
    }
    PrepAddGridGroup[] PrepGroup = new PrepAddGridGroup[3];
    void AddPrepGroupRoot()
    {
        for (int i = 0; i < 3; i++)
        {
            Vector2 pos = new Vector2((i - 1) * (6*GameGloab.wh_2), 0);
            var obj = ObjectMgr.InstantiateGameObj(ObjectMgr.LoadResource("Prefab/addgridbg") as GameObject);
            obj.transform.SetParent(GameGloab.root_prep);
            obj.transform.localPosition = pos;
#if UNITY_EDITOR
            obj.name = i.ToString();
#endif
            PrepGroup[i] = obj.transform.GetComponent<PrepAddGridGroup>();
            if (PrepGroup[i] == null)
            {
                PrepGroup[i] = obj.gameObject.AddComponent<PrepAddGridGroup>();
            }
            PrepGroup[i].Root = obj.transform;
        }
    }
    public void RefreshPrepGridGroup()
    {
        for (int i = 0; i < 3; i++)
        {
            PrepGroup[i].Reset();
            var data = PoolMgr.Allocate(IPoolsType.GridGroup_MinPrep) as GridGroup_MinPrep;
            PrepGroup[i].SetGridData(data);
            data.SetData(datalist[UnityEngine.Random.Range(0, datalist.Count - 1)], PrepGroup[i].Root);
            data.CreatGrids();
        }
    }
    public bool IsCanPrepNext()
    {
        var alldata = gridGroup_Ground;
        bool canuse = false;
        for (int p = 0; p < 3; p++)
        {
            var prepgroup = PrepGroup[p];
            if (!prepgroup.IsUse)
            {
                canuse = false; ;
                for (int i = 0; i < alldata.H_count; i++)
                {
                    for (int j = 0; j < alldata.W_count; j++)
                    {
                        if (CanAddPrep(prepgroup.minPrepGroup, alldata, i, j))//每一个都判断能不能放，设置不能放置的表现
                        {
                            canuse = true;
                            break;
                        }
                    }
                    if (canuse)
                    {
                        break;
                    }
                }
                prepgroup.IsCanUse = canuse;
            }
        }
        for (int p = 0; p < 3; p++)
        {
            var prepgroup = PrepGroup[p];
            if (!prepgroup.IsUse)
            {
                if (prepgroup.IsCanUse)
                {
                    return true;//有可以放置的返回true
                }
            }
        }
        return false;
    }

    public bool IsCantUseAllPrep()
    {
        for (int i = 0; i < 3; i++)
        {
            if (!PrepGroup[i].IsUse)
            {
                return false;
            }
        }
        return true;
    }

    Dictionary<int, int> husecount = new Dictionary<int, int>();
    Dictionary<int, int> wusecount = new Dictionary<int, int>();
    bool GetCanClear(List<GridData> data)
    {
        bool isadd = false;
        var alldata = gridGroup_Ground;
        husecount.Clear();
        wusecount.Clear();
        for (int i = 0; i < alldata.H_count; i++)
        {
            husecount[i] = 0;
            wusecount[i] = 0;
            for (int j = 0; j < alldata.W_count; j++)
            {
                if (alldata.Grid[i, j]._TempStatus > 0)
                {
                    husecount[i] += 1;
                }
                if (alldata.Grid[j, i]._TempStatus > 0)
                {
                    wusecount[i] += 1;
                }
            }
            if (husecount[i] == alldata.W_count)
            {
                ////i这一行满了
                for (int _w = 0; _w < alldata.W_count; _w++)
                {
                    if (!data.Contains(alldata.Grid[i, _w]))
                    {
                        isadd = true;
                        data.Add(alldata.Grid[i, _w]);
                    }
                }
            }
            if (wusecount[i] == alldata.H_count)
            {
                ////i这一竖满了
                for (int _h = 0; _h < alldata.H_count; _h++)
                {
                    if (!data.Contains(alldata.Grid[_h, i]))
                    {
                        isadd = true;
                        data.Add(alldata.Grid[_h, i]);
                    }
                }
            }
        }
        return isadd;
    }

    /// <summary>
    /// 根据可放置的拖动的gridgroup 放入maingroup 
    /// </summary>
    public bool RefreshMainGrid()
    {
        bool isuse = false;
        foreach (var v in swPrepGridList)
        {
            v.IsUse = true;
            v.Revert();
            if (!isuse)
            {
                isuse = true;
            }
        }
        swPrepGridList.Clear();

        bool isswEff=false;
        foreach (var v in swClearGridList)
        {
            UIManager.Inst.SetNowScore(v.TrueStatus);
            v.IsUse = false;
            v.TrueStatus = 0;
            v.Revert();
            EffectPool.Inst.PlayBubbleExplode(1, v.Position);//播放销毁动画
            //Debug.LogError("播放销毁动画");
            isswEff = true;
        }
        if (isswEff)
        {
            AudioManager.Inst.PlayBoom();
        }
        swClearGridList.Clear();

        return isuse;
    }
    void RevertswClearGrid()
    {
        foreach (var v in swClearGridList)
        {
            v.swClearRevert();
        }
        swClearGridList.Clear();
    }
    /// <summary>
    /// 还原临时显示的grid
    /// </summary>
    void RevertswGrid()
    {
        foreach (var v in swPrepGridList)
        {
            v.swPrepRevert();
        }
        swPrepGridList.Clear();
    }
    /// <summary>
    /// 检测现在移动到的位置能不能放
    /// </summary>
    public void CheckAvailable(Vector2 _pos)
    {
        Vector2 pos = _pos;
        var gdata = DragingGridMgr.Inst.prepData;
        var alldata = gridGroup_Ground;
        if (M_math.Even(gdata.H_count))
            pos.y += GameGloab.wh_2;
        if (M_math.Even(gdata.W_count))
            pos.x -= GameGloab.wh_2;

        //根据 pos 计算出 i j 对应的grid
        int w = OutGridPos(pos.x);
        if (!Postox.ContainsKey(w))
        {
            RevertswClearGrid();
            RevertswGrid();
            return;//超出 不处理
        }
        int h = OutGridPos(pos.y);
        if (!Postoy.ContainsKey(h))
        {
            RevertswClearGrid();
            RevertswGrid();
            return;//超出 不处理
        }
        int h_index = Postoy[h];//h:w  坐标
        int w_index = Postox[w];
        RevertswClearGrid();
        RevertswGrid();//先清理再筛选
        if (CanAddPrep(gdata, alldata, h_index, w_index, true))
        {
            foreach (var v in swPrepGridList)
            {
                v.swPrep();
            }
        }
        else
        {
            RevertswGrid();
        }
        //判断有没有可以销毁的 显示不同状态

        if (GetCanClear(swClearGridList))
        {
            foreach (var v in swClearGridList)
            {
                v.swClear();
            }
        }
    }

    /// <summary>
    /// 判断GridGroup_prep能不能放
    /// </summary>
    /// <param name="gdata"></param>
    /// <param name="alldata"></param>
    /// <param name="h_index"></param>
    /// <param name="w_index"></param>
    /// <param name="isadd">是否处理swgrid列表</param>
    /// <returns></returns>
    private bool CanAddPrep(GridGroup gdata, GridGroup_Ground alldata, int h_index, int w_index, bool isadd = false)
    {
        bool h_even = M_math.Even(gdata.H_count);
        bool w_even = M_math.Even(gdata.W_count);
        int all_maxh = alldata.H_count - 1;
        int all_maxw = alldata.W_count - 1;
        int h_ban = h_index - ((int)(gdata.H_count * 0.5f));
        int w_ban = w_index - ((int)(gdata.W_count * 0.5f));
        int add_h = h_even ? 1 : 0;
        int add_w = w_even ? 1 : 0;
        //当前选中的位置 根据拖动出来的展开获取需要处理的grid
        int all_h;
        int all_w;
        for (int _h = 0; _h < gdata.H_count; _h++)
        {
            for (int _w = 0; _w < gdata.W_count; _w++)
            {
                //将gdata ij的位置 与alldata的_i_j对应起来
                all_h = h_ban + _h + add_h;
                all_w = w_ban + _w + add_w;
                if (all_h < 0 || all_h > all_maxh || all_w < 0 || all_w > all_maxw)
                {
                    return false; //超出边界
                }
                if (gdata.Grid[_h, _w] != null && gdata.Grid[_h, _w].IsUse)
                {
                    if (alldata.Grid[all_h, all_w].IsUse)
                    {
                        return false;//若gdata有数据 alldata也有数据 说明不能放
                    }
                    else if (isadd)
                    {
                        alldata.Grid[all_h, all_w]._TempStatus = gdata.Grid[_h, _w].TrueStatus;
                        swPrepGridList.Add(alldata.Grid[all_h, all_w]);
                    }
                }
            }
        }
        return true;
    }

    /// <summary>
    /// 根据坐标的值 装换成最靠近的规整坐标的值
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public static int OutGridPos(float index)
    {
        //x -270 到 270 为0到9   y 270 到 -270 为0到9
        //x    : -270  -210  -150  -90  -30   30   90   150   210   270
        //30倍数   -9    -7    -5   -3   -1    1   3     5     7     9
        //        0       1     2    3    4    5    6    7    8      9   
        // 坐标数除30 得到奇数向下取整  偶数向上取整
        float num = index / GameGloab.wh_2;//30倍数
        int p_n = num > 0 ? 1 : -1;//正负值
        float num_abs = M_math.Abs(num);
        int endind = 0;
        if (M_math.Even((int)num_abs))
            endind = (int)(GameGloab.wh_2 * p_n * Math.Ceiling(num_abs));//向上取整
        else
            endind = (int)(GameGloab.wh_2 * p_n * (float)Math.Floor(num_abs));//向下取整
        if (M_math.Abs(endind - index) < (GameGloab.wh_2 - 2))//一个格子半径30  28聊胜于无
            return endind;
        else
            return 0;
    }
}
