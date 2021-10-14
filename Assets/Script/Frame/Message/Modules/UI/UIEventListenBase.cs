
/// <summary>
/// UI消息监听基类
/// </summary>
public class UIEventListenBase : MonoBase
{
    /// <summary>
    /// 注册消息监听
    /// </summary>
    /// <param name="tmpListen"></param>
    /// <param name="msgs"></param>
    public override bool RegistEventListen(IEventListen tmpListen, params ushort[] msgs)
    {
        UIManager.instance.RegistEventListen(tmpListen, msgs);
        return base.RegistEventListen(tmpListen, msgs);
    }
    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="msg"></param>
    public override void SendMsg(MessageBase msg)
    {
        UIManager.instance.SendMsg(msg);
    }

    /// <summary>
    /// 注销消息监听
    /// </summary>
    /// <param name="tmpListen"></param>
    /// <param name="msgs"></param>
    public override bool UnRegistEventListen(IEventListen tmpListen, params ushort[] msgs)
    {
        UIManager.instance.UnRegistEventListen(tmpListen, msgs);
        return true;
    }
    ///// <summary>
    ///// 发送通用类
    ///// </summary>
    ///// <param name="id"></param>
    ///// <param name="_back"></param>
    ///// <param name="_objs"></param>
    //public virtual void SendMsg(ushort id, System.Action<object[]> _back, params object[] _objs)
    //{
    //    Message msg = (Message)PoolMgr.Allocate(IPoolsType.Message);
    //    msg.SetValue(id, _back, _objs);
    //    SendMsg(msg);

    //}
    //public virtual void SendMsg(ushort id, int v)
    //{
    //    Message msg = (Message)PoolMgr.Allocate(IPoolsType.Message);
    //    msg.SetValue(id, v);
    //    SendMsg(msg);

    //}
    //public virtual void SendMsg(ushort id, int v, int v2)
    //{
    //    Message msg = (Message)PoolMgr.Allocate(IPoolsType.Message);
    //    msg.SetValue(id, v, v2);
    //    SendMsg(msg);

    //}
    //public virtual void SendMsg(ushort id, string str)
    //{
    //    Message msg = (Message)PoolMgr.Allocate(IPoolsType.Message);
    //    msg.SetValue(id, str);
    //    SendMsg(msg);

    //}
}

