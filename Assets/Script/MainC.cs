using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WUtils;
public class channles
{
    public int version;
    public string resUpdateIp;
    public string pkgName;
    public string notice_url;
    public int baseNum;
    public int smallGame;
    public int NG;
}
public class MainC : MonoBehaviour
{
    public ResLoadModel  _ResLoadModel;
    internal static MainC Inst;
    private void Awake()
    {
        Inst = this;
#if UNITY_EDITOR
        Log.EnableLog = true;
#else
        Log.EnableLog = false;
#endif

        AudioMgr.Inst.isPlaying_Music = StaticTools.MusicOnOff == 0;
        AudioMgr.Inst.isPlaying_Sound = StaticTools.SoundIsOnOff == 0;


        Application.targetFrameRate = 60;
        gameObject.AddComponent<TimeMgr>();

        MEC.Timing.RunCoroutine(NetStatus());
        GoogleAdMgr.CheckInstance();//初始化的interstitial在下个update中执行
        DebugTool.CheckInstance();

        AppParam._ResLoadModel = _ResLoadModel;
        //先载入数据文件
        //LoadLanguageData();
        //
        ScriptMgr.Inst.InitFirstScript();
    }
    void Start()
    {
    }
    public void StartGame()
    {
        gameObject.AddMissingComponent<GameStart>();//开始游戏
    }
    private void OnApplicationQuit()
    {
        ScriptMgr.Inst.Reset();
    }
    private void ReadStreamingInit()
    {
        string path1 = string.Concat(PathTools.GetWWWAssetBundlePath(true, true), "/GameConfig.json");
        DownloadTools.LoadUrl(path1,5, (s1) =>
        {
            
        });  
    }

    IEnumerator<float> NetStatus()
    {
        while (true)
        {
            AppParam._NetStatus = AppParam.GetNetState();
            //通知 lua侧 网络状态
            //var objs = new object[] { GlobalData._NetStatus };
            //FreeSendEvent.GSendMsg((ushort)LMainUIListenID.NetStatus, null, objs);
            yield return MEC.Timing.WaitForSeconds(1);
        }
    }
}
