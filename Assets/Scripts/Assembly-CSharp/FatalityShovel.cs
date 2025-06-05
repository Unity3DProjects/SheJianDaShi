using UnityEngine;

public class FatalityShovel : FatalityBreak
{
	[SerializeField]
	private FatalityMask masks;

	[SerializeField]
	private FatalityMask decal;

	private SkeletonUtilityBone cachedHead;

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

	public override void StartFatality(CollisionInfo infoCollision)
	{
		if (!base.IsActive)
		{
			base.StartFatality(infoCollision);
			cachedMask = masks;
			base.CachedManager.CurrentCharacter.ApplyCharacterRagdoll();
			SkeletonUtilityBone utilityBone = base.CachedManager.GetUtilityBone(infoCollision.otherCollider);
			if (utilityBone == CachedHead)
			{
				Collider collider = base.CachedManager.GetCollider(utilityBone);
				BreakPart(collider, utilityBone);
				base.CachedEffect.EmitBrainEffect(base.CachedManager.CurrentCharacter.CharacterType, base.transform, utilityBone.transform.position, utilityBone.transform.eulerAngles);
				base.CachedEffect.EmitBrain(base.transform, utilityBone.transform.position, Vector3.up);
			}
			else if (utilityBone == base.CachedBody)
			{
				base.CachedEffect.EmitBloodEffect(base.CachedManager.CurrentCharacter.CharacterType, infoCollision.otherCollider.transform, infoCollision.position, new Vector3(0f, 0f, Mathf.Atan2(infoCollision.direction.y, infoCollision.direction.x) * 57.29578f - 180f));
			}
			decal.Activate(startFatalityCollider, startFatalityUtilityBone);
		}
	}

	protected override void BreakPartOnStart(CollisionInfo infoCollision)
	{
	}
}
