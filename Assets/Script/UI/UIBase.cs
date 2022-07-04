using System.Collections;
using System.Collections.Generic;
using UnityEngine;
enum WndStatus
{
    Closed = 0,
    Hided = 1,
    Opend = 2,
    Opening = 3,
}
public class UIBase
{
    public string rootname;
    public string WndName;
    int layer = (int)UILayer.Panel;
    int orderInLayer = 0;
    UIHideType hideType = UIHideType.Hide;
    UIHideFunc hideFunc = UIHideFunc.MoveOutOfScreen;
    UIEscClose escClose = UIEscClose.DontClose;
    bool isFull = false;
    bool visible = false;
    bool loaded = false;
    bool bBaseUI = true;
    GameObject root;
    
    private float rootSavedX;
    private float rootSavedY;
    void SetUIVisible(bool _visible)
    {
        if (hideFunc == UIHideFunc.MoveOutOfScreen)
        {
            if (visible)
            {
                if (rootSavedX!=0 && rootSavedY!=0)
                {
                    root.GetComponent<RectTransform>().anchoredPosition = new Vector2(rootSavedX, rootSavedY);
                }
            }
            else
            {
                var root_rect = root.GetComponent<RectTransform>();
                rootSavedX = root_rect.anchoredPosition.x;
                rootSavedY = root_rect.anchoredPosition.y;
                root_rect.anchoredPosition = new Vector2(100000000, 100000000);
            }
        }
        else
        {
            root.SetActive(_visible);
        }
    }
   
    void SetVisible(bool _visible, bool _animation)
    {
        if (visible == _visible)
            return;
        DebugMgr.Log(_visible ? "show" : "hide" + rootname);
    }
}
