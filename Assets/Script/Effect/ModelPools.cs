using System.Collections.Generic;
using UnityEngine;
sealed class ModelSingle
{
	public int _Num;
	public Queue<EffectModel> data = new Queue<EffectModel>();

	public void Enqueue(EffectModel model)
	{
		if (!data.Contains(model))
		{
			data.Enqueue(model);
			_Num--;
		}
	}
	public EffectModel Dequeue()
	{
		if (_Num > ModelPools._SingleMaxNum)
		{
			return null;
		}
		_Num++;
		if (data.Count > 0)
		{
			return data.Dequeue();
		}
		return new EffectModel();
	}
}
sealed class ModelGroup
{
	public Dictionary<string, ModelSingle> data = new Dictionary<string, ModelSingle>();
}
sealed public class ModelPools
{
	Dictionary<string, ModelGroup> data = new Dictionary<string, ModelGroup>();
	private static Transform _pool;
	public static Transform pool
	{
		get
		{
			if (inst != null)
				return _pool;
			return null;
		}
	}
	public static int _SingleMaxNum = 300;
	static ModelPools _inst;
	static ModelPools inst
	{
		get
		{
			if (_inst == null)
			{
				_pool = new GameObject("ModelPools").transform;
				Object.DontDestroyOnLoad(_pool);
				_inst = new ModelPools();
			}
			return _inst;
		}
	}
	public static void Push(EffectModel model)
	{
		ModelGroup group;
		if (!inst.data.TryGetValue(model._PkgName, out group))
		{
			group = new ModelGroup();
			inst.data.Add(model._PkgName, group);
		}
		ModelSingle queue;
		if (!group.data.TryGetValue(model._ResName, out queue))
		{
			queue = new ModelSingle();
			group.data.Add(model._ResName, queue);
		}
		queue.Enqueue(model);
		model.Parent = pool;

	}
	public static EffectModel Pop(string pkgName, string resName)
	{
		ModelGroup group;
		if (!inst.data.TryGetValue(pkgName, out group))
		{
			group = new ModelGroup();
			inst.data.Add(pkgName, group);
		}
		ModelSingle queue;
		if (!group.data.TryGetValue(resName, out queue))
		{
			queue = new ModelSingle();
			group.data.Add(resName, queue);
		}
		return queue.Dequeue();
	}

	public static EffectModel Pop(string pkgName, string resName,   Vector3 diff)
	{
		var _PkgName = pkgName;
		var _ResName = resName;

 

		if (string.IsNullOrEmpty(_PkgName) || string.IsNullOrEmpty(_ResName))
		{
			return null;
		}
		return Pop(_PkgName, _ResName).SetValue(_PkgName, _ResName, diff);
	}

 
}
