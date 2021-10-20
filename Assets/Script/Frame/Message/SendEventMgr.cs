using System.Collections.Generic;
using UnityEngine;
public class SendEventMgr : MonoSingleton<SendEventMgr>
{
    public static void GSendMsg(MessageBase msg)
    {
        UIManager.instance.SendMsg(msg);
    }
    public static void GSendMsg(ushort msgId, string v)
    {
        Message msg = (Message)PoolMgr.Allocate(IPoolsType.Message);
        msg.SetValue(msgId, v);
        GSendMsg(msg);
    }
   
    public static void GSendMsg(ushort msgId, System.Action<object[]> back, params object[] objs)
    {
        Message msg = (Message)PoolMgr.Allocate(IPoolsType.Message);
        msg.SetValue(msgId, back, objs);
        GSendMsg(msg);
    }
    public static void GSendMsg(ushort msgId, int v)
    {
        Message msg = (Message)PoolMgr.Allocate(IPoolsType.Message);
        msg.SetValue(msgId, v);
        GSendMsg(msg);
    }
    public static void GSendMsg(ushort msgId, int v, int v2)
    {
        Message msg = (Message)PoolMgr.Allocate(IPoolsType.Message);
        msg.SetValue(msgId, v, v2);
        GSendMsg(msg);
    }

    public static void GSendMsg_Pos(ushort id, int v, int[] v2)
    {
        Message msg = (Message)PoolMgr.Allocate(IPoolsType.Message);
        msg.SetValue(id, v, v2);
        GSendMsg(msg);
    }
    public static void GSendMsg(ushort id, float[] v, string v2)
    {
        Message msg = (Message)PoolMgr.Allocate(IPoolsType.Message);
        msg.SetValue(id, v, v2);
        GSendMsg(msg);
    }
    public static void GSendMsg(ushort msgId)
    {
        MessageBase msg = ((MessageBase)PoolMgr.Allocate(IPoolsType.Message)).SetValue(msgId);
        GSendMsg(msg);
    }
}