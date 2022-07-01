using UnityEngine;
using System.Collections;
using System;

public abstract class Music
{
    protected float smaple=1;
    protected AudioSource m_source;
    float m_volume = 1;
    public float volumeScale
    {
        get
        {
            return m_volume;
        }
        set
        {
            m_volume = value;
            m_source.volume = m_volume;
        }
    }

    public virtual int Play(AudioClip cp,int volume,int time=0,int longtime=0)
    {        
        if (cp!=null)
        {
            var tmp = m_source.clip;
            if (tmp!=cp)
            {
                if (tmp != null)
                {
                    m_source.Stop();
                    m_source.clip = null;
                }
                m_source.clip = cp;
                m_source.volume = volumeScale* smaple* (volume*0.1f);
                m_source.Play();
            }            
            return 0;
        }else
        {
            return time + longtime > TimeMgr.Instance._MsTime ? time : 0;
        }        
    }
    public void Enable(bool status)
    {
        m_source.enabled = status;
    }
    public bool IsPlaying()
    {
        return m_source.isPlaying;
    }
}

public class BackGroundMusic:Music
{       
    public BackGroundMusic()
    {
        smaple = 1f;
        GameObject go = new GameObject();
        go.transform.SetParent(null);
#if UNITY_EDITOR
        go.name = "BackGroundMusic";
#endif
        go.transform.localPosition = Vector3.zero;        
        m_source= go.AddComponent<AudioSource>();
        m_source.loop = true;
        
    }    
}

public class NPCMusic : Music
{
    public NPCMusic()
    {
        smaple = 2;
        GameObject go = new GameObject();
        go.transform.SetParent(null);
#if UNITY_EDITOR
        go.name = "NPCMusic";
#endif
        go.transform.localPosition = Vector3.zero;
        m_source = go.AddComponent<AudioSource>();
        m_source.playOnAwake = false;       
    }
}

public class EffectMusic : Music
{
    public EffectMusic()
    {
        smaple =1f;
        GameObject go = new GameObject();
        go.transform.SetParent(null);
#if UNITY_EDITOR
        go.name = "EffectMusic";
#endif
        go.transform.localPosition = Vector3.zero;
        m_source = go.AddComponent<AudioSource>();        
    }
    public override int Play(AudioClip cp,int valume, int time = 0, int longtime = 0)
    {
        if (cp != null)
        {            
            m_source.PlayOneShot(cp, volumeScale*valume*0.1f);
            return 0;
        }
        else
        {
            return time + longtime > TimeMgr.Instance._MsTime ? time : 0;
        }
    }
}

public class RunMusic : Music
{
    bool isInit=true;
    MusicUnit m_clip;
    public RunMusic()
    {
        GameObject go = new GameObject();
        go.transform.SetParent(null);
#if UNITY_EDITOR
        go.name = "RunMusic";
#endif
        go.transform.localPosition = Vector3.zero;
        m_source = go.AddComponent<AudioSource>();
        m_source.loop = true;
    }
    internal void InitData()
    {
        m_clip = new MusicUnit("a_pd",SoundType.RunSound);
        isInit = false;
    }

    internal void RunPlay()
    {
        if (isInit)
        {
            InitData();
        }
        if (m_source.clip==null)
        {
            if (m_clip.m_clip != null)
            {
                m_source.clip = m_clip.m_clip;
            }
        }        
        if (!m_source.isPlaying)
            m_source.Play();
    }
    internal void RunStop()
    {
        if (m_source.isPlaying)
            m_source.Stop();
    }
}
