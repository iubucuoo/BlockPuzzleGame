using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SetPanel : UIBase,IPoolable
{
    public override string WndName => IPoolsType.UI_SetPanel.ToString();

    public override UIHideType hideType => UIHideType.WaitDestroy;

    public override UIHideFunc hideFunc => UIHideFunc.MoveOutOfScreen;

    public override int layer { get => (int)UILayer.Panel; set => layer = value; }

    public override bool isFull => false;

    public IPoolsType GroupType => IPoolsType.UI_SetPanel;
    public override int orderInLayer { get => 10; set => orderInLayer = value; }
    public bool IsRecycled { get; set; }

    public void OnRecycled()
    {

    }
    UI_SetPanelJob paneljob;
    public override void OnCreate()
    {
        paneljob = AddMissingCom<UI_SetPanelJob>();

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
