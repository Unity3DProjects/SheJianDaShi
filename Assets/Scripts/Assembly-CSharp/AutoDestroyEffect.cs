using UnityEngine;

public class AutoDestroyEffect : ObjectFromPool
{
	[SerializeField]
	private ParticleSystem[] systems;

	[SerializeField]
	private float forcedRemoveTime = -1f;

	private ParticleSystem mainSystem;

	private float lifetime;

	public bool IsPaused { get; set; }

	public ParticleSystem MainSystem
	{
		get
		{
			if (mainSystem == null)
			{
				mainSystem = GetComponent<ParticleSystem>();
			}
			return mainSystem;
		}
	}

	private void Update()
	{
		if (!IsPaused)
		{
			lifetime -= Time.deltaTime;
			if (lifetime < 0f)
			{
				ReturnToPool();
			}
		}
	}

	public override void OnCreateFromPool()
	{
		if (forcedRemoveTime > 0f)
		{
			lifetime = forcedRemoveTime;
			return;
		}
		ParticleSystem[] array = systems;
		foreach (ParticleSystem particleSystem in array)
		{
			if (particleSystem.main.duration > lifetime)
			{
				lifetime = particleSystem.main.duration;
			}
			particleSystem.Play();
		}
	}

	public override void OnReturnToPool()
	{
		StopSystems();
	}

	public void StopSystems()
	{
		ParticleSystem[] array = systems;
		foreach (ParticleSystem particleSystem in array)
		{
			particleSystem.Stop();
		}
	}
}
