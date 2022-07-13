using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WUtils;

public class UI_AddRotatePanel : UIBase
{
    public override string WndName => IPoolsType.UI_AddRotatePanel.ToString();
 
    public override IPoolsType GroupType => IPoolsType.UI_AddRotatePanel;
    public override int orderInLayer { get => 10; set => orderInLayer = value; }
 
    UI_AddRotatePanelJob paneljob;
    public override void OnCreate()
    {
        paneljob =WndRoot.AddMissingComponent<UI_AddRotatePanelJob>();

    }
    public override void OnShow()
    {
        //paneljob.ShowGameOver();
    }
    public override void UnRegistEvents()
    {
    }
 
}
