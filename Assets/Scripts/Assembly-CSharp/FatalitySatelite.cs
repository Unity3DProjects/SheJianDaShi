using System;
using System.Collections;
using UnityEngine;

public class FatalitySatelite : FatalityBase
{
	[SerializeField]
	private GameObject prefabSatelite;

	[SerializeField]
	private Vector2 spawnPoint;

	[SerializeField]
	protected float startVerticalSpeed = -1000f;

	[SerializeField]
	protected float delayBeforeStart;

	[SerializeField]
	private bool shouldPierceInGround;

	[SerializeField]
	private float leftAngleVar;

	[SerializeField]
	private float rightAngleVar;

	[SerializeField]
	protected string characterCollisionSound;

	[SerializeField]
	private string createSound;

	[SerializeField]
	private bool isExplodeCharacterNeeded = true;

	[SerializeField]
	[ResourceLink]
	protected AssetLink groundCollisionEffectPrefab;

	[SerializeField]
	[ResourceLink]
	protected AssetLink trailEffectPrefab;

	[SerializeField]
	private bool isApplyCharacterRagdollNeeded = true;

	[SerializeField]
	private BoneParticleSystem characterBone;

	[SerializeField]
	private float shakeForce = 5f;

	private GameObject trail;

	private SkeletonUtilityBone cachedHead;

	private SkeletonUtilityBone cachedBody;

	protected GameObject currentSatelite;

	protected bool isCharacterDamaged;

	protected SkeletonUtilityBone CachedHead
	{
		get
		{
			if (cachedHead == null)
			{
				cachedHead = base.CachedManager.GetUtilityBoneHead();
			}
			return cachedHead;
		}
	}

	protected SkeletonUtilityBone CachedBody
	{
		get
		{
			if (cachedBody == null)
			{
				cachedBody = base.CachedManager.GetUtilityBoneBody();
			}
			return cachedBody;
		}
	}

	public override void StartFatality(CollisionInfo infoCollision)
	{
		if (!base.IsActive)
		{
			base.StartFatality(infoCollision);
			if (isApplyCharacterRagdollNeeded)
			{
				base.CachedManager.CurrentCharacter.ApplyCharacterRagdoll();
			}
			StartCoroutine(CreateSatellite());
		}
	}

	public override void StopFatality()
	{
		base.StopFatality();
		if ((bool)currentSatelite)
		{
			StopAllCoroutines();
		}
	}

	protected override void PlayCrowdHappySound()
	{
	}

	protected virtual void RemoveEventHandlers()
	{
		ObserverPhysics component = currentSatelite.GetComponent<ObserverPhysics>();
		if ((bool)component)
		{
			component.EventCollisionEnter = null;
			component.EventTriggerEnter = null;
		}
	}

	protected virtual void SetSateliteVelocity()
	{
		Rigidbody component = currentSatelite.GetComponent<Rigidbody>();
		if (!component)
		{
			return;
		}
		float num = 0f;
		float x = 0f;
		float num2 = startVerticalSpeed;
		if (!Mathf.Approximately(Physics.gravity.y, 0f))
		{
			float num3 = Mathf.Pow(num2, 2f) - 2f * Physics.gravity.y * spawnPoint.y;
			if (num3 >= 0f)
			{
				float num4 = (0f - num2 + Mathf.Sqrt(num3)) / Physics.gravity.y;
				float num5 = (0f - num2 - Mathf.Sqrt(num3)) / Physics.gravity.y;
				if (num4 > 0f)
				{
					num = ((!(num5 > 0f) || !(num4 > num5)) ? num4 : num5);
				}
				else if (num5 > 0f)
				{
					num = num5;
				}
				x = (0f - spawnPoint.x) / num;
			}
		}
		component.velocity = new Vector2(x, num2);
	}

	protected virtual void GroundCollided(Collider collider)
	{
		if (groundCollisionEffectPrefab != null && !string.IsNullOrEmpty(groundCollisionEffectPrefab.assetGUID))
		{
			GameObject gameObject = (GameObject)groundCollisionEffectPrefab.GetInstance();
			gameObject.transform.parent = collider.gameObject.transform;
			gameObject.transform.localPosition = Vector3.zero;
		}
		if (shouldPierceInGround)
		{
			RemoveEventHandlers();
			currentSatelite.GetComponent<Rigidbody>().isKinematic = true;
		}
	}

	protected virtual void CharacterCollided(Collider collider, bool isSoundNeed = true)
	{
		isCharacterDamaged = true;
		if (isExplodeCharacterNeeded)
		{
			base.ExplodeCharacter();
		}
		base.CachedMask.Activate(startFatalityCollider, startFatalityUtilityBone);
		base.CachedEffect.EmitBrainEffect(base.CachedManager.CurrentCharacter.CharacterType, base.transform, CachedHead.transform.position, CachedHead.transform.eulerAngles);
		base.CachedEffect.EmitBrain(base.transform, CachedHead.transform.position, Vector3.up);
		base.CachedEffect.EmitGutsEffect(base.CachedManager.CurrentCharacter.CharacterType, base.transform, CachedBody.transform.position, CachedBody.transform.eulerAngles);
		base.CachedEffect.EmitGuts(base.transform, CachedBody.transform.position, Vector3.up);
		if (!shouldPierceInGround && (groundCollisionEffectPrefab == null || string.IsNullOrEmpty(groundCollisionEffectPrefab.assetGUID)))
		{
			RemoveEventHandlers();
		}
		foreach (Transform item in currentSatelite.transform)
		{
			item.gameObject.layer = LayerMask.NameToLayer("CharacterGuts");
		}
		AudioManager.PlayAudioWithKey(characterCollisionSound);
		if (isSoundNeed)
		{
			base.PlayCrowdHappySound();
		}
		if (trail != null)
		{
			UnityEngine.Object.Destroy(trail.gameObject);
			trail = null;
		}
		SingletonMonoBehaviour<GameManager>.Instance.CameraController.ApplyShakeWithDuration(0.5f, shakeForce);
	}

	protected virtual IEnumerator CreateSatellite()
	{
		yield return new WaitForSeconds(delayBeforeStart);
		if (!(prefabSatelite != null))
		{
			yield break;
		}
		float num = leftAngleVar * ((float)Math.PI / 180f);
		float num2 = rightAngleVar * ((float)Math.PI / 180f);
		float min = spawnPoint.x - spawnPoint.magnitude * Mathf.Sin(num) / Mathf.Sin(num + Mathf.Atan2(spawnPoint.y, spawnPoint.x));
		float max = spawnPoint.x + spawnPoint.magnitude * Mathf.Sin(num2) / Mathf.Sin(0f - num2 + Mathf.Atan2(spawnPoint.y, spawnPoint.x));
		spawnPoint.x = UnityEngine.Random.Range(min, max);
		currentSatelite = UnityEngine.Object.Instantiate(prefabSatelite);
		SphereCollider componentInChildren = currentSatelite.GetComponentInChildren<SphereCollider>();
		RaycastHit hitInfo;
		if (componentInChildren != null && Physics.SphereCast(CachedBody.transform.position + (Vector3)spawnPoint, componentInChildren.radius, -(Vector3)spawnPoint, out hitInfo))
		{
			ICollidable componentInParent = hitInfo.collider.gameObject.GetComponentInParent<ICollidable>();
			if (componentInParent is Ground)
			{
				spawnPoint.x = 0f - spawnPoint.x;
			}
		}
		currentSatelite.transform.parent = SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.gameObject.transform;
		currentSatelite.transform.position = CachedBody.transform.position + (Vector3)spawnPoint;
		currentSatelite.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(spawnPoint.y, spawnPoint.x) * 57.29578f + 90f);
		ObserverPhysics observerPhysics = currentSatelite.AddComponent<ObserverPhysics>();
		if ((bool)observerPhysics)
		{
			observerPhysics.EventCollisionEnter = ObserverPhysics_EventCollisionEnter;
			observerPhysics.EventTriggerEnter = ObserverPhysics_EventTriggerEnter;
		}
		SetSateliteVelocity();
		AudioManager.PlayAudioWithKey("satellite_flying");
		AudioManager.PlayAudioWithKey(createSound);
		if (trailEffectPrefab != null && !string.IsNullOrEmpty(trailEffectPrefab.assetGUID))
		{
			trail = (GameObject)trailEffectPrefab.GetInstance();
			trail.transform.parent = currentSatelite.transform;
			trail.transform.localPosition = Vector3.zero;
		}
	}

	private void ObserverPhysics_EventCollisionEnter(Collision coll)
	{
		ObserverPhysics_EventTriggerEnter(coll.collider);
		if (!(coll.contacts[0].thisCollider is SphereCollider) || groundCollisionEffectPrefab == null || string.IsNullOrEmpty(groundCollisionEffectPrefab.assetGUID))
		{
			return;
		}
		ICollidable[] componentsInParent = coll.collider.gameObject.GetComponentsInParent<ICollidable>();
		ICollidable[] array = componentsInParent;
		foreach (ICollidable collidable in array)
		{
			if (collidable is Ground)
			{
				GameObject gameObject = (GameObject)groundCollisionEffectPrefab.GetInstance();
				gameObject.transform.parent = coll.collider.gameObject.transform;
				gameObject.transform.position = Vector3.zero;
				break;
			}
		}
	}

	protected virtual void ObserverPhysics_EventTriggerEnter(Collider coll)
	{
		if (base.CachedManager.WithCharacterCollision(coll))
		{
			if (!isCharacterDamaged)
			{
				CharacterCollided(coll);
			}
			return;
		}
		ICollidable[] componentsInParent = coll.gameObject.GetComponentsInParent<ICollidable>();
		ICollidable[] array = componentsInParent;
		foreach (ICollidable collidable in array)
		{
			if (collidable is Ground && !(collidable as Ground).IsWallPart)
			{
				GroundCollided(coll);
				break;
			}
		}
	}
}
