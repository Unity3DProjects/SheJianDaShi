using UnityEngine;

public class PowerUp : FlyingObject
{
	[SerializeField]
	private float timeAddMin;

	[SerializeField]
	private float timeAddMax;

	[SerializeField]
	private tk2dSprite spriteCoin;

	[SerializeField]
	private tk2dSprite spriteTime;

	[SerializeField]
	private GameObject particlePrefab;

	[SerializeField]
	private TrailVertex trailPrefab;

	[SerializeField]
	[ResourceLink]
	private AssetLink destroyTimeEffectPrefab;

	[SerializeField]
	[ResourceLink]
	private AssetLink destroyGoldEffectPrefab;

	private TrailVertex currentTrail;

	private float currentTimeAdd;

	private PowerUpType typePowerUp;

	public PowerUpType TypePowerUp
	{
		get
		{
			return typePowerUp;
		}
		set
		{
			typePowerUp = value;
			switch (typePowerUp)
			{
			case PowerUpType.Gold:
				spriteCoin.gameObject.SetActive(true);
				spriteTime.gameObject.SetActive(false);
				break;
			case PowerUpType.Time:
				spriteCoin.gameObject.SetActive(false);
				spriteTime.gameObject.SetActive(true);
				break;
			}
		}
	}

	private void Awake()
	{
		if ((bool)trailPrefab)
		{
			currentTrail = Object.Instantiate(trailPrefab);
			currentTrail.transform.parent = base.CachedTransform;
			currentTrail.transform.localPosition = Vector3.zero;
		}
	}

	private void OnTriggerEnter(Collider collider)
	{
		ICollidable[] componentsInParent = collider.gameObject.GetComponentsInParent<ICollidable>();
		ICollidable[] array = componentsInParent;
		foreach (ICollidable collidable in array)
		{
			if (collidable is WeaponGeneral)
			{
				Hit();
				break;
			}
		}
	}

	public override void Init(LevelBirdsHunt level)
	{
		base.Init(level);
		currentTimeAdd = Random.Range(timeAddMin, timeAddMax);
	}

	public override void UpdateObject(float deltaTime)
	{
		if (base.IsFlying)
		{
			flyTime += deltaTime;
			if (flyTime > currentPeriod)
			{
				flyTime -= currentPeriod;
			}
			base.UpdateObject(deltaTime);
		}
		if (currentTrail != null)
		{
			currentTrail.UpdateTrail(deltaTime, base.IsFlying);
		}
	}

	public void Hit()
	{
		DamageApplied(FlyingObjectDamageType.None, Vector2.zero);
	}

	public override void DamageApplied(FlyingObjectDamageType type, Vector2 direction)
	{
		switch (typePowerUp)
		{
		case PowerUpType.Gold:
			if (!SingletonMonoBehaviour<TutorialManager>.Instance.IsBirdsGameTutorialInProgress)
			{
				SingletonMonoBehaviour<StatisticsManager>.Instance.AddCoins(currentGold);
			}
			AudioManager.PlayAudioWithKey("coins_power_up_destroy");
			if (destroyGoldEffectPrefab != null && !string.IsNullOrEmpty(destroyGoldEffectPrefab.assetGUID))
			{
				GameObject obj2 = Object.Instantiate(destroyGoldEffectPrefab.GetAsset(), base.transform.position, Quaternion.identity, base.Level.FXsTransform) as GameObject;
				Object.Destroy(obj2, 5f);
			}
			break;
		case PowerUpType.Time:
			if (!SingletonMonoBehaviour<TutorialManager>.Instance.IsBirdsGameTutorialInProgress)
			{
				base.Level.CachedTimer.AddTime(currentTimeAdd);
			}
			if (destroyTimeEffectPrefab != null && !string.IsNullOrEmpty(destroyTimeEffectPrefab.assetGUID))
			{
				GameObject obj = Object.Instantiate(destroyTimeEffectPrefab.GetAsset(), base.transform.position, Quaternion.identity, base.Level.FXsTransform) as GameObject;
				Object.Destroy(obj, 5f);
			}
			break;
		}
		if ((bool)particlePrefab)
		{
			Object.Instantiate(particlePrefab, base.transform.position, Quaternion.identity);
		}
		base.IsFlying = false;
		Remove();
	}

	public override bool OnCollision(CollisionInfo collisionInfo)
	{
		return base.IsFlying;
	}

	public override void ApplyDamage(CollisionInfo collisionInfo)
	{
		DamageApplied(FlyingObjectDamageType.None, Vector2.zero);
	}
}
