using System;
using System.Collections.Generic;
using UnityEngine;
class CreateInstance : IObjectFactory<IPoolable>
{
    readonly Type t;
    public CreateInstance(IPoolsType _type)
    {
        t = Type.GetType(_type.ToString());
    }
    public IPoolable Create()
    {
        return Activator.CreateInstance(t, true) as IPoolable;
    }
    public IPoolable Create(IPoolsType _type)
    {
        return Activator.CreateInstance(Type.GetType(_type.ToString()), true) as IPoolable;
    }
}
public abstract class Pool : IPool
{
    protected int maxCount = 500;
    protected int allCreate = 0;
    public int AllCreate { get { return allCreate; } }
    public int CountActive { get { return AllCreate - mCacheStack.Count; } }
    public int CountInactive { get { return mCacheStack.Count; } }
    public int CurCount { get { return mCacheStack.Count; } }
    protected IObjectFactory<IPoolable> mFactory;
    protected Stack<IPoolable> mCacheStack = new Stack<IPoolable>();
    public virtual IPoolable Allocate(IPoolsType _type)
    {
        if (CurCount <= 0)
        {
            allCreate++;
            return mFactory.Create();
        }
        else
            return mCacheStack.Pop();
    }
    public abstract bool Recycle(IPoolable obj);
    public virtual void ClearAll()
    {
        while (mCacheStack.Count > 0)
        {
            var e = mCacheStack.Pop();
            if (e != null)
            {
                e.Dispose();
            }
        }
    }
}

public class ObjectPool : Pool
{
    public ObjectPool(IPoolsType _type)
    {
        mFactory = new CreateInstance(_type);
    }
    public override IPoolable Allocate(IPoolsType _type)
    {
        IPoolable result = base.Allocate(_type);
        result.IsRecycled = false;
        return result;
    }
   
    public override bool Recycle(IPoolable obj)
    {
        if (obj == null)
        {
            return false;
        }
        if (mCacheStack.Count>=maxCount)
        {
            allCreate--;
            obj.Dispose();
            return false;
        }
        if (!obj.IsRecycled)
        {
            obj.OnRecycled();
            mCacheStack.Push(obj);
        }
        obj.IsRecycled = true;
        return true;
    }
}