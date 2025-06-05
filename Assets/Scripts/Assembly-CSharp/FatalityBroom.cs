using System.Collections.Generic;
using UnityEngine;

public class FatalityBroom : FatalityBase
{
	private const string BODY_SLOT_NAME = "skeleton_body";

	[SerializeField]
	private float fatalityAnimationDelay = 2f;

	[SerializeField]
	private float springValue = 1000000f;

	[SerializeField]
	private float springOffsetValue = 40f;

	[SerializeField]
	private float jointSpeedPerFrame = 1.65f;

	[SerializeField]
	private GameObject skullAnimationPrefab;

	[SerializeField]
	private float skullPositionOffset = 500f;

	[SerializeField]
	private float skullMouthPositionOffset = 200f;

	[SerializeField]
	private float skullLeftCharacterUnderOffset;

	private SkeletonUtilityBone cachedHead;

	private SkeletonUtilityBone cachedBody;

	private Character affectedCharacter;

	private SpringJoint springJoint;

	private SpringJoint springJointSecond;

	private bool isNeedRaiseCharacter;

	private GameObject firstJointRoot;

	private GameObject secondJointRoot;

	private float raiseStartPosition;

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
		if (base.IsActive)
		{
			return;
		}
		affectedCharacter = infoCollision.otherCollider.GetComponentInParent<Character>();
		affectedCharacter.ForceNotRestore = true;
		affectedCharacter.StartDefaultAnimation();
		affectedCharacter.SkeletonAnimation.State.ClearTrack(0);
		affectedCharacter.StartFaceDefaultAnimation();
		affectedCharacter.ApplyCharacterRagdoll();
		SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
		{
			raiseStartPosition = affectedCharacter.RootRigidBody.position.y;
			GameObject gameObject = Object.Instantiate(skullAnimationPrefab, affectedCharacter.RootRigidBody.position + Vector3.up * skullPositionOffset, Quaternion.identity, base.transform);
			if (SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.CharacterFirst == affectedCharacter)
			{
				gameObject.transform.localPosition += Vector3.forward * skullLeftCharacterUnderOffset;
			}
			GetComponentInChildren<TweenPosition>().enabled = true;
			firstJointRoot = new GameObject();
			firstJointRoot.transform.position = affectedCharacter.RootRigidBody.position;
			firstJointRoot.transform.parent = base.transform;
			firstJointRoot.transform.rotation = Quaternion.identity;
			firstJointRoot.AddComponent<Rigidbody>();
			firstJointRoot.GetComponent<Rigidbody>().isKinematic = true;
			secondJointRoot = new GameObject();
			secondJointRoot.transform.position = affectedCharacter.RootRigidBody.position;
			secondJointRoot.transform.parent = base.transform;
			secondJointRoot.transform.rotation = Quaternion.identity;
			secondJointRoot.AddComponent<Rigidbody>();
			secondJointRoot.GetComponent<Rigidbody>().isKinematic = true;
			isNeedRaiseCharacter = true;
			springJoint = affectedCharacter.RootRigidBody.gameObject.AddComponent<SpringJoint>();
			springJoint.autoConfigureConnectedAnchor = false;
			springJoint.connectedAnchor = Vector3.zero;
			springJoint.connectedBody = firstJointRoot.GetComponent<Rigidbody>();
			springJoint.enableCollision = false;
			springJoint.anchor = Vector3.down * springOffsetValue;
			springJoint.spring = 2f * springValue;
			springJointSecond = affectedCharacter.RootRigidBody.gameObject.AddComponent<SpringJoint>();
			springJointSecond.autoConfigureConnectedAnchor = false;
			springJointSecond.connectedAnchor = Vector3.zero;
			springJointSecond.connectedBody = secondJointRoot.GetComponent<Rigidbody>();
			springJointSecond.enableCollision = false;
			springJointSecond.anchor = Vector3.zero;
			springJointSecond.spring = springValue;
			AudioManager.PlayAudioWithKey("witch_fatality");
		}, fatalityAnimationDelay);
		base.StartFatality(infoCollision);
	}

	public override void UpdateFatality(float deltaTime)
	{
		if (!isNeedRaiseCharacter || !base.IsActive || !(base.CachedManager.ActiveFatality == this))
		{
			return;
		}
		firstJointRoot.transform.rotation = Quaternion.identity;
		firstJointRoot.transform.position += Vector3.up * jointSpeedPerFrame;
		secondJointRoot.transform.rotation = Quaternion.identity;
		secondJointRoot.transform.position += Vector3.up * jointSpeedPerFrame;
		if (!(affectedCharacter.RootRigidBody.transform.position.y > raiseStartPosition + skullMouthPositionOffset))
		{
			return;
		}
		SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
		{
			Object.Destroy(firstJointRoot.gameObject);
			Object.Destroy(springJoint);
			Object.Destroy(springJointSecond);
			ExplodeCharacter();
			ApplyFatalityEffects();
			base.CachedEffect.EmitBrainEffect(base.CachedManager.CurrentCharacter.CharacterType, base.transform, CachedHead.transform.position, Vector3.zero);
			base.CachedEffect.EmitGutsEffect(base.CachedManager.CurrentCharacter.CharacterType, base.transform, CachedBody.transform.position, Vector3.zero);
			SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.IsNeedFollowArcherOrWeapon = false;
			SingletonMonoBehaviour<GameManager>.Instance.CameraController.SetTarget(secondJointRoot.transform, Vector2.zero, Vector2.zero);
		}, 0f);
		SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
		{
			PlayCrowdHappySound();
			if (OnFatalityFinish != null)
			{
				OnFatalityFinish(this);
			}
		}, currentTime);
		isNeedRaiseCharacter = false;
	}

	protected override void PlayFatalityStartSound()
	{
		AudioManager.PlayAudioWithKey("fatality_start");
	}

	private void ApplyFatalityEffects()
	{
		List<SkeletonUtilityBone> list = new List<SkeletonUtilityBone>();
		list.Add(base.CachedManager.GetUtilityBoneBody());
		list.Add(base.CachedManager.GetUtilityBoneHead());
		if (list.Count > 0)
		{
			foreach (SkeletonUtilityBone item in list)
			{
				if (item == CachedHead)
				{
					base.CachedEffect.EmitBrain(base.transform, item.transform.position, Vector3.up);
				}
				else if (item == CachedBody)
				{
					base.CachedEffect.EmitGuts(base.transform, item.transform.position, Vector3.up);
				}
			}
		}
		base.CachedMask.Activate(startFatalityCollider, startFatalityUtilityBone);
	}
}
