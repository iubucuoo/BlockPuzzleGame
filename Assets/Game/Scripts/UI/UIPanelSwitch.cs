using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelSwitch: UIEventListenBase
{
   UI_GameOverPanel UI_GameOverPanel;
   UI_SetPanel UI_SetPanel;
   UI_AddRotatePanel UI_AddRotatePanel;
    // Start is called before the first frame update
    void Start()
    {
        //UI_GameOverPanel =UIMgr.Inst.UIRoot.Find("gameoverPanel").GetComponent<UI_GameOverPanel>();
        //UI_SetPanel = UIMgr.Inst.UIRoot.Find("SetPanel").GetComponent<UI_SetPanel>();
        //UI_AddRotatePanel = UIMgr.Inst.UIRoot.Find("AddRotatePanel").GetComponent<UI_AddRotatePanel>();
    }

    public override void InitEventListen()
    {
        messageIds = new ushort[]
       {
           (ushort)UIMainListenID.SwPanel_Set,
           (ushort)UIMainListenID.SwPanel_AddRotate,
           (ushort)UIMainListenID.SwPanel_GameOver,
       };
        RegistEventListen(this, messageIds);
        base.InitEventListen();
    }
    public override void ProcessEvent(MessageBase tmpMsg)
    {
        switch (tmpMsg.messageId)
        {
            case (ushort)UIMainListenID.SwPanel_Set:
                UI_SetPanel.ShowBoxX();
                break;
            case (ushort)UIMainListenID.SwPanel_AddRotate:
                UI_AddRotatePanel.ShowBoxX();
                break;
            case (ushort)UIMainListenID.SwPanel_GameOver:
                //UI_GameOverPanel.ShowGameOver();
                break;
            default:
                break;
        }
        base.ProcessEvent(tmpMsg);
    }
}
