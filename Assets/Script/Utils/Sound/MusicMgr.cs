using UnityEngine;
using System.Collections;

using System.Collections.Generic;
using System;
public enum SoundType
{
    BgSound,
    OtherSound,
    RunSound,
    OtherPlayer,
    UI
}
public class MusicMgr
{
    const int MAX_SOUND=10;
    float bgVolume;
    public float BgVolume
    {
        set
        {
            bgVolume = value;
            if (m_bgMusic.volumeScale != value)
            {
                if (status != 0)
                {
                    m_bgMusic.volumeScale= bgVolume;
                    m_runMusic.volumeScale = bgVolume;
                    m_npcMusic.volumeScale = bgVolume;
                }
            }
        }
        get
        {
            return bgVolume;
        }
    }
    AudioSource otherAudioSource;
    float _effectVolume;
    public float effectVolume
    {
        set
        {
            
            if (m_effectMusic.volumeScale != value)
            {
                m_effectMusic.volumeScale = value;
                if (status != 0)
                    m_effectMusic.volumeScale = value;
            }
            _effectVolume = value;
        }
        get
        {
            return _effectVolume;
        }
    }
    int status;
    public int ChangeStatus
    {
        set
        {
            status = value;
            if (status == 0)
            {
                m_bgMusic.volumeScale= 0f;                
                m_runMusic.volumeScale= 0f;
                m_npcMusic.volumeScale = 0f;
                m_effectMusic.volumeScale = 0;
                //GRoot.inst.soundVolume = 0;
            }
            else
            {
                m_bgMusic.volumeScale= BgVolume;                
                m_runMusic.volumeScale= BgVolume;
                m_npcMusic.volumeScale = BgVolume;
                m_effectMusic.volumeScale = effectVolume;
                //GRoot.inst.soundVolume = BgVolume;
            }
        }
        get
        {
            return status;
        }
    }


    BackGroundMusic m_bgMusic;
    NPCMusic m_npcMusic;
    EffectMusic m_effectMusic;
    RunMusic m_runMusic;
    public static MusicMgr inst;
    Dictionary<string,MusicUnit> units;
    List<MusicUnit> lists;
    public MusicMgr()
    {
        inst = this;                
        units = new Dictionary<string, MusicUnit>();
        m_bgMusic = new BackGroundMusic();
        m_npcMusic = new NPCMusic();
        m_effectMusic = new EffectMusic();
        m_runMusic = new RunMusic();
        lists = new List<MusicUnit>();

        BgVolume = PlayerPrefs.GetFloat("AmbientVolume", 1.0f);
        effectVolume = PlayerPrefs.GetFloat("EffectVolume", 0.5f);
        ChangeStatus = PlayerPrefs.GetInt("volTragle", 1);
          
        TimeMgr.Instance.AddIntervelEvent(_CallBack, 16, 0, -1);
    }   
    internal static void PdStop()
    {
        //iosÈ¥µôÉùÒô
        //if (GlobalData.IOSReview > 0)
        //{
        //    PlayerPrefs.SetInt("volTragle", 0);
        //    inst.ChangeStatus = 0;
        //}
    }
    public static void PlayLoginBg()
    {
        //var tmp= UIPackage.GetItemAsset("LoginWnd", "login") as AudioClip;        
        //inst.m_bgMusic.Play(tmp,1);
    }
    internal static void PlayCreateAndDownload()
    {
        //var tmp = UIPackage.GetItemAsset("LoginWnd", "CreateAndDownload") as AudioClip;
        //inst.m_bgMusic.Play(tmp,1);
    }
    internal static void RunSoundPlay()
    {
        inst.m_runMusic.RunPlay();
    }
    internal static void RunSoundStop()
    {
        inst.m_runMusic.RunStop();
    }

    internal static void PlayMapSound(int mapid)
    {
        //Map map = MapManager.GetSingleData(mapid);
        //if (map == null)
        //    return;
        //Voice_effect vc = Voice_effectManager.GetSingleData(map.sound);
        //if (vc == null)
        //{
        //    return;
        //}
        //MusicUnit tmp = null;        
        //if (inst.units.TryGetValue(vc.url,out tmp))
        //{
        //    tmp.Play(TimeMgr.Instance._MsTime + 80);
        //}
        //else
        //{
        //    tmp = new MusicUnit(vc.url, SoundType.BgSound);
        //    tmp.Play(TimeMgr.Instance._MsTime + 80);
        //    Add(vc.url, tmp);
        //}
    }
    public static void PlayNpc(string url)
    {        
        MusicUnit tmp = null;
        if (inst.units.TryGetValue(url, out tmp))
        {
            tmp.Play(TimeMgr.Instance._MsTime + 80);
        }
        else
        {
            tmp = new MusicUnit(url, SoundType.OtherPlayer);
            tmp.Play(TimeMgr.Instance._MsTime + 80);
            Add(url, tmp);
        }
    }
    public static void PlayNpc(int soundId)
    {        
        MusicUnit tmp = null;
        var str = "npc_" + soundId;
        if (inst.units.TryGetValue(str, out tmp))
        {
            tmp.Play(TimeMgr.Instance._MsTime + 50);
        }
        else
        {
            tmp = new MusicUnit(str, SoundType.OtherPlayer);
            tmp.Play(TimeMgr.Instance._MsTime + 50);
            Add(str, tmp);
        }
    }
    #region effect
    internal static void PlayEffect(int x,int y,string sound,int time=0)
    {
        //var cha_x = x - UnitsManager.MyPlayer.xAxis;
        //cha_x = cha_x > 0 ? cha_x : -cha_x;

        //var cha_y = y - UnitsManager.MyPlayer.yAxis;
        //cha_y = cha_y > 0 ? cha_y : -cha_y;

        //if (cha_x < cha_y)
        //{
        //    cha_x = cha_y;
        //}
        //var volume = (MAX_SOUND - cha_x);
        //if (volume > 0)
        //{            
        //    MusicUnit tmp = null;
        //    if (inst.units.TryGetValue(sound, out tmp))
        //    {
        //        tmp.Play(TimeMgr.Instance._MsTime+ time, (sbyte)volume);
        //    }
        //    else
        //    {
        //        tmp = new MusicUnit(sound, SoundType.OtherSound);
        //        tmp.Play(TimeMgr.Instance._MsTime+ time, (sbyte)volume);
        //        Add(sound, tmp);
        //    }
        //}
    }
    internal static void PlayEffect(int x, int y, int soundId, int time=0)
    {
        //var cha_x = x - UnitsManager.MyPlayer.xAxis;
        //cha_x = cha_x > 0 ? cha_x : -cha_x;

        //var cha_y = y - UnitsManager.MyPlayer.yAxis;
        //cha_y = cha_y > 0 ? cha_y : -cha_y;

        //if (cha_x < cha_y)
        //{
        //    cha_x = cha_y;
        //}

        //var vound = (MAX_SOUND - cha_x);
        //if (vound>0)
        //{
        //    Voice_effect vc = Voice_effectManager.GetSingleData(soundId);
        //    MusicUnit tmp = null;
        //    if (inst.units.TryGetValue(vc.url, out tmp))
        //    {
        //        tmp.Play(TimeMgr.Instance._MsTime + time, (sbyte)vound);
        //    }
        //    else
        //    {
        //        tmp = new MusicUnit(vc.url, SoundType.OtherSound);
        //        tmp.Play(TimeMgr.Instance._MsTime + time, (sbyte)vound);
        //        Add(vc.url, tmp);
        //    }
        //}
    }
    internal static void PlayEffect(string soundname, int time = 0, sbyte vound = 10)
    {        
        MusicUnit tmp = null;
        if (inst.units.TryGetValue(soundname, out tmp))
        {
            tmp.Play(TimeMgr.Instance._MsTime + time, vound);
        }
        else
        {
            tmp = new MusicUnit(soundname, SoundType.OtherSound);
            tmp.Play(TimeMgr.Instance._MsTime + time, vound);
            Add(soundname, tmp);
        }
    }
    internal static void PlayEffect(int soundId,sbyte vound=10)
    {
        //Voice_effect vc = Voice_effectManager.GetSingleData(soundId);
        //MusicUnit tmp = null;
        //if (inst.units.TryGetValue(vc.url, out tmp))
        //{
        //    tmp.Play(TimeMgr.Instance._MsTime, vound);
        //}
        //else
        //{
        //    tmp = new MusicUnit(vc.url, SoundType.OtherSound);
        //    tmp.Play(TimeMgr.Instance._MsTime, vound);
        //    Add(vc.url, tmp);
        //}
    }
    #endregion
    static void Add(string str,MusicUnit tmp)
    {
        inst.lists.Add(tmp);
        inst.units.Add(str, tmp);
    }    
    int last = 0;
    void _CallBack(int _i,float _f)
    {
        var curTime = TimeMgr.Instance._MsTime;
        if ((curTime- last) > 150&&ChangeStatus!=0)
        {
            bool npc_play = m_npcMusic.IsPlaying();
            if (npc_play)
            {
                var c = BgVolume * 0.25f;
                if (m_bgMusic.volumeScale==BgVolume)
                {
                    m_bgMusic.volumeScale = c;
                    m_effectMusic.volumeScale = c;
                }                
            }
            else
            {

                if (m_bgMusic.volumeScale != BgVolume)
                {
                    m_bgMusic.volumeScale = BgVolume;
                    m_effectMusic.volumeScale = effectVolume;
                }                
            }
            last = curTime;
        }

        var count = lists.Count-1;        
        for (int i = count; i >= 0; i--)
        {
            var tmp = lists[i];
            int _top = tmp.GetTop();
            if (_top!=0&& curTime >=_top)
            {                              
                switch (tmp.m_type)
                {
                    case SoundType.BgSound:
                        tmp.SetTip(m_bgMusic.Play(tmp.m_clip, tmp.volum, _top, 60000));
                        break;
                    case SoundType.OtherPlayer:
                        tmp.SetTip(m_npcMusic.Play(tmp.m_clip, tmp.volum, _top, 5000));
                        break;
                    case SoundType.OtherSound:
                        tmp.SetTip(m_effectMusic.Play(tmp.m_clip,tmp.volum, _top, 1000));
                        break;
                    case SoundType.UI: break;
                }
            }
            else
            {
                var tmptime = (curTime - tmp.m_lastTime);
                if (tmptime > 120000&& tmp.m_type!=SoundType.BgSound)
                {
                    if (tmp.m_type==SoundType.OtherPlayer && tmptime>600000)
                    {
                        if (tmp.m_clip!=null)
                        {
                            tmp.m_clip.UnloadAudioData();
                        }                        
                    }
                    lists.Remove(tmp);
                    units.Remove(tmp.ArtName());                    
                }
            }
        }
        
    }

    internal static void AudioSourceStateC(bool isAll, bool state, byte number = 0)
    {
        if (isAll)
        {
            inst.m_bgMusic.Enable(state);
            inst.m_runMusic.Enable(state);
            inst.m_npcMusic.Enable(state);
            inst.m_effectMusic.Enable(state);
        }
        else
        {
            switch (number)
            {
                case 1:
                    inst.m_bgMusic.Enable(state);
                    break;
                case 2:
                    inst.m_effectMusic.Enable(state);
                    break;
                case 3:
                    inst.m_runMusic.Enable(state);
                    break;
                case 4:
                    inst.m_npcMusic.Enable(state);
                    break;
                default:
                    break;
            }
        }
    }
    internal static void ClearAll()
    {
        inst.units.Clear();
        inst.lists.Clear();
    }

    public static void SoundUI(string sound,sbyte volume)
    {        
        MusicUnit tmp = null;
        if (inst.units.TryGetValue(sound, out tmp))
        {
            tmp.Play(TimeMgr.Instance._MsTime + 30,volume);
        }
        else
        {
            tmp = new MusicUnit(sound, SoundType.OtherSound);
            tmp.Play(TimeMgr.Instance._MsTime + 30,volume);
            inst.lists.Add(tmp);
        }
    }
    public static void HeYao()
    {
        PlayEffect("a_hys",vound:7);
    }
    public static void HuiCheng()
    {
        PlayEffect("a_hcj", vound: 7);
    }
    public static void RandomCS()//Ëæ»ú´«ËÍ
    {
        PlayEffect("a_sjcsj", vound: 7);
    }
}