using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Spine;
using UnityEngine;

public class FatalityMagicSkull : FatalityBase
{
	private const string HEAD_BONE = "head";

	[SerializeField]
	private float delayBeforeStart;

	[SerializeField]
	private float explodeDelay;

	[SerializeField]
	private float explodeCharacterForce = 400f;

	[SerializeField]
	[ResourceLink]
	private AssetLink runeEffectPrefab;

	[SerializeField]
	[ResourceLink]
	private AssetLink soulStealingEffectPrefab;

	[SerializeField]
	[ResourceLink]
	private AssetLink burningBodyEffectPrefab;

	private SkeletonUtilityBone cachedHead;

	private SkeletonUtilityBone cachedBody;

	protected List<string> breakBonesNames = new List<string>();

	private GameObject runeEffect;

	private GameObject soulStealingEffect;

	private GameObject burningBodyEffect;

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
			base.CachedManager.CurrentCharacter.SkeletonAnimation.SkeletonDataAsset = base.CachedManager.CurrentCharacter.SkeletonDataAssetClean;
			base.CachedManager.CurrentCharacter.SkeletonAnimation.Reset();
			base.CachedManager.CurrentCharacter.SkeletonAnimation.Initialize();
			base.CachedManager.CurrentCharacter.SkeletonAnimation.Update();
			base.CachedManager.CurrentCharacter.SkeletonAnimation.ignoreMonoUpdate = true;
			StartCoroutine(CoroutineAnimationFinished(delayBeforeStart, infoCollision));
		}
	}

	public override void StopFatality()
	{
		base.StopFatality();
		if (soulStealingEffect != null)
		{
			Object.Destroy(soulStealingEffect);
			soulStealingEffect = null;
		}
		if (runeEffect != null)
		{
			Object.Destroy(runeEffect);
			runeEffect = null;
		}
	}

	protected override void PlayCrowdHappySound()
	{
	}

	private IEnumerator CoroutineAnimationFinished(float delayFatalityEffect, CollisionInfo infoCollision)
	{
		AudioManager.PlayAudioWithKey("magic_skull_fatality_ghost");
		if (runeEffectPrefab != null && !string.IsNullOrEmpty(runeEffectPrefab.assetGUID))
		{
			runeEffect = (GameObject)runeEffectPrefab.GetInstance();
			runeEffect.transform.parent = SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.transform;
			runeEffect.transform.position = base.CachedManager.CurrentCharacter.HeadColliderBody.transform.position;
		}
		yield return new WaitForSeconds(delayFatalityEffect);
		base.CachedManager.CurrentCharacter.SkeletonAnimation.State.SetAnimation(0, base.CachedManager.AnimationMagicSkull, false);
		yield return new WaitForSeconds(base.CachedManager.AnimationMagicSkull.Duration / 4f);
		if (soulStealingEffectPrefab != null && !string.IsNullOrEmpty(soulStealingEffectPrefab.assetGUID))
		{
			soulStealingEffect = (GameObject)soulStealingEffectPrefab.GetInstance();
			soulStealingEffect.transform.parent = SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.transform;
			soulStealingEffect.transform.position = base.CachedManager.CharacterAnimation.transform.position;
		}
		AudioManager.PlayAudioWithKey("magic_skull_fatality_explode");
		yield return new WaitForSeconds(explodeDelay);
		ExplodeCharacter(infoCollision);
		AudioManager.StopAudioWithKey("magic_skull_fatality_explode");
		AudioManager.PlayAudioWithKey("rocket_explosion");
		AudioManager.PlayAudioWithKey("frozen_spear_fatality_explode_4", delegate(SoundObject obj)
		{
			if (obj != null)
			{
				obj.Volume = 100f;
			}
		});
		_003CPlayCrowdHappySound_003E__BaseCallProxy0();
	}

	private void ExplodeCharacter(CollisionInfo infoCollision)
	{
		base.CachedManager.CurrentCharacter.StartDefaultAnimation();
		base.CachedManager.CurrentCharacter.ApplyCharacterRagdoll();
		SkeletonUtilityBone utilityBoneBody = base.CachedManager.GetUtilityBoneBody();
		if ((bool)utilityBoneBody)
		{
			Rigidbody rigidbody = base.CachedManager.CharacterRagdoll.GetRigidbody(utilityBoneBody.boneName);
			Rigidbody[] array = base.CachedManager.CharacterRagdoll.GetRigidbodyArray();
			if ((bool)rigidbody && array != null && array.Length > 0)
			{
				rigidbody.velocity = Vector3.up * explodeCharacterForce;
				rigidbody.angularVelocity = Vector3.forward * 30f;
				rigidbody.gameObject.GetComponent<MeshCollider>().enabled = false;
				SphereCollider sphereCollider = rigidbody.gameObject.AddComponent<SphereCollider>();
				sphereCollider.radius = 50f;
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] != rigidbody && array[i] != null)
					{
						Quaternion quaternion = Quaternion.LookRotation(array[i].transform.position - rigidbody.transform.position, Vector3.forward);
						array[i].velocity = quaternion * new Vector3(explodeCharacterForce, 0f, 0f);
						array[i].constraints = RigidbodyConstraints.FreezeRotationX;
						if (utilityBoneBody.bone.children.Exists((Bone bone) => bone.Data.Name == array[i].name))
						{
							base.CachedEffect.EmitBloodEffect(base.CachedManager.CurrentCharacter.CharacterType, utilityBoneBody.transform, array[i].transform.position, new Vector3(0f, 0f, Mathf.Atan2(array[i].transform.position.y - rigidbody.transform.position.y, array[i].transform.position.x - rigidbody.transform.position.x) * 57.29578f));
						}
					}
					if (array[i] != null)
					{
						array[i].constraints = (RigidbodyConstraints)56;
					}
					BreakAllJoint(array[i]);
					if (burningBodyEffectPrefab != null && !string.IsNullOrEmpty(burningBodyEffectPrefab.assetGUID))
					{
						burningBodyEffect = (GameObject)burningBodyEffectPrefab.GetInstance();
						burningBodyEffect.transform.parent = array[i].gameObject.transform;
						burningBodyEffect.transform.position = array[i].gameObject.transform.position;
					}
				}
			}
		}
		List<SkeletonUtilityBone> list = new List<SkeletonUtilityBone>();
		SkeletonUtilityBone utilityBone = base.CachedManager.GetUtilityBone(infoCollision.otherCollider);
		if ((bool)utilityBone)
		{
			list.Add(utilityBone);
		}
		if (list.Count > 0)
		{
			foreach (SkeletonUtilityBone item in list)
			{
				if (item == CachedHead)
				{
					base.CachedEffect.EmitBrainEffect(base.CachedManager.CurrentCharacter.CharacterType, base.transform, item.transform.position, item.transform.eulerAngles);
					base.CachedEffect.EmitBrain(base.transform, item.transform.position, Vector3.up);
				}
				else if (item == CachedBody)
				{
					base.CachedEffect.EmitGutsEffect(base.CachedManager.CurrentCharacter.CharacterType, base.transform, item.transform.position, item.transform.eulerAngles);
					base.CachedEffect.EmitGuts(base.transform, item.transform.position, Vector3.up);
				}
			}
		}
		base.CachedMask.Activate(startFatalityCollider, startFatalityUtilityBone);
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private void _003CPlayCrowdHappySound_003E__BaseCallProxy0()
	{
		base.PlayCrowdHappySound();
	}
}
