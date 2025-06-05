using System.Collections.Generic;
using UnityEngine;

public class FatalityExplode : FatalityBase
{
	[SerializeField]
	private bool isExplodeCollidedObject = true;

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
		if (base.IsActive)
		{
			return;
		}
		base.StartFatality(infoCollision);
		base.ExplodeCharacter();
		List<SkeletonUtilityBone> list = new List<SkeletonUtilityBone>();
		if (isExplodeCollidedObject)
		{
			SkeletonUtilityBone utilityBone = base.CachedManager.GetUtilityBone(infoCollision.otherCollider);
			if ((bool)utilityBone)
			{
				list.Add(utilityBone);
			}
		}
		else
		{
			list.Add(base.CachedManager.GetUtilityBoneBody());
			list.Add(base.CachedManager.GetUtilityBoneHead());
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
}
