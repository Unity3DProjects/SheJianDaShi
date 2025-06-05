using System.Collections.Generic;
using UnityEngine;

public class FatalityBreak : FatalityBase
{
	protected List<string> breakBonesNames = new List<string>();

	private SkeletonUtilityBone cachedBody;

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
			base.CachedManager.CurrentCharacter.ApplyCharacterRagdoll();
			BreakPartOnStart(infoCollision);
		}
	}

	public void BreakPart(CollisionInfo infoCollision)
	{
		SkeletonUtilityBone utilityBone = base.CachedManager.GetUtilityBone(infoCollision.otherCollider);
		BreakPart(infoCollision.otherCollider, utilityBone);
	}

	protected virtual void BreakPartOnStart(CollisionInfo infoCollision)
	{
		BreakPart(infoCollision);
	}

	protected virtual void BreakPart(Collider partCollider, SkeletonUtilityBone utilityBone)
	{
		if (utilityBone != null && utilityBone != CachedBody && !breakBonesNames.Contains(utilityBone.boneName))
		{
			breakBonesNames.Add(utilityBone.boneName);
			Rigidbody rigidbody = base.CachedManager.CharacterRagdoll.GetRigidbody(utilityBone.boneName);
			Vector3 vector = new Vector3(utilityBone.transform.position.x - CachedBody.transform.position.x, utilityBone.transform.position.y - CachedBody.transform.position.y, 0f);
			Vector3 eulerAngles = new Vector3(0f, 0f, Mathf.Atan2(vector.y, vector.x) * 57.29578f);
			if ((bool)rigidbody)
			{
				rigidbody.velocity = vector.normalized * Random.Range(300f, 500f);
				rigidbody.angularVelocity = new Vector3(0f, 0f, Random.Range(5f, 15f));
				rigidbody.maxAngularVelocity = 15f;
				rigidbody.constraints = (RigidbodyConstraints)56;
			}
			base.CachedEffect.EmitBloodEffect(base.CachedManager.CurrentCharacter.CharacterType, base.CachedManager.GetUtilityBone(utilityBone.bone.Parent.Data.Name).transform, utilityBone.transform.position, eulerAngles);
			base.CachedMask.Activate(partCollider, utilityBone);
			BreakAllJoint(rigidbody);
		}
	}

	protected override void PlayFatalityStartSound()
	{
		base.PlayFatalityStartSound();
		AudioManager.PlayAudioWithKey("fatality_break");
	}
}
