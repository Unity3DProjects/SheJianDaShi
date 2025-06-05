using System.Collections.Generic;
using UnityEngine;

public class FatalityBreakRandom : FatalityBreak
{
	public override void StartFatality(CollisionInfo infoCollision)
	{
		if (!base.IsActive)
		{
			base.StartFatality(infoCollision);
			List<SkeletonUtilityBone> list = new List<SkeletonUtilityBone>();
			list.Add(base.CachedManager.GetUtilityBoneHead());
			list.AddRange(base.CachedManager.GetUtilityBonesArms());
			list.AddRange(base.CachedManager.GetUtilityBonesLegs());
			SkeletonUtilityBone utilityBone = list.RandomObject();
			Collider collider = base.CachedManager.GetCollider(utilityBone);
			BreakPart(collider, utilityBone);
		}
	}

	protected override void BreakPartOnStart(CollisionInfo infoCollision)
	{
	}
}
