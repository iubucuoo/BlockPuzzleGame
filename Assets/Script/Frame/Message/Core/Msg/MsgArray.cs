//数组
public class MsgArray : MessageBase
{
	public object[] objs { get; private set; }

	public override IPoolsType PoolType { get { return IPoolsType.MsgArray; } }

	public MsgArray()
	{

	}

	MsgArray SetValue(ushort _msgId, object[] _objs = null)
	{
		msgId = _msgId;
		objs = _objs;
		return this;
	}

	public override void OnRecycled()
	{
		msgId = 0;
		objs = null;
	}
	static MsgArray Create()
	{
		return PoolMgr.Allocate(IPoolsType.MsgArray) as MsgArray;
	}
	public static void CreateToSend(int msgId, object[] _objs = null)
	{
		MsgSend.GSendMsg(Create().SetValue((ushort)msgId, _objs));
	}
}
