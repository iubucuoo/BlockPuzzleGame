using System;
using System.Collections.Generic;
using UnityEngine;
class CreateInstance : IObjectFactory<IPoolable>
{
    public IPoolable Create(IPoolsType _type)
    {
        return Activator.CreateInstance(Type.GetType(_type.ToString()), true) as IPoolable;
    }
}
public abstract class Pool : IPool
{
    public int CurCount { get { return mCacheStack.Count; } }
    protected IObjectFactory<IPoolable> mFactory;
    protected Stack<IPoolable> mCacheStack = new Stack<IPoolable>();
    public virtual IPoolable Allocate(IPoolsType _type)
    {
        if (CurCount <= 0)
            return mFactory.Create(_type);
        else
            return mCacheStack.Pop();
    }
    public abstract bool Recycle(IPoolable obj);
}

public class ObjectPool : Pool
{
    public ObjectPool()
    {
        mFactory = new CreateInstance();
    }
    public override IPoolable Allocate(IPoolsType _type)
    {
        IPoolable result = base.Allocate(_type);
        result.IsRecycled = false;
        return result;
    }

    public override bool Recycle(IPoolable obj)
    {
        if (obj == null || obj.IsRecycled)
        {
            return false;
        }
        obj.IsRecycled = true;
        obj.OnRecycled();
        mCacheStack.Push(obj);
        return true;
    }
}