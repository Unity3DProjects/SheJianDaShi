using System;
using System.Collections.Generic;
using UnityEngine;

public class FatalitySpinner : FatalityBase
{
	private const string RAGDOLL_HEAD_PART_NAME = "head";

	private const float RAGDOLL_RIGIDBODY_VELOCITY_BORDER = 100f;

	[SerializeField]
	private ObserverPhysics collisionObserverPrefab;

	[SerializeField]
	private float collisionRadiusBegin = 0.01f;

	[SerializeField]
	private float collisionRadiusEnd = 100f;

	[SerializeField]
	private float collisionRadiusChangeDuration = 2f;

	[SerializeField]
	[Range(0f, 1f)]
	private float onlyBodyColliderRelativeRadius = 0.05f;

	[SerializeField]
	private AnimationCurve collisionRadiusChangeCurve;

	[SerializeField]
	[ResourceLink]
	private AssetLink collisionEffectPrefab;

	[SerializeField]
	[ResourceLink]
	private AssetLink removePartEffectPrefab;

	[SerializeField]
	private float delayBeforeRotation = 2f;

	[SerializeField]
	private float delayAfterRotation = 5f;

	[SerializeField]
	private float rotationVelocity = 90f;

	[SerializeField]
	private float rotationDuration = 5f;

	[SerializeField]
	private float forceAfterCollision = 3000f;

	[SerializeField]
	private float forceBodyAfterCollision;

	private Transform cachedSpinnerTransform;

	private GameObject collisionEffect;

	private ObserverPhysics collisionObserver;

	private CapsuleCollider collisionCollider;

	private CapsuleCollider onlyBodyCollider;

	private bool isCollisionRadiusChanging;

	private float currentCollisionRadiusChangingDuration;

	private float beforeRotationTimer;

	private float centiperalVelocity;

	private bool isClockwiseRotation;

	private bool isCharacterRotating;

	private float currentRotationRadius;

	private float currentRotationAngle;

	private float currentRotationDuration;

	private List<Rigidbody> rotatingCharacterRigidbodies;

	private List<Rigidbody> ragdollRigidbodies;

	private bool isBodyDestroyed;

	private Transform effectsParent;

	private bool isFatalityStarted;

	private Transform CachedSpinnerTransform
	{
		get
		{
			if (cachedSpinnerTransform == null && collisionObserver != null)
			{
				cachedSpinnerTransform = collisionObserver.transform;
			}
			return cachedSpinnerTransform;
		}
	}

	private bool IsCharacterRotating
	{
		get
		{
			return isCharacterRotating;
		}
		set
		{
			if (isCharacterRotating == value)
			{
				return;
			}
			isCharacterRotating = value;
			if (isCharacterRotating)
			{
				CharacterBase currentCharacter = base.CachedManager.CurrentCharacter;
				currentCharacter.StartDefaultAnimation();
				currentCharacter.SkeletonAnimation.State.ClearTrack(0);
				currentCharacter.StartFaceKOAnimation();
				currentCharacter.ApplyCharacterRagdoll();
				rotatingCharacterRigidbodies = new List<Rigidbody>(currentCharacter.RagdollRigidbodies);
				ragdollRigidbodies = new List<Rigidbody>(rotatingCharacterRigidbodies);
				Vector3 vector = rotatingCharacterRigidbodies[0].position - CachedSpinnerTransform.position;
				currentRotationRadius = vector.magnitude;
				currentRotationAngle = Mathf.Atan2(vector.y, vector.x);
				centiperalVelocity = currentRotationRadius / rotationDuration;
				for (int i = 0; i < rotatingCharacterRigidbodies.Count; i++)
				{
					rotatingCharacterRigidbodies[i].interpolation = RigidbodyInterpolation.Extrapolate;
					rotatingCharacterRigidbodies[i].gameObject.layer = LayerMask.NameToLayer("FirstSingleCollision");
				}
				if (collisionCollider != null)
				{
					Physics.IgnoreCollision(rotatingCharacterRigidbodies[0].GetComponent<Collider>(), collisionCollider, true);
				}
			}
			else
			{
				currentTime = delayAfterRotation;
			}
		}
	}

	private bool IsBodyDestroyed
	{
		get
		{
			return isBodyDestroyed;
		}
		set
		{
			if (isBodyDestroyed != value)
			{
				isBodyDestroyed = value;
				if (isBodyDestroyed)
				{
					IsCharacterRotating = false;
				}
			}
		}
	}

	private void OnDestroy()
	{
		if (SingletonMonoBehaviour<AudioManager>.InstanceIfExist != null)
		{
			AudioManager.StopAudioWithKey("spinner_fatality_idle");
		}
	}

	public override void StartFatality(CollisionInfo infoCollision)
	{
		if (isFatalityStarted)
		{
			return;
		}
		isFatalityStarted = true;
		base.StartFatality(infoCollision);
		effectsParent = SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.FXsTransform;
		collisionObserver = UnityEngine.Object.Instantiate(collisionObserverPrefab);
		collisionObserver.transform.parent = SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.transform;
		collisionObserver.transform.position = infoCollision.position;
		collisionObserver.EventCollisionEnter = CollisionObserver_EventCollisionEnter;
		CapsuleCollider[] components = collisionObserver.GetComponents<CapsuleCollider>();
		if (components != null && components.Length > 0)
		{
			collisionCollider = components[0];
			isCollisionRadiusChanging = true;
			if (components.Length > 1)
			{
				onlyBodyCollider = components[1];
			}
		}
		isClockwiseRotation = base.CachedManager.CurrentCharacter.CachedTransform.position.x < infoCollision.position.x;
		if (collisionEffectPrefab != null && !string.IsNullOrEmpty(collisionEffectPrefab.assetGUID))
		{
			collisionEffect = UnityEngine.Object.Instantiate(collisionEffectPrefab.GetAsset()) as GameObject;
			collisionEffect.transform.parent = collisionObserver.transform;
			collisionEffect.transform.localPosition = Vector3.zero;
			if (isClockwiseRotation)
			{
				collisionEffect.transform.localScale = new Vector3(-1f, 1f, 1f);
			}
		}
		base.CachedManager.CurrentCharacter.SetAnimation(0, "finish_him", true);
		SingletonMonoBehaviour<GameManager>.Instance.CameraController.ApplyShakeWithDuration(delayBeforeRotation + rotationDuration, 10f);
		AudioManager.PlayAudioWithKey("spinner_fatality_idle");
	}

	public override void StopFatality()
	{
		base.StopFatality();
		collisionObserver.EventCollisionEnter = null;
	}

	public override void UpdateFatality(float deltaTime)
	{
		base.UpdateFatality(deltaTime);
		if (isCollisionRadiusChanging)
		{
			currentCollisionRadiusChangingDuration += deltaTime;
			float num = collisionRadiusChangeCurve.Evaluate(Mathf.Clamp01(currentCollisionRadiusChangingDuration / collisionRadiusChangeDuration));
			collisionCollider.radius = collisionRadiusBegin + (collisionRadiusEnd - collisionRadiusBegin) * num;
			onlyBodyCollider.radius = collisionCollider.radius * onlyBodyColliderRelativeRadius;
			if (num >= 1f)
			{
				isCollisionRadiusChanging = false;
			}
		}
		if (!IsCharacterRotating && beforeRotationTimer < delayBeforeRotation)
		{
			beforeRotationTimer += deltaTime;
			if (beforeRotationTimer > delayBeforeRotation && !IsBodyDestroyed)
			{
				IsCharacterRotating = true;
			}
		}
		if (!IsCharacterRotating || IsBodyDestroyed)
		{
			return;
		}
		currentRotationDuration += deltaTime;
		if (currentRotationDuration < rotationDuration)
		{
			currentRotationRadius -= centiperalVelocity * deltaTime;
			currentRotationAngle += rotationVelocity * ((float)Math.PI / 180f) * deltaTime * ((!isClockwiseRotation) ? 1f : (-1f));
			Vector3 vector = new Vector3(Mathf.Cos(currentRotationAngle), Mathf.Sin(currentRotationAngle)) * currentRotationRadius + CachedSpinnerTransform.position;
			Vector3 vector2 = vector - rotatingCharacterRigidbodies[0].position;
			for (int i = 0; i < rotatingCharacterRigidbodies.Count; i++)
			{
				Rigidbody rigidbody = rotatingCharacterRigidbodies[i];
				if (rigidbody != null)
				{
					if (i == 0)
					{
						rigidbody.position += vector2;
					}
					Vector3 velocity = rigidbody.velocity;
					velocity.x = Mathf.Clamp(velocity.x, -100f, 100f);
					velocity.y = Mathf.Clamp(velocity.y, -100f, 100f);
					velocity.z = 0f;
					rigidbody.velocity = velocity;
				}
			}
		}
		else
		{
			IsCharacterRotating = false;
		}
	}

	private void RemoveDependentRigidbodiesFromRotation(Rigidbody currentRigidbody)
	{
		List<Rigidbody> list = new List<Rigidbody>();
		for (int num = rotatingCharacterRigidbodies.Count - 1; num >= 0; num--)
		{
			bool flag = false;
			Rigidbody rigidbody = rotatingCharacterRigidbodies[num];
			do
			{
				if (rigidbody == currentRigidbody)
				{
					flag = true;
					break;
				}
				HingeJoint component = rigidbody.GetComponent<HingeJoint>();
				if (component == null)
				{
					flag = false;
					break;
				}
				rigidbody = component.connectedBody;
			}
			while (rigidbody != null);
			if (flag)
			{
				list.Add(rotatingCharacterRigidbodies[num]);
			}
		}
		for (int num2 = list.Count - 1; num2 >= 0; num2--)
		{
			rotatingCharacterRigidbodies.Remove(list[num2]);
		}
	}

	private void RemoveConnectedJoints(Rigidbody currentBody)
	{
		if (!GoreContentFilter.AllowDismemberment)
		{
			return;
		}
		HingeJoint component = currentBody.GetComponent<HingeJoint>();
		if (component != null)
		{
			UnityEngine.Object.Destroy(component);
		}
		int i = 0;
		for (int count = ragdollRigidbodies.Count; i < count; i++)
		{
			component = ragdollRigidbodies[i].GetComponent<HingeJoint>();
			if (component != null && component.connectedBody == currentBody)
			{
				UnityEngine.Object.Destroy(component);
			}
		}
	}

	private void ApplySkeletonMask(Collider currentCollider)
	{
		if (!GoreContentFilter.AllowCutMasks)
		{
			return;
		}
		Collider collider = null;
		List<Collider> partsColliders = base.CachedManager.CurrentCharacter.PartsColliders;
		if (partsColliders.Contains(currentCollider) && base.CachedManager.GetUtilityBone(currentCollider) != null)
		{
			collider = currentCollider;
		}
		else
		{
			for (int i = 0; i < partsColliders.Count; i++)
			{
				if (partsColliders[i] != null && currentCollider != null && partsColliders[i].transform.parent.name == currentCollider.name)
				{
					collider = partsColliders[i];
					break;
				}
			}
		}
		if (collider != null)
		{
			base.CachedMask.Activate(collider, base.CachedManager.GetUtilityBone(collider));
		}
	}

	private void CollisionObserver_EventCollisionEnter(Collision collision)
	{
		IsCharacterRotating = !IsBodyDestroyed;
		Rigidbody rigidbody = collision.rigidbody;
		bool flag = rigidbody == base.CachedManager.CurrentCharacter.RootRigidBody;
		if (!(rigidbody != null))
		{
			return;
		}
		Vector3 vector;
		if (!flag)
		{
			vector = collision.contacts[0].point - CachedSpinnerTransform.position;
			vector = new Vector3(vector.y, 0f - vector.x, 0f);
			vector.Normalize();
		}
		else
		{
			vector = Vector3.up;
		}
		if (!isClockwiseRotation && !flag)
		{
			vector *= -1f;
		}
		Vector3 direction = new Vector3(0f, 0f, Mathf.Atan2(vector.y, vector.x) * 57.29578f);
		Vector3 point = collision.contacts[0].point;
		if (ragdollRigidbodies.Contains(rigidbody))
		{
			if (!GoreContentFilter.AllowDismemberment)
			{
				if (!flag)
				{
					rigidbody.velocity = vector * forceAfterCollision;
				}
				else
				{
					rigidbody.velocity = vector * forceBodyAfterCollision;
				}
				AudioManager.PlayAudioWithKey("spinner_hit_character");
				return;
			}
			if (!IsBodyDestroyed && rigidbody == rotatingCharacterRigidbodies[0])
			{
				IsBodyDestroyed = true;
				base.CachedEffect.EmitGuts(effectsParent, point, direction);
			}
			if (rigidbody.name == "head")
			{
				base.CachedEffect.EmitBrain(effectsParent, point, direction);
			}
			RemoveDependentRigidbodiesFromRotation(rigidbody);
			RemoveConnectedJoints(rigidbody);
			ApplySkeletonMask(collision.collider);
			if (!flag)
			{
				rigidbody.velocity = vector * forceAfterCollision;
			}
			else
			{
				rigidbody.velocity = vector * forceBodyAfterCollision;
			}
			rigidbody.gameObject.layer = LayerMask.NameToLayer("CharacterGuts");
			rigidbody.constraints = (RigidbodyConstraints)56;
			if (removePartEffectPrefab != null && !string.IsNullOrEmpty(removePartEffectPrefab.assetGUID))
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(removePartEffectPrefab.GetAsset()) as GameObject;
				gameObject.transform.parent = effectsParent;
				gameObject.transform.position = point;
			}
			AudioManager.PlayAudioWithKey("spinner_hit_character");
		}
	}
}
