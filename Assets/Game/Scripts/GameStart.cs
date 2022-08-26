using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart:MonoBehaviour
{
    // Start is called before the first frame update
    public void Start()
    {
        loadindex = 0;
        PackageMgr.LoadObjectCallBack("UICommonWnd", InitStart, true);
        PackageMgr.LoadObjectCallBack("UICommonWnd1", InitStart, true);
    }
    int loadindex;
    void InitStart(string packageName)
    {
        //DebugMgr.LogError(packageName);
        if (++loadindex == 2)
        {
            AllUIPanelManager.Inst.Show(IPoolsType.UI_StartPanel);
            EffectPool.PlayEffect("bubblefffect", "bubbleexplodeyellow");
        }
    }
}
