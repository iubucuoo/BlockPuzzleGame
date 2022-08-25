using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BaseExpand
{
	public static float GetMaxValue(this ParticleSystem.MinMaxCurve minMaxCurve)
	{
		switch (minMaxCurve.mode)
		{
			case ParticleSystemCurveMode.Constant:
				return minMaxCurve.constant;
			case ParticleSystemCurveMode.TwoConstants:
				return minMaxCurve.constantMax;
				/*
			case ParticleSystemCurveMode.Curve:
				return minMaxCurve.curve.MoveKey.GetMaxValue();
			case ParticleSystemCurveMode.TwoCurves:
				var ret1 = minMaxCurve.curveMin.GetMaxValue();
				var ret2 = minMaxCurve.curveMax.GetMaxValue();
				return ret1 > ret2 ? ret1 : ret2;
				*/
		}
		return -1f;
	}
	public static float GetDuration(this ParticleSystem particle, bool allowLoop = false)
	{
		if (!particle.emission.enabled) return 0f;
		if (particle.main.loop && !allowLoop)
		{
			return -1f;
		}
		if (true/*particle.emission.rateOverTime.GetMinValue() <= 0*/)
		{
			return particle.main.startDelay.GetMaxValue() + particle.main.startLifetime.GetMaxValue();
		}
		else
		{
			return particle.main.startDelay.GetMaxValue() + Mathf.Max(particle.main.duration, particle.main.startLifetime.GetMaxValue());
		}
	}
	public static float GetParticleDuration(this GameObject gameObject, bool includeChildren = true, bool includeInactive = false, bool allowLoop = false)
	{
		if (includeChildren)
		{
			var particles = gameObject.GetComponentsInChildren<ParticleSystem>(includeInactive);
			var duration = -1f;
			for (var i = 0; i < particles.Length; i++)
			{
				var ps = particles[i];
				var time = ps.GetDuration(allowLoop);
				if (time > duration)
				{
					duration = time;
				}
			}

			return duration;
		}
		else
		{
			var ps = gameObject.GetComponent<ParticleSystem>();
			if (ps != null)
			{
				return ps.GetDuration(allowLoop);
			}
			else
			{
				return -1f;
			}
		}
	}
}
