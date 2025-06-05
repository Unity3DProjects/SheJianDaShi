using System.Collections;
using UnityEngine;

public class MagicArrow : WeaponGeneral
{
	[SerializeField]
	private float rotationTime;

	[SerializeField]
	private float activationRadius;

	[SerializeField]
	private float rotationVelocity;

	[SerializeField]
	private float bodyRotationVelocity;

	[SerializeField]
	private float pushDownForce;

	[SerializeField]
	private Collider weaponCollider;

	[SerializeField]
	private float groundCollisionDamage = 10f;

	[SerializeField]
	private float velocityPerTurnMoveCharacterUp;

	[SerializeField]
	private float velocityMoveCharacterUp;

	private bool rotateArcher;

	private float currentRotationTime;

	private float rotationRadius;

	private float rotationAngle;

	private float centriperalVelocity;

	private Rigidbody[] bodies;

	private static bool isDamageApplied;

	private static bool isTapActivated;

	private Character nearestCharacter;

	private Rigidbody[] nearestCharacterRigidbodies;

	private GameObject holeEffect;

	private bool moveCharacterDown;

	private bool firstTimeMoveUp;

	protected virtual void OnEnable()
	{
		CharacterBase.OnCharacterGroundCollided += CharacterBase_OnCharacterGroundCollided;
	}

	protected virtual void OnDisable()
	{
		CharacterBase.OnCharacterGroundCollided -= CharacterBase_OnCharacterGroundCollided;
	}

	public override ICollidable Collided(CollisionInfo collisionInfo)
	{
		if (!isActivated)
		{
			collisionInfo.fatalityActvationType = FatalityActivationType.Collision;
		}
		ICollidable collidable = base.Collided(collisionInfo);
		if (collidable != null && base.Activations.Exists((WeaponActivation t) => t.Type == WeaponActivationType.Tap))
		{
			RemoveAllActivations();
		}
		if (collidable is CharacterBase)
		{
			AudioManager.PlayAudioWithKey("card_activation");
		}
		return collidable;
	}

	private IEnumerator CoroutineDestroyEffect(float delay)
	{
		yield return new WaitForSeconds(delay);
		Object.Destroy(holeEffect);
		level.RemoveWeapon(this);
	}

	protected override void OnTapActivation()
	{
		if (!level.IsMultiplayerSecondaryPlayer)
		{
			if (level.GameType == GameType.AppleShoot || level.GameType == GameType.BirdsHunt)
			{
				base.transform.localScale = Vector3.zero;
				SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
				{
					holeEffect = Object.Instantiate(SingletonMonoBehaviour<EffectsManager>.Instance.EffectPrefabByType(EffectType.WeaponMagicArrowHoleActivation));
					holeEffect.transform.parent = SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.gameObject.transform;
					holeEffect.transform.position = base.CachedTransform.position;
				}, 0f);
				ExplodeComponent component = GetComponent<ExplodeComponent>();
				if (component != null)
				{
					component.Explode(this, level);
				}
				weaponCollider.enabled = false;
				Disabled();
				StartCoroutine(CoroutineDestroyEffect(3f));
			}
			else
			{
				float num = Vector3.Distance(level.CharacterFirst.transform.position, base.CachedTransform.position);
				float num2 = Vector3.Distance(level.CharacterSecond.transform.position, base.CachedTransform.position);
				float num3 = Mathf.Min(num, num2);
				nearestCharacter = ((!(num < num2)) ? level.CharacterSecond : level.CharacterFirst);
				if (num3 <= activationRadius)
				{
					if (nearestCharacter.IsFinishHim)
					{
						ExplosionInfo explosionInfo = new ExplosionInfo
						{
							weaponType = base.WeaponType,
							damage = 0f,
							force = 0f,
							minForce = 0f,
							position = base.transform.position,
							radius = activationRadius
						};
						nearestCharacter.ApplyExplosionForce(explosionInfo);
						Disabled();
						level.RemoveWeapon(this);
					}
					else
					{
						base.transform.localScale = Vector3.zero;
						nearestCharacter.ApplyCharacterRagdoll(true);
						nearestCharacter.ForceNotRestore = true;
						nearestCharacterRigidbodies = nearestCharacter.RagdollRigidbodies;
						nearestCharacterRigidbodies[0].useGravity = false;
						rotateArcher = true;
						rotationRadius = Vector3.Distance(nearestCharacterRigidbodies[0].position, base.CachedTransform.position);
						rotationAngle = Mathf.Atan2(nearestCharacterRigidbodies[0].position.y - base.CachedTransform.position.y, nearestCharacterRigidbodies[1].position.x - base.CachedTransform.position.x);
						centriperalVelocity = num3 / rotationTime;
						for (int num4 = 0; num4 < nearestCharacterRigidbodies.Length; num4++)
						{
							nearestCharacterRigidbodies[num4].interpolation = RigidbodyInterpolation.Extrapolate;
							nearestCharacterRigidbodies[num4].GetComponent<Collider>().gameObject.layer = LayerMask.NameToLayer("IgnoreCollisions");
						}
						base.CachedRigidbody.isKinematic = true;
						weaponCollider.enabled = false;
						SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
						{
							holeEffect = Object.Instantiate(SingletonMonoBehaviour<EffectsManager>.Instance.EffectPrefabByType(EffectType.WeaponMagicArrowHoleActivation));
							holeEffect.transform.parent = SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.gameObject.transform;
							holeEffect.transform.position = base.CachedTransform.position;
						}, 0f);
						isTapActivated = true;
						isDamageApplied = false;
					}
				}
				else
				{
					base.transform.localScale = Vector3.zero;
					SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
					{
						holeEffect = Object.Instantiate(SingletonMonoBehaviour<EffectsManager>.Instance.EffectPrefabByType(EffectType.WeaponMagicArrowHoleActivation));
						holeEffect.transform.parent = SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.gameObject.transform;
						holeEffect.transform.position = base.CachedTransform.position;
					}, 0f);
					weaponCollider.enabled = false;
					Disabled();
					StartCoroutine(CoroutineDestroyEffect(3f));
				}
			}
		}
		else
		{
			float num5 = Vector3.Distance(level.CharacterFirst.transform.position, base.CachedTransform.position);
			float num6 = Vector3.Distance(level.CharacterSecond.transform.position, base.CachedTransform.position);
			nearestCharacter = ((!(num5 < num6)) ? level.CharacterSecond : level.CharacterFirst);
			if (!nearestCharacter.IsFinishHim)
			{
				base.transform.localScale = Vector3.zero;
				SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
				{
					holeEffect = Object.Instantiate(SingletonMonoBehaviour<EffectsManager>.Instance.EffectPrefabByType(EffectType.WeaponMagicArrowHoleActivation));
					holeEffect.transform.parent = SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.gameObject.transform;
					holeEffect.transform.position = base.CachedTransform.position;
				}, 0f);
				weaponCollider.enabled = false;
				Disabled();
				StartCoroutine(CoroutineDestroyEffect(3f));
			}
			else
			{
				ExplosionInfo explosionInfo2 = new ExplosionInfo
				{
					weaponType = base.WeaponType,
					damage = 0f,
					force = 0f,
					minForce = 0f,
					position = base.transform.position,
					radius = activationRadius
				};
				nearestCharacter.ApplyExplosionForce(explosionInfo2);
				Disabled();
				level.RemoveWeapon(this);
			}
		}
		AudioManager.PlayAudioWithKey("alien_probe_idle");
		AudioManager.PlayAudioWithKey("magic_arrow_activation_1");
	}

	protected override void UpdatePhysicalWeapon(float deltaTime)
	{
		base.UpdatePhysicalWeapon(deltaTime);
		if (level.IsMultiplayerSecondaryPlayer || !rotateArcher)
		{
			return;
		}
		currentRotationTime += deltaTime;
		if (currentRotationTime < rotationTime)
		{
			rotationRadius -= centriperalVelocity * deltaTime;
			rotationAngle += rotationVelocity * deltaTime;
			Vector3 vector = new Vector3(Mathf.Cos(rotationAngle), Mathf.Sin(rotationAngle)) * rotationRadius + base.CachedTransform.position;
			Vector3 vector2 = vector - nearestCharacterRigidbodies[0].position;
			if (level.IsMultiplayerSecondaryPlayer)
			{
				return;
			}
			nearestCharacterRigidbodies[0].rotation = Quaternion.Euler(nearestCharacterRigidbodies[0].rotation.eulerAngles + new Vector3(0f, 0f, bodyRotationVelocity * deltaTime));
			for (int i = 0; i < nearestCharacterRigidbodies.Length; i++)
			{
				if (nearestCharacterRigidbodies[i] != null)
				{
					if (i == 0)
					{
						nearestCharacterRigidbodies[i].position += vector2;
					}
					else
					{
						nearestCharacterRigidbodies[i].velocity += vector2 * (1f / deltaTime);
					}
				}
			}
			return;
		}
		if (moveCharacterDown)
		{
			rotateArcher = false;
			nearestCharacter.ForceNotRestore = false;
			for (int j = 0; j < nearestCharacterRigidbodies.Length; j++)
			{
				nearestCharacterRigidbodies[j].useGravity = true;
				nearestCharacterRigidbodies[j].GetComponent<Collider>().gameObject.layer = LayerMask.NameToLayer("CharacterParts");
			}
			if (!level.IsMultiplayerSecondaryPlayer)
			{
				for (int k = 0; k < nearestCharacterRigidbodies.Length; k++)
				{
					nearestCharacterRigidbodies[k].velocity = new Vector3(0f, 0f - pushDownForce, 0f);
				}
			}
			nearestCharacter.AddPhysicsObserver();
			return;
		}
		if (!firstTimeMoveUp)
		{
			firstTimeMoveUp = true;
			for (int l = 0; l < nearestCharacterRigidbodies.Length; l++)
			{
				if (!level.IsMultiplayerSecondaryPlayer)
				{
					nearestCharacterRigidbodies[l].velocity = Vector3.zero;
				}
				nearestCharacterRigidbodies[l].useGravity = false;
			}
		}
		Disabled();
		MoveCharacterUp();
	}

	protected override void CheckDisablePhysicalWeapon(float deltaTime, bool flying = false)
	{
	}

	protected void MoveCharacterUp()
	{
		if (nearestCharacterRigidbodies[0].velocity.y <= velocityMoveCharacterUp)
		{
			if (!level.IsMultiplayerSecondaryPlayer)
			{
				for (int i = 0; i < nearestCharacterRigidbodies.Length; i++)
				{
					nearestCharacterRigidbodies[i].velocity += velocityPerTurnMoveCharacterUp * Vector3.up;
				}
			}
			return;
		}
		if (!level.IsMultiplayerSecondaryPlayer)
		{
			for (int j = 0; j < nearestCharacterRigidbodies.Length; j++)
			{
				nearestCharacterRigidbodies[j].velocity = Vector3.zero;
			}
		}
		moveCharacterDown = true;
	}

	private void CharacterBase_OnCharacterGroundCollided(CharacterBase character)
	{
		if (!isDamageApplied && isTapActivated)
		{
			CollisionInfo collisionInfo = new CollisionInfo();
			collisionInfo.boneName = "body";
			collisionInfo.damage = groundCollisionDamage;
			character.ApplyDamage(collisionInfo);
			isDamageApplied = true;
			isTapActivated = false;
			AudioManager.PlayAudioWithKey("magic_arrow_activation_2");
			Object.Destroy(holeEffect);
			level.RemoveWeapon(this);
		}
	}
}
