using System;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemHandler : MonoBehaviour
{
	public Action<ParticleSystemHandler> OnReturnToPool;

	[SerializeField]
	private List<ParticleSystem> particleSystems = new List<ParticleSystem>();

	[SerializeField]
	private bool isPlayOnAwake = true;

	[SerializeField]
	private bool isLooping;

	private Transform cachedTransform;

	public Transform CachedTransform
	{
		get
		{
			if (cachedTransform == null)
			{
				cachedTransform = base.transform;
			}
			return cachedTransform;
		}
	}

	public bool IsPlayOnAwake
	{
		get
		{
			return isPlayOnAwake;
		}
		set
		{
			isPlayOnAwake = value;
		}
	}

	public bool IsLooping
	{
		get
		{
			return isLooping;
		}
		set
		{
			isLooping = value;
		}
	}

	public List<ParticleSystem> ParticleSystems
	{
		get
		{
			return particleSystems;
		}
	}

	public bool isPaused
	{
		get
		{
			return !(particleSystems.FirstObject() != null) || particleSystems.FirstObject().isPaused;
		}
	}

	public bool isPlaying
	{
		get
		{
			return particleSystems.FirstObject() != null && particleSystems.FirstObject().isPlaying;
		}
	}

	public bool isStopped
	{
		get
		{
			return !(particleSystems.FirstObject() != null) || particleSystems.FirstObject().isStopped;
		}
	}

	private void Awake()
	{
		if (isPlayOnAwake)
		{
			Play();
		}
	}

	public bool IsAlive()
	{
		bool result = false;
		int i = 0;
		for (int count = particleSystems.Count; i < count; i++)
		{
			if (particleSystems[i] == null)
			{
				continue;
			}
			ParticleSystem particleSystem = particleSystems[i];
			if (particleSystem != null && particleSystem.IsAlive())
			{
				result = true;
				break;
			}
		}
		return result;
	}

	public void SetPlayDelay(float delay)
	{
		for (int i = 0; i < particleSystems.Count; i++)
		{
			ParticleSystem particleSystem = particleSystems[i];
			particleSystem.startDelay = delay;
		}
	}

	public void Clear()
	{
		int i = 0;
		for (int count = particleSystems.Count; i < count; i++)
		{
			ParticleSystem particleSystem = particleSystems[i];
			if (particleSystem != null)
			{
				particleSystem.Clear();
			}
		}
	}

	public void Play(bool withChildren = true)
	{
		int i = 0;
		for (int count = particleSystems.Count; i < count; i++)
		{
			ParticleSystem particleSystem = particleSystems[i];
			if (particleSystem != null)
			{
				particleSystem.Play(withChildren);
			}
		}
	}

	public void Stop()
	{
		int i = 0;
		for (int count = particleSystems.Count; i < count; i++)
		{
			ParticleSystem particleSystem = particleSystems[i];
			if (particleSystem != null)
			{
				particleSystem.Stop();
			}
		}
	}

	public void Pause()
	{
		int i = 0;
		for (int count = particleSystems.Count; i < count; i++)
		{
			ParticleSystem particleSystem = particleSystems[i];
			if (particleSystem != null)
			{
				particleSystem.Pause();
			}
		}
	}

	public void Simulate(float t)
	{
		int i = 0;
		for (int count = particleSystems.Count; i < count; i++)
		{
			ParticleSystem particleSystem = particleSystems[i];
			if (particleSystem != null)
			{
				particleSystem.Simulate(t);
			}
		}
	}

	public void Simulate(float t, bool withChildren)
	{
		int i = 0;
		for (int count = particleSystems.Count; i < count; i++)
		{
			ParticleSystem particleSystem = particleSystems[i];
			if (particleSystem != null)
			{
				particleSystem.Simulate(t, withChildren);
			}
		}
	}

	public void Simulate(float t, bool withChildren, bool restart)
	{
		int i = 0;
		for (int count = particleSystems.Count; i < count; i++)
		{
			ParticleSystem particleSystem = particleSystems[i];
			if (particleSystem != null)
			{
				particleSystem.Simulate(t, withChildren, restart);
			}
		}
	}

	public void Simulate(float t, bool withChildren, bool restart, bool fixedTimeStep)
	{
		int i = 0;
		for (int count = particleSystems.Count; i < count; i++)
		{
			ParticleSystem particleSystem = particleSystems[i];
			if (particleSystem != null)
			{
				particleSystem.Simulate(t, withChildren, restart, fixedTimeStep);
			}
		}
	}
}
