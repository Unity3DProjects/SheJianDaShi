using System;
using Spine;
using UnityEngine;

public class Raccoon : Mjolnir
{
	public static Action OnRaccoonExplode;

	[SerializeField]
	private SkeletonAnimation raccoonAnimation;

	[SerializeField]
	private SkeletonDataAsset animationDataAsset;

	[SerializeField]
	[SpineAnimation("", "animationDataAsset")]
	private string preFatalityAnimationName;

	[SerializeField]
	[SpineAnimation("", "animationDataAsset")]
	private string fatalityAnimationName;

	[SerializeField]
	[SpineAnimation("", "animationDataAsset")]
	private string flyAnimationName;

	[SerializeField]
	[SpineAnimation("", "animationDataAsset")]
	private string rocketFlyAnimationName;

	[SerializeField]
	private GameObject springJointRoot;

	[SerializeField]
	private GameObject springJointSecondRoot;

	[SerializeField]
	[ResourceLink]
	protected AssetLink fatalityExplodeEffect;

	[SerializeField]
	[ResourceLink]
	protected AssetLink fatalityTrailEffect;

	[SerializeField]
	[ResourceLink]
	protected AssetLink activationEffect;

	[SerializeField]
	private Transform tailTransform;

	private bool collideWithCharacterWasDone;

	private Rigidbody springJointRigidbody;

	private EventData cachedEventDataExplode;

	private EventData cachedEventDataFalalityFlight;

	public Rigidbody SpringJointRoot
	{
		get
		{
			if (springJointRoot.GetComponent<Rigidbody>() == null)
			{
				springJointRoot.AddComponent<Rigidbody>();
				springJointRoot.GetComponent<Rigidbody>().isKinematic = true;
			}
			return springJointRoot.GetComponent<Rigidbody>();
		}
	}

	public Rigidbody SpringJointSecondRoot
	{
		get
		{
			if (springJointSecondRoot.GetComponent<Rigidbody>() == null)
			{
				springJointSecondRoot.AddComponent<Rigidbody>();
				springJointSecondRoot.GetComponent<Rigidbody>().isKinematic = true;
			}
			return springJointSecondRoot.GetComponent<Rigidbody>();
		}
	}

	private EventData CachedEventDataExplode
	{
		get
		{
			if (cachedEventDataExplode == null)
			{
				cachedEventDataExplode = raccoonAnimation.Skeleton.Data.FindEvent("Explode");
			}
			return cachedEventDataExplode;
		}
	}

	private EventData CachedEventDataFalalityFlight
	{
		get
		{
			if (cachedEventDataFalalityFlight == null)
			{
				cachedEventDataFalalityFlight = raccoonAnimation.Skeleton.Data.FindEvent("FX_Fire");
			}
			return cachedEventDataFalalityFlight;
		}
	}

	private void OnEnable()
	{
		raccoonAnimation.State.Event += RaccoonAnimation_State_Event;
	}

	private void OnDisable()
	{
		raccoonAnimation.State.Event -= RaccoonAnimation_State_Event;
	}

	protected override void OnTriggerEnter(Collider otherCollider)
	{
	}

	public override ICollidable Collided(CollisionInfo collisionInfo)
	{
		ICollidable collidable = base.Collided(collisionInfo);
		if (collidable != null)
		{
			if (collidable is CharacterBase)
			{
				if (!base.Level.IsMainBattleFinished)
				{
					OnHitActivation();
				}
				else
				{
					base.CachedEmission.StopTrails();
				}
			}
			else
			{
				OnHitActivation();
			}
		}
		return collidable;
	}

	public override void Shoot()
	{
		base.Shoot();
		raccoonAnimation.State.SetAnimation(0, flyAnimationName, true);
		isMultipleTimeActivated = false;
	}

	public void EnablePreFatalityAnimation()
	{
		raccoonAnimation.State.SetAnimation(0, preFatalityAnimationName, false);
		AudioManager.PlayAudioWithKey("raccoon_fatality_jump");
	}

	public void EnableFatalityAnimation()
	{
		raccoonAnimation.timeScale = 2f;
		raccoonAnimation.State.SetAnimation(0, fatalityAnimationName, false);
		AudioManager.PlayAudioWithKey("raccoon_fatality_jetpack");
	}

	public override void Activate(WeaponActivationType type, ICollidable collidedObject)
	{
		base.Activate(type, collidedObject);
		raccoonAnimation.State.SetAnimation(0, rocketFlyAnimationName, false);
		InstantiatePrefab((GameObject)activationEffect.GetAsset(), base.Level.FXsTransform, base.CachedTransform.position);
	}

	protected override void OnTapActivation()
	{
		base.CachedRigidbody.useGravity = false;
		base.CachedRigidbody.velocity = base.CachedRigidbody.velocity.normalized * 2f * base.CachedRigidbody.velocity.magnitude;
		if (!base.Level.IsMainBattleFinished)
		{
			WeaponActivation weaponActivation = base.gameObject.AddComponent<WeaponActivation>();
			weaponActivation.Init(WeaponActivationType.Hit, CollideObjectType.All, this, "OnHitActivation");
			weaponActivation.IsAutoRemoved = true;
			AddActivation(weaponActivation);
		}
	}

	protected override void OnHitActivation()
	{
		ExplodeComponent component = GetComponent<ExplodeComponent>();
		if (component != null && !collideWithCharacterWasDone)
		{
			collideWithCharacterWasDone = true;
			component.Explode(this, level, destroyByExplode);
		}
		base.CachedRigidbody.isKinematic = true;
	}

	protected override void OnHit(ICollidable collidedObject)
	{
	}

	protected override void OnExplode(ExplodeComponent component)
	{
	}

	private GameObject InstantiatePrefab(GameObject prefab, Transform parent, Vector3 position)
	{
		GameObject gameObject = null;
		if (prefab != null)
		{
			gameObject = UnityEngine.Object.Instantiate(prefab);
			gameObject.transform.parent = parent;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.position = position;
		}
		return gameObject;
	}

	private void RaccoonAnimation_State_Event(Spine.AnimationState state, int trackIndex, Spine.Event e)
	{
		if (e.Data == CachedEventDataExplode)
		{
			if (OnRaccoonExplode != null)
			{
				OnRaccoonExplode();
			}
			InstantiatePrefab((GameObject)fatalityExplodeEffect.GetAsset(), base.CachedTransform.parent, tailTransform.position);
			AudioManager.StopAudioWithKey("raccoon_fatality_jetpack");
			OnHitActivation();
		}
		if (e.Data == CachedEventDataFalalityFlight)
		{
			InstantiatePrefab((GameObject)fatalityTrailEffect.GetAsset(), tailTransform, tailTransform.position);
		}
	}
}
