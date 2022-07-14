//int
public class MsgInt : MessageBase
{
	public int _IntValue;
	public override IPoolsType PoolType => IPoolsType.MsgInt;

	MsgInt SetValue(ushort _msgId, int v)
	{
		msgId = _msgId;
		_IntValue = v;
		return this;
	}
	static MsgInt Create()
	{
		return PoolMgr.Allocate(IPoolsType.MsgInt) as MsgInt;
	}
	public static void CreateToSend(int msgId, int v)
	{
		MsgSend.GSendMsg(Create().SetValue((ushort)msgId, v));
	}

}
