using Spine;
using UnityEngine;

public class Turkey : WeaponGeneral
{
	private const string FLIGHT_ANIMATION_NAME = "flight";

	private const string FLIGHT_HASTE_ANIMATION_NAME = "flight_haste";

	[SerializeField]
	private int eggsCount;

	[SerializeField]
	private float eggVelocity;

	[SerializeField]
	private float eggAngle;

	[SerializeField]
	private Transform eggsSpawnRoot;

	[SerializeField]
	private Transform trailSpawnRoot;

	[SerializeField]
	private SkeletonAnimation turkeyAnimation;

	[SerializeField]
	[ResourceLink]
	protected AssetLink activationEffect;

	[SerializeField]
	[ResourceLink]
	protected AssetLink trailEffect;

	[SerializeField]
	private float fatalityDelay;

	private int currentEggsCount;

	private Emission emission;

	private bool isEggsSpawnActive;

	private bool isFatalityWithDelay;

	private EventData cachedEventDataStart;

	private EventData cachedEventDataEnd;

	private Emission Emission
	{
		get
		{
			if (emission == null)
			{
				emission = GetComponent<Emission>();
			}
			return emission;
		}
	}

	private EventData CachedEventDataStart
	{
		get
		{
			if (cachedEventDataStart == null)
			{
				cachedEventDataStart = turkeyAnimation.Skeleton.Data.FindEvent("Start");
			}
			return cachedEventDataStart;
		}
	}

	private EventData CachedEventDataEnd
	{
		get
		{
			if (cachedEventDataEnd == null)
			{
				cachedEventDataEnd = turkeyAnimation.Skeleton.Data.FindEvent("End");
			}
			return cachedEventDataEnd;
		}
	}

	private void OnEnable()
	{
		turkeyAnimation.State.Event += TurkeyAnimation_State_Event;
	}

	private void OnDisable()
	{
		turkeyAnimation.State.Event -= TurkeyAnimation_State_Event;
	}

	public override ICollidable Collided(CollisionInfo collisionInfo)
	{
		ICollidable collidable = base.Collided(collisionInfo);
		ExplodeComponent component = GetComponent<ExplodeComponent>();
		Character character = collidable as Character;
		if (character != null)
		{
			isFatalityWithDelay = character.IsFinishHim;
		}
		if (component != null)
		{
			component.Explode(this, level, true, true, isFatalityWithDelay, fatalityDelay);
		}
		Disabled(true);
		base.CachedRigidbody.isKinematic = true;
		return collidable;
	}

	public override void Shoot()
	{
		base.Shoot();
		turkeyAnimation.State.SetAnimation(0, "flight", true);
		if (trailEffect != null && !string.IsNullOrEmpty(trailEffect.assetGUID))
		{
			Object.Instantiate(trailEffect.GetAsset(), trailSpawnRoot.position, trailSpawnRoot.rotation, base.CachedTransform);
		}
	}

	protected override void OnTapActivation()
	{
		if (currentEggsCount < eggsCount && !isEggsSpawnActive)
		{
			turkeyAnimation.State.SetAnimation(0, "flight_haste", false);
			if (activationEffect != null && !string.IsNullOrEmpty(activationEffect.assetGUID))
			{
				Object.Instantiate(activationEffect.GetAsset(), eggsSpawnRoot.position, eggsSpawnRoot.rotation, base.CachedTransform);
			}
		}
	}

	private void TurkeyAnimation_State_Event(Spine.AnimationState state, int trackIndex, Spine.Event e)
	{
		if (e.Data == CachedEventDataStart)
		{
			isEggsSpawnActive = true;
			AudioManager.PlayAudioWithKey("egg_create");
			Emission.EmitObjects(base.Level, eggsSpawnRoot.position, eggAngle, eggVelocity);
			currentEggsCount++;
		}
		if (e.Data == CachedEventDataEnd)
		{
			isEggsSpawnActive = false;
		}
	}
}
