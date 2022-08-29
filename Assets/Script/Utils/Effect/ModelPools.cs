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
sealed public class ModelPools:MonoSingleton<ModelPools>
{
	Dictionary<string, ModelGroup> data = new Dictionary<string, ModelGroup>();
	private static Transform _pool;
	public static Transform pool
	{
		get
		{
			if (Inst != null)
				return Inst.transform;
			return null;
		}
	}
	public static int _SingleMaxNum = 300;
	 
	public static void Push(EffectModel model)
	{
        if (!Inst.data.TryGetValue(model._PkgName, out ModelGroup group))
        {
            group = new ModelGroup();
            Inst.data.Add(model._PkgName, group);
        }
        if (!group.data.TryGetValue(model._ResName, out ModelSingle queue))
        {
            queue = new ModelSingle();
            group.data.Add(model._ResName, queue);
        }
        queue.Enqueue(model);
		model.Parent = pool;

	}
	public static EffectModel Pop(string pkgName, string resName)
	{
        if (!Inst.data.TryGetValue(pkgName, out ModelGroup group))
        {
            group = new ModelGroup();
            Inst.data.Add(pkgName, group);
        }
        if (!group.data.TryGetValue(resName, out ModelSingle queue))
        {
            queue = new ModelSingle();
            group.data.Add(resName, queue);
        }
        return queue.Dequeue();
	}
}
