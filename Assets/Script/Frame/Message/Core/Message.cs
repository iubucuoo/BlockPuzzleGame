using System;
/// <summary>
/// 通用的Message
/// </summary>
public class Message : MessageBase
{
    public int num, num2;
    public int[] pos;
    /// <summary>
    /// 消息回调
    /// </summary>
    public Action<object[]> callback { get; private set; }
    /// <summary>
    /// 对象
    /// </summary>
    public object[] objs { get; private set; }


    /// <summary>
    /// 通用的Message
    /// </summary>
    /// <param name="id">请求消息</param>
    /// <param name="_back">回调函数</param>
    /// <param name="_objs">传递参数</param>
    internal Message(object id, Action<object[]> _back = null, params object[] _objs) : base((ushort)id)
    {
        callback = _back;
        objs = _objs;
    }
    internal Message(ushort id, Action<object[]> _back, object[] _objs) : base((ushort)id)
    {
        callback = _back;
        objs = _objs;
    }

    internal Message(ushort id, Action<object[]> _back, object _objs) : base((ushort)id)
    {
        callback = _back;
        objs = new object[] { _objs };
    }

    public string str;
    internal Message SetValue(ushort id, string _str)
    {
        messageId = id;
        str = _str;
        return this;
    }
    internal Message _SetValue(ushort id, Action<object[]> _back, object _str)
    {
        messageId = id;
        callback = _back;
        objs = new object[] { _str };
        return this;
    }

    internal Message SetValue(int id, Action<object[]> _back, params object[] _objs)
    {
        messageId = (ushort)id;
        callback = _back;
        objs = _objs;
        return this;
    }
    internal Message _SetValue(int id, Action<object[]> _back, object[] _objs)
    {
        messageId = (ushort)id;
        callback = _back;
        objs = _objs;
        return this;
    }

    internal Message SetValue(object id, int _v)
    {
        messageId = (ushort)id;
        num = _v;
        return this;
    }
    internal Message SetValue(object id, int _v, int _v2)
    {
        messageId = (ushort)id;
        num = _v;
        num2 = _v2;
        return this;
    }
    internal Message SetValue(object id, int _v, int[] _v2)
    {
        messageId = (ushort)id;
        num = _v;
        pos = _v2;
        return this;
    }
    internal override void Reset()
    {
        callback = null;
        objs = null;
        num = 0;
        base.Reset();
    }
    internal Message() : base()
    {

    }

    //internal override Pools_Type GetPoolType()
    //{
    //    return Pools_Type.Message;
    //}
}

