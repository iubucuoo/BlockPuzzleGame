public class MsgStringInt : MessageBase
{
	public string _string1;
	public float _int1;
	public override IPoolsType PoolType => IPoolsType.MsgStringInt;

	MsgStringInt SetValue(ushort _msgId, string v1, int v2)
	{
		msgId = _msgId;
		_string1 = v1;
		_int1 = v2;
		return this;
	}

	static MsgStringInt Create()
	{
		return PoolMgr.Allocate(IPoolsType.MsgStringInt) as MsgStringInt;
	}
	public static void CreateToSend(int msgId, string v1, int v2)
	{
		MsgSend.GSendMsg(Create().SetValue((ushort)msgId, v1, v2));
	}
}
