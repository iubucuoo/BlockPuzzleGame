using System.Collections.Generic;
using UnityEngine;
public class SendEventMgr : UIEventListenBase
{
    public static SendEventMgr Instance
    {
        get
        {
            return _instance;
        }
    }
    void Awake()
    {
        _instance = this;
    }
    static SendEventMgr _instance;

    public static void GSendMsg(MessageBase msg)
    {
        Instance.SendMsg(msg);
    }
    public static void GNetSendMsg(MessageBase msg)
    {
        Instance.SendMsg(msg);
    }
    public static void GSendMsg(ushort msgId, string v)
    {
        Message msg = (Message)PoolMgr.Allocate(IPoolsType.Message);
        msg.SetValue(msgId, v);
        Instance.SendMsg(msg);
    }
   
    public static void GSendMsg(ushort msgId, System.Action<object[]> back, params object[] objs)
    {
        Message msg = (Message)PoolMgr.Allocate(IPoolsType.Message);
        msg.SetValue(msgId, back, objs);
        Instance.SendMsg(msg);
    }
    public static void GSendMsg(ushort msgId, int v)
    {
        Message msg = (Message)PoolMgr.Allocate(IPoolsType.Message);
        msg.SetValue(msgId, v);
        Instance.SendMsg(msg);
    }
    public static void GSendMsg(ushort msgId, int v, int v2)
    {
        Message msg = (Message)PoolMgr.Allocate(IPoolsType.Message);
        msg.SetValue(msgId, v, v2);
        Instance.SendMsg(msg);
    }

    public static void GSendMsg_Pos(ushort id, int v, int[] v2)
    {
        Message msg = (Message)PoolMgr.Allocate(IPoolsType.Message);
        msg.SetValue(id, v, v2);
        Instance.SendMsg(msg);
    }
    public static void GSendMsg(ushort id, float[] v, string v2)
    {
        Message msg = (Message)PoolMgr.Allocate(IPoolsType.Message);
        msg.SetValue(id, v, v2);
        Instance.SendMsg(msg);
    }
    public static void GSendMsg(ushort msgId)
    {
        MessageBase msg = ((MessageBase)PoolMgr.Allocate(IPoolsType.Message)).SetValue(msgId);
        Instance.SendMsg(msg);
    }

    public override void InitEventListen()
    {

    }
    public override void ProcessEvent(MessageBase tmpMsg)
    {
        base.ProcessEvent(tmpMsg);
    }
}