using System;
/// <summary>
/// 通用的Message
/// </summary>
public class Message : MessageBase
{
    public int num, num2;
    public int[] pos;
    public float[] uipos;
    public string str;
    /// <summary>
    /// 消息回调
    /// </summary>
    public Action<object[]> callback { get; private set; }
    /// <summary>
    /// 对象
    /// </summary>
    public object[] objs { get; private set; }

    
    internal Message SetValue(ushort id, string _str)
    {
        messageId = id;
        str = _str;
        return this;
    }
 

    internal Message SetValue(int id, Action<object[]> _back, params object[] _objs)
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
    internal Message SetValue(object id, float[] _v, string _v2)
    {
        messageId = (ushort)id;
        uipos = _v;
        str = _v2;
        return this;
    }
    internal Message SetValue(object id, int _v, int[] _v2)
    {
        messageId = (ushort)id;
        num = _v;
        pos = _v2;
        return this;
    }

    internal Message() : base()
    {

    }
    public override IPoolsType GroupType { get { return IPoolsType.Message; } }

    public override void OnRecycled()
    {
        callback = null;
        objs = null;
        num = 0;
        base.OnRecycled();
    }
}

