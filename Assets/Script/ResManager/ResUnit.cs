using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[ProtoBuf.ProtoContract]
public class ResUnit
{
	[ProtoBuf.ProtoMember(1)]
	public int _ID;
	[ProtoBuf.ProtoMember(2)]
	public string _AbName;
	[ProtoBuf.ProtoMember(3)]
	public string _ObjName;
	[ProtoBuf.ProtoMember(4)]
	public int _ModelID;
	[ProtoBuf.ProtoMember(5)]
	public string _ModelName;
	[ProtoBuf.ProtoMember(6)]
	public string _Path;    //#运行中无用 打资源时用

	public Object _Obj;
	public int _LoadTime;
	public string ToDebug()
	{
		return string.Format("ID={0},ObjName={1},AbName={2},ModelID={3},ModelName={4},Path={5}", _ID, _ObjName, _AbName, _ModelID, _ModelName, _Path);
	}
	internal bool FindResUnit(string path)
	{
		return _Path == path;
	}
	public void SetObj(Object _obj)
	{
		_Obj = _obj;
		_LoadTime = (int)(Time.time * 1000);
	}
}
