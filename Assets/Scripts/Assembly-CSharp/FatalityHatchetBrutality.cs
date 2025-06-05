using UnityEngine;

public class FatalityHatchetBrutality : FatalityBreak
{
	private enum State
	{
		None = 0,
		Up = 1,
		Cut = 2
	}

	[SerializeField]
	private float fatalityAnimationDelay = 2f;

	[SerializeField]
	private float springValue = 1000000f;

	[SerializeField]
	private float springOffsetValue = 40f;

	[SerializeField]
	private float jointSpeedPerFrame = 1.65f;

	[SerializeField]
	private float raisePositionOffset = 200f;

	[SerializeField]
	private float biteShakeDuration = 0.05f;

	[SerializeField]
	private float biteShakeAmplitude = 0.1f;

	[SerializeField]
	private float xOffsetToCharacter;

	[SerializeField]
	private float rotationSpeed;

	[SerializeField]
	private float minRotationSpeed;

	[SerializeField]
	private float axeSpeed;

	[SerializeField]
	private float minAxeSpeed;

	[SerializeField]
	private float timeMoveToCharacter;

	[SerializeField]
	private float offsetY;

	[SerializeField]
	private float breakBoneChanse;

	[SerializeField]
	private float breakBoneDelay;

	[SerializeField]
	private float shakeChanse;

	[SerializeField]
	private float hitForce;

	[SerializeField]
	private DecalType decalType;

	[SerializeField]
	[ResourceLink]
	private AssetLink bloodEffectPrefab;

	[SerializeField]
	[ResourceLink]
	private AssetLink smallBloodEffectPrefab;

	[SerializeField]
	private float hatchetRaiseDelay = 0.3f;

	[SerializeField]
	private Rigidbody[] bonesPrefabs;

	[SerializeField]
	private float minForceBone;

	[SerializeField]
	private float maxForceBone;

	[SerializeField]
	private Vector3 angularVelosityBoneParts;

	[SerializeField]
	private float endFatalityDelay;

	[SerializeField]
	private float brutalityEffectDelay;

	private LevelsManager cachedLevelManager;

	private Axe cachedWeapon;

	private Vector3 currentYoffset = Vector3.zero;

	private Vector3 floorPosition;

	private Character affectedCharacter;

	private SpringJoint springJoint;

	private SpringJoint springJointSecond;

	private bool isNeedRaiseCharacter;

	private GameObject firstJointRoot;

	private GameObject secondJointRoot;

	private State currentWeaponState;

	private SkeletonUtilityBone cachedHead;

	private float breakBoneTime;

	private int moveDirection = 1;

	private bool isTweenerEnabled;

	private bool isBrainsEmitted;

	private float raiseStartPosition;

	private float currentHatchetRaiseDelay;

	private float axeResultSpeed;

	private float moveFactor;

	private tk2dSprite sprite;

	private bool isBrutalityEffectsAdded;

	private LevelsManager CachedLevelManager
	{
		get
		{
			if (cachedLevelManager == null)
			{
				cachedLevelManager = SingletonMonoBehaviour<Managers>.Instance.CachedLevelsManager;
			}
			return cachedLevelManager;
		}
	}

	private SkeletonUtilityBone CachedHead
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

	private Vector3 BodyPosition
	{
		get
		{
			return affectedCharacter.RootRigidBody.position;
		}
	}

	private float LeftBorderXPosition
	{
		get
		{
			return BodyPosition.x - xOffsetToCharacter;
		}
	}

	private float RightBorderXPosition
	{
		get
		{
			return BodyPosition.x + xOffsetToCharacter;
		}
	}

	private float AxeYPosition
	{
		get
		{
			float y = CachedLevelManager.CurrentLevel.Location.GetPositionOnFloor(cachedWeapon.transform.position.x).y;
			float y2 = CachedLevelManager.CurrentLevel.Location.GetPositionOnFloor(base.CachedBody.transform.position.x).y;
			float num = ((!(cachedWeapon.transform.position.y - base.CachedBody.transform.position.y > offsetY) && !(y <= y2)) ? y : y2);
			return num + offsetY + currentYoffset.y;
		}
	}

	public override void StartFatality(CollisionInfo infoCollision)
	{
		Axe.OnFloor += Axe_OnFloor;
		if (!base.IsActive)
		{
			base.StartFatality(infoCollision);
			affectedCharacter = infoCollision.otherCollider.GetComponentInParent<Character>();
			affectedCharacter.ForceNotRestore = true;
			affectedCharacter.StartDefaultAnimation();
			affectedCharacter.SkeletonAnimation.State.ClearTrack(0);
			affectedCharacter.StartFaceDefaultAnimation();
			affectedCharacter.ApplyCharacterRagdoll();
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				raiseStartPosition = affectedCharacter.RootRigidBody.position.y;
				firstJointRoot = new GameObject();
				firstJointRoot.transform.position = affectedCharacter.RootRigidBody.position;
				firstJointRoot.transform.parent = base.transform;
				firstJointRoot.transform.rotation = Quaternion.identity;
				Rigidbody rigidbody = firstJointRoot.AddComponent<Rigidbody>();
				rigidbody.isKinematic = true;
				secondJointRoot = new GameObject();
				secondJointRoot.transform.position = affectedCharacter.RootRigidBody.position;
				secondJointRoot.transform.parent = base.transform;
				secondJointRoot.transform.rotation = Quaternion.identity;
				Rigidbody rigidbody2 = secondJointRoot.AddComponent<Rigidbody>();
				rigidbody2.isKinematic = true;
				isNeedRaiseCharacter = true;
				springJoint = affectedCharacter.RootRigidBody.gameObject.AddComponent<SpringJoint>();
				springJoint.autoConfigureConnectedAnchor = false;
				springJoint.connectedAnchor = Vector3.zero;
				springJoint.connectedBody = rigidbody;
				springJoint.enableCollision = false;
				springJoint.anchor = Vector3.down * springOffsetValue;
				springJoint.spring = 2f * springValue;
				springJointSecond = affectedCharacter.RootRigidBody.gameObject.AddComponent<SpringJoint>();
				springJointSecond.autoConfigureConnectedAnchor = false;
				springJointSecond.connectedAnchor = Vector3.zero;
				springJointSecond.connectedBody = rigidbody2;
				springJointSecond.enableCollision = false;
				springJointSecond.anchor = Vector3.zero;
				springJointSecond.spring = springValue;
			}, fatalityAnimationDelay);
			ShowBrutalityEffect(brutalityEffectDelay);
		}
	}

	public override void UpdateFatality(float deltaTime)
	{
		base.UpdateFatality(deltaTime);
		breakBoneTime -= deltaTime;
		endFatalityDelay -= deltaTime;
		if (endFatalityDelay > 0f && (bool)cachedWeapon)
		{
			switch (currentWeaponState)
			{
			case State.Up:
				if (cachedWeapon.transform.position.y < floorPosition.y + offsetY)
				{
					if (!isTweenerEnabled)
					{
						TweenPosition tweenPosition = cachedWeapon.transform.gameObject.AddComponent<TweenPosition>();
						tweenPosition.beginPosition = cachedWeapon.transform.position;
						float x2 = ((!(BodyPosition.x > cachedWeapon.transform.position.x)) ? RightBorderXPosition : LeftBorderXPosition);
						tweenPosition.endPosition = new Vector3(x2, floorPosition.y + offsetY, 0f);
						tweenPosition.duration = timeMoveToCharacter;
						tweenPosition.enabled = true;
						isTweenerEnabled = true;
					}
				}
				else
				{
					cachedWeapon.CachedRigidbody.isKinematic = false;
					cachedWeapon.CachedRigidbody.useGravity = false;
					cachedWeapon.ColliderEnable(true);
					currentWeaponState = State.Cut;
					AudioManager.PlayLoopAudioWithKey("axe_upgraded_brutality_flying");
				}
				break;
			case State.Cut:
			{
				if (cachedWeapon.transform.position.x >= RightBorderXPosition)
				{
					moveDirection = -1;
					sprite.CachedRenderer.sortingLayerName = "UnderCharacter";
				}
				else if (cachedWeapon.transform.position.x <= LeftBorderXPosition)
				{
					moveDirection = 1;
					sprite.CachedRenderer.sortingLayerName = "Weapons";
				}
				if (cachedWeapon.transform.position.x > base.CachedBody.transform.position.x)
				{
					moveFactor = Mathf.Clamp01(1f - (cachedWeapon.transform.position.x - base.CachedBody.transform.position.x) / (RightBorderXPosition - base.CachedBody.transform.position.x));
				}
				else if (cachedWeapon.transform.position.x <= base.CachedBody.transform.position.x)
				{
					moveFactor = Mathf.Clamp01(1f - (base.CachedBody.transform.position.x - cachedWeapon.transform.position.x) / (base.CachedBody.transform.position.x - LeftBorderXPosition));
				}
				axeResultSpeed = axeSpeed * moveFactor;
				float num = (minRotationSpeed + rotationSpeed * moveFactor) * deltaTime;
				float x = cachedWeapon.transform.position.x + (axeResultSpeed + minAxeSpeed) * (float)moveDirection;
				if (affectedCharacter != null)
				{
					affectedCharacter.CachedTransform.position += Vector3.up * jointSpeedPerFrame;
				}
				cachedWeapon.transform.position = new Vector3(x, AxeYPosition, cachedWeapon.transform.position.z);
				cachedWeapon.transform.RotateAround(cachedWeapon.transform.position, Vector3.forward, 0f - num);
				break;
			}
			}
		}
		else if (cachedWeapon != null)
		{
			cachedWeapon.CachedRigidbody.isKinematic = false;
			cachedWeapon.CachedRigidbody.useGravity = true;
			currentWeaponState = State.None;
			Object.Destroy(firstJointRoot.gameObject);
			Object.Destroy(springJoint);
			Object.Destroy(springJointSecond);
			base.CachedManager.CharacterRagdoll.GetRigidbody("body").constraints = (RigidbodyConstraints)56;
			AudioManager.StopAudioWithKey("axe_upgraded_brutality_flying");
		}
		if (!isNeedRaiseCharacter || !base.IsActive || !(base.CachedManager.ActiveFatality == this))
		{
			return;
		}
		currentYoffset += Vector3.up * jointSpeedPerFrame;
		if (currentHatchetRaiseDelay < hatchetRaiseDelay)
		{
			firstJointRoot.transform.rotation = Quaternion.identity;
			firstJointRoot.transform.position += Vector3.up * jointSpeedPerFrame;
			secondJointRoot.transform.rotation = Quaternion.identity;
			secondJointRoot.transform.position += Vector3.up * jointSpeedPerFrame;
			currentHatchetRaiseDelay += deltaTime;
		}
		if (!(endFatalityDelay <= 0f) || !(affectedCharacter.RootRigidBody.transform.position.y > raiseStartPosition + raisePositionOffset))
		{
			return;
		}
		SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
		{
			SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.IsNeedFollowArcherOrWeapon = false;
			SingletonMonoBehaviour<GameManager>.Instance.CameraController.SetTarget(base.CachedBody.transform, Vector2.zero, Vector2.zero);
			cachedWeapon.CachedRigidbody.useGravity = true;
			currentWeaponState = State.None;
		}, 0f);
		SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
		{
			if (OnFatalityFinish != null)
			{
				OnFatalityFinish(this);
			}
		}, time - endFatalityDelay);
		isNeedRaiseCharacter = false;
	}

	public void TryBreakPart(CollisionInfo infoCollision)
	{
		if (!(Random.value <= breakBoneChanse) || !(breakBoneTime <= 0f) || currentWeaponState != State.Cut)
		{
			return;
		}
		SkeletonUtilityBone component = infoCollision.otherCollider.transform.parent.GetComponent<SkeletonUtilityBone>();
		if ((bool)component)
		{
			Transform transform = null;
			Rigidbody rigidbody = null;
			CharacterBase componentInParent = component.GetComponentInParent<CharacterBase>();
			MaskBase mask = componentInParent.masksManager.GetMask(component.boneName);
			if (component.boneName != "body")
			{
				BreakPart(infoCollision);
			}
			else if (mask != null)
			{
				componentInParent.masksManager.AddDecal(mask, decalType, component.transform.position.ToVector2());
			}
			for (int i = 0; i < componentInParent.RagdollRigidbodies.Length; i++)
			{
				if (componentInParent.RagdollRigidbodies[i].name == component.boneName)
				{
					transform = componentInParent.RagdollRigidbodies[i].transform;
					rigidbody = componentInParent.RagdollRigidbodies[i];
					break;
				}
			}
			if (transform != null)
			{
				if (mask != null)
				{
					componentInParent.masksManager.AddDecal(mask, decalType, component.transform.position.ToVector2());
				}
				if (bloodEffectPrefab != null && !string.IsNullOrEmpty(bloodEffectPrefab.assetGUID))
				{
					Object.Instantiate((GameObject)bloodEffectPrefab.GetAsset(), base.transform.position, infoCollision.collider.transform.localRotation, transform);
				}
				breakBoneTime = breakBoneDelay;
				rigidbody.AddForce(-infoCollision.collider.transform.position * 100f, ForceMode.Impulse);
				Vector3 normalized = new Vector3(Random.Range(-1f, 1f), 1f, 0f).normalized;
				if (component == CachedHead && !isBrainsEmitted)
				{
					base.CachedEffect.EmitBrain(base.transform, component.transform.position, normalized);
					isBrainsEmitted = true;
				}
				else if (component == base.CachedBody)
				{
					base.CachedEffect.EmitGuts(base.transform, component.transform.position, normalized);
				}
				AudioManager.PlayAudioWithKey("axe_brutality_collision");
			}
			if (Random.value <= shakeChanse)
			{
				componentInParent.RootRigidBody.AddForce(cachedWeapon.CachedTransform.position * hitForce);
				SingletonMonoBehaviour<GameManager>.Instance.CameraController.ApplyShakeWithDuration(biteShakeDuration, biteShakeAmplitude);
			}
		}
		Vector3 normalized2 = new Vector3(Random.Range(-1f, 1f), 1f, 0f).normalized;
		Rigidbody rigidbody2 = Object.Instantiate(bonesPrefabs.RandomObject(), infoCollision.otherCollider.transform.position, Quaternion.identity, base.transform);
		rigidbody2.velocity = Random.Range(minForceBone, maxForceBone) * normalized2;
		rigidbody2.angularVelocity = angularVelosityBoneParts;
		if (smallBloodEffectPrefab != null && !string.IsNullOrEmpty(smallBloodEffectPrefab.assetGUID))
		{
			Object.Instantiate((GameObject)smallBloodEffectPrefab.GetAsset(), rigidbody2.transform.position, rigidbody2.transform.localRotation, rigidbody2.transform);
		}
		if (!isBrutalityEffectsAdded)
		{
			isBrutalityEffectsAdded = true;
			base.CachedEffect.EmitGUICameraEffects(BrutalityEffectType.Finished);
		}
	}

	protected override void PlayFatalityStartSound()
	{
		AudioManager.PlayAudioWithKey("fatality_start");
	}

	private void Axe_OnFloor(Axe obj)
	{
		cachedWeapon = obj;
		sprite = cachedWeapon.GetComponentInChildren<tk2dSprite>();
		currentWeaponState = State.Up;
		floorPosition = cachedWeapon.transform.position;
		Axe.OnFloor -= Axe_OnFloor;
		cachedWeapon.CachedEmission.StartTrails(cachedWeapon.CachedTransform, cachedWeapon.CachedTransform.position, cachedWeapon.Velocity);
	}
}
