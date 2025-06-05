using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatalityTrident : FatalityBase
{
	[SerializeField]
	private float explodeDelay;

	private SkeletonUtilityBone cachedHead;

	private SkeletonUtilityBone cachedBody;

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
		base.StartFatality(infoCollision);
		StartCoroutine(AnimationFinished(explodeDelay));
	}

	protected void ApplyFatalityEffects()
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

	protected virtual IEnumerator AnimationFinished(float delay)
	{
		StartAnimation(base.CachedManager.CurrentCharacter.AnimationChache.FinishHimAnimation);
		yield return new WaitForSeconds(delay);
		SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
		{
			GameObject gameObject = Object.Instantiate(SingletonMonoBehaviour<EffectsManager>.Instance.EffectPrefabByType(EffectType.WeapomTridentActivationFountain));
			if (gameObject != null)
			{
				gameObject.transform.position = CachedBody.transform.position;
				Object.Destroy(gameObject.gameObject, 5f);
			}
		}, 0f);
		ExplodeCharacter();
		ApplyFatalityEffects();
	}
}
