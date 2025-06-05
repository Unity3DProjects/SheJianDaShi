using System;
using System.Collections.Generic;
using UnityEngine;

public class FatalityArrowSimpleUpgrade : FatalityBase
{
	private const string BODY_NAME = "body";

	private const string HEAD_NAME = "head";

	[SerializeField]
	private float fatalityAnimationDelay = 2f;

	[SerializeField]
	private float springValue = 1000000f;

	[SerializeField]
	private float springOffsetValue = 40f;

	[SerializeField]
	private float jointSpeedPerFrame = 1.65f;

	[SerializeField]
	private int maxInterruptedArrowsCount = 15;

	[SerializeField]
	private int additionalInterruptedArrowsCount = 4;

	[SerializeField]
	private float sideShotWeight = 3f;

	[SerializeField]
	private float bottomShotWeight = 1f;

	[SerializeField]
	private float sideShotAngle = 60f;

	[SerializeField]
	private float bottomShotAngle = 60f;

	[SerializeField]
	private float shotRadius = 1000f;

	[SerializeField]
	private float arrowFlyTime = 1f;

	[SerializeField]
	private float additionalInterruptedArrowsDelay = 0.5f;

	[SerializeField]
	private UpgradeArrowPart arrowPrefab;

	[SerializeField]
	private float totalShotingTime;

	[SerializeField]
	private float[] shotingWeights;

	[SerializeField]
	private float biteShakeDuration = 0.05f;

	[SerializeField]
	private float biteShakeAmplitude = 0.1f;

	[SerializeField]
	private float additionalInterruptedArrowsForce;

	[SerializeField]
	private Vector3 additionalInterruptedArrowsAngualarVelocity;

	[SerializeField]
	private float brutalityEffectDelay;

	private SkeletonUtilityBone cachedHead;

	private SkeletonUtilityBone cachedBody;

	private Character affectedCharacter;

	private SpringJoint springJoint;

	private SpringJoint springJointSecond;

	private bool isNeedRaiseCharacter;

	private GameObject firstJointRoot;

	private GameObject secondJointRoot;

	private int piercedArrowsCount;

	private int releasedArrowsCount;

	private float timeToNextShot;

	private int arrowDirection;

	private bool isAdditionalArrowsReleased;

	private float sessionTime;

	private List<UpgradeArrowPart> arrows = new List<UpgradeArrowPart>();

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

	private void OnEnable()
	{
		UpgradeArrowPart.OnArrowInterrupted = (Action)Delegate.Combine(UpgradeArrowPart.OnArrowInterrupted, new Action(GradedArrowPart_OnArrowInterrupted));
		if (shotingWeights.Length != 0)
		{
			sessionTime = totalShotingTime / (float)shotingWeights.Length;
		}
		timeToNextShot = GetTimeInterval();
	}

	private void OnDisable()
	{
		UpgradeArrowPart.OnArrowInterrupted = (Action)Delegate.Remove(UpgradeArrowPart.OnArrowInterrupted, new Action(GradedArrowPart_OnArrowInterrupted));
	}

	public override void StartFatality(CollisionInfo infoCollision)
	{
		if (!base.IsActive)
		{
			affectedCharacter = infoCollision.otherCollider.GetComponentInParent<Character>();
			affectedCharacter.ForceNotRestore = true;
			affectedCharacter.StartDefaultAnimation();
			affectedCharacter.SkeletonAnimation.State.ClearTrack(0);
			affectedCharacter.StartFaceDefaultAnimation();
			affectedCharacter.ApplyCharacterRagdoll();
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
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
			base.StartFatality(infoCollision);
			ShowBrutalityEffect(brutalityEffectDelay);
		}
	}

	public override void UpdateFatality(float deltaTime)
	{
		if (isNeedRaiseCharacter && base.IsActive && base.CachedManager.ActiveFatality == this)
		{
			int num = ((!isAdditionalArrowsReleased) ? 1 : 0);
			firstJointRoot.transform.rotation = Quaternion.identity;
			firstJointRoot.transform.position += Vector3.up * jointSpeedPerFrame * num;
			secondJointRoot.transform.rotation = Quaternion.identity;
			secondJointRoot.transform.position += Vector3.up * jointSpeedPerFrame * num;
			if (piercedArrowsCount >= maxInterruptedArrowsCount)
			{
				SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
				{
					UnityEngine.Object.Destroy(firstJointRoot.gameObject);
					UnityEngine.Object.Destroy(springJoint);
					UnityEngine.Object.Destroy(springJointSecond);
					for (int i = 0; i < arrows.Count; i++)
					{
						arrows[i].transform.localPosition = Vector3.zero;
						arrows[i].transform.parent.gameObject.transform.localRotation = Quaternion.identity;
					}
					Rigidbody[] rigidbodyArray = base.CachedManager.CharacterRagdoll.GetRigidbodyArray();
					for (int j = 0; j < rigidbodyArray.Length; j++)
					{
						rigidbodyArray[j].velocity = Vector3.zero;
						rigidbodyArray[j].angularVelocity = Vector3.zero;
					}
					base.CachedEffect.EmitBrainEffect(base.CachedManager.CurrentCharacter.CharacterType, base.transform, CachedHead.transform.position, Vector3.zero);
					base.CachedEffect.EmitGutsEffect(base.CachedManager.CurrentCharacter.CharacterType, base.transform, CachedBody.transform.position, Vector3.zero);
					ExplodeCharacter();
					ApplyFatalityEffects();
					base.CachedEffect.EmitGUICameraEffects(BrutalityEffectType.Finished);
					SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.IsNeedFollowArcherOrWeapon = false;
					base.CachedManager.CurrentCharacter.RootRigidBody.AddForce(Vector3.up * additionalInterruptedArrowsForce);
					base.CachedManager.CurrentCharacter.RootRigidBody.angularVelocity = additionalInterruptedArrowsAngualarVelocity;
					AudioManager.PlayAudioWithKey("green_arrow_brutality_explode");
				}, 0f);
				SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
				{
					if (OnFatalityFinish != null)
					{
						OnFatalityFinish(this);
					}
				}, currentTime);
				isNeedRaiseCharacter = false;
			}
		}
		if (isNeedRaiseCharacter)
		{
			timeToNextShot -= deltaTime;
		}
		if (timeToNextShot <= 0f && releasedArrowsCount <= maxInterruptedArrowsCount - additionalInterruptedArrowsCount)
		{
			timeToNextShot = GetTimeInterval();
			Shot((releasedArrowsCount < maxInterruptedArrowsCount - additionalInterruptedArrowsCount) ? 1 : additionalInterruptedArrowsCount);
			isAdditionalArrowsReleased = releasedArrowsCount >= maxInterruptedArrowsCount - additionalInterruptedArrowsCount;
			if (isAdditionalArrowsReleased)
			{
				timeToNextShot += additionalInterruptedArrowsDelay;
			}
		}
	}

	protected override void PlayFatalityStartSound()
	{
		AudioManager.PlayAudioWithKey("fatality_start");
	}

	private void ApplyFatalityEffects()
	{
		base.CachedEffect.EmitBrain(base.transform, CachedHead.transform.position, Vector3.up);
		base.CachedEffect.EmitGuts(base.transform, CachedBody.transform.position, Vector3.up);
		base.CachedMask.Activate(startFatalityCollider, startFatalityUtilityBone);
	}

	private void Shot(int count)
	{
		for (int i = 0; i < count; i++)
		{
			releasedArrowsCount++;
			ReleaseArrow(count == 1);
			AudioManager.PlayAudioWithKey("green_arrow_brutality_flying");
		}
	}

	private void ReleaseArrow(bool isForceNeeded)
	{
		arrowDirection = ((arrowDirection != 0) ? (arrowDirection *= -1) : ((!(UnityEngine.Random.Range(0f, 1f) > 0.5f)) ? 1 : (-1)));
		float num = ((!(UnityEngine.Random.Range(0f, bottomShotWeight + sideShotWeight) > bottomShotWeight)) ? (UnityEngine.Random.Range(0f, bottomShotAngle) + 180f) : UnityEngine.Random.Range(bottomShotAngle, sideShotAngle + bottomShotAngle));
		Vector3 vector = new Vector3(Mathf.Sin((float)Math.PI / 180f * num * (float)arrowDirection) * shotRadius, Mathf.Cos((float)Math.PI / 180f * num * (float)arrowDirection) * shotRadius, 0f);
		Quaternion rotation = Quaternion.Euler(0f, 0f, 270f - num * (float)arrowDirection);
		UpgradeArrowPart upgradeArrowPart = UnityEngine.Object.Instantiate(arrowPrefab, vector + CachedBody.CachedTransform.position, rotation, CachedBody.CachedTransform.parent);
		upgradeArrowPart.IsForceNeeded = isForceNeeded;
		arrows.Add(upgradeArrowPart);
		upgradeArrowPart.ShotArrow(upgradeArrowPart.transform, CachedBody.CachedTransform, arrowFlyTime);
	}

	private float GetTimeInterval()
	{
		int num = 0;
		float result = 0f;
		for (int i = 0; i < shotingWeights.Length; i++)
		{
			int num2 = Mathf.CeilToInt(shotingWeights[i] * (float)(maxInterruptedArrowsCount - additionalInterruptedArrowsCount));
			num += num2;
			if (releasedArrowsCount + 1 <= num && num2 != 0)
			{
				return Mathf.Max(sessionTime / (float)num2, 0f);
			}
		}
		return result;
	}

	private void GradedArrowPart_OnArrowInterrupted()
	{
		piercedArrowsCount++;
		SingletonMonoBehaviour<GameManager>.Instance.CameraController.ApplyShakeWithDuration(biteShakeDuration, biteShakeAmplitude);
	}
}
