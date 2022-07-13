using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct FullPanel
{
    public string name;
    public IPoolsType pooltype;
}
public class AllUIPanelManager : MonoSingleton<AllUIPanelManager>
{
    //Dictionary<int, CreateInstance> Factorys = new Dictionary<int, CreateInstance>();
    //Dictionary<int, UIBase> UIInstanceCache = new Dictionary<int, UIBase>();
    Dictionary<string, UIBase> name2UI = new Dictionary<string, UIBase>();
    Stack<FullPanel> fullPanelList = new Stack<FullPanel>();
    public UIBase Show(IPoolsType pooltype,bool move = false)
    {
        string _name = pooltype.ToString();
        UIBase ui;
        if (name2UI.ContainsKey(_name))
        {
            ui = name2UI[_name];
            //DebugMgr.LogError(_name + "     " + ui.visible);
            if (ui.visible)
                return ui;
        }
        else
        {
            ui = GetUIClass(pooltype);
            name2UI[_name] = ui;
        }
        //DebugMgr.LogError(_name + "  show   ");
        ui.Show(move);
        ShowFullPanel(ui, pooltype);
        return ui;
    }
 
    UIBase GetUIClass(IPoolsType pooltype)
    {
        //if (UIInstanceCache.TryGetValue((int) pooltype,out UIBase uIBase))
        //{
        //    DebugMgr.LogError(pooltype.ToString());
        //    return uIBase;
        //}
        //if (Factorys.TryGetValue((int)pooltype, out CreateInstance mFactory))
        //{
        //    uIBase = mFactory.Create() as UIBase;
        //    UIInstanceCache.Add((int)pooltype, uIBase);
        //    return uIBase;
        //}
        //mFactory = new CreateInstance(pooltype);
        //Factorys.Add((int)pooltype, mFactory);
        //uIBase = mFactory.Create() as UIBase;
        //UIInstanceCache.Add((int)pooltype, uIBase);
        //return uIBase;
        return PoolMgr.Allocate(pooltype) as UIBase;
    }

    private void ShowFullPanel(UIBase ui, IPoolsType pooltype)
    {
        if (!ui.isFull)
        {
            return;
        }
        if (fullPanelList.Count > 0)
        {
            var _name = fullPanelList.Peek().name;
            if (_name==ui.WndName)
            {
                return;
            }
            name2UI[_name].Hide();
        }
        fullPanelList.Push(new FullPanel() {name= ui.WndName,pooltype = pooltype } );
    }

    void HideFullPanel(UIBase ui)
    {
        if (!ui.isFull)
        {
            return;
        }
        fullPanelList.Pop();
        if (fullPanelList.Count<1)
        {
            return;
        }
        var fullpanel = fullPanelList.Peek();
        var _name = fullpanel.name;
        if (name2UI.TryGetValue(_name,out UIBase _ui))
        {
            _ui.Show();
        }
        else
        {
            Show(fullpanel.pooltype);
        }
    }
    public UIBase Hide(IPoolsType pooltype,bool move = false)
    {
        string _name = pooltype.ToString();
        if (name2UI.TryGetValue(_name, out UIBase ui))
        {
            ui.Hide(move);
            HideFullPanel(ui);
        }
        return ui;
    }
    public void ReleaseUI(UIBase ui)
    {
        if (name2UI.ContainsKey(ui.WndName))
        {
            name2UI.Remove(ui.WndName);
            PoolMgr.Recycle(ui);
        }
    }
}
