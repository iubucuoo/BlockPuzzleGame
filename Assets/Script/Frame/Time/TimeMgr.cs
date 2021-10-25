using UnityEngine;
using System.Collections.Generic;
using System;

public enum Intervel_Time:byte
{
    Index=16,//数据
    Float=32,//模型    
    Intervalometer=1,//定时器   
}


public class TimeMgr : MonoBehaviour
{
    List<TimeEventBase> mgr;
    const int _Interval = 16;
    internal float _CurTime;
    float _PreTime;   
    internal double _STimestamp { get;private set; }
    public int _Timestamp { get; private set; }
    public double _MsTimestamp;
    internal int _MsTime;
    float up;
    int _SumIndex;
    internal static TimeMgr Instance;
    private void Awake()
    {
        Instance = this;
        mgr = new List<TimeEventBase>();
        _CurTime = Time.realtimeSinceStartup * 1000;
        AddEvent(new TimeEvent(null, Intervel_Time.Float, 16));
        AddEvent(new TimeEvent(null, Intervel_Time.Index, 32));
        AddEvent(new TimeEvent(null, Intervel_Time.Intervalometer, 20000,0,-1));
    }
    internal void Clear()
    {
        for (int i = 0; i < mgr.Count; i++)
        {
            var tmp = mgr[i];
            while (tmp.next!=null)
            {
                if((tmp.next is TimeEvent)&&(tmp.next as TimeEvent).isLua)
                {
                    var k = tmp.next;
                    if (tmp.next.next!=null)
                    {
                        tmp.next = tmp.next.next;
                        (k as TimeEvent).Destroy();                        
                        k.next = null;
                    }else
                    {
                        (k as TimeEvent).Destroy();
                        tmp.next = null;                        
                    }
                    return;
                }
                tmp = tmp.next;
            }
        }
    }
    void AddEvent(TimeEventBase _e)
    {
        for (int i = 0; i < mgr.Count; i++)
        {
            if (mgr[i]._Type == _e._Type)
            {
                TimeEventBase t = mgr[i];
                while (t.next!=null)
                {
                    t = t.next;
                }
                t.next = _e;
                return;
            }
        }
        mgr.Add(_e);
    }
#if UNITY_EDITOR
    bool test = false;
    DateTime dateTimeStart;
    private void OnGUI()
    {
        if (!test)
        {
            test = true;
            dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));            
        }
        GUI.color = Color.red;
        GUILayout.Label(dateTimeStart.AddSeconds(_Timestamp).ToLocalTime().ToString());
    }
#endif
    
    internal void AddEvent(Intervel_Time _inter, System.Action<int,float> callback)
    {
        int Max = mgr.Count;
        for (int i = 0; i < Max; i++)
        {
            if (mgr[i]._Type==_inter)
            {
                TimeEventBase t = mgr[i];
                while (t.next != null)
                {                    
                    t = t.next;
                }
                t.next = new TimeEventBase(callback, _inter);
                return;
            }
        }
        mgr.Add(new TimeEventBase(callback, _inter));
    }

    internal TimeEvent AddIntervelEvent(System.Action<int,float> callback, int _interTime,int mid=0,int _loop=-1,bool _CanGiveUp=false)
    {
        TimeEvent t;
        Intervel_Time _type = Intervel_Time.Intervalometer;
        int Max = mgr.Count;
        for (int i = 0; i < Max; i++)
        {
            t = (TimeEvent)mgr[i];
            if (t._Type==_type)
            {                
                while (t.next != null)
                {
                    t = (TimeEvent)t.next;
                }
                TimeEvent nt = new TimeEvent(callback, _type, _interTime, mid, _loop,can_give_up: _CanGiveUp);
                t.next = nt;
                t = nt;
                return t;
            }
        }
        t = new TimeEvent(callback, _type, _interTime, mid, _loop, can_give_up: _CanGiveUp);
        mgr.Add(t);
        return t;
    }

    //internal TimeEvent AddIntervelEvent(LuaFunction callback,LuaTable tab, int _interTime, int _loop = -1)
    //{
    //    TimeEvent t;
    //    int Max = mgr.Count;
    //    for (int i = 0; i < Max; i++)
    //    {
    //        t = (TimeEvent)mgr[i];
    //        if (t._Type == Intervel_Time.Intervalometer)
    //        {                
    //            while (t.next != null)
    //            {
    //                t = (TimeEvent)t.next;
    //            }
    //            TimeEvent nt = new TimeEvent((_int, _float) =>
    //            {                    
    //                LuaBehMgr.CallFunc(tab, callback);
    //            }, Intervel_Time.Intervalometer, _interTime, 0, _loop,true);
    //            t.next = nt;
    //            t = nt;
    //            return t;
    //        }            
    //    }
    //    t = new TimeEvent((_int, _float) =>
    //    {
    //        LuaBehMgr.CallFunc(tab, callback);            
    //    }, Intervel_Time.Intervalometer, _interTime, 0, _loop,true);
    //    mgr.Add(t);
    //    return t;
    //}

    internal void SetTimesTamp(double _time)
    {
        _STimestamp = _time;
        _PreTime = Time.realtimeSinceStartup * 1000;
    }
    public string GetTimeStamp()
    {
        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return Convert.ToInt64(ts.TotalMilliseconds/60000).ToString();
    }
    public Int64 GetMsTimeStamp()
    {
        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return Convert.ToInt64(ts.TotalMilliseconds);
    }
    TimeEvent  preEvent;
    TimeEventBase _Event;


    private void Update()
    {
        _CurTime = Time.realtimeSinceStartup * 1000;
        _MsTime = (int)_CurTime;
        _MsTimestamp = ((_CurTime - _PreTime) + _STimestamp);//时间戳              
        _Timestamp = (int)(_MsTimestamp * 0.001);
        int Max = mgr.Count;
        for (int i = 0; i < Max; i++)
        {
            _Event = mgr[i] as TimeEvent;            
            Intervel_Time _inter = _Event._Type;
            float update = _CurTime - ((TimeEvent)_Event)._LastTime;
            if (_inter == Intervel_Time.Intervalometer)
            {
                preEvent = _Event as TimeEvent;
                int _index = 0;
                do
                {
                    if (_index++ > 0)
                    {
                        preEvent = _Event as TimeEvent;
                        _Event = _Event.next;
                    }
                    update = _CurTime - ((TimeEvent)_Event)._LastTime;
                    var _Interval = ((TimeEvent)_Event)._Interval;
                    if (update >= _Interval)
                    {
                        ((TimeEvent)_Event)._LastTime += ((TimeEvent)_Event)._CanGiveUp ? update : _Interval;
                        if (_Event.back != null)
                        {
                            if (((TimeEvent)_Event).CanRun())
                            {
                                _Event.back(((TimeEvent)_Event)._SumIndex, 0);
                            }
                            else
                            {
                                if (preEvent == _Event)
                                {
                                    mgr[i] = _Event.next;
                                }else
                                {
                                    preEvent.next = _Event.next;
                                }
                                _Event.next = null;
                            }
                        }
                    }
                } while (_Event.next != null);
            }
            else
            {
                if (update >= ((TimeEvent)_Event)._Interval)
                {

                    if (_inter == Intervel_Time.Index)
                    {                        
                        int _double = (int)(update / _Interval);
                        ((TimeEvent)_Event)._SumIndex += _double;
                        int output = _double * _Interval;
                        float cha = update - output;
                        ((TimeEvent)_Event)._LastTime = (_CurTime - cha);
                        _SumIndex = ((TimeEvent)_Event)._SumIndex;
                        up = update * 0.001f;
                        if (_Event.back != null)
                        {
                            _Event.back(_SumIndex, up);
                        }
                        while (_Event.next != null)
                        {
                            _Event.next.back(_SumIndex, up);
                            _Event = _Event.next;
                        }
                    }
                    else if (_inter == Intervel_Time.Float)
                    {
                        ((TimeEvent)_Event)._LastTime = _CurTime;
                        up = update *0.001f;
                        if (_Event.back != null)
                        {
                            _Event.back(0, up);
                        }
                        while (_Event.next != null)
                        {
                            _Event.next.back(0, up);
                            _Event = _Event.next;
                        }
                    }

                }
            }
        }    
    }       
}
