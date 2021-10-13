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
    /// Lua网络消息ID
    /// </summary>
    LNetManager = ToolsParms.MessageSpan * 10,

    /// <summary>
    /// Lua单位消息ID
    /// </summary>
    LUnitManager = ToolsParms.MessageSpan * 20,

    /// <summary>
    /// Lua资源消息ID
    /// </summary>
    LAssetManager = ToolsParms.MessageSpan * 21,
    /// <summary>
    /// Lua声音消息ID
    /// </summary>
    LAudioManager = ToolsParms.MessageSpan * 22,
    /// <summary>
    /// Lua游戏流程逻辑消息ID
    /// </summary>
    LGameManager = ToolsParms.MessageSpan * 23,
    /// <summary>
    /// LuaUI界面消息ID
    /// </summary>
    LUIManager = ToolsParms.MessageSpan * 24,

    /// <summary>
    /// 游戏流程逻辑
    /// </summary>
    GameManager = ToolsParms.MessageSpan * 25,
    /// <summary>
    /// UI界面消息ID
    /// </summary>
    UIManager = ToolsParms.MessageSpan * 26,
    /// <summary>
    /// 单位消息ID
    /// </summary>
    UnitManager = ToolsParms.MessageSpan * 27,
    /// <summary>
    /// 资源消息ID
    /// </summary>
    AssetManager = ToolsParms.MessageSpan * 28,
    /// <summary>
    /// 声音消息ID
    /// </summary>
    AudioManager = ToolsParms.MessageSpan * 29,
    /// <summary>
    /// 本地请求网络消息ID
    /// </summary>
    LocalNetManager = ToolsParms.MessageSpan * 30,
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

