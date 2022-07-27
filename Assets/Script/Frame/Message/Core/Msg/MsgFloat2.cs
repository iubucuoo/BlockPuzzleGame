public class MsgFloat2 : MessageBase
{
	public float _f1;
	public float _f2;
	public float x { get { return _f1; } }
	public float y { get { return _f2; } }
	public override IPoolsType PoolType => IPoolsType.MsgFloat2;

	MsgFloat2 SetValue(ushort _msgId, float v1, float v2)
	{
        messageId = _msgId;
		_f1 = v1;
		_f2 = v2;
		return this;
	}

	static MsgFloat2 Create()
	{
		return PoolMgr.Allocate(IPoolsType.MsgFloat2) as MsgFloat2;
	}
	public static void CreateToSend(int msgId, float v1, float v2)
	{
		MsgSend.GSendMsg(Create().SetValue((ushort)msgId, v1, v2));
	}

}
