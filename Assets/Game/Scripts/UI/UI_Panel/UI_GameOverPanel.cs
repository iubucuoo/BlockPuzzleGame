using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WUtils;

public class UI_GameOverPanel : UIBase
{
    public override string WndName => IPoolsType.UI_GameOverPanel.ToString();
    public override IPoolsType GroupType => IPoolsType.UI_GameOverPanel;
    public override int orderInLayer { get => 10; set => orderInLayer = value; }
  
    UI_GameOverPanelJob paneljob;
    public override void OnCreate()
    {
        paneljob = WndRoot.AddMissingComponent<UI_GameOverPanelJob>();

    }
    public override void OnShow()
    {
        paneljob.ShowGameOver();
    }
    public override void UnRegistEvents()
    {
    }
}
