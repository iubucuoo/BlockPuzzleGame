using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum IPoolsType
{
    UnitPlayer = 1,
    UnitMonster,
    UnitTrap = 5,
    UnitTransfer,
    UnitDrop,
    UnitNPC,
    UnitShadow,
    UnitZoology,
    UnitSaiyan,


    GridGroup,
    GridGroup_Ground,
    GridGroup_MinPrep,
    GridGroup_Prep,
    GridData,
    GridDataMin,
    GridDataDef,
    GridDataPrep,
    UIBase,
    UI_StartPanel,
    UI_AddRotatePanel,
    UI_GameOverPanel,
    UI_GamePanel,
    UI_SetPanel,


    Message,
    MessageBase,

    CameraHandle = 2000,
    CatHandle,
    MoveDistanceHandle,
    AddUnitHandle,
    RemoveMyselfHandle,
    RandomAddUnitHandle,
    PlayEffectHandle,
    ListTargetsHandle,
    BreakHandle,
    LinkHandle,
    NetHandle,
    NetStopSkillHandle,
    NetAtkedHandle,
    NetRotateHandle,
    ChangeSkinColorHandle,
    ChangeAtkBoxHandle,
    FaceTargetsHandle,
    AccelerationHandle,
    RevolvingHandle,
    SetVelocityHandle,
    RockerCtrlHandle,
    SummonHandle,
    BlinkHandle,
    SetGravityHandle,
    SkinHideHandle,
    SkinResumeHandle,
    ReviseYHandle,
    SpeakHandle,
    RockerReceiverHandle,
    LockTargetHandle,
    ACTHandle,
    ResetAttackHandle,
    JumpHandle,
    RandomPosHandle,
    RimEffectHandle,
    CtrlRotateHandle,
    CtrlMoveHandle,
    CtrlActiveHandle,
    CtrlActiveHeadUIHandle,

    AttackHandle = 3000,//攻击判定
    LinkAtkHandle,
    IdleHandle,


    NetMsg = 10000,

    MsgArray,
    MsgInt,
    MsgFloat2,
    MsgStringInt,
    TcpSendMsg,
    HunkResMsg,
    TcpConnectMsg,
    DownLoadItem,
    MissilePro,
    SkillPro,
    RequestResMsg,



    DropGroup,// 一个人掉落组
    PickGroup,// 一个拾取掉落组
    PickData, // 拾取对象封装
    DropEffectGroup, // 特效
    PickJumpComponent, //跳字
    TitleData,
    LinePools,
    PointPools,



    BuffData,
    HUDUnit_S,
    #region 跳字
    HUDUnit,
    HUDBehaviorBase,
    CommonBehavior,
    ZhiyuBehavior,
    ShanBIBehavior,
    GeDangBehavior,
    Buff_DotBehavior,
    BJBehavior,
    #endregion
    Action_Lens,
    Living,
    HitBuffUnit,
    Max,
    HeadUI_Base,
    HeadUI_None,
    HeadUI_Player,
    HeadUI_Drop,
    HeadUI_LocalPlayer,
    HeadUI_NPC,
    HeadUI_Monster,
    HeadUI_Transfer,
    HeadUI_CoolUnit,
    HeadUI_Trap,
    HeadUI_MAX = 12000,
    CoolLink,
    MissileContext,
    SkillContext,

    LodData = 20000,
    VertexCache,
    MaterialBlock,
    HandleEventPkg,
    DataLink,
    EchoCmd,
    MissileDataLink,
    BuffEventHandle,
    AtkUnit,
    HUDUnitGroup,

    ModelBase,
    ModelDrop,
    ModelPlayer,
    CoolLinkGroup,
    CommonArt,
    LinkEventData,
    MapDropItemGroup,
    MapDropIteminfo,
    SkillEntityGroup,
    SkillEntityList,
    SkillEntityBlock,
    NetSkillParams,
    ForceMoveData,
    NET_SKILL_MOVE,
}
public interface IPoolable
{
    IPoolsType PoolType { get; }
    void OnRecycled();//重置
    void Dispose();//删除
    bool IsRecycled { get; set; }

}
public interface IPool
{
    IPoolable Allocate(IPoolsType _type);//分配
    bool Recycle(IPoolable obj);//回收
}
public interface IObjectFactory<T>
{
    T Create();
}
