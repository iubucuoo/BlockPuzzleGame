using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WUtils;
using WUtils.Utils;
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
    public static int _height = 90;// 60;
    public static int _width = 90;// 60;
    public static int wh_2 = 45;// 30;
    public static Vector2 DragUp = new Vector2(0, _width * 4);//y高度 对应的倍数
    public ResLoadModel  _ResLoadModel;
    public bool IsTopScore { get; set; }
    public bool IsRotateState { get; set; }
    public Vector3 RotateGoldAddPos { get; set; }

    public Dictionary<string, Sprite> Sprites;


    channles channle_info;

    public static MainC Inst;
    private void Awake()
    {
        Inst = this;
#if UNITY_EDITOR
        DebugMgr.EnableLog = true;
#else
        DebugMgr.EnableLog = false;
#endif
        Sprites = new Dictionary<string, Sprite>();

        AudioMgr.Inst.isPlaying_Music = GameGloab.MusicOnOff == 0;
        AudioMgr.Inst.isPlaying_Sound = GameGloab.SoundIsOnOff == 0;


        Application.targetFrameRate = 60;
        gameObject.AddComponent<TimeMgr>();

        MEC.Timing.RunCoroutine(NetStatus());
        GoogleAdMgr.CheckInstance();//初始化的interstitial在下个update中执行
        FPS.CheckInstance();
        foreach (var v in sprites)
        {
            Sprites[v.name] = v;
        }
        StaticTools._ResLoadModel = _ResLoadModel;
        //先载入数据文件
        //LoadLanguageData();
        //
        ScriptMgr.Inst.InitFirstScript();
    }
    void Start()
    {
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

    public List<Sprite> sprites = new List<Sprite>();
    

    bool errorOver = true;
    void ForegroundErrorOver(string title)
    {
        //需要在主线程中处理数据
        if (!errorOver)
            return;
        TimeMgr.Instance.AddIntervelEvent((int i, float f) =>
        {
            if (errorOver)
            {
                errorOver = !errorOver;
                string sw = title == null ? "网络不给力哦！ 点击重试。" : title;
                //弹出框 提示网络不行重新链接
                //TipWnd.inst.SwTipCom(title == null ? "网络不给力哦！ 点击重试。" : title, "提示", callback: () =>
                //{
                //    SaveFile();
                //    StaticTools.ReStartGame();
                //    errorOver = true;
                //}, btn_name: "重试", isMaskBtn: false);
            }
        }, 16, 0, 1);
    }
    IEnumerator<float> NetStatus()
    {
        while (true)
        {
            StaticTools._NetStatus = StaticTools.GetNetState();
            //通知 lua侧 网络状态
            //var objs = new object[] { GlobalData._NetStatus };
            //FreeSendEvent.GSendMsg((ushort)LMainUIListenID.NetStatus, null, objs);
            yield return MEC.Timing.WaitForSeconds(1);
        }
    }
    void LoadLanguageData()
    {
        var ab = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/language.ly");
        StartCoroutine(Loading(ab));
    }
    IEnumerator<float> Loading(AssetBundle ab)
    {
        var objs = ab.LoadAllAssets();
        UseArt(objs);
        yield return 0;
        ab.Unload(false);
    }
    void UseArt(object[] objs)
    {
        foreach (var str in objs)
        {
            var languagedata = JsonUtility.FromJson<LanguageData>((str as TextAsset).text);
            foreach (var v in languagedata.datas)
            {
                if (!LanguageManger.Inst.languagedic.ContainsKey(v.LanguageList))
                {
                    Dictionary<string, string> ky = new Dictionary<string, string>();
                    foreach (var vv in v.ky)
                    {
                        ky.Add(vv.key, vv.value);
                    }
                    LanguageManger.Inst.languagedic.Add(v.LanguageList, ky);
                }
            }
        }
    }
}
