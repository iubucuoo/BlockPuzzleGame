using UnityEngine;

public class EffectCtrl
{
	public EffectModel _Model;


	public void Init(  string PkgName, string ResName, float _Multiple)
	{
			var _PkgName = PkgName;
			var _ResName = ResName;
			if (string.IsNullOrEmpty(_PkgName) || string.IsNullOrEmpty(_ResName))
			{
				return;
			}

			var basedata = new EffectBaseData()
			{
				offsetx = 0,
				offsety = 0,
				offsetz = 0,
				_BoneType = BINDTYPE.NONE,
			};
			_Model = ModelPools.Pop(_PkgName, _ResName);
			if (_Model != null)
			{
				_Model.SetValue( ref basedata, _PkgName, _ResName, _Multiple);
			}
		
	}

	public void Init(string PkgName, string ResName, out bool _BindBone)
	{
		_BindBone = false;
		 
            //影响传承 技能的元素 就需要从父级传承下来			
            var _pkgName = PkgName;
			var _resName = ResName; 
			//如果查询出来的资源是null 那么就跳出，一般是找不到施法者，导致的。
			if (string.IsNullOrEmpty(_resName) || string.IsNullOrEmpty(_pkgName))
			{
				return;
			}
             

			var basedata = new EffectBaseData()
			{
				offsetx = 0,
				offsety = 0,
				offsetz = 0,
				_BoneType = BINDTYPE.NONE,
			};
			_Model = ModelPools.Pop(_pkgName, _resName);
			if (_Model != null)
			{
				_Model.SetValue(ref basedata, _pkgName, _resName, 1);
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
