/// <summary>
/// 各个消息模块ID范围 如UIManager 从1个MessageSpan开始到2个MessageSpan
/// </summary>
public enum ManagerID
{
    /// <summary>
    /// 网络消息ID
    /// </summary>
    NetManager = 0,
    /// <summary>
    /// 资源消息ID
    /// </summary>
    AssetManager = ToolsParms.MessageSpan * 10,
    /// <summary>
    /// 游戏流程逻辑
    /// </summary>
    GameManager = ToolsParms.MessageSpan * 11,
    /// <summary>
    /// UI界面消息ID
    /// </summary>
    UIManager = ToolsParms.MessageSpan * 12,
    /// <summary>
    /// 单位消息ID
    /// </summary>
    UnitManager = ToolsParms.MessageSpan * 13,
    /// <summary>
    /// 声音消息ID
    /// </summary>
    AudioManager = ToolsParms.MessageSpan * 14,
    /// <summary>
    /// 本地请求网络消息ID
    /// </summary>
    LocalNetManager = ToolsParms.MessageSpan * 15,

    
    /// <summary>
    /// LuaUI界面消息ID
    /// </summary>
    LUIManager = ToolsParms.MessageSpan * 16,

    ///// <summary>
    ///// Lua网络消息ID
    ///// </summary>
    //LNetManager = ToolsParms.MessageSpan * 17,

    ///// <summary>
    ///// Lua单位消息ID
    ///// </summary>
    //LUnitManager = ToolsParms.MessageSpan * 18,

    ///// <summary>
    ///// Lua资源消息ID
    ///// </summary>
    //LAssetManager = ToolsParms.MessageSpan * 19,

    ///// <summary>
    ///// Lua声音消息ID
    ///// </summary>
    //LAudioManager = ToolsParms.MessageSpan * 20,

    ///// <summary>
    ///// Lua游戏流程逻辑消息ID
    ///// </summary>
    //LGameManager = ToolsParms.MessageSpan * 21,

}
/// <summary>
/// 消息发送状态
/// </summary>
public enum MessageStatus
{
    /// <summary>
    /// 成功
    /// </summary>
    Success,
    /// <summary>
    /// 失败
    /// </summary>
    Error,
    /// <summary>
    /// 未知
    /// </summary>
    Unknown,
    /// <summary>
    /// 处理
    /// </summary>
    Dealing,
}

