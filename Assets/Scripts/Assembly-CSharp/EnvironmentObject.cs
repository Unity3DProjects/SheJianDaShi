using Spine;
using UnityEngine;

public class EnvironmentObject : MonoBehaviour
{
	[SerializeField]
	private SkeletonAnimation animationEnvironment;

	private Spine.Animation animationIdle;

	internal Spine.AnimationState EnvironmentState
	{
		get
		{
			return animationEnvironment.state;
		}
	}

	internal Skeleton EnvironmentSkeleton
	{
		get
		{
			return animationEnvironment.Skeleton;
		}
	}

	internal Spine.Animation AnimationIdle
	{
		get
		{
			if (animationIdle == null)
			{
				animationIdle = EnvironmentState.Data.SkeletonData.FindAnimation("idle");
			}
			return animationIdle;
		}
	}

	private void Awake()
	{
		animationEnvironment.Reset();
		StartEnvironment();
	}

	public virtual void StartEnvironment()
	{
		EnvironmentState.SetAnimation(0, AnimationIdle, true);
	}

	public virtual void UpdateEnvironment(float deltaTime)
	{
		animationEnvironment.Update(deltaTime);
	}
}
