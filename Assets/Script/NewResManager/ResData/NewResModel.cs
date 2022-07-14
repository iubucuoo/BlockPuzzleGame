using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[ProtoBuf.ProtoContract]
public class NewResModel
{
	public string _ModelName;
	[ProtoBuf.ProtoMember(1)]
	public int _ModelID;
	[ProtoBuf.ProtoMember(2)]
	public NewResAb[] _Data;

	public int _Count { get { return _Data == null ? 0 : _Data.Length; } }
	public NewResAb this[int index] { get { return _Data == null ? null : _Data[index]; } set { _Data[index] = value; } }
	public NewResModel()
	{

	}
	public NewResModel(int id, string Name)
	{
		_ModelID = id;
		_ModelName = Name.ToLower();
	}
	internal void PushUnit(NewResUnit unit)
	{
		NewResAb temp;
		for (int i = 0; i < _Count; i++)
		{
			temp = this[i];
			if (temp._AbName == unit._AbName)
			{
				temp.PushUnit(unit);
				return;
			}
		}
		var _TempDatas = new NewResAb[_Count + 1];
		for (int i = 0; i < _Count; i++)
		{
			_TempDatas[i] = this[i];
		}
		temp = new NewResAb(unit._AbName);
		_TempDatas[_Count] = temp;
		temp.PushUnit(unit);
		_Data = _TempDatas;
	}


	internal void CopyAbParams(NewResModel newResModelUnit)
	{
		for (int i = 0; i < _Count; i++)
		{
			var temp = this[i];
			for (int j = 0; j < newResModelUnit._Count; j++)
			{
				var old = newResModelUnit[j];
				if (temp._AbName == old._AbName)
				{
					temp.CopyAbParams(old);
					break;
				}
			}
		}
	}

	internal bool GetObj(string artName, out NewResUnit unit)
	{
		unit = null;
		for (int i = 0; i < _Count; i++)
		{
			var temp = this[i];
			if (temp.GetObj(artName, out unit))
			{
				return true;
			}
		}
		return false;
	}

	internal bool GetABForAbID(int abID, out NewResAb ab)
	{
		ab = null;
		for (int i = 0; i < _Count; i++)
		{
			if (this[i]._ID == abID)
			{
				ab = this[i];
				return true;
			}
		}
		return false;
	}




	internal bool GetObj(IArt art)
	{
		for (int i = 0; i < _Count; i++)
		{
			var temp = this[i];
			if (temp._AbName == art.AbSingleName())
			{
				temp.GetObj(art);
					return true;
			}
			else if (art.AbSingleName() == null)
			{
				if (temp.GetObj(art))
				{
					return true;
				}
			}
		}
		return false;
	}
	internal string ToDebug()
	{
		StringBuilder sb = new StringBuilder();
		for (int i = 0; i < _Count; i++)
		{
			sb.Append(this[i].ToDebug());
		}
		sb.Append("\n");
		return sb.ToString();
	}

	internal bool FindResUnit(string path, out NewResUnit temp)
	{
		temp = null;
		for (int i = 0; i < _Count; i++)
		{
			if (this[i].FindResUnit(path, out temp))
			{
				return true;
			}
		}
		return false;
	}
	internal bool GetAbForDownloadID(int downloadId, ref int _SortID, out NewResAb res)
	{
		res = null;
		while (_SortID < _Count)
		{
			var temp = this[_SortID];
			_SortID++;
			if (temp._DownloadID == downloadId)
			{
				res = temp;
				return true;
			}
		}
		return false;
	}
	internal bool GetAb(int abId, out NewResAb res)
	{
		res = null;
		for (int i = 0; i < _Count; i++)
		{
			var temp = this[i];
			if (temp._ID == abId)
			{
				res = temp;
				return true;
			}
		}
		return false;
	}
	internal bool GetAb(string abName, out NewResAb res)
	{
		res = null;
		for (int i = 0; i < _Count; i++)
		{
			var temp = this[i];
			if (temp._AbName == abName)
			{
				res = temp;
				return true;
			}
		}
		return false;
	}
	internal void UnloadAbAndObj()
	{
		for (int i = 0; i < _Count; i++)
		{
			var temp = this[i];
			temp.UnloadAbAndObj();
		}
	}
}
