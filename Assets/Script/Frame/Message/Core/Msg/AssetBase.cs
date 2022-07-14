
using UnityEngine;

class AssetBase : IEventListen
{
	public ushort[] messageIds { get; set; }

    public void InitEventListen()
    {
        
    }

    public virtual void ProcessEvent(MessageBase tmpMsg)
	{
        AssetManager.instance.ProcessEvent(tmpMsg);
		
	}
	public bool RegistEventListen(IEventListen mono, params ushort[] msgids)
	{
       return AssetManager.instance.RegistEventListen(mono, msgids);
	}
	public void SendMsg(MessageBase tmpMsg)
	{
        AssetManager.instance.SendMsg(tmpMsg);
	}
	public bool UnRegistEventListen(IEventListen mono, params ushort[] msgids)
	{
        return AssetManager.instance.UnRegistEventListen(mono, msgids);
	}
}
