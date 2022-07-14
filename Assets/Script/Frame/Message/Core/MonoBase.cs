using UnityEngine;
/// <summary>
/// 监听基类
/// </summary>
public class MonoBase : MonoBehaviour, IEventListen
{
    /// <summary>
    /// 监听的消息
    /// </summary>
    [HideInInspector]
    public  ushort[] messageIds { get; set; }

    /// <summary>
    /// 初始化
    /// </summary>
    public virtual void InitEventListen()
    {

    }

    void Awake()
    {
        InitEventListen();
    }
    /// <summary>
    /// 处理
    /// </summary>
    /// <param name="tmpMsg"></param>
    public virtual void ProcessEvent(MessageBase tmpMsg)
    {

    }
    /// <summary>
    /// 消息注册监听机制
    /// </summary>
    /// <param name="tmpListen"></param>
    /// <param name="msgs"></param>
    /// <returns></returns>
    public virtual bool RegistEventListen(IEventListen tmpListen, params ushort[] msgs)
    {
        return true;
    }
    /// <summary>
    /// 消息发送
    /// </summary>
    /// <param name="msg"></param>
    public virtual void SendMsg(MessageBase msg)
    {

    }
    /// <summary>
    /// 消息注销
    /// </summary>
    /// <param name="tmpListen"></param>
    /// <param name="msgs"></param>
    /// <returns></returns>
    public virtual bool UnRegistEventListen(IEventListen tmpListen, params ushort[] msgs)
    {
        return false;
    }

    void OnDestroy()
    {
        if (messageIds != null)
        {
            UnRegistEventListen(this, messageIds);
            UnRegistEnd();
        }
    }
    internal virtual void UnRegistEnd()
    {

    }
}

