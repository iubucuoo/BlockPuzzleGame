using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WUtils;

public class UI_SetPanel : UIBase
{
    public override string WndName => IPoolsType.UI_SetPanel.ToString();
    
    public override IPoolsType GroupType => IPoolsType.UI_SetPanel;
    public override int orderInLayer { get => 10; set => orderInLayer = value; }


    public override void OnRecycled()
    {

    }
    UI_SetPanelJob paneljob;
    public override void OnCreate()
    {
        paneljob = WndRoot.AddMissingComponent<UI_SetPanelJob>();

    }
    public override void OnShow()
    {
         
    }
    public override void UnRegistEvents()
    {
         
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
