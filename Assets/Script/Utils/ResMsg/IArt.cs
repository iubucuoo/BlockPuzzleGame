using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MEC;

public interface IArt
{
    int _MapID { get; set; }
    int _ModelID { get; }
    ResSort _Sort { get; }
    NewResAb GetNewResAb { get; }
    bool UseArt(object obj);

    void UseArt(object[] objs);
    string ArtName();
    string AbSingleName();
    int AbSingleID();
    IEnumerator<float> Loading(AssetBundle ab);
    void Destroy();
    bool ComportRes(string abName, string artName);
    bool IsWaitArt(int key = 0);
    bool _CanCacheObj { get; }
    bool _CanCacheAb { get; }
}

public class ArtBase : IArt
{
    public virtual int _ModelID => throw new System.NotImplementedException();

    public virtual ResSort _Sort => throw new System.NotImplementedException();

    public virtual NewResAb GetNewResAb { get { ResCenter.inst._ResMgr.GetAB(_ModelID, AbSingleName(), out NewResAb ab); return ab; } }

    public virtual bool _CanCacheObj => true;
    public virtual bool _CanCacheAb => true;

    public virtual bool _CallBackWhenFailed { get; set; } = false;

    public int _MapID { get; set; }

    public virtual int AbSingleID()
    {
        throw new System.NotImplementedException();
    }

    public virtual string AbSingleName()
    {
        return null;
    }

    public virtual string ArtName()
    {
        return null;
    }

    public virtual bool ComportRes(string abName, string artName)
    {
        throw new System.NotImplementedException();
    }

    public virtual void Destroy()
    {
        throw new System.NotImplementedException();
    }

    public virtual bool IsWaitArt(int key = 0)
    {
        return true;
    }

    public virtual IEnumerator<float> Loading(AssetBundle ab)
    {
        if (!string.IsNullOrEmpty(ArtName()))
        {
            var temp = ab.LoadAssetAsync(ArtName());
            yield return Timing.WaitUntilDone(temp);
            if (IsWaitArt())
            {
                UseArt(temp.asset);
            }
        }
    }
    public virtual void UseArt(object obj)
    {
        throw new System.NotImplementedException();
    }

    public virtual void UseArt(object[] objs)
    {
        throw new System.NotImplementedException();
    }

    public virtual void FailArt()
    {
        throw new System.NotImplementedException();
    }

    bool IArt.UseArt(object obj)
    {
        throw new System.NotImplementedException();
    }
}

public class DependArt : ArtBase
{
    public DependArt(NewResAb ab)
    {
        _Ab = ab;
    }
    NewResAb _Ab;

    public override NewResAb GetNewResAb { get { return _Ab; } }

}