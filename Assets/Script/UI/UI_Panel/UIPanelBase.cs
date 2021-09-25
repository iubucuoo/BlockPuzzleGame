using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIPanelBase : MonoBehaviour
{
    public virtual void InitBox()
    {

    }
    public virtual void ShowBox()
    {
        gameObject.SetActive(true);
        transform.localScale = Vector3.zero;
        transform.DOScale(1, 0.3f).SetEase(Ease.InBounce).OnComplete(ShowFinish);
    }
    void ShowFinish()
    {

    }
    public virtual void HideBox()
    {
        gameObject.SetActive(false);

    }
}
