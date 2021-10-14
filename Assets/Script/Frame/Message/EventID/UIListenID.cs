
public enum UIMainListenID : ushort
{
    Min = ManagerID.UIManager + 1,
    SwPanel_Set,
    SwPanel_GameOver,
    SwPanel_AddRotate,
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