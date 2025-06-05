using UnityEngine;

public static class GameObjectAnimationExtension
{
	public static void SpeedFX(this GameObject go, float speed)
	{
		Animation[] componentsInChildren = go.GetComponentsInChildren<Animation>();
		Animation[] array = componentsInChildren;
		foreach (Animation animation in array)
		{
			foreach (AnimationState item in animation)
			{
				item.speed = speed;
			}
		}
		ParticleSystem[] componentsInChildren2 = go.GetComponentsInChildren<ParticleSystem>();
		ParticleSystem[] array2 = componentsInChildren2;
		foreach (ParticleSystem particleSystem in array2)
		{
			ParticleSystem.MainModule main = particleSystem.main;
			main.simulationSpeed = speed;
		}
	}

	public static void ResetAnimationToStart(this GameObject go)
	{
		Animation[] componentsInChildren = go.GetComponentsInChildren<Animation>();
		Animation[] array = componentsInChildren;
		foreach (Animation animation in array)
		{
			animation.Play();
			animation.Rewind();
			animation.Sample();
			animation.Stop();
		}
	}

	public static void ResetFX(this GameObject go)
	{
		ParticleSystem[] componentsInChildren = go.GetComponentsInChildren<ParticleSystem>();
		ParticleSystem[] array = componentsInChildren;
		foreach (ParticleSystem particleSystem in array)
		{
			particleSystem.Clear();
			particleSystem.Play();
		}
		Animation[] componentsInChildren2 = go.GetComponentsInChildren<Animation>();
		Animation[] array2 = componentsInChildren2;
		foreach (Animation animation in array2)
		{
			animation.Stop();
			animation.Rewind();
			animation.Play();
		}
	}

	public static void StopFX(this GameObject go, bool cleanup)
	{
		ParticleSystem[] componentsInChildren = go.GetComponentsInChildren<ParticleSystem>();
		ParticleSystem[] array = componentsInChildren;
		foreach (ParticleSystem particleSystem in array)
		{
			particleSystem.Stop();
			if (cleanup)
			{
				particleSystem.Clear(true);
			}
		}
		Animation[] componentsInChildren2 = go.GetComponentsInChildren<Animation>();
		Animation[] array2 = componentsInChildren2;
		foreach (Animation animation in array2)
		{
			animation.Stop();
		}
	}

	public static void PlayFX(this GameObject go)
	{
		ParticleSystem[] componentsInChildren = go.GetComponentsInChildren<ParticleSystem>();
		ParticleSystem[] array = componentsInChildren;
		foreach (ParticleSystem particleSystem in array)
		{
			particleSystem.Play();
		}
		Animation[] componentsInChildren2 = go.GetComponentsInChildren<Animation>();
		Animation[] array2 = componentsInChildren2;
		foreach (Animation animation in array2)
		{
			animation.Play();
		}
	}
}
