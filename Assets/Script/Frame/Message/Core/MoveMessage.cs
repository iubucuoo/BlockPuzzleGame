public class MoveMessage : MessageBase
{
    public byte _Dir;
    public int xAxis, yAxis;
    /// 通用的Message
    /// </summary>
    /// <param name="id">请求消息</param>
    /// <param name="_back">回调函数</param>
    /// <param name="_objs">传递参数</param>
    internal MoveMessage(ushort id, byte dir) : base(id)
    {
        _Dir = dir;
    }

    internal MoveMessage SetValue(ushort id, byte dir, int x, int y)
    {
        messageId = (ushort)id;
        _Dir = dir;
        xAxis = x;
        yAxis = y;
        return this;
    }

    internal override void Reset()
    {
        base.Reset();
        _Dir = (byte)255;
        xAxis = 0;
        yAxis = 0;
    }
    internal MoveMessage()
    {

    }

    //internal override Pools_Type GetPoolType()
    //{
    //    return Pools_Type.MoveMessage;
    //}
}
