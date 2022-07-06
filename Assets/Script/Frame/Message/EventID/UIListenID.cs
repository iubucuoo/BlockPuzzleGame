
public enum UIMainListenID : ushort
{
    Min = ManagerID.UIManager + 1,
    AdAndRefreshGame,
    Max,
}

public enum UITopPanelListenID : ushort
{
    Min = UIMainListenID.Max + 1,
    Up,
    WriteTopScore,
    ResetTop,
    SetNowScore,
    Max,
}
public enum UIGroupRotateListenID : ushort
{
    Min = UITopPanelListenID.Max + 1,
    Up,
    AddRotateGold,
    SwOne,
    HideOne,
    OnRotate,
    OffRotate,
    Max,
}
public enum UIAddRotateListenID : ushort
{
    Min = UIGroupRotateListenID.Max+1,
    SwPanel,
    Max,

}
public enum UISwTextEffectListenID : ushort
{
    Min = UIAddRotateListenID.Max + 1,
    SwEffect,
    Max,
}
public enum CaneraShakeListenID : ushort
{
    Min = UISwTextEffectListenID.Max+1,
    Shake,
    Max,
}

public enum SetPanelListenID : ushort
{
    Min = CaneraShakeListenID.Max + 1,
    Open,
    Hide,
    Close,
    Max,
}
public enum StartPanelListenID : ushort
{
    Min = SetPanelListenID.Max + 1,
    Open,
    Hide,
    Close,
    Max,
}
public enum GamePanelListenID : ushort
{
    Min = StartPanelListenID.Max + 1,
    Close,
    Test1,
    Test2,
    Test3,
    Test4,
    Max,
}
public enum GameOverPanelListenID : ushort
{
    Min = GamePanelListenID.Max + 1,
    Close,
    Test1,
    Test2,
    Test3,
    Test4,
    Max,
}