using System;
/// <summary>
/// 通用的Message
/// </summary>
public class Message : MessageBase
{
    public int _Int_V, _Int_V2;

    public float[] _UIPos;
    public string _Str;
    /// <summary>
    /// 消息回调
    /// </summary>
    public Action<object[]> _Callback { get; private set; }
    /// <summary>
    /// 对象
    /// </summary>
    public object[] _Objs { get; private set; }

    
    internal Message SetValue(ushort id, string _str)
    {
        messageId = id;
        _Str = _str;
        return this;
    }
 

    internal Message SetValue(int id, Action<object[]> _back, params object[] _objs)
    {
        messageId = (ushort)id;
        _Callback = _back;
        _Objs = _objs;
        return this;
    }
  
    internal Message SetValue(object id, int _v, int _v2)
    {
        messageId = (ushort)id;
        _Int_V = _v;
        _Int_V2 = _v2;
        return this;
    }
    internal Message SetValue(object id, float[] _v, string _v2)
    {
        messageId = (ushort)id;
        _UIPos = _v;
        _Str = _v2;
        return this;
    }


    internal Message() : base()
    {

    }
    public override IPoolsType PoolType { get { return IPoolsType.Message; } }

    public override void OnRecycled()
    {
        _Callback = null;
        _Objs = null;
        _Int_V = 0;
        base.OnRecycled();
    }
}

