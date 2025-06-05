using System.Collections.Generic;
using UnityEngine;

public class WeaponAlienProbe : WeaponGeneral
{
	private const float characterOffset = 50f;

	private const float GROUND_HIT_VOLUME_MIN_VELOCITY_LENGTH = 100f;

	private const float GROUND_HIT_VOLUME_MAX_VELOCITY_LENGTH = 1000f;

	private const float DELAY_BETWEEN_PERIODICAL_DAMAGE = 0.1f;

	[SerializeField]
	private float activationDelay;

	[SerializeField]
	private float activatedSpeed;

	[SerializeField]
	private float shotDuration;

	[SerializeField]
	private float shotsDelay;

	[SerializeField]
	private float rayDamage;

	[SerializeField]
	private tk2dSprite rayBack;

	[SerializeField]
	private tk2dSprite rayFront;

	[SerializeField]
	private int maxShotsCount = 5;

	[SerializeField]
	private float blueFireDuration = 3f;

	private bool isShooting;

	private float currentActionTime;

	private bool isPreActivatedDelay;

	private Vector3 startingVelocity;

	private Vector3 activatedVelocity;

	private int curShotsCount;

	private float raysScale = 1f;

	private float curDelayBetweenPeriodicalDamage;

	private List<Collider> affectedColliders;

	private GameObject upperEffect;

	private GameObject bottomEffect;

	private bool IsShooting
	{
		get
		{
			return isShooting;
		}
		set
		{
			if (isShooting != value)
			{
				isShooting = value;
				if (isShooting)
				{
					upperEffect.SetActive(true);
					bottomEffect.SetActive(true);
					AudioManager.PlayAudioWithKey("alien_probe_shot");
				}
				else
				{
					rayBack.scale = new Vector3(raysScale, 0f, 1f);
					rayFront.scale = new Vector3(raysScale, 0f, 1f);
					upperEffect.SetActive(false);
					bottomEffect.SetActive(false);
					CurShotsCount++;
				}
			}
		}
	}

	private int CurShotsCount
	{
		get
		{
			return curShotsCount;
		}
		set
		{
			if (curShotsCount != value)
			{
				curShotsCount = value;
				if (curShotsCount >= maxShotsCount)
				{
					base.CachedRigidbody.useGravity = true;
					IsShooting = false;
					RemoveAllActivations();
					AudioManager.StopAudioWithKey(activationFlyingSound, 1f);
				}
			}
		}
	}

	protected override void Awake()
	{
		IsShooting = false;
		affectedColliders = new List<Collider>();
		raysScale = ((!tk2dSystem.IsRetina) ? 1f : 0.5f);
		EffectsManager instance = SingletonMonoBehaviour<EffectsManager>.Instance;
		upperEffect = Object.Instantiate(instance.EffectPrefabByType(EffectType.WeaponAlienProbeUpLight));
		upperEffect.transform.parent = base.CachedTransform;
		upperEffect.transform.localPosition = Vector3.zero;
		upperEffect.gameObject.SetActive(false);
		bottomEffect = Object.Instantiate(instance.EffectPrefabByType(EffectType.WeaponAlienProbeBottomLight));
		bottomEffect.transform.parent = base.CachedTransform;
		bottomEffect.transform.localPosition = Vector3.zero;
		bottomEffect.gameObject.SetActive(false);
	}

	private float RayLength()
	{
		Vector3 positionOnFloor = level.Location.GetPositionOnFloor(base.CachedTransform.position.x);
		return Mathf.Abs(base.CachedTransform.position.y - positionOnFloor.y);
	}

	private void CheckDamagingCharacter(Character character, float deltaTime)
	{
		float num = character.transform.position.x - 50f;
		float num2 = character.transform.position.x + 50f;
		if (!(num < base.CachedTransform.position.x) || !(base.CachedTransform.position.x < num2))
		{
			return;
		}
		PeriodicalDamageInfo damageInfo = new PeriodicalDamageInfo
		{
			damage = rayDamage * deltaTime,
			weaponType = base.WeaponType,
			type = PeriodicalDamageType.AlienProbeFire,
			duration = blueFireDuration
		};
		character.ApplyPeriodicalDamage(damageInfo);
		curDelayBetweenPeriodicalDamage -= deltaTime;
		if (!(curDelayBetweenPeriodicalDamage <= 0f))
		{
			return;
		}
		List<Collider> list = RayAffectedObjects(character);
		foreach (Collider item in list)
		{
			if (!affectedColliders.Contains(item))
			{
				character.DamageHandler.ApplyPeriodicalDamage(damageInfo, item);
				affectedColliders.Add(item);
			}
		}
		if (list.Count != 0)
		{
			curDelayBetweenPeriodicalDamage = 0.1f;
		}
	}

	private void CheckDamagingCharacters(float deltaTime)
	{
		if ((bool)level.CharacterFirst)
		{
			CheckDamagingCharacter(level.CharacterFirst, deltaTime);
		}
		if ((bool)level.CharacterSecond)
		{
			CheckDamagingCharacter(level.CharacterSecond, deltaTime);
		}
	}

	private List<Collider> RayAffectedObjects(Character character)
	{
		List<Collider> list = new List<Collider>();
		Collider[] componentsInChildren = character.GetComponentsInChildren<Collider>();
		Ray ray = new Ray(new Vector3(base.CachedTransform.position.x, base.CachedTransform.position.y, character.transform.position.z), Vector3.down);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			RaycastHit hitInfo;
			if (componentsInChildren[i] != null && componentsInChildren[i].Raycast(ray, out hitInfo, float.PositiveInfinity))
			{
				list.Add(componentsInChildren[i]);
			}
		}
		list.RemoveAll((Collider obj) => obj.gameObject.layer == LayerMask.NameToLayer("CharacterParts"));
		return list;
	}

	public override void Shoot()
	{
		base.Shoot();
		base.CachedTransform.rotation = Quaternion.identity;
	}

	public override void UpdateWeapon(float deltaTime)
	{
		if (isPreActivatedDelay)
		{
			base.CachedRigidbody.velocity = Vector3.Lerp(startingVelocity, activatedVelocity, currentActionTime);
			currentActionTime += deltaTime;
			if (currentActionTime >= activationDelay)
			{
				currentActionTime = 0f;
				isPreActivatedDelay = false;
				base.CachedRigidbody.velocity = activatedVelocity;
			}
			return;
		}
		base.UpdateWeapon(deltaTime);
		if (!isActive || !isActivated)
		{
			return;
		}
		currentActionTime += deltaTime;
		if (isShooting)
		{
			float num = RayLength();
			rayBack.scale = new Vector3(raysScale, num, 1f);
			rayFront.scale = new Vector3(raysScale, num, 1f);
			bottomEffect.transform.localPosition = new Vector3(0f, 0f - num, 0f);
			CheckDamagingCharacters(deltaTime);
			if (currentActionTime >= shotDuration)
			{
				IsShooting = false;
				currentActionTime = 0f;
			}
		}
		else if (currentActionTime >= shotsDelay)
		{
			IsShooting = true;
			currentActionTime = 0f;
			SingletonMonoBehaviour<GameManager>.Instance.CameraController.ApplyShakeWithDuration(0.1f);
		}
	}

	public override ICollidable Collided(CollisionInfo collisionInfo)
	{
		ICollidable collidable = base.Collided(collisionInfo);
		if (collidable != null)
		{
			Disabled();
			level.RemoveWeapon(this);
		}
		return collidable;
	}

	protected override void OnTapActivation()
	{
		base.CachedRigidbody.useGravity = false;
		startingVelocity = new Vector3(base.CachedRigidbody.velocity.x, 0f, 0f);
		activatedVelocity = new Vector3(base.CachedRigidbody.velocity.normalized.x * activatedSpeed, 0f, 0f);
		base.CachedRigidbody.velocity = Vector3.zero;
		isPreActivatedDelay = true;
	}
}
