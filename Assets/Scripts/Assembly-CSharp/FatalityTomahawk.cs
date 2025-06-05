using System.Collections;
using System.Collections.Generic;
using Spine;
using UnityEngine;

public class FatalityTomahawk : FatalitySatelite
{
	[SerializeField]
	[ResourceLink]
	private AssetLink bloodEffectPrefab;

	[SerializeField]
	private float speedOfPushingHalfOfCharacter = 400f;

	[SerializeField]
	private float timeBeforeCharacterDivideUp = 0.6f;

	[SerializeField]
	private string boneForPushing = "head";

	[SerializeField]
	private string characterCopySortingLayer = "UnderCharacter";

	private CharacterBase characterCopy;

	private List<GameObject> particleSystems = new List<GameObject>();

	public override void StartFatality(CollisionInfo infoCollision)
	{
		if (!base.IsActive)
		{
			base.StartFatality(infoCollision);
			StartCoroutine(CoroutineAnimationFinished(base.CachedManager.AnimationGuillotine.Duration));
			StartAnimation(base.CachedManager.AnimationGuillotine);
			AudioManager.PlayAudioWithKey("frozen_spear_fatality_start_1");
			AudioManager.PlayAudioWithKey("frozen_spear_fatality_start_2");
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				AudioManager.StopAudioWithKey("frozen_spear_fatality_start_2", 0.5f);
			}, 1f);
		}
		base.StartFatality(infoCollision);
	}

	public override void StopFatality()
	{
		foreach (GameObject particleSystem in particleSystems)
		{
			particleSystem.StopFX(true);
		}
		base.StopFatality();
		StopAnimation(base.CachedManager.AnimationFreeze);
	}

	protected override void AnimationEnd(Spine.Animation anim)
	{
	}

	protected override void ObserverPhysics_EventTriggerEnter(Collider coll)
	{
		if (base.CachedManager.WithCharacterCollision(coll))
		{
			currentSatelite.GetComponent<ObserverPhysics>().EventTriggerEnter = GetStuck;
			if (base.CachedManager.CurrentCharacter != null)
			{
				characterCopy = Object.Instantiate(base.CachedManager.CurrentCharacter);
				characterCopy.SkeletonAnimation.state.SetAnimation(0, base.CachedManager.AnimationGuillotinePose, true);
				characterCopy.transform.parent = base.CachedManager.CurrentCharacter.transform.parent;
				characterCopy.SkeletonAnimation.ignoreMonoUpdate = false;
				characterCopy.SkeletonAnimation.MeshRenderer.sortingLayerName = characterCopySortingLayer;
				characterCopy.SetLevel(SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel);
				base.CachedManager.CurrentCharacter.SkeletonAnimation.ignoreMonoUpdate = false;
				base.CachedEffect.EmitBloodEffect(base.CachedManager.CurrentCharacter.CharacterType, base.CachedHead.transform, base.CachedHead.transform.position, Vector3.zero);
			}
		}
		else
		{
			GetStuck(coll);
		}
	}

	private void GetStuck(Collider coll)
	{
		ICollidable[] componentsInParent = coll.gameObject.GetComponentsInParent<ICollidable>();
		ICollidable[] array = componentsInParent;
		foreach (ICollidable collidable in array)
		{
			if (collidable is Ground)
			{
				if (groundCollisionEffectPrefab != null && !string.IsNullOrEmpty(groundCollisionEffectPrefab.assetGUID))
				{
					GameObject gameObject = (GameObject)groundCollisionEffectPrefab.GetInstance();
					gameObject.transform.parent = coll.gameObject.transform;
					gameObject.transform.localPosition = Vector3.zero;
				}
				RemoveEventHandlers();
				currentSatelite.GetComponent<Rigidbody>().isKinematic = true;
				StartCoroutine(CleaveCharacter(timeBeforeCharacterDivideUp));
				break;
			}
		}
	}

	private IEnumerator CoroutineAnimationFinished(float delay)
	{
		yield return new WaitForSeconds(delay);
		AudioManager.PlayAudioWithKey("frozen_spear_fatality_freeze");
	}

	private IEnumerator CleaveCharacter(float timeBeforeCleaving)
	{
		yield return new WaitForSeconds(timeBeforeCleaving);
		characterCopy.StartDefaultAnimation();
		characterCopy.ApplyCharacterRagdoll();
		base.CachedManager.CurrentCharacter.StartDefaultAnimation();
		base.CachedManager.CurrentCharacter.ApplyCharacterRagdoll();
		characterCopy.GetComponentsInChildren<Rigidbody>();
		SkeletonUtilityBone bone = base.CachedManager.GetUtilityBone(boneForPushing);
		Rigidbody[] componentsInChildren = characterCopy.GetComponentsInChildren<Rigidbody>();
		foreach (Rigidbody rigidbody in componentsInChildren)
		{
			if (rigidbody.gameObject.name == bone.boneName)
			{
				rigidbody.velocity = Vector3.left * speedOfPushingHalfOfCharacter;
			}
		}
		Rigidbody[] componentsInChildren2 = base.CachedManager.CurrentCharacter.GetComponentsInChildren<Rigidbody>();
		foreach (Rigidbody rigidbody2 in componentsInChildren2)
		{
			if (rigidbody2.gameObject.name == bone.boneName)
			{
				rigidbody2.velocity = Vector3.right * speedOfPushingHalfOfCharacter;
			}
		}
		if (bloodEffectPrefab != null && !string.IsNullOrEmpty(bloodEffectPrefab.assetGUID))
		{
			Object.Instantiate(bloodEffectPrefab.GetAsset(), base.CachedHead.transform.position, Quaternion.Euler(0f, 0f, 90f), characterCopy.transform);
		}
	}
}
