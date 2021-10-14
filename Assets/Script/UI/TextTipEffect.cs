using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TextTipEffect : UIEventListenBase
{
    public Text swtext;
    //// Start is called before the first frame update
    //void Start()
    //{

    //}
    System.Action CallBack;
    Vector3 swpos;
    public void PlayEffect(float[] pos, string str, System.Action cb = null)
    {
        swpos.x = pos[0];
        swpos.y = pos[1];
        swtext.gameObject.SetActive(true);
        transform.position = swpos + Vector3.up * 30;
        swtext.text = str;
        swtext.transform.localPosition = Vector3.zero;
        swtext.transform.DOLocalMoveY(100, .8f).OnComplete(MoveEnd);
        CallBack = cb;
    }
    public void PlayEffect(Vector3 pos, string str, System.Action cb = null)
    {
        swtext.gameObject.SetActive(true);
        transform.position = pos + Vector3.up * 30;
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
        if (CallBack != null)
        {
            CallBack();
        }
    }
    //// Update is called once per frame
    //void Update()
    //{

    //}
    public override void InitEventListen()
    {
        messageIds = new ushort[]{
            (ushort)UISwTextEffectListenID.SwEffect,
        };
        RegistEventListen(this, messageIds);
        base.InitEventListen();
    }
    public override void ProcessEvent(MessageBase tmpMsg)
    {
        switch (tmpMsg.messageId)
        {
            case (ushort)UISwTextEffectListenID.SwEffect:
                Message msg = (Message)tmpMsg;
                PlayEffect(msg.uipos, msg.str);
                break;
            default:
                break;
        }
        base.ProcessEvent(tmpMsg);
    }
}
