/// <(ushort)summary>
/// 消息类型的基类
/// <(ushort)/summary>
public class MessageBase:IPoolable
{
    /// <(ushort)summary>
    /// 消息ID
    /// <(ushort)/summary>
    public ushort messageId;

   
    internal MessageBase()
    {
        messageId = 0;
    }
    internal MessageBase(ushort _messageId)
    {
        messageId = _messageId;
    }
    public MessageBase SetValue(ushort _messageId)
    {
        messageId = _messageId;
        return this;
    }
    /// <(ushort)summary>
    /// 获得ManagerID
    /// <(ushort)/summary>
    /// <(ushort)returns><(ushort)/returns>
    internal ManagerID GetManagerID()
    {
        if (messageId < (ushort)ManagerID.LNetManager)
            return ManagerID.NetManager;
        else if (messageId < (ushort)ManagerID.LUnitManager)
            return ManagerID.LNetManager;
        else if (messageId < (ushort)ManagerID.LAssetManager)
            return ManagerID.LUnitManager;
        else if (messageId < (ushort)ManagerID.LAudioManager)
            return ManagerID.LAssetManager;
        else if (messageId < (ushort)ManagerID.LGameManager)
            return ManagerID.LAudioManager;
        else if (messageId < (ushort)ManagerID.LUIManager)
            return ManagerID.LGameManager;
        else if (messageId < (ushort)ManagerID.GameManager)
            return ManagerID.LUIManager;
        else if (messageId < (ushort)ManagerID.UIManager)
            return ManagerID.GameManager;
        else if (messageId < (ushort)ManagerID.UnitManager)
            return ManagerID.UIManager;
        else if (messageId < (ushort)ManagerID.AssetManager)
            return ManagerID.UnitManager;
        else if (messageId < (ushort)ManagerID.AudioManager)
            return ManagerID.AssetManager;
        else if (messageId < (ushort)ManagerID.LocalNetManager)
            return ManagerID.AudioManager;
        else
        {
            return ManagerID.LocalNetManager;
        }
    }
    public virtual IPoolsType PoolType => IPoolsType.MessageBase;

    public bool IsRecycled { get ; set ; }
    public virtual void OnRecycled()
    {
        messageId = 0;
    }
    public virtual void Dispose() { }
}