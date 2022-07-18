//int
public class MsgInt : MessageBase
{
	public int _Int_V;
	public override IPoolsType PoolType => IPoolsType.MsgInt;

	MsgInt SetValue(ushort _msgId, int v)
	{
		msgId = _msgId;
		_Int_V = v;
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
