using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemColor : MonoBehaviour, ITweenColor
{
	[SerializeField]
	private bool isNeedApplyToAllChildrens = true;

	[SerializeField]
	private List<ParticleSystem> affectedParticleSystems = new List<ParticleSystem>();

	private List<ParticleSystem> cachedParticleSystems;

	private List<ParticleSystem> CachedParticleSystems
	{
		get
		{
			if (cachedParticleSystems == null)
			{
				if (isNeedApplyToAllChildrens)
				{
					cachedParticleSystems = new List<ParticleSystem>();
					GetComponentsInChildren(cachedParticleSystems);
				}
				else
				{
					cachedParticleSystems = affectedParticleSystems;
				}
			}
			return cachedParticleSystems;
		}
	}

	public Color Color
	{
		get
		{
			if (CachedParticleSystems.Count > 0)
			{
				return CachedParticleSystems[0].main.startColor.color;
			}
			return Color.white;
		}
		set
		{
			int i = 0;
			for (int count = CachedParticleSystems.Count; i < count; i++)
			{
				ParticleSystem.MainModule main = CachedParticleSystems[i].main;
				main.startColor = new ParticleSystem.MinMaxGradient(value);
			}
		}
	}
}
