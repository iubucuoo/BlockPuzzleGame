public class TimeEventBase
{
    internal Intervel_Time _Type;
    internal System.Action<int,float> back;
    internal TimeEventBase next;
    internal TimeEventBase(System.Action<int,float> callback,Intervel_Time _type)
    {
        back = callback;
        _Type = _type;
    }
}
public class TimeEvent : TimeEventBase
{
    internal float _LastZ;//帧
    internal float _LastTime;//上一个时间啊段
    internal int _Interval;//时间间隔
    int _OldInterval;
    internal int _SumIndex;// 如果是Index类型的就是累计，如果是定时器类型的就是外面传进来的，再传出去，Index累计
    internal int _Loop = -1;
    int _CurLoop;
    internal bool isLua = false;
    internal bool _CanGiveUp;
    internal TimeEvent(System.Action<int,float> callback, Intervel_Time _type,int _inter = 0,int mid=0,int _loop=-1,bool _isLua=false,bool can_give_up=false) : base(callback,_type)
    {
        _CanGiveUp = can_give_up;
        isLua = _isLua;
        _Interval = _inter;
        _OldInterval = _Interval;
        _SumIndex = mid;
        _Loop = _loop;
        _LastTime = TimeMgr.Instance._CurTime;

    }
    internal void Stop()
    {        
        _Interval = int.MaxValue;
    }
    internal void Recovery()
    {
        _Interval = _OldInterval;
        _LastTime = TimeMgr.Instance._CurTime;
    }
    internal void Resetting(int _lastTime)
    {
        _Interval = _lastTime;
        _OldInterval = _lastTime;
        _LastTime = TimeMgr.Instance._CurTime;
    }
    internal void Destroy()
    {
        _Loop = 0;
        _CurLoop = 0;
    }
    internal bool CanRun()
    {
        return _Loop == -1 || _CurLoop++ < _Loop;
    }    
}

