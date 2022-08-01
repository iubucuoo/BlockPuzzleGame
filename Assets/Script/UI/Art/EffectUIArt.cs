using MEC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectUIArt : ArtBase
{
	public override int _ModelID => (int)RES_MODEL_INDEX.effects;

	public override ResSort _Sort => ResSort.UI;

	public NewResUnit GetNewResUnit { get { ResCenter.inst._ResMgr.GetObj(_ModelID, ArtName(), out NewResUnit unit); return unit; } }
	public override NewResAb GetNewResAb
	{
		get
		{
			var unit = GetNewResUnit;
			if (unit != null)
			{
				ResCenter.inst._ResMgr.GetAB(_ModelID, unit._AbName, out NewResAb ab);
				return ab;
			}
			else
			{
				return null;
			}
		}
	}

	
	string _ResName;
	public void SetValue( string _res)
	{
		_ResName = _res;
		MsgSend.GetRes(this);
	}
	public override string ArtName()
	{
		return _ResName;
	}

	public override bool IsWaitArt(int key = 0)
	{
		return true;
	}

	public override void UseArt(object obj)
	{
		var eff = (ObjectMgr.InstantiateObj((Object)obj) as GameObject);
		eff.transform.localPosition = Vector3.zero;
		eff.transform.localRotation = Quaternion.Euler(Vector3.zero);
	}

	public override void FailArt()
	{

	}
}
