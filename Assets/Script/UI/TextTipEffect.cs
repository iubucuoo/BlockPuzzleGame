using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextTipEffect : MonoBehaviour
{
    public Text swtext;
    //// Start is called before the first frame update
    //void Start()
    //{

    //}
    System.Action CallBack;
    public void PlayEffect(Vector3 pos, string str , System.Action cb=null)
    {
        swtext.gameObject.SetActive(true);
        transform.position = pos+Vector3.up*30;
        swtext.text = str;
        swtext.transform.localPosition = Vector3.zero;
        swtext.transform.DOLocalMoveY(100, .8f).OnComplete(MoveEnd);
        CallBack = cb;

    }
    void MoveEnd()
    {
        transform.localPosition = GameGloab.OutScreenV2;
        swtext.text = "";
        swtext.gameObject.SetActive(false);
        if (CallBack!=null)
        {
            CallBack();
        }
    }
    //// Update is called once per frame
    //void Update()
    //{
        
    //}
}
