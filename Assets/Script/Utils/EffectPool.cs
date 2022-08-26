using UnityEngine;
using System.Collections;
using PathologicalGames;
using DG.Tweening;
/// <summary>
/// 特效池
/// </summary>
public class EffectPool : MonoBehaviour {
	public static EffectPool Inst ;
	SpawnPool pool;
    EffectCtrl _ReleaseEff = new EffectCtrl();
    void Awake()
	{
		Inst = this;
	}
	// Use this for initialization
	void Start () {
		pool = PoolManager.Pools["EffectPool"];
	}
    public void PlayEffect(string PkgName, string ResName)
    {
        _ReleaseEff.Init(PkgName, ResName);
    }
    /// <summary>
    /// Play the specified name and position.
    /// </summary>
    /// <param name="name">Name.</param>
    /// <param name="position">Position.</param>
    public void Play(string name,Vector3 position)
	{
		Transform particleTran = pool.Spawn (name);
		if (particleTran == null) {
			return;
		}
		ParticleSystem particle = particleTran.GetComponent<ParticleSystem> ();
		if (particle == null) {
			return;
		}
        particleTran.position = position;
		particle.Play ();
		StartCoroutine (Recycle(particle));
	}

	IEnumerator Recycle(ParticleSystem particle)
	{
        float time = particle.main.duration;
		yield return new WaitForSeconds(time +0.1f);
        pool.Despawn (particle.transform);
	}

    public void PlayBubbleExplode(int type, Vector3 pos)
	{
		string effectName = "";
	    if (type == 1) {
			effectName = "BubbleExplodeYellow";
		} else  if (type == 2) {
			effectName = "BubbleExplodeRed";
		} else  if (type == 3) {
			effectName = "BubbleExplodeBlue";
		} else  if (type == 4) {
			effectName = "BubbleExplodeOrange";
		} else  if (type == 5) {
			effectName = "BubbleExplodeGreen";
		} else {
			effectName = "BubbleExplodeGreen";
		}
		Play (effectName,pos);
	}

	public void PlayFlowEffect(Vector3 pos, Vector3 endpos, System.Action cb=null)
	{
		StartCoroutine (IEPlayFlowEffect(pos, endpos,cb));
	}
    WaitForSeconds Secounds = new WaitForSeconds(1f);
    Vector3[] path = new Vector3[3];
    IEnumerator IEPlayFlowEffect(Vector3 pos,Vector3 endpos, System.Action cb = null)
	{
		Transform particleTran = pool.Spawn ("FlowEffect");
		ParticleSystem particle = particleTran.GetComponent<ParticleSystem> ();
		particleTran.position = pos;
		particle.Play ();
        path[0] = pos;
        path[1] = new Vector3( (endpos.x - pos.x)/2+ pos.x, 200,0);
        path[2] = endpos;
        particleTran.DOLocalPath(path, .8f, PathType.CatmullRom);

        //particleTran.DOMove (endpos, 0.7f);
        yield return Secounds;
		pool.Despawn (particleTran);
        if (cb!=null)
        {
            cb();
        }
	}
}
