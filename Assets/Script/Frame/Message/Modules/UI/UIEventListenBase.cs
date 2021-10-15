
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
}

