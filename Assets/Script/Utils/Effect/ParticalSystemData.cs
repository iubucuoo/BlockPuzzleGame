using System;
using UnityEngine;

[Serializable]
public class ParticalSystemData : MonoBehaviour
{
	GameObject _CacheGameObject;
	public ParticleSystem[] particalSystems;
	public Animator[] animators;
	public int[] fullPathHashes;
	public Transform root;
	public float _LongTime;
	public float _PlayTime;
	public bool isLoop;
	public bool[] _Emittings;
	public bool isCreateShow = true;//立即显示
	public bool _ActionStopDispose;
	bool _IsPlay;
	bool isinit=false;
	public TrailRenderer[] trailRenders;
	public LineRenderer[] lineRenders;
	public GameObject[] _Gos;
    
	Action _Over;

	private void Awake()
	{		
		Init();
		if (!isCreateShow)
		{
			ReleaseSkillsSerialize(false);
		}
    }
	void Init()
	{
		if (!isinit)
		{
			_CacheGameObject = gameObject;
			isinit = true;
		}		
	}
	public void Update()
	{
		JsTime();
	}
	void JsTime()
	{
		if (_Over != null && (_PlayTime > _LongTime || _ActionStopDispose))
		{
			ReleaseSkillsSerialize(false);
			_Over();
			_Over = null;
			ChangeLayer(0);
		}
		_PlayTime += Time.unscaledDeltaTime;
		if (_IsPlay == false && (_PlayTime - _LongTime) > 10)//10s之后就需要关闭gameobject
		{
			SetActive(false);
		}
    }
	void ChangeLayer(int b)
	{
        //internal const int EFFECT_LAYER = 8;
        //internal const int HIDE_LAYER = 0;
        var len = _Gos.Length;
		for (int i = 0; i < len; i++)
		{
			_Gos[i].layer = b;
		}
	}
	void SetPosition(LineRenderer line, Vector3 p1, Vector3 p2)
	{
		line.SetPosition(0, p1);
		line.SetPosition(1, p2);
	}
	void SetActive(bool t)
	{
		if (isinit)
		{
			_CacheGameObject.SetActive(t);
		}		
	}
	public void Reset(Action over)
	{		
		if (isinit&&_CacheGameObject.activeSelf==false)
		{
			over();
		}
		else
		{
			_Over = over;
		}
	}
	internal void ReleaseSkillsSerialize(bool play)
	{
		try
		{
			_IsPlay = play;
			if (play)
			{
				ChangeLayer(8);
				SetActive(play);
				_PlayTime = 0;
			}
			if (particalSystems != null)
			{
				for (int i = 0, len = particalSystems.Length; i < len; i++)
				{
					ParticleSystem ps = particalSystems[i];
					if (play)
					{
						ps.Simulate(0);
						ps.Play();
					}
					else
					{
						ps.Stop();
					}
				}
			}
			for (int i = 0, len = trailRenders.Length; i < len; i++)
			{
				TrailRenderer tr = trailRenders[i];
				tr.enabled = play;
				tr.emitting = _Emittings[i] ? play : false;
				if (!tr.emitting)
				{
					tr.Clear();
				}
			}
			for (int i = 0, len = animators.Length; i < len; i++)
			{
				if (play)
				{
					Animator temp = animators[i];
					temp.Play(fullPathHashes[i], 0, 0);
				}
			}
		}
		catch (Exception e)
		{
			Log.Error(gameObject.name + "特效资源有点问题，需要点一下脚本上面的[绑定数据] \n" + e.ToString());
		}
	}
}
