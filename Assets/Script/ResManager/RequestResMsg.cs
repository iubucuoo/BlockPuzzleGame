using UnityEngine;

public class RequestResMsg : MessageBase
{
	public override IPoolsType PoolType { get { return IPoolsType.RequestResMsg; } }
	public IArt _ArtUnit;

	public override void OnRecycled()
	{
		_ArtUnit = null;
	}
	RequestResMsg SetMsg(ushort _msgId, IArt art)
	{
        messageId = _msgId;
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
