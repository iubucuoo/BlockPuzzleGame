using MEC;
using System.Collections.Generic;
using UnityEngine;


public enum ResSort
{
	Config,
	UI,
	LocalData,
	Model,
	Other,
}

public class NewResLoop
{
	Stack<IArt>[] _CurrentData;
	Queue<IArt>[] _Echelon = new Queue<IArt>[5]
	{
		new Queue<IArt>(),//表/配置
		new Queue<IArt>(),//UI最要紧
		new Queue<IArt>(),//主角模型/数据
		new Queue<IArt>(),//模型
		new Queue<IArt>(),//其他
	};
	int MaxLine = 3;
	public NewResLoop()
	{
		_CurrentData = new Stack<IArt>[MaxLine];
		for (int i = 0; i < MaxLine; i++)
		{
			_CurrentData[i] = new Stack<IArt>();
			Timing.RunCoroutine(UpdateLoad(i));
		}
	}

	public void BeginLoadData(IArt _art)
	{
		try
		{
			//if (_art.ArtName().ToLower().Contains("loginwnd"))
			//{
			//	//DebugMgr.LogError("BeginLoadData {0} {1} ", _art.ArtName(), _art.AbSingleName());
			//}
			var abdata = _art.GetNewResAb;
			if (abdata.GetLoadPath(out string path, _art._MapID))
				_Echelon[(int)_art._Sort].Enqueue(_art);
			else
			{
				if(DebugMgr.CanLogError()) DebugMgr.LogError("{0} 需要下载资源 弹界面---建议加传送主城功能", abdata._AbPath);
			}
		}
		catch (System.Exception ex)
		{
			//if(DebugMgr.CanLogError()) DebugMgr.LogError("怎么又缺资源了 {0},{1} {2}", _art.AbSingleName(), _art.ArtName(), ex.ToString());
		}
	}
	void SetDenpend(Stack<IArt> _Data, NewResAb res)
	{
		var indexs = res._Dependencies;
		if (indexs != null)
		{
			for (int z = 0; z < indexs.Length; z++)
			{
				if (ResCenter.inst._ResMgr.GetABForAbID(indexs[z], out NewResAb depenAb))
				{
					_Data.Push(new DependArt(depenAb));
					SetDenpend(_Data, depenAb);
				}
			}
		}
	}
	IEnumerator<float> UpdateLoad(int _MainData)
	{
		var _Data = _CurrentData[_MainData];
		while (true)
		{
			GetNextData(_Data);
			if (_Data.Count != 0)
			{
				if ((StaticTools._ResLoadModel == ResLoadModel.DEFAULT ||
					(StaticTools._ResLoadModel == ResLoadModel.ARTIST) && _Data.Peek()._ModelID != (int)RES_MODEL_INDEX.lua))
				{
					ResCenter.inst._ResMgr.GetObj(_Data.Pop());
				}
				else
				{
					//0.A部件同一帧加载10个
					//1.载体(需要加载的信息都在这里)
					var temp = _Data.Peek();
					if (!temp.IsWaitArt())
					{
						_Data.Pop();//去掉这个资源
						continue;
					}
					SetDenpend(_Data, temp.GetNewResAb);

					while (_Data.Count > 0)
					{
						var loadData = _Data.Pop();
						var abdata = loadData.GetNewResAb;

						if (SetArt(loadData, abdata))//跳出逻辑
						{
							continue;
						}

						AssetBundle _Ab = null;
						{//加载逻辑
							//DebugMgr.LogError("Try load {0} {1}", abdata._AbPath, Time.time);
							while (abdata._LoadStatus == LoadStatus.Loading)//同ab再被加载中
							{
								yield return 0;//另一条协程再加载中......
								//DebugMgr.LogError("wait load {0} {1}", abdata._AbPath, Time.time);
							}
							if (abdata._LoadStatus != LoadStatus.Loading && abdata._LoadStatus != LoadStatus.Done)//还没开始下载
							{
								//DebugMgr.LogError("start load {0} {1}", abdata._AbPath, Time.time);
								abdata._LoadStatus = LoadStatus.Loading;
								abdata._StartLoadTime = Time.realtimeSinceStartup;
								if (abdata.GetLoadPath(out string path, loadData._MapID))
								{
									//AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(path);
									//yield return Timing.WaitUntilDone(request);
									//_Ab = request.assetBundle;
									//DebugMgr.LogError("AssetBundle.LoadFromFile {0} {1}", path, Time.time);
									_Ab = AssetBundle.LoadFromFile(path);
								}
								else
								{
									if(DebugMgr.CanLogError()) DebugMgr.LogError("异常了=" + abdata._AbPath);
								}
								//DebugMgr.LogError("finish load {0} {1}", abdata._AbPath, Time.time);
#if _CHECK_OPTIMIZE
		if (Time.realtimeSinceStartup - abdata._StartLoadTime > 0.01)
			if(DebugMgr.CanLogError()) DebugMgr.LogError(string.Format("AssetBundle.LoadFromFileAsync {0} costtime = {1}", path, (int)((Time.realtimeSinceStartup - abdata._StartLoadTime)*1000)));
#endif
							}
							else//已经被done了
							{
								_Ab = abdata._AssetBundle;
							}
						}


						if (SetArt(loadData, abdata))//跳出逻辑
						{
							continue;
						}
						//DebugMgr.LogError("wait art {0} {1}", abdata._AbPath, Time.time);
						//近一步加载Obj
						if (loadData.IsWaitArt(abdata._ID))
						{
							yield return Timing.WaitUntilDone(Timing.RunCoroutine(loadData.Loading(_Ab)));
						}
						//DebugMgr.LogError("finish art {0} {1}", abdata._AbPath, Time.time);
						SetAssetBundle(loadData, abdata, _Ab);
					}
				}
			}
			else
			{
				yield return 1;
			}
			UnloadAbAndObj();
		}
	}

	bool SetArt(IArt loadData, NewResAb abdata)
	{
		if (loadData is DependArt)
		{
			return abdata._AssetBundle != null;
		}
		else if (!string.IsNullOrEmpty(loadData.ArtName()))
		{
			if (abdata.GetObj(loadData.ArtName(), out NewResUnit _u))
			{
				if (_u._Obj != null)
				{
					loadData.UseArt(_u._Obj);//如果资源存在就直接回调
					return true;
				}
			}
		}
		return false;
	}
	void SetAssetBundle(IArt art, NewResAb _NewResAB, AssetBundle _ab)
	{
		if (art._CanCacheAb)
		{
			_NewResAB.SetAb(_ab);
			_NewResAB._LoadStatus = LoadStatus.Done;
		}
		else
		{
			_NewResAB._LoadStatus = LoadStatus.None;
		}
	}
	float _AutoUnloadTime = NewResAb._RemoveTime;
	void UnloadAbAndObj()
	{
		if (StaticTools._ResLoadModel == ResLoadModel.DEFAULT)
		{
			return;
		}
		_AutoUnloadTime -= (int)(Time.deltaTime * 1000);
		if (_AutoUnloadTime < 0)
		{
			ResCenter.inst._ResMgr.UnloadAbAndObj();
			_AutoUnloadTime = NewResAb._RemoveTime;
		}
	}
	void GetNextData(Stack<IArt> _Data)
	{
		if (_Data.Count == 0)
		{
			IArt _Temp = null;
			for (int i = 0; i < _Echelon.Length; i++)
			{
				if (_Echelon[i].Count > 0)
				{
					_Temp = _Echelon[i].Dequeue();
					break;
				}
			}
			if (_Temp != null)
			{
				_Data.Push(_Temp);
			}
		}
	}
}
