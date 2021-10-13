/// <summary>
/// 事件节点单位
/// </summary>
internal class EventNode : Node
{
    /// <summary>
    /// 事件节点上的监听物
    /// </summary>
    public new IEventListen listen;
    /// <summary>
    /// 下一个事件节点
    /// </summary>
    public new EventNode next;
    /// <summary>
    /// 事件节点结构体
    /// </summary>
    /// <param name="tmpLister">事件监听单位</param>
    public EventNode(IEventListen tmpLister)
    {
        listen = tmpLister;
        next = null;
    }
}
abstract class Node
{
    public object listen;
    public Node next;
}