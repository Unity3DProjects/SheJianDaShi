using System.Collections.Generic;
using UnityEngine;

public class Emission : MonoBehaviour
{
	[SerializeField]
	[ResourceLink]
	protected AssetLink onShootEffect;

	[SerializeField]
	[ResourceLink]
	protected AssetLink onActivateEffect;

	[SerializeField]
	[ResourceLink]
	protected AssetLink onCollisionEffect;

	[SerializeField]
	[ResourceLink]
	protected AssetLink onHitEffect;

	[SerializeField]
	[ResourceLink]
	protected AssetLink onHitDriveIntoEffect;

	[SerializeField]
	[ResourceLink]
	protected AssetLink onExplosionEffect;

	[SerializeField]
	[ResourceLink]
	protected AssetLink idleEffect;

	[SerializeField]
	[ResourceLink]
	protected AssetLink trailEffect;

	[SerializeField]
	[ResourceLink]
	protected AssetLink anotherTrailEffect;

	[SerializeField]
	protected LevelObject[] emittedObjectsPrefabs;

	[SerializeField]
	protected float angle;

	[SerializeField]
	protected float velocity;

	[SerializeField]
	protected float spreadAngle;

	[SerializeField]
	protected bool isEvenlySpread;

	[SerializeField]
	protected bool isRandomEmitAngle;

	[SerializeField]
	protected bool isDependentByEmitAngle;

	private GameObject trail;

	private GameObject secondTrail;

	private GameObject idle;

	private Managers cachedManagers;

	public LevelObject[] EmittedObjectsPrefabs
	{
		get
		{
			return emittedObjectsPrefabs;
		}
	}

	public Managers CachedManagers
	{
		get
		{
			if (cachedManagers == null)
			{
				cachedManagers = SingletonMonoBehaviour<Managers>.Instance;
			}
			return cachedManagers;
		}
	}

	private void AddObject(GameObject obj, Transform parent, Vector3 position)
	{
		if (obj != null)
		{
			obj.transform.parent = parent;
			obj.transform.localPosition = Vector3.zero;
			obj.transform.localRotation = Quaternion.identity;
			obj.transform.position = position;
		}
	}

	private GameObject InstantiatePrefab(GameObject prefab, Transform parent, Vector3 position)
	{
		GameObject gameObject = null;
		if (prefab != null)
		{
			gameObject = Object.Instantiate(prefab);
			AddObject(gameObject, parent, position);
		}
		return gameObject;
	}

	public GameObject StartIdleEffect(Transform parent, Vector3 position, Vector2 velocity, EmitType emitType = EmitType.Shoot)
	{
		if (idleEffect != null && !string.IsNullOrEmpty(idleEffect.assetGUID))
		{
			GameObject gameObject = (GameObject)idleEffect.GetAsset();
			EmittedSystemInfo component = gameObject.GetComponent<EmittedSystemInfo>();
			if (!component || component.EmitType == emitType)
			{
				idle = Object.Instantiate(gameObject);
				AddObject(idle, parent, position);
				idle.transform.localScale = gameObject.transform.localScale;
				idle.transform.localPosition = gameObject.transform.localPosition;
			}
		}
		return idle;
	}

	public void StartTrails(Transform parent, Vector3 position, Vector2 velocity, EmitType emitType = EmitType.Shoot)
	{
		if (trailEffect != null && !string.IsNullOrEmpty(trailEffect.assetGUID))
		{
			GameObject gameObject = (GameObject)trailEffect.GetAsset();
			EmittedSystemInfo component = gameObject.GetComponent<EmittedSystemInfo>();
			if ((!component && emitType == EmitType.Shoot) || ((bool)component && component.EmitType == emitType))
			{
				trail = Object.Instantiate(gameObject);
				AddObject(trail, parent, position);
				trail.PlayFX();
			}
		}
		if (anotherTrailEffect != null && !string.IsNullOrEmpty(anotherTrailEffect.assetGUID))
		{
			GameObject gameObject2 = (GameObject)anotherTrailEffect.GetAsset();
			EmittedSystemInfo component2 = gameObject2.GetComponent<EmittedSystemInfo>();
			if ((!component2 && emitType == EmitType.Shoot) || ((bool)component2 && component2.EmitType == emitType))
			{
				secondTrail = Object.Instantiate(gameObject2);
				AddObject(secondTrail, parent, position);
				secondTrail.PlayFX();
			}
		}
	}

	public void StopTrails()
	{
		if (trail != null)
		{
			Object.Destroy(trail);
			trail = null;
		}
		if (secondTrail != null)
		{
			Object.Destroy(secondTrail);
			secondTrail = null;
		}
	}

	public void StopIdle()
	{
		if (idle != null)
		{
			Object.Destroy(idle);
			idle = null;
		}
	}

	public void EmitOnShoot(Vector3 position, Transform parent = null)
	{
		CachedManagers.CachedLevelsManager.CurrentLevel.CreateWeaponEffect(onShootEffect, false, position, parent);
	}

	public void EmitOnActivate(Vector3 position, Transform parent = null)
	{
		CachedManagers.CachedLevelsManager.CurrentLevel.CreateWeaponEffect(onActivateEffect, true, position, parent);
	}

	public void EmitOnCollision(Vector3 position, Transform parent = null)
	{
		CachedManagers.CachedLevelsManager.CurrentLevel.CreateWeaponEffect(onCollisionEffect, false, position, parent);
	}

	public void EmitOnHit(Vector3 position, Transform parent = null)
	{
		CachedManagers.CachedLevelsManager.CurrentLevel.CreateWeaponEffect(onHitEffect, false, position, parent);
	}

	public void EmitOnHitDriveInto(Vector3 position, Transform parent = null)
	{
		CachedManagers.CachedLevelsManager.CurrentLevel.CreateWeaponEffect(onHitDriveIntoEffect, false, position, parent);
	}

	public void EmitOnExplosion(Vector3 position, Transform parent = null)
	{
		CachedManagers.CachedLevelsManager.CurrentLevel.CreateWeaponEffect(onExplosionEffect, false, position, parent);
	}

	public virtual void EmitObjects(Level level, Vector3 position, float angle, float velocity)
	{
		List<EmissionSettings> emissionSettings = CreateEmissionObjectsSettings(position, angle, velocity);
		CreateEmittedObjects(level, emissionSettings);
		EmissionMultiplayerMessage(level, emissionSettings, null);
	}

	public virtual List<EmissionSettings> CreateEmissionObjectsSettings(Vector3 position, float angle, float velocity)
	{
		float num = angle;
		float emissionVelocity = ((!(this.velocity < 0f)) ? this.velocity : velocity);
		if (!isDependentByEmitAngle)
		{
			num = (isRandomEmitAngle ? Random.Range(-180f, 180f) : this.angle);
		}
		float num2 = 0f;
		if (isEvenlySpread)
		{
			int num3 = emittedObjectsPrefabs.Length;
			if (spreadAngle < 360f)
			{
				num3--;
			}
			if (num3 != 0)
			{
				num2 = spreadAngle / (float)num3;
			}
			if (isDependentByEmitAngle)
			{
				num -= spreadAngle * 0.5f;
			}
		}
		List<EmissionSettings> list = new List<EmissionSettings>();
		EmissionSettings item = default(EmissionSettings);
		for (int i = 0; i < emittedObjectsPrefabs.Length; i++)
		{
			item.emissionPosition = position;
			item.emissionAngle = num + num2 * (float)i;
			item.emissionVelocity = emissionVelocity;
			list.Add(item);
		}
		return list;
	}

	public virtual void EmissionMultiplayerMessage(Level level, List<EmissionSettings> emissionSettings, List<int> trashCanEmittedObjectsNumbers)
	{
		if (level.GameType == GameType.Multiplayer && SingletonMonoBehaviour<MultiplayerManager>.Instance.IsMainPlayer)
		{
			(level as LevelMultiplayer).ObjectsWasEmitted(emissionSettings, trashCanEmittedObjectsNumbers);
		}
	}

	public virtual void CreateEmittedObjects(Level level, List<EmissionSettings> emissionSettings)
	{
		if (level.GameType != GameType.Multiplayer || SingletonMonoBehaviour<MultiplayerManager>.Instance.IsMainPlayer)
		{
			CreateEmittedObjects(level, emittedObjectsPrefabs, emissionSettings);
			return;
		}
		LevelMultiplayer levelMultiplayer = level as LevelMultiplayer;
		levelMultiplayer.LastEmittedObjectsPrefabs = EmittedObjectsPrefabs;
		levelMultiplayer.IncreaseTurnCountTemporally(2f);
	}

	public static void CreateEmittedObjects(Level level, LevelObject[] prefabs, List<EmissionSettings> emissionSettings)
	{
		if (prefabs.Length != emissionSettings.Count)
		{
			return;
		}
		for (int i = 0; i < prefabs.Length; i++)
		{
			if (prefabs[i] is WeaponGeneral)
			{
				WeaponGeneral weaponGeneral = Object.Instantiate(prefabs[i]) as WeaponGeneral;
				level.AddEmittedWeapon(weaponGeneral);
				ShootingInfo shootingInfo = new ShootingInfo(emissionSettings[i].emissionAngle, emissionSettings[i].emissionVelocity);
				weaponGeneral.SetShootingInfo(emissionSettings[i].emissionPosition, shootingInfo, true);
				weaponGeneral.Shoot();
			}
			else if (prefabs[i] is EmittedObject)
			{
				EmittedObject emittedObject = Object.Instantiate(prefabs[i]) as EmittedObject;
				level.AddEmittedObject(emittedObject);
				emittedObject.transform.position = emissionSettings[i].emissionPosition;
				emittedObject.Init(level, Quaternion.AngleAxis(emissionSettings[i].emissionAngle, Vector3.forward) * new Vector3(emissionSettings[i].emissionVelocity, 0f));
			}
		}
	}
}
