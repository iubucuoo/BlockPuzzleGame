using UnityEngine;

public class EffectCtrl
{
	public EffectModel _Model;


	public void Init(string PkgName, string ResName, EffectBaseData basedata, float _Multiple=1)
	{
		var _PkgName = PkgName;
		var _ResName = ResName;
		if (string.IsNullOrEmpty(_PkgName) || string.IsNullOrEmpty(_ResName))
		{
			return;
		}
		_Model = ModelPools.Pop(_PkgName, _ResName);
		if (_Model != null)
		{
			_Model.SetValue( _PkgName, _ResName, basedata, _Multiple);
		}
	}

	internal void SetPosition(float posY)
	{
		if (_Model != null)
		{
			_Model.position = new Vector3(_Model.position.x, posY, _Model.position.z);
		}
	}

	internal void SetScale(float _Scale)
	{
		if (_Model != null)
		{
			_Model.localScale = new Vector3(_Scale,1, _Scale);//v3缩放
		}
	}
	internal void Destroy()
	{
		if (_Model != null)
		{
			_Model.Destroy();
			_Model = null;
		}
	}
}
