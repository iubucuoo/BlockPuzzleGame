using UnityEngine;
using System.Collections;


/// <summary>
/// 网络包协议 ServerMsgCodes，ClientMsgCodes
/// </summary>
class NetManager : ManagerBase
{
    NetManager(ManagerID id) : base(id)
    {

    }
    private static NetManager inst;

	public static NetManager instance
    {
		get
		{
			if (inst == null)
			{
				inst = new NetManager(ManagerID.NetManager);
			}
			return inst;
		}
	}
	 
	public void Reset()
	{
		if (inst != null)
			inst = null;
	}
}

/// <summary>
/// 网络状态 TCP_STATUS
/// </summary>
class LocalNetManager : ManagerBase
{
    LocalNetManager(ManagerID id) : base(id)
    {

    }
    private static LocalNetManager inst;

	public static LocalNetManager instance
    {
		get
		{
			if (inst == null)
			{
				inst = new LocalNetManager(ManagerID.LocalNetManager);
			}
			return inst;
		}
	}
	public void Reset()
	{
		if (inst != null)
			inst = null;
	}

}
