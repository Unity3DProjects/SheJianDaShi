using System.Collections.Generic;
using UnityEngine;

public class FatalityBleeding : FatalityBase
{
	[SerializeField]
	private FatalityParts bleedingParts;

	private SkeletonUtilityBone cachedHead;

	private SkeletonUtilityBone cachedBody;

	private List<SkeletonUtilityBone> possibleBones = new List<SkeletonUtilityBone>();

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
		if ((FatalityParts.Head & bleedingParts) != FatalityParts.None)
		{
			possibleBones.Add(base.CachedManager.GetUtilityBoneHead());
		}
		if ((FatalityParts.Arms & bleedingParts) != FatalityParts.None)
		{
			possibleBones.AddRange(base.CachedManager.GetUtilityBonesArms());
		}
		if ((FatalityParts.Legs & bleedingParts) != FatalityParts.None)
		{
			possibleBones.AddRange(base.CachedManager.GetUtilityBonesLegs());
		}
		if ((FatalityParts.Body & bleedingParts) != FatalityParts.None)
		{
			possibleBones.Add(base.CachedManager.GetUtilityBoneBody());
		}
		SkeletonUtilityBone utilityBone = base.CachedManager.GetUtilityBone(infoCollision.otherCollider);
		if (!possibleBones.Contains(utilityBone))
		{
			return;
		}
		base.StartFatality(infoCollision);
		base.CachedManager.CurrentCharacter.ApplyCharacterRagdoll();
		if (utilityBone != null)
		{
			Vector3 normalized = new Vector3(Mathf.Sign(infoCollision.direction.x) * 0.2f, 1f, 0f).normalized;
			base.CachedEffect.EmitBloodEffect(base.CachedManager.CurrentCharacter.CharacterType, infoCollision.otherCollider.transform, infoCollision.position, new Vector3(0f, 0f, Mathf.Atan2(infoCollision.direction.y, infoCollision.direction.x) * 57.29578f - 180f));
			base.CachedMask.Activate(startFatalityCollider, startFatalityUtilityBone);
			if (utilityBone == CachedHead)
			{
				base.CachedEffect.EmitBrainEffect(base.CachedManager.CurrentCharacter.CharacterType, base.transform, utilityBone.transform.position, utilityBone.transform.eulerAngles);
				base.CachedEffect.EmitBrain(base.transform, utilityBone.transform.position, normalized);
			}
			else if (utilityBone == CachedBody)
			{
				base.CachedEffect.EmitGutsEffect(base.CachedManager.CurrentCharacter.CharacterType, base.transform, utilityBone.transform.position, utilityBone.transform.eulerAngles);
				base.CachedEffect.EmitGuts(base.transform, utilityBone.transform.position, normalized);
			}
		}
	}

	public override void StopFatality()
	{
		base.StopFatality();
	}
}
