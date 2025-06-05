using UnityEngine;

public class Trident : WeaponGeneral
{
	[SerializeField]
	private float timeFountain;

	[SerializeField]
	private float forceFountain;

	[SerializeField]
	private float minForceFountain = 0.3f;

	[SerializeField]
	private float damageFountain;

	[SerializeField]
	private float radiusFountain;

	[SerializeField]
	private float activationTimer;

	[SerializeField]
	private float activationFatalityTimer;

	[SerializeField]
	private float fatalityDelay = 2f;

	private bool isTimerActivate;

	private bool isActivate;

	private float currentTime;

	private CharacterBase currentCharacter;

	public override void UpdateWeapon(float deltaTime)
	{
		base.UpdateWeapon(deltaTime);
		if (isTimerActivate && (!currentCharacter.IsFinishHim || (currentCharacter.IsFinishHim && base.WeaponType == WeaponType.TridentUpgrade)))
		{
			currentTime -= deltaTime;
			if (!(currentTime < 0f) && (!currentCharacter.IsFinishHim || base.WeaponType != WeaponType.TridentUpgrade))
			{
				return;
			}
			isTimerActivate = false;
			isActivate = true;
			currentTime = timeFountain;
			if (!currentCharacter)
			{
				return;
			}
			Transform bodyTransform = currentCharacter.BodyTransform;
			Disabled(true);
			Object.Destroy(base.CachedRigidbody);
			Vector3 curPos = bodyTransform.position;
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				EffectType effectType = ((weaponType != WeaponType.Trident) ? EffectType.WeapomUpgradeTridentActivationFountain : EffectType.WeapomTridentActivationFountain);
				GameObject gameObject = Object.Instantiate(SingletonMonoBehaviour<EffectsManager>.Instance.EffectPrefabByType(effectType));
				if (gameObject != null)
				{
					gameObject.transform.position = level.Location.GetPositionOnFloor(curPos.x);
					Object.Destroy(gameObject.gameObject, 5f);
				}
			}, 0f);
			if (currentCharacter.IsFinishHim && base.WeaponType == WeaponType.TridentUpgrade)
			{
				currentCharacter.ApplyCharacterRagdoll(true);
			}
			ApplyExplosion(curPos);
			AudioManager.PlayAudioWithKey("trident_fountain_start");
			SingletonMonoBehaviour<GameManager>.Instance.CameraController.ApplyShakeWithDuration();
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				if (this != null)
				{
					AudioManager.StopAudioWithKey("trident_fountain_start", 0.5f);
				}
			}, 1f);
			level.TurnActionsChangeCount(-1);
		}
		else if (isActivate)
		{
			currentTime -= deltaTime;
			if (currentTime < 0f)
			{
				isActivate = false;
				isTimerActivate = false;
			}
		}
	}

	public override ICollidable Collided(CollisionInfo collisionInfo)
	{
		level.TurnActionsChangeCount(1);
		collisionInfo.isDisableRestore = true;
		collisionInfo.isDisableFatality = true;
		ICollidable collidable = base.Collided(collisionInfo);
		if (collidable is CharacterBase)
		{
			currentCharacter = collidable as CharacterBase;
			if (currentCharacter.IsFinishHim && base.WeaponType == WeaponType.Trident)
			{
				ApplyExplosion(currentCharacter.BodyTransform.position);
				Object.Destroy(this);
			}
			else
			{
				currentTime = activationTimer;
			}
		}
		else
		{
			level.TurnActionsChangeCount(-1);
		}
		return collidable;
	}

	private void ApplyExplosion(Vector3 position)
	{
		ExplosionInfo explosionInfo = new ExplosionInfo
		{
			weaponType = base.WeaponType,
			damage = damageFountain,
			force = forceFountain * 3000000f,
			minForce = minForceFountain,
			position = new Vector3(position.x, position.y - 100f, position.z),
			radius = radiusFountain,
			isFatalityStartedWithDelay = (currentCharacter.IsFinishHim && base.WeaponType == WeaponType.TridentUpgrade),
			fatalityDelay = fatalityDelay
		};
		currentCharacter.ApplyExplosionForce(explosionInfo);
	}

	protected override void OnHitActivation()
	{
		isTimerActivate = true;
	}
}
