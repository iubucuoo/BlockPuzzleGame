using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class UIPanelBase : MonoBehaviour
{
    public virtual void InitBox()
    {

    }
    public virtual void ShowBoxY(TweenCallback Finish = null)
    {
        gameObject.SetActive(true);
        Vector3 pos = transform.localPosition;
        pos.y = 2020;
        transform.localPosition = pos;
        if (Finish != null)
        {
            transform.DOLocalMoveY(0, .3f).SetEase(Ease.OutFlash).OnComplete(Finish);
        }
        else
            transform.DOLocalMoveY(0, .3f).SetEase(Ease.OutFlash);
    }

    public virtual void ShowBoxX(TweenCallback Finish = null)
    {
        gameObject.SetActive(true);
        Vector3 pos = transform.localPosition;
        pos.x = 1480;
        transform.localPosition = pos;
        transform.DOLocalMoveX(0, .3f).SetEase(Ease.OutBack);
        if (Finish != null)
        {
            transform.DOLocalMoveX(0, .3f).SetEase(Ease.OutBack).OnComplete(Finish);
        }
        else
        {
            transform.DOLocalMoveX(0, .3f).SetEase(Ease.OutBack);
        }
    }

    void HideFinish()
    {
        gameObject.SetActive(false);
    }
    public virtual void HideBox()
    {
        transform.DOLocalMoveX(-1480, .2f).SetEase(Ease.InBack).OnComplete(HideFinish);
    }
}
