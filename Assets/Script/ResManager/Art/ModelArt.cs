using MEC;
using System.Collections.Generic;
using UnityEngine;
namespace ModelSys
{
	public class ModelArt : ArtBase
	{
		public GameObject self { get; protected set; }
		protected string _ArtName;
		protected int part_index;

		public virtual void SetValue(string artName, int index)
		{
			_ArtName = artName;
			part_index = index;
			if (string.IsNullOrEmpty(_ArtName))
			{
				Destroy();//卸载
			}
			else
			{
				MsgSend.GetRes(this);
			}
		}

        public override int _ModelID => (int)RES_MODEL_INDEX.uiwnds;

		public override ResSort _Sort
		{
			get
			{
				return ResSort.UI;
			}
		}
		public ResUnit GetNewResUnit { get { ResCenter.inst._ResMgr.GetObj(_ModelID, ArtName(), out ResUnit unit); return unit; } }
		public override ResAb GetNewResAb
		{
			get
			{
				var unit = GetNewResUnit;
				if (unit != null)
				{
					ResCenter.inst._ResMgr.GetAB(_ModelID, unit._AbName, out ResAb ab);
					return ab;
				}
				else
				{
					return null;
				}
			}
		}


		public override string AbSingleName() { return null; }

		public override string ArtName() { return _ArtName; }


		public override IEnumerator<float> Loading(AssetBundle ab)
		{
			var unit = GetNewResUnit;
			var objs = ab.LoadAssetAsync(ArtName());
			yield return Timing.WaitUntilDone(objs);
			var result = objs.asset;
			unit.SetObj(result);
			UseArt(result);
		}

		public override void UseArt(object obj)
		{
			RemoveChild();
			self = (ObjectMgr.InstantiateObj((Object)obj) as GameObject);
			//_Unit.LoadPart(this, part_index);
		}

		public override void FailArt()
		{

		}

		protected virtual void RemoveChild()
		{
			if (/*(_Unit is UnitDrop) == false && */self != null)
			{
				Object.DestroyImmediate(self);
				self = null;
			}
		}
		public override void Destroy()
		{
			RemoveChild();
			_ArtName = null;
		}

		public override bool ComportRes(string abName, string artName)
		{
			return artName == ArtName();
		}
		public override bool IsWaitArt(int key = 0)
		{
			return true;
		}
	}
}
