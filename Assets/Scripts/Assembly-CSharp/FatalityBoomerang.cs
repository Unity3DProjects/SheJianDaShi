using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatalityBoomerang : FatalityBase
{
	private const float boomerangActivationTime = 1.3f;

	private SkeletonUtilityBone cachedHead;

	private SkeletonUtilityBone cachedBody;

	private bool isFatalityStarted;

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
		if (!isFatalityStarted)
		{
			base.StartFatality(infoCollision);
			StartCoroutine(StartCollisionFatality(infoCollision));
			isFatalityStarted = true;
		}
	}

	private IEnumerator StartCollisionFatality(CollisionInfo infoCollision)
	{
		yield return new WaitForSeconds(1.3f);
		if (base.CachedManager.CurrentCharacter.IsFinishHim)
		{
			ExplodeCharacter();
			ApplyFatalityEffects();
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
}
