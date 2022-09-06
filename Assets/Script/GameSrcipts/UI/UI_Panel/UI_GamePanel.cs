using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WUtils;

public class UI_GamePanel : UIBase
{
    public override IPoolsType PoolType => IPoolsType.UI_GamePanel;

    public override string WndName => IPoolsType.UI_GamePanel.ToString();

    public override UIHideType hideType { get => UIHideType.Hide; }

    public override UIHideFunc hideFunc => UIHideFunc.MoveOutOfScreen;
     

    public override bool isFull => true;

  
    UI_GamePanelJob paneljob;
    public override void OnCreate()
    {
        paneljob = WndRoot.AddMissingComponent<UI_GamePanelJob>();
        paneljob.SetPanel(this);
        
    }
    public override void OnShow()
    {

    }
    public override void UnRegistEvents()
    {
        paneljob.UnRegistEvents();
    }
}
