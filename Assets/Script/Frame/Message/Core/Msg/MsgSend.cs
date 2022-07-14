using System;
using UnityEngine;

public class MsgSend
{
	public static void ToSend(ushort msgId)
	{
		MessageBase.CreateToSend(msgId);
	}
	public static void GSendMsg(MessageBase msg)
	{
        MessageCenter.instance.SendMessage(msg);
	}

	internal static void ToSend(ushort msgId, object[] vs)
	{
		MsgArray.CreateToSend(msgId, vs);
	}
	internal static void ToSendArray(ushort msgId, object[] vs)
	{
		MsgArray.CreateToSend(msgId, vs);
	}
	//internal static void ToSend(ushort msgId, NetMsg msg)
	//{
	//	TcpSendMsg.CreateToSend(msgId, msg);
	//}
	internal static void ToSend(ushort msgId, float v1, float v2)
	{
		MsgFloat2.CreateToSend(msgId, v1, v2);
	}
	internal static void ToSend(ushort msgId, int v)
	{
		MsgInt.CreateToSend(msgId, v);
	}
	internal static void ToSend(ushort msgId, string s1, int i1)
	{
		MsgStringInt.CreateToSend(msgId, s1, i1);
	}


    //public static void GSendBundleAllRes(string abName, Action<UnityEngine.Object[]> cb = null, Action cb2 = null)//请求一个ab内所有资源 会自动卸载
    //{
    //	//HunkResMsg.CreateToSend(msgid, abName, cb);
    //	RequestResMsg.CreateToSend((ushort)RES_ID.GET_OBJ, new CommonArt().SetValue(0, abName, null, cb, cb2));
    //}
    public static void GetRes(string abName, string ArtName, Action<UnityEngine.Object> cb, Action cb2 = null)
    {
        //RequestResMsg.CreateToSend((ushort)RES_ID.GET_OBJ, new CommonArt().SetValue(0, abName, ArtName, cb, cb2));
    }
    public static void GetRes(RES_MODEL_INDEX _ModelID, string ArtName, Action<UnityEngine.Object> cb, Action cb2 = null)
    {
        //RequestResMsg.CreateToSend((ushort)RES_ID.GET_OBJ, new CommonArt().SetValue((int)_ModelID, null, ArtName, cb, cb2));
    }
    public static void GetRes(RES_MODEL_INDEX _ModelID, string abName, string ArtName, Action<UnityEngine.Object> cb, Action cb2 = null)
    {
        //RequestResMsg.CreateToSend((ushort)RES_ID.GET_OBJ, new CommonArt().SetValue((int)_ModelID, abName, ArtName, cb, cb2));
    }
    //public static void GetRes(IArt art)
    //{
    //    if (AppParam._GameMode == GameModel.SuperModel && art is EffectModel)
    //    {
    //        var name = (art as EffectModel).ArtName();
    //        var g = JoystickTestDevice.FindEffect(name);
    //        if (g != null)
    //        {
    //            g.SetActive(true);
    //            art.UseArt(g);
    //            return;
    //        }
    //    }

        //	if (ModelSys.CsUtils.IsIDE())
        //	{
        //		new NewEditorLoad().BuilderResData().GetObj(art);
        //	}
        //	else
        //	{
        //		RequestResMsg.CreateToSend((ushort)RES_ID.GET_OBJ, art);
        //	}
        //}
    }
