using UnityEngine;

public class RequestResMsg : MessageBase
{
	public override IPoolsType PoolType { get { return IPoolsType.RequestResMsg; } }
	public IArt _ArtUnit;

	public override void OnRecycled()
	{
		_ArtUnit = null;
		msgId = 0;
	}
	RequestResMsg SetMsg(ushort _msgId, IArt art)
	{
		msgId = _msgId;
		_ArtUnit = art;
		return this;
	}
	static RequestResMsg Create()
	{
		return PoolMgr.Allocate(IPoolsType.RequestResMsg) as RequestResMsg;
	}

	public static void CreateToSend(ushort MsgId, IArt _art)
	{
		MsgSend.GSendMsg(Create().SetMsg(MsgId, _art));
	}
}
