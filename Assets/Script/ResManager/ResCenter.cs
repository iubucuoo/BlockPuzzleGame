
using MEC;
using System.Collections.Generic;
using System.IO;
using WUtils;
enum LoginStatus
{
    None,
    LoadCfg,
    Downloading,
    Done,
    ErrorCfg,
    ErrorDownload,
}


/// <summary>
///////////////////////// 这里消息ID关联LUA  正式出包后不能改 会导致id对不上无法热更加载
/// </summary>
public enum HotFixListenID
{
    StartCheck = ManagerID.AssetManager + 1,
    StartUpdateRes,
    EnterGame,
    EndUpdateRes,
    Max,
}
public enum RES_ID
{
    GET_OBJ = HotFixListenID.Max + 1,
    SaveVersion,
}

public enum LoadStatus
{
    None,
    BeginLoad,
    Loading,
    Downloading,
    Done,
    Error,
}
public enum ResLoadModel
{
    DEFAULT,//默认,全部本地
    ONLINE,//线上资源
}
//资源中心，主要负责资源管理
class ResCenter : AssetBase, IMgr
{
    public static ResCenter inst;
	public ResMgr _ResMgr;
	ResLoop _LoadLoop;
	DownloadThread _DownloadThread;
	List<ResAb> _DownloadList;
	LoginStatus _LoginStatus;

	ResUI resUI = new ResUI();
	//ScLightMapMgr lightMapMgr = new ScLightMapMgr();
	public int _Version { get { return inst._ResMgr._Version; } }

	public long _MaxSize;
	public long _CurSize;
	public int _ChapterCurSize;
	public bool _IsdotLogic;
	public void InitMgr()
	{
		inst = this;
		_LoadLoop = new ResLoop();
		_DownloadThread = new DownloadThread();
		_DownloadList = new List<ResAb>();

		resUI.InitMgr();
		//lightMapMgr.InitMgr();

		messageIds = new ushort[]
		{
			(ushort)HotFixListenID.StartCheck,
			(ushort)HotFixListenID.StartUpdateRes,
			(ushort)HotFixListenID.EnterGame,
			(ushort)HotFixListenID.EndUpdateRes,
			(ushort)RES_ID.GET_OBJ,
			(ushort)RES_ID.SaveVersion,
		};
		RegistEventListen(this, messageIds);


		if (!AppParam.LoadArtIsAb )
		{
			_ResMgr = new EditorLoad().BuilderResData();
			Load();
		}
		else
		{
			Timing.RunCoroutine(LoadVersion());
		}
	}
    //都用包内的Stream下的
    IEnumerator<float> LoadVersion()
    {
        ResMgr _StreamResMgr = null;

        LoadStatus _StreamIsOk = LoadStatus.Loading;
        DownloadTools.LoadUrl(PathTools.STREAM_VERSION, 30, (obj) =>
        {
            _StreamIsOk = LoadStatus.Done;
            _StreamResMgr = new ResMgr(obj.data);

        }, (err) => { _StreamIsOk = LoadStatus.Done; Log.Error(err); });
        while (_StreamIsOk != LoadStatus.Done)
        {
            yield return 0;
        }
        _ResMgr = _StreamResMgr;
        Load();
    }
    //需要检查更新的时候用这个
	IEnumerator<float> LoadVersionCheck()
	{
		ResMgr _StreamResMgr = null;
		ResMgr _CacheResMgr = null;

		LoadStatus _StreamIsOk = LoadStatus.Loading;
		LoadStatus _CacheIsOk = LoadStatus.Loading;

		DownloadTools.LoadUrl(PathTools.STREAM_VERSION, 30, (obj) =>
		{
			_StreamIsOk = LoadStatus.Done;
			_StreamResMgr = new ResMgr(obj.data);

		}, (err) => { _StreamIsOk = LoadStatus.Done;  Log.Error(err); });


		DownloadTools.LoadUrl(PathTools.WWW_CACHE_VERSION, 30, (obj) =>
		{
			_CacheResMgr = new ResMgr(obj.data);
			_CacheIsOk = LoadStatus.Done;

		}, (err) => { _CacheIsOk = LoadStatus.Done;  Log.Error(err); });

		//等version 配置下载完成
		while (_CacheIsOk != LoadStatus.Done || _StreamIsOk != LoadStatus.Done)
		{
			yield return 0;
		}

		if (_StreamResMgr == null)
		{
			 Log.Error("StreamAsset 路径下没有 Version.bytes");
		}
		else
		{
			if (_CacheResMgr == null)
			{
				 Log.Error(PathTools.CACHE_VERSION + "=>没有资源 可能被清缓存/首次运行");
				_ResMgr = _StreamResMgr;
			}
			else
			{
				bool isUseCache = _CacheResMgr._Version >= _StreamResMgr._Version;
				_ResMgr = isUseCache ? _CacheResMgr : _StreamResMgr;
				if (!isUseCache)
					 Log.Warning("包内的版本大于缓存内的版本(更新后等同svn上版本)，清理Cache 或 更新svn上美术资源");
			}
		}
        Load();
        //LoadLuaAB();
	}
	void LoadLuaAB()
	{
		var modeldata = _ResMgr.GetModel(RES_MODEL_INDEX.lua);
		if (modeldata == null)
			return;
		var _Count = modeldata._Count;
		int _Sum = 0;
		for (int i = 0; i < _Count; i++)
		{
			var newResAb = modeldata[i];
#if UNITY_EDITOR
			if(newResAb._AbPath == null)
			{
				newResAb._AbPath = "Lua/" + newResAb._AbName;
			}
#endif
			MsgSend.GetRes(RES_MODEL_INDEX.lua, newResAb._AbName, null, (ab) =>
			{

				if (ab == null)
				{
					Log.Error("加载Lua路径为空== " + newResAb._AbPath);
				}
				else
				{
					var file = newResAb.GetPath(true);
					//LuaFileUtils.Instance.AddSearchBundle(Path.GetFileNameWithoutExtension(file), ab as AssetBundle);
				}
				_Sum++;
				if (_Sum == _Count)
				{
					Load();
				}
			}, ()=> {
                Log.Error(string.Format("加载Lua ab文件失败 path = {0}, name = {1}", newResAb._AbPath, newResAb._AbName)); 
            }
            );
		}
	}
	
	//加载
	//0.加载管理脚本
	//1.加载场景
	//2.加载lua
	void Load()
	{
		ScriptMgr.Inst.Init();
        MainC.Inst.StartGame();
    }
	public override void ProcessEvent(MessageBase tmpMsg)
	{
		switch (tmpMsg.messageId)
		{
			//通过Lua UI点击登录
			case (ushort)HotFixListenID.StartCheck:
				{
					_MaxSize = 0;
					//1.如果是走AB 需要去服务器上比对相关资源
					//2.如果是走本地 不需要比对资源，直接Load
					Timing.RunCoroutine(CanLogin());
				}
				break;
			case (ushort)HotFixListenID.StartUpdateRes:
				StartDownLoad();
				break;
			case (ushort)HotFixListenID.EndUpdateRes:
				EndDownLoad(tmpMsg);
				break;
			case (ushort)HotFixListenID.EnterGame:
				break;
			case (ushort)RES_ID.SaveVersion:
				SaveVersion();
				break;
			case (ushort)RES_ID.GET_OBJ:
				GetObj(tmpMsg as RequestResMsg);
				break;
			default:
				break;
		}
	}



	IEnumerator<float> CanLogin()
	{
		if (AppParam.LoadArtIsAb)
		{
			NetDiff();
			_LoginStatus = LoginStatus.None;
		}
		else
		{
			NoDownload();
		}

		while (_LoginStatus != LoginStatus.Done)
		{
			if (_LoginStatus == LoginStatus.Downloading && _DownloadThread._Idle)
			{
				_LoginStatus = LoginStatus.Done;
				SaveVersion();
			}
			yield return 1;
		}
	}
	void NetDiff()
	{
		DownloadTools.LoadUrl(PathTools.NET_VERSION, 30, (obj) =>
		{
			_LoginStatus = LoginStatus.LoadCfg;
			_IsdotLogic = false;
			//把新增/更新/删除查出来，主要还是路径，没有变化自然不用
			var tempResMgr = new ResMgr(obj.data);
			//_ResMgr
			for (int i = 0; i < tempResMgr._Data._Count; i++)
			{
				var tempModel = tempResMgr._Data[i];
				for (int j = 0; j < tempModel._Count; j++)
				{
					var tempAb = tempModel[j];
					if (_ResMgr.GetABForAbID(tempAb._ID, out ResAb ab))
					{
						//查到对应的ab了
						if (ab._VersionNum != tempAb._VersionNum)
						{
							 Log.Warning("需要下载[更新]" + ab._AbPath + "=>" + ab._VersionNum + " size= " + ab._Size + ";net_VersionNum=" + tempAb._VersionNum);
							DeleteAb(tempAb);
						}
						else
						{
							tempModel[j] = ab;
						}
					}
					else
					{
						if (tempAb._Size == 0)
						{
							//没有查到对应的ab，需要去服务器下载
							 Log.Error("需要下载[新增],出现这种情况ab资源没有打，但verions.bytes 生成了，" + tempAb._AbPath + "=>" + tempAb._VersionNum + " size= " + tempAb._AbName);
						}
						else
						{
							//没有查到对应的ab，需要去服务器下载
							 Log.Warning("需要下载[新增]" + tempAb._AbPath + "=>" + tempAb._VersionNum + " size= " + tempAb._Size);
						}
						DeleteAb(tempAb);
					}
				}
			}
			_ResMgr = tempResMgr;
			SetNeedDownList();
		}, (err) => {  Log.Error("配置没有请求到：" + PathTools.NET_VERSION + ";error" + err); _LoginStatus = LoginStatus.ErrorCfg; });
	}

	void SetNeedDownList()
	{
		_DownloadList.Clear();
		for (int i = 0; i < _ResMgr._Data._Count; i++)
		{
			var tempModel = _ResMgr._Data[i];
			for (int j = 0; j < tempModel._Count; j++)
			{
				var tempAb = tempModel[j];
				if (tempAb.GetNeedDownloadOnLogin())
				{
					_DownloadList.Add(tempAb);
				}
			}
		}
		if (_DownloadList.Count > 0)
		{
			 Log.Warning("NeedDownLoad:" + _DownloadList.Count);
			//LoginMgr.CheckResCallback(true);
		}
		else
		{
			NoDownload();
		}
	}
	void StartDownLoad()
	{
		for (int i = 0; i < _DownloadList.Count; i++)
		{
			var temp = _DownloadList[i];
			_DownloadThread.DownQueue(temp);
			_MaxSize += temp._Size;
		}
		_LoginStatus = LoginStatus.Downloading;
		_CurSize = 0;
		_DownloadThread._Process = Process;
	}
	void EndDownLoad(MessageBase tmpMsg)
	{
        AppParam._LuaLoadError = (tmpMsg is MsgInt) && (tmpMsg as MsgInt)._Int_V == 1;
		if (_IsdotLogic)
		{
#if UNITY_EDITOR
            //SceneMgr.LoadScene("Loading");
#else
            //SdkMgr.Instance.RestartGame();//重启app
#endif
		}
		else
        {
            //SceneMgr.LoadScene("Loading");
        }
        Log.Error("SceneMgr.LoadScene(Loading)");
	}

	public void DownloadChapter(int _ChapterID)
	{
		if (ResUI._inst._Chapters.TryGetValue(_ChapterID, out Chapter v))
		{
			for (int i = 0; i < v._Data.Count; i++)
			{
				var abs = v._Data[i]._Abs;
				for (int j = 0; j < abs.Count; j++)
				{
					if (!abs[j]._IsFinish)
						Log.Info("添加下载的内容  " + abs[j]._AbName + "  size = " + abs[j]._Size);
					_DownloadThread.DownQueue(abs[j]);
				}
			}
			_ChapterCurSize = 0;
			_DownloadThread.ResDownCB = ResUIDownCB;
		}
	}
	void ResUIDownCB(ResAb data)
	{
		_ChapterCurSize += data._Size;//给下载界面显示每秒下载的量与估算下载完成时间
									  // DebugMgr.LogError("下载回调  下载完成资源 ={0}   下载size = {1}", data._AbName, data._Size);
	}
	public void ClearChapterData(Chapter chapter)
	{
		_DownloadThread.ClearChapterData(chapter._Data);
	}
	void NoDownload()
	{
		_LoginStatus = LoginStatus.Done;
		//LoginMgr.CheckResCallback(false);
	}
	void Process(int v)
	{
		_CurSize += v;
		// DebugMgr.LogError("下载回调热更的    下载size = {0}", v);
	}
	void DeleteAb(ResAb tempAb)
	{
		//查找是否有 dll/main 有则表示更新c#代码
		if (tempAb._AbPath.Contains(PathTools.DllABPath))
		{
			_IsdotLogic = true;
			 Log.Error("更新c# dll");
		}
		tempAb._DownloadID = -1;
		tempAb._IsFinish = false;
		var path = tempAb.GetPathBak();
		if (File.Exists(path))
		{
			File.Delete(path);
			 Log.Warning("干掉包外资源");
		}
	}

	void GetObj(RequestResMsg msg)
	{
		_LoadLoop.BeginLoadData(msg._ArtUnit);
	}
	public void SaveVersion()
	{
		_ResMgr.SaveToFile(PathTools.CACHE_VERSION);
	}

	public void ResetMgr()
	{
		resUI.ResetMgr();
		//lightMapMgr.ResetMgr();
	}
}
