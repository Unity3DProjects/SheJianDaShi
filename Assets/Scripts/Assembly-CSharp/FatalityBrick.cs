using System.Collections.Generic;
using UnityEngine;

public class FatalityBrick : MonoBehaviour
{
	private const string BLOOD_SOUND_KEY = "blood_fountain_loop";

	[SerializeField]
	[Range(0f, 1f)]
	private float breakProbability = 0.2f;

	[SerializeField]
	[ResourceLink]
	protected AssetLink brickCollisionEffectPrefab;

	[SerializeField]
	[ResourceLink]
	protected AssetLink brickTrailEffectPrefab;

	[SerializeField]
	private string soundCollisionName;

	[SerializeField]
	private List<Collider> brickColliders;

	protected List<string> breakBonesNames = new List<string>();

	private FatalityManager cachedFatalityManager;

	private FatalityEffect cachedEffect;

	private FatalityMask cachedMask;

	private SkeletonUtilityBone cachedHead;

	private SkeletonUtilityBone cachedBody;

	protected GameObject trail;

	private GameObject collisionEffect;

	private bool isCharacterDamaged;

	private ObserverPhysics observer;

	private bool isCollided;

	public FatalityManager CachedManager
	{
		get
		{
			if (cachedFatalityManager == null)
			{
				cachedFatalityManager = GetComponentInParent<FatalityManager>();
			}
			return cachedFatalityManager;
		}
	}

	protected SkeletonUtilityBone CachedHead
	{
		get
		{
			if (cachedHead == null)
			{
				cachedHead = CachedManager.GetUtilityBoneHead();
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
				cachedBody = CachedManager.GetUtilityBoneBody();
			}
			return cachedBody;
		}
	}

	public FatalityEffect CachedEffect
	{
		get
		{
			if (cachedEffect == null)
			{
				cachedEffect = GetComponent<FatalityEffect>();
			}
			return cachedEffect;
		}
	}

	public FatalityMask CachedMask
	{
		get
		{
			if (cachedMask == null)
			{
				cachedMask = GetComponent<FatalityMask>();
			}
			return cachedMask;
		}
	}

	protected virtual void Start()
	{
		if (brickTrailEffectPrefab != null && !string.IsNullOrEmpty(brickTrailEffectPrefab.assetGUID))
		{
			trail = (GameObject)brickTrailEffectPrefab.GetInstance();
			trail.transform.parent = base.transform;
			trail.transform.localPosition = Vector3.zero;
		}
		observer = base.gameObject.AddComponent<ObserverPhysics>();
		if ((bool)observer)
		{
			observer.EventCollisionEnter = ObserverPhysics_EventCollisionEnter;
			observer.EventTriggerEnter = ObserverPhysics_EventTriggerEnter;
		}
	}

	protected virtual void RemoveEventHandlers()
	{
		ObserverPhysics component = GetComponent<ObserverPhysics>();
		if ((bool)component)
		{
			component.EventCollisionEnter = null;
			component.EventTriggerEnter = null;
		}
	}

	protected void BreakAllJoint(Rigidbody rgBody)
	{
		if (!GoreContentFilter.AllowDismemberment)
		{
			return;
		}
		if ((bool)rgBody)
		{
			BreakSelfJoint(rgBody);
			BreakRelativeJoint(rgBody);
		}
	}

	private void BreakSelfJoint(Rigidbody rgBody)
	{
		if ((bool)rgBody)
		{
			HingeJoint component = rgBody.GetComponent<HingeJoint>();
			if ((bool)component)
			{
				Object.Destroy(component);
			}
		}
	}

	private void BreakRelativeJoint(Rigidbody rgBody)
	{
		if (!rgBody || !CachedManager.CharacterRagdoll.IsActive)
		{
			return;
		}
		Rigidbody[] rigidbodyArray = CachedManager.CharacterRagdoll.GetRigidbodyArray();
		if (rigidbodyArray == null)
		{
			return;
		}
		for (int i = 0; i < rigidbodyArray.Length; i++)
		{
			HingeJoint component = rigidbodyArray[i].GetComponent<HingeJoint>();
			if (component != null && component.connectedBody == rgBody)
			{
				Object.Destroy(component);
			}
		}
	}

	private void ObserverPhysics_EventCollisionEnter(Collision coll)
	{
		ObserverPhysics_EventTriggerEnter(coll.collider);
		if (coll.contacts[0].thisCollider is SphereCollider && brickCollisionEffectPrefab != null && !string.IsNullOrEmpty(brickCollisionEffectPrefab.assetGUID))
		{
			ICollidable[] componentsInParent = coll.collider.gameObject.GetComponentsInParent<ICollidable>();
			ICollidable[] array = componentsInParent;
			foreach (ICollidable collidable in array)
			{
				if (collidable is Ground)
				{
					if (collisionEffect == null)
					{
						collisionEffect = (GameObject)brickCollisionEffectPrefab.GetInstance();
						collisionEffect.transform.parent = coll.collider.gameObject.transform;
						collisionEffect.transform.position = Vector3.zero;
						observer.EventCollisionEnter = null;
					}
					break;
				}
			}
		}
		if (!string.IsNullOrEmpty(soundCollisionName) && !isCollided)
		{
			isCollided = true;
			AudioManager.StopAudioWithKey("blood_fountain_loop");
			AudioManager.PlayAudioWithKey(soundCollisionName);
			SingletonMonoBehaviour<GameManager>.Instance.CameraController.ApplyShakeWithDuration();
		}
	}

	private void ObserverPhysics_EventTriggerEnter(Collider coll)
	{
		if (trail != null)
		{
			Object.Destroy(trail.gameObject);
			trail = null;
		}
		if (CachedManager.WithCharacterCollision(coll))
		{
			if (isCharacterDamaged)
			{
				return;
			}
			isCharacterDamaged = true;
			SkeletonUtilityBone utilityBone = CachedManager.GetUtilityBone(coll);
			if (utilityBone != null && utilityBone != CachedBody && !breakBonesNames.Contains(utilityBone.boneName) && Random.value <= breakProbability)
			{
				if (!GoreContentFilter.AllowDismemberment)
				{
					RemoveEventHandlers();
					if (brickColliders != null && brickColliders.Count > 0 && brickColliders[0] != null)
					{
						brickColliders[0].isTrigger = false;
					}
					foreach (Transform item in base.transform)
					{
						base.gameObject.layer = LayerMask.NameToLayer("IgnoreCharacterCollisions");
					}
					return;
				}
				breakBonesNames.Add(utilityBone.boneName);
				Rigidbody rigidbody = CachedManager.CharacterRagdoll.GetRigidbody(utilityBone.boneName);
				Vector3 vector = new Vector3(utilityBone.transform.position.x - CachedBody.transform.position.x, utilityBone.transform.position.y - CachedBody.transform.position.y, 0f);
				Vector3 eulerAngles = new Vector3(0f, 0f, Mathf.Atan2(vector.y, vector.x) * 57.29578f);
				if ((bool)rigidbody)
				{
					rigidbody.velocity = vector.normalized * Random.Range(300f, 500f);
					rigidbody.angularVelocity = new Vector3(0f, 0f, Random.Range(5f, 15f));
					rigidbody.maxAngularVelocity = 15f;
					rigidbody.constraints = (RigidbodyConstraints)56;
				}
				CachedEffect.EmitBloodEffect(CachedManager.CurrentCharacter.CharacterType, CachedManager.GetUtilityBone(utilityBone.bone.Parent.Data.Name).transform, utilityBone.transform.position, eulerAngles);
				CachedMask.Activate(coll, utilityBone);
				BreakAllJoint(rigidbody);
			}
			RemoveEventHandlers();
			brickColliders[0].isTrigger = false;
			{
				foreach (Transform item in base.transform)
				{
					base.gameObject.layer = LayerMask.NameToLayer("IgnoreCharacterCollisions");
				}
				return;
			}
		}
		if ((coll.GetComponentInParent<Ground>() != null || (bool)coll.GetComponent<FatalityBrick>()) && collisionEffect == null && brickCollisionEffectPrefab != null && !string.IsNullOrEmpty(brickCollisionEffectPrefab.assetGUID))
		{
			GameObject gameObject = (GameObject)brickCollisionEffectPrefab.GetInstance();
			gameObject.transform.parent = coll.gameObject.transform;
			gameObject.transform.localPosition = Vector3.zero;
			observer.EventCollisionEnter = null;
		}
	}
}
