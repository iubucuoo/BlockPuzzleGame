/// <summary>
/// 事件监听单位
/// </summary>
public interface IEventListen
{
    ushort[] messageIds { get; set; }
    /// <summary>
    /// 接受消息
    /// </summary>
    /// <param name="tmpMsg"></param>
    void ProcessEvent(MessageBase tmpMsg);
    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="msg"></param>
    void SendMsg(MessageBase msg);
    /// <summary>
    /// 注册消息监听
    /// </summary>
    /// <param name="tmpListen"></param>
    /// <param name="msgs"></param>
    bool RegistEventListen(IEventListen tmpListen, params ushort[] msgs);
    /// <summary>
    /// 销毁消息监听
    /// </summary>
    /// <param name="tmpListen"></param>
    /// <param name="msgs"></param>
    bool UnRegistEventListen(IEventListen tmpListen, params ushort[] msgs);
    /// <summary>
    /// 初始化
    /// </summary>
    void InitEventListen();
}