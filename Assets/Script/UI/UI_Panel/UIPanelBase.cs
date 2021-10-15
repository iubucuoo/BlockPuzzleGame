using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class UIPanelBase : UIEventListenBase
{
    float _y = 2020;
    float _x = 1480;
    float swtime = .3f;
    float hidetime = .2f;
    public virtual void InitBox()
    {

    }
    public void RefreshGame()
    {
        SendEventMgr.GSendMsg((ushort)UITopPanelListenID.ResetTop);
        GridGroupMgr.Inst.GameReset();//重新启动游戏
        AudioMgr.Inst.PlayGameOpen();
    }
   
    public virtual void ShowBoxY(TweenCallback Finish = null)
    {
        gameObject.SetActive(true);
        Vector3 pos = transform.localPosition;
        pos.y = _y;
        transform.localPosition = pos;
        if (Finish != null)
        {
            transform.DOLocalMoveY(0, swtime).SetEase(Ease.OutFlash).OnComplete(Finish);
        }
        else
            transform.DOLocalMoveY(0, swtime).SetEase(Ease.OutFlash);
    }

    public virtual void ShowBoxX(TweenCallback Finish = null)
    {
        gameObject.SetActive(true);
        Vector3 pos = transform.localPosition;
        pos.x = _x;
        transform.localPosition = pos;
        if (Finish != null)
        {
            transform.DOLocalMoveX(0, swtime).SetEase(Ease.OutBack).OnComplete(Finish);
        }
        else
        {
            transform.DOLocalMoveX(0, swtime).SetEase(Ease.OutBack);
        }
    }

    public void HideFinish()
    {
        gameObject.SetActive(false);
    }
    public virtual void HideBox()
    {
        transform.DOLocalMoveX(-_x, hidetime).SetEase(Ease.InBack).OnComplete(HideFinish);
    }
}
