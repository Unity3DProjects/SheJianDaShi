using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatalityUSAFlag : FatalitySatelite
{
	private const string BODY_SLOT_NAME = "skeleton_body";

	[SerializeField]
	private float jumpForce;

	[SerializeField]
	private float explodeCharacterForce;

	[SerializeField]
	private float fireworksDelay = 1f;

	[SerializeField]
	[ResourceLink]
	private AssetLink fireworksEffectPrefab;

	[SerializeField]
	private List<SoundElement> sounds;

	private GameObject fireworksEffect;

	public override void StartFatality(CollisionInfo infoCollision)
	{
		if (!base.IsActive)
		{
			base.StartFatality(infoCollision);
		}
	}

	protected override void ExplodeCharacter(bool isNeedBloodEffects = true)
	{
		base.CachedManager.CurrentCharacter.RemoveAllDriveIntoWeapon();
		base.CachedManager.CurrentCharacter.StartDefaultAnimation();
		base.CachedManager.CurrentCharacter.ApplyCharacterRagdoll();
		if (!GoreContentFilter.AllowDismemberment)
		{
			SkeletonUtilityBone utilityBoneBodyForSafeRagdoll = base.CachedManager.GetUtilityBoneBody();
			if ((bool)utilityBoneBodyForSafeRagdoll)
			{
				Rigidbody rigidbodyForSafeRagdoll = base.CachedManager.CharacterRagdoll.GetRigidbody(utilityBoneBodyForSafeRagdoll.boneName);
				if ((bool)rigidbodyForSafeRagdoll)
				{
					rigidbodyForSafeRagdoll.velocity = Vector3.up * explodeCharacterForce * 0.25f;
					rigidbodyForSafeRagdoll.constraints = (RigidbodyConstraints)56;
				}
			}
			base.CachedMask.Activate(startFatalityCollider, startFatalityUtilityBone);
			return;
		}
		SkeletonUtilityBone utilityBoneBody = base.CachedManager.GetUtilityBoneBody();
		if ((bool)utilityBoneBody)
		{
			Rigidbody rigidbody = base.CachedManager.CharacterRagdoll.GetRigidbody(utilityBoneBody.boneName);
			Rigidbody[] rigidbodyArray = base.CachedManager.CharacterRagdoll.GetRigidbodyArray();
			if ((bool)rigidbody && rigidbodyArray != null && rigidbodyArray.Length > 0)
			{
				rigidbody.velocity = Vector3.up * explodeCharacterForce;
				rigidbody.angularVelocity = Vector3.forward * 30f;
				for (int i = 0; i < rigidbodyArray.Length; i++)
				{
					if (rigidbodyArray[i] != rigidbody && rigidbodyArray[i] != null)
					{
						Quaternion quaternion = Quaternion.LookRotation(rigidbodyArray[i].transform.position - rigidbody.transform.position, Vector3.forward);
						rigidbodyArray[i].velocity = quaternion * new Vector3(explodeCharacterForce, 0f, 0f);
						rigidbodyArray[i].constraints = (RigidbodyConstraints)56;
					}
					if (rigidbodyArray[i] != null)
					{
						rigidbodyArray[i].constraints = (RigidbodyConstraints)56;
					}
					BreakAllJoint(rigidbodyArray[i]);
				}
			}
		}
		base.CachedEffect.EmitBrainEffect(base.CachedManager.CurrentCharacter.CharacterType, base.transform, base.CachedHead.transform.position, Vector3.zero);
		base.CachedEffect.EmitBrain(base.transform, base.CachedHead.transform.position, Vector3.up);
		base.CachedEffect.EmitGutsEffect(base.CachedManager.CurrentCharacter.CharacterType, base.transform, base.CachedBody.transform.position, Vector3.zero);
		base.CachedEffect.EmitGuts(base.transform, base.CachedBody.transform.position, Vector3.up);
		base.CachedMask.Activate(startFatalityCollider, startFatalityUtilityBone);
		if (base.CachedManager.CurrentCharacter.SkeletonAnimation.Skeleton.FindSlot("back") != null)
		{
			base.CachedManager.CurrentCharacter.SkeletonAnimation.Skeleton.SetAttachment("back", null);
		}
		if (base.CachedManager.CurrentCharacter.SkeletonAnimation.Skeleton.FindSlot("body") != null)
		{
			base.CachedManager.CurrentCharacter.SkeletonAnimation.Skeleton.SetAttachment("body", null);
		}
	}

	protected override void SetSateliteVelocity()
	{
		Rigidbody component = currentSatelite.GetComponent<Rigidbody>();
		if ((bool)component)
		{
			component.velocity = Vector2.up * startVerticalSpeed;
		}
	}

	protected override void ObserverPhysics_EventTriggerEnter(Collider coll)
	{
		base.ObserverPhysics_EventTriggerEnter(coll);
		SkeletonUtilityBone utilityBoneBody = base.CachedManager.GetUtilityBoneBody();
		if ((bool)utilityBoneBody)
		{
			Rigidbody rigidbody = base.CachedManager.CharacterRagdoll.GetRigidbody(utilityBoneBody.boneName);
			rigidbody.velocity = new Vector3(0f, jumpForce * 10f, 0f);
		}
		StartCoroutine(StartFireworks());
	}

	private IEnumerator StartFireworks()
	{
		yield return new WaitForSeconds(fireworksDelay);
		ExplodeCharacter();
		if (fireworksEffectPrefab != null && !string.IsNullOrEmpty(fireworksEffectPrefab.assetGUID))
		{
			fireworksEffect = (GameObject)fireworksEffectPrefab.GetInstance();
			fireworksEffect.transform.parent = SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.transform;
			fireworksEffect.transform.position = base.CachedManager.CurrentCharacter.HeadColliderBody.transform.position;
			if (GoreContentFilter.AllowDismemberment)
			{
				base.CachedManager.CurrentCharacter.RootRigidBody.velocity = Vector3.zero;
				base.CachedManager.CurrentCharacter.RootRigidBody.isKinematic = true;
				base.CachedManager.CurrentCharacter.SkeletonAnimation.skeleton.SetAttachment("skeleton_body", null);
			}
			for (int i = 0; i < sounds.Count; i++)
			{
				StartCoroutine(PlayFireworksSound(sounds[i]));
			}
		}
	}

	private IEnumerator PlayFireworksSound(SoundElement sound)
	{
		yield return new WaitForSeconds(sound.time);
		AudioManager.PlayAudioWithKey(sound.soundName);
	}
}
