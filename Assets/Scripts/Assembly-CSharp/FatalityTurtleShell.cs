using System.Collections.Generic;
using UnityEngine;

public class FatalityTurtleShell : FatalityBreak
{
	[SerializeField]
	private FatalityMask masks;

	[SerializeField]
	private FatalityMask decal;

	private SkeletonUtilityBone cachedHead;

	private List<SkeletonUtilityBone> cachedLegs;

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

	protected List<SkeletonUtilityBone> CachedLegs
	{
		get
		{
			if (cachedLegs == null)
			{
				cachedLegs = new List<SkeletonUtilityBone>(base.CachedManager.GetUtilityBonesLegs());
			}
			return cachedLegs;
		}
	}

	public override void StartFatality(CollisionInfo infoCollision)
	{
		if (base.IsActive)
		{
			return;
		}
		base.StartFatality(infoCollision);
		base.CachedManager.CurrentCharacter.ApplyCharacterRagdoll();
		cachedMask = masks;
		SkeletonUtilityBone utilityBone = base.CachedManager.GetUtilityBone(infoCollision.otherCollider);
		if (utilityBone == CachedHead)
		{
			Collider collider = base.CachedManager.GetCollider(utilityBone);
			BreakPart(collider, utilityBone);
			base.CachedEffect.EmitBrainEffect(base.CachedManager.CurrentCharacter.CharacterType, base.transform, utilityBone.transform.position, utilityBone.transform.eulerAngles);
			base.CachedEffect.EmitBrain(base.transform, utilityBone.transform.position, Vector3.up);
		}
		else if (CachedLegs.Contains(utilityBone))
		{
			Rigidbody rigidbody = base.CachedManager.CharacterRagdoll.GetRigidbody(base.CachedBody.boneName);
			rigidbody.velocity = new Vector3(0f, 2000f, 0f);
			foreach (SkeletonUtilityBone cachedLeg in CachedLegs)
			{
				Collider collider2 = base.CachedManager.GetCollider(cachedLeg);
				BreakPart(collider2, cachedLeg);
			}
		}
		else
		{
			Rigidbody rigidbody2 = base.CachedManager.CharacterRagdoll.GetRigidbody(base.CachedBody.boneName);
			if ((bool)rigidbody2)
			{
				rigidbody2.velocity = infoCollision.direction * 3000f;
			}
		}
		decal.Activate(startFatalityCollider, startFatalityUtilityBone);
	}

	protected override void BreakPartOnStart(CollisionInfo infoCollision)
	{
	}
}
