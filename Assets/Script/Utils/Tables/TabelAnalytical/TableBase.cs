using System.Collections.Generic;
using System;

public abstract class TableBase
{
    public abstract int Key();
    public virtual long KeyLong()
    {
        return 0;
    }

}
public abstract class TableBaseManager
{    
    public virtual bool Open
    {
        get
        {
            return false;
        }
    }
    public abstract string GetTableName();
    protected abstract Type[] GetTableType();
    internal TableBase[] array;
    protected virtual bool isLong { get { return false; } }
    protected Dictionary<long, int> _mKeyItemMapLong;
    protected Dictionary<int, int> _mKeyItemMapInt;
    /// <summary>
    /// 获得所有的值
    /// </summary>
    /// <param name="tableStructs"></param>
    /// <returns></returns>
    protected virtual TableBase[] _GetData()
    {
        if (!Open)
        {
            return null;
        }
        if (IsFilter())
        {
            array = (TableBase[])TabelLoadData.ProcessTable(GetTableType(), GetTableName());
            SetDataMap();
        }
        return array;
    }
    protected virtual void SetDataMap()
    {
        if (!IsFilter())
        {
            if (isLong)
            {
                _mKeyItemMapLong = new Dictionary<long, int>(array.Length);
                for (int i = 0; i < array.Length; i++)
                    _mKeyItemMapLong[array[i].KeyLong()] = i;
            }
            else
            {
                _mKeyItemMapInt = new Dictionary<int, int>(array.Length);
                for (int i = 0; i < array.Length; i++)
                    _mKeyItemMapInt[array[i].Key()] = i;
            }
        }
    }
    protected virtual object _GetSingleData(long key)
    {
        if (!Open)
        {
            return null;
        }
        if (IsFilter())
        {
            _GetData();
        }
        int tmp = 0;
        if (_mKeyItemMapLong.TryGetValue(key, out tmp))
        {
            return array[tmp];
        }
        else
        {
            return null;
        }
    }
    protected virtual object _GetSingleData(int key)
    {
        if (!Open)
        {
            return null;
        }
        if (IsFilter())
        {
            _GetData();
        }
        int tmp=0;
        if (_mKeyItemMapInt.TryGetValue(key, out tmp))
        {
            return array[tmp];
        }
        else
        {
            return null;
        }
    }
   
    internal void _Clear()
    {
        array = null;
        if (isLong)
        {
            if (_mKeyItemMapLong != null)
            {
                _mKeyItemMapLong.Clear();
            }
            _mKeyItemMapLong = null;
        }
        else
        {
            if (_mKeyItemMapInt != null)
            {
                _mKeyItemMapInt.Clear();
            }
            _mKeyItemMapInt = null;
        }
    }
    #region 表过滤
    protected bool IsFilter()
    {
        if (array == null)
            return true;
        else
            return false;
    }
    #endregion
    public void ThisClear()
    {
        _Clear();
    }
}