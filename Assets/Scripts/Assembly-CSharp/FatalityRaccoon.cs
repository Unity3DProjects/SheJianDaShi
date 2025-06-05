using System;
using System.Collections.Generic;
using UnityEngine;

public class FatalityRaccoon : FatalityBase
{
	[SerializeField]
	private float preFatalityAnimationDelay = 0.1f;

	[SerializeField]
	private float fatalityAnimationDelay = 1f;

	[SerializeField]
	private float springValue = 1000000f;

	[SerializeField]
	private float springOffsetValue = 40f;

	private SkeletonUtilityBone cachedHead;

	private SkeletonUtilityBone cachedBody;

	private Raccoon raccoon;

	private Character affectedCharacter;

	private SpringJoint springJoint;

	private SpringJoint springJointSecond;

	private bool isNeedForceRaccoonPosition;

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
		Raccoon.OnRaccoonExplode = (Action)Delegate.Combine(Raccoon.OnRaccoonExplode, new Action(Raccoon_OnRaccoonExplode));
	}

	private void OnDisable()
	{
		Raccoon.OnRaccoonExplode = (Action)Delegate.Remove(Raccoon.OnRaccoonExplode, new Action(Raccoon_OnRaccoonExplode));
	}

	public override void StartFatality(CollisionInfo infoCollision)
	{
		if (!base.IsActive && infoCollision.collider != null)
		{
			affectedCharacter = infoCollision.otherCollider.GetComponentInParent<Character>();
			affectedCharacter.ForceNotRestore = true;
			affectedCharacter.StartDefaultAnimation();
			affectedCharacter.SkeletonAnimation.State.ClearTrack(0);
			affectedCharacter.StartFaceDefaultAnimation();
			affectedCharacter.ApplyCharacterRagdoll();
			raccoon = infoCollision.collider.GetComponentInParent<Raccoon>();
			ExplodeComponent component = raccoon.GetComponent<ExplodeComponent>();
			if (component != null)
			{
				component.AddExplodeForceFromWeapon(affectedCharacter.RootRigidBody, raccoon);
			}
			for (int i = 0; i < raccoon.Colliders.Count; i++)
			{
				raccoon.Colliders[i].enabled = false;
			}
			raccoon.CachedRigidbody.isKinematic = true;
			Vector3 eulerAngles = raccoon.CachedTransform.localRotation.eulerAngles;
			raccoon.CachedTransform.localRotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, 0f);
			isNeedForceRaccoonPosition = true;
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				raccoon.EnablePreFatalityAnimation();
			}, preFatalityAnimationDelay);
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				isNeedForceRaccoonPosition = false;
				raccoon.EnableFatalityAnimation();
				springJoint = affectedCharacter.RootRigidBody.gameObject.AddComponent<SpringJoint>();
				springJoint.autoConfigureConnectedAnchor = false;
				springJoint.connectedAnchor = Vector3.zero;
				springJoint.connectedBody = raccoon.SpringJointRoot;
				springJoint.enableCollision = false;
				springJoint.anchor = Vector3.right * springOffsetValue;
				springJoint.spring = 2f * springValue;
				springJointSecond = affectedCharacter.RootRigidBody.gameObject.AddComponent<SpringJoint>();
				springJointSecond.autoConfigureConnectedAnchor = false;
				springJointSecond.connectedAnchor = Vector3.zero;
				springJointSecond.connectedBody = raccoon.SpringJointSecondRoot;
				springJointSecond.enableCollision = false;
				springJointSecond.anchor = Vector3.zero;
				springJointSecond.spring = springValue;
			}, fatalityAnimationDelay);
			base.StartFatality(infoCollision);
		}
	}

	public override void UpdateFatality(float deltaTime)
	{
		if (raccoon != null && isNeedForceRaccoonPosition)
		{
			raccoon.CachedTransform.position = startFatalityCollider.transform.position;
		}
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

	private void Raccoon_OnRaccoonExplode()
	{
		if (base.CachedManager.ActiveFatality == this)
		{
			if (springJoint != null)
			{
				UnityEngine.Object.Destroy(springJoint);
			}
			if (springJointSecond != null)
			{
				UnityEngine.Object.Destroy(springJointSecond);
			}
			ExplodeCharacter();
			ApplyFatalityEffects();
			AudioManager.PlayAudioWithKey("raccoon_fatality_explode");
			if (OnFatalityFinish != null)
			{
				OnFatalityFinish(this);
			}
		}
	}
}
