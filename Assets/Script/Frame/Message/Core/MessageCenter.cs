﻿using System;
/// <summary>
/// 消息管理中心
/// </summary>
public class MessageCenter
{
    static MessageCenter mInstance;
    public static MessageCenter instance
    {
        get
        {
            return mInstance ?? (mInstance = new MessageCenter());
        }
    }

    MessageCenter()
    {
        SendMsgToLua = _base => { };
    }

    /// <summary>
    /// 設置LUA消息處理
    /// </summary>
    /// <param name="_SendMsgToLua">向lua发送</param>
    /// <param name="_LuaSendMsgToCs">向Cs发送消息</param>
    /// <param name="_IntToStr">Int转String</param>
    public void SetEvent(Action<MessageBase> _SendMsgToLua)
    {
        SendMsgToLua = _SendMsgToLua;
    }

    /// <summary>
    /// 消息转发到LuaMessageCenter
    /// </summary>
    event Action<MessageBase> SendMsgToLua;

    /// <summary>
    /// 向Cs发送消息
    /// </summary>


    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="msg"></param>
    public void SendMessage(MessageBase msg)
    {
        AnasysisMessage(msg);
    }
    public static void _SendMessage(MessageBase msg)
    {
        instance.SendMessage(msg);
    }

    void LuaProcess(MessageBase msg)
    {
        SendMsgToLua(msg);
    }

    void AnasysisMessage(MessageBase msg)
    {
        ManagerID tmpId = msg.GetManagerID();
        switch (tmpId)
        {
            //case ManagerID.LAssetManager:
            //case ManagerID.LAudioManager:
            //case ManagerID.LGameManager:
            //case ManagerID.LNetManager:
            //case ManagerID.LUnitManager:
            //case ManagerID.LUIManager: LuaProcess(msg); PoolsMgr.Release(msg); break;

            //case ManagerID.AssetManager: AssetManager.instance.SendMsg(msg); break;
            //case ManagerID.GameManager: GameManager.instance.SendMsg(msg); break;
            //case ManagerID.NetManager: NetManager.instance.SendMsg(msg); break;
            //case ManagerID.LocalNetManager: LocalNetManager.instance.SendMsg(msg); break;
            //case ManagerID.UnitManager: Modules.Unit.UnitManager.instance.SendMsg(msg); break;
            case ManagerID.UIManager: UIManager.instance.SendMsg(msg); break;
            case ManagerID.AudioManager: break;
            default: UnityEngine.Debug.LogError(string.Format("{0},此消息没有在相应的ManagerID中", tmpId)); break;
        }
    }
}

