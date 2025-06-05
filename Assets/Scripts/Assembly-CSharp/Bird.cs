using Spine;
using UnityEngine;

public class Bird : FlyingObject
{
	[SerializeField]
	[ResourceLink]
	private AssetLink bloodEffectPrefab;

	[SerializeField]
	private SkeletonAnimation birdSkeletonAnimationFly;

	[SerializeField]
	private MeshRenderer birdRendererAnimationFly;

	[SerializeField]
	private SkeletonAnimation birdSkeletonAnimationDead;

	[SerializeField]
	private MeshRenderer birdRendererAnimationDead;

	[SerializeField]
	private SkeletonRagdoll birdRagdollDead;

	[SerializeField]
	private float velocityImpulseMin;

	[SerializeField]
	private float velocityImpulseMax;

	[SerializeField]
	private float destroyBirdDelay;

	[SerializeField]
	[ResourceLink]
	private AssetLink birdMeatEffectPrefab;

	[SerializeField]
	private float timeAddMin;

	[SerializeField]
	private float timeAddMax;

	private Spine.Animation animationFly;

	private bool isKilled;

	private Skeleton Skeleton
	{
		get
		{
			return birdSkeletonAnimationFly.Skeleton;
		}
	}

	private Spine.AnimationState AnimationStateFly
	{
		get
		{
			return birdSkeletonAnimationFly.state;
		}
	}

	private Spine.AnimationState AnimationStateDead
	{
		get
		{
			return birdSkeletonAnimationDead.state;
		}
	}

	private Spine.Animation AnimationFly
	{
		get
		{
			if (animationFly == null)
			{
				animationFly = AnimationStateFly.Data.SkeletonData.FindAnimation("fly");
			}
			return animationFly;
		}
	}

	private Spine.Animation AnimationFire
	{
		get
		{
			return AnimationStateDead.Data.SkeletonData.FindAnimation("fire");
		}
	}

	private Spine.Animation AnimationShock
	{
		get
		{
			return AnimationStateDead.Data.SkeletonData.FindAnimation("shock");
		}
	}

	public Rigidbody RagdollRootRigid
	{
		get
		{
			return birdRagdollDead.RootRigidbody;
		}
	}

	private void Awake()
	{
		birdSkeletonAnimationFly.Reset();
		birdSkeletonAnimationDead.Reset();
		birdRendererAnimationFly.enabled = true;
		birdRendererAnimationDead.enabled = false;
	}

	public override void Init(LevelBirdsHunt level)
	{
		base.Init(level);
		AnimationStateFly.SetAnimation(0, AnimationFly, true);
	}

	public override void UpdateObject(float deltaTime)
	{
		if (isKilled)
		{
			return;
		}
		if (base.IsFlying)
		{
			flyTime += deltaTime;
			if (flyTime > currentPeriod)
			{
				flyTime -= currentPeriod;
				currentPeriod = Mathf.Max(Random.Range(periodMin, periodMax), 0.1f);
				currentAmplitude = Random.Range(amplitudeMin, amplitudeMax);
			}
			base.UpdateObject(deltaTime);
			birdSkeletonAnimationFly.Update(deltaTime);
			return;
		}
		isKilled = true;
		base.Level.CachedTimer.AddTime(Random.Range(timeAddMin, timeAddMax));
		SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
		{
			if (this != null)
			{
				EmitMeat();
			}
		}, destroyBirdDelay);
	}

	public override void DamageApplied(FlyingObjectDamageType type, Vector2 direction)
	{
		if (base.IsFlying)
		{
			base.Level.IncreaseScore();
			if (bloodEffectPrefab != null && !string.IsNullOrEmpty(bloodEffectPrefab.assetGUID))
			{
				GameObject gameObject = (GameObject)bloodEffectPrefab.GetInstance();
				gameObject.transform.parent = base.Level.FXsTransform;
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localRotation = Quaternion.identity;
				gameObject.transform.position = base.transform.position;
			}
			base.IsFlying = false;
			startPosition = base.CachedTransform.position;
			Skeleton.SetToSetupPose();
			AnimationStateFly.ClearTracks();
			birdRendererAnimationFly.enabled = false;
			birdRendererAnimationDead.enabled = true;
			birdRagdollDead.Apply();
			Rigidbody[] rigidbodyArray = birdRagdollDead.GetRigidbodyArray();
			for (int i = 0; i < rigidbodyArray.Length; i++)
			{
				rigidbodyArray[i].gameObject.layer = base.gameObject.layer;
			}
			RagdollRootRigid.velocity = direction.normalized * Random.Range(velocityImpulseMin, velocityImpulseMax);
			bodyCollider.gameObject.transform.parent = RagdollRootRigid.transform;
		}
		switch (type)
		{
		case FlyingObjectDamageType.Fire:
			AnimationStateDead.SetAnimation(0, AnimationFire, false);
			break;
		case FlyingObjectDamageType.Electricity:
			AnimationStateDead.SetAnimation(0, AnimationShock, true);
			break;
		}
	}

	private void EmitMeat()
	{
		LevelBirdsHunt levelBirdsHunt = base.Level;
		int value = Random.Range(goldMin, goldMax);
		SingletonMonoBehaviour<StatisticsManager>.Instance.AddCoins(value);
		if (birdMeatEffectPrefab != null && !string.IsNullOrEmpty(birdMeatEffectPrefab.assetGUID))
		{
			GameObject gameObject = birdMeatEffectPrefab.GetInstance() as GameObject;
			gameObject.transform.position = RagdollRootRigid.transform.position;
			gameObject.transform.parent = levelBirdsHunt.FXsTransform;
		}
		levelBirdsHunt.RemoveFlyObject(this);
		Object.Destroy(base.gameObject);
	}

	public override bool OnCollision(CollisionInfo collisionInfo)
	{
		return base.IsFlying;
	}

	public override void ApplyDamage(CollisionInfo collisionInfo)
	{
		DamageApplied(FlyingObjectDamageType.None, collisionInfo.direction);
		SingletonMonoBehaviour<TutorialManager>.Instance.IsBirdsGameTutorialInProgress = false;
	}
}
