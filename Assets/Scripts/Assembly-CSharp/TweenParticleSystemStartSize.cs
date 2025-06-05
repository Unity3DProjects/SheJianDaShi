using UnityEngine;

[AddComponentMenu("Inventain/Tween/ParticleSystemStartSize")]
public class TweenParticleSystemStartSize : Tweener
{
	[SerializeField]
	private float endSize = 1f;

	[SerializeField]
	private float beginSize;

	private ParticleSystem targetParticleSystem;

	public float EndSize
	{
		get
		{
			return endSize;
		}
		set
		{
			endSize = value;
		}
	}

	public float BeginSize
	{
		get
		{
			return beginSize;
		}
		set
		{
			beginSize = value;
		}
	}

	public ParticleSystem TargetParticleSystem
	{
		get
		{
			if (targetParticleSystem == null)
			{
				targetParticleSystem = GetComponent<ParticleSystem>();
			}
			return targetParticleSystem;
		}
	}

	public float CurrentSize
	{
		get
		{
			return TargetParticleSystem.main.startSizeMultiplier;
		}
		set
		{
			ParticleSystem.MainModule main = TargetParticleSystem.main;
			main.startSizeMultiplier = value;
		}
	}

	public static TweenParticleSystemStartSize SetSize(GameObject go, float size, float duration = 1f)
	{
		TweenParticleSystemStartSize tweenParticleSystemStartSize = Tweener.InitGO<TweenParticleSystemStartSize>(go);
		tweenParticleSystemStartSize.BeginSize = tweenParticleSystemStartSize.CurrentSize;
		tweenParticleSystemStartSize.EndSize = size;
		tweenParticleSystemStartSize.duration = duration;
		tweenParticleSystemStartSize.Play(true);
		return tweenParticleSystemStartSize;
	}

	protected override void TweenUpdateRuntime(float factor, bool isFinished)
	{
		CurrentSize = BeginSize + (EndSize - BeginSize) * factor;
	}

	protected override void TweenUpdateEditor(float factor)
	{
		CurrentSize = BeginSize + (EndSize - BeginSize) * factor;
	}
}
