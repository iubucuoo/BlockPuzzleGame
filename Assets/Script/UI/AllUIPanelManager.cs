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
    Dictionary<string, UIBase> name2UI = new Dictionary<string, UIBase>();
    Stack<FullPanel> fullPanelList = new Stack<FullPanel>();
    public UIBase Show(IPoolsType pooltype)
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
        ui.Show();
        ShowFullPanel(ui, pooltype);
        return ui;
    }
    UIBase GetUIClass(IPoolsType pooltype)
    {
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
    public UIBase Hide(IPoolsType pooltype)
    {
        string _name = pooltype.ToString();
        if (name2UI.TryGetValue(_name, out UIBase ui))
        {
            ui.Hide();
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
