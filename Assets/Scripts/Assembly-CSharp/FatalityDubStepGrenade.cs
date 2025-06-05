using UnityEngine;

public class FatalityDubStepGrenade : FatalityPulse
{
	protected override void ExplodePart(SkeletonUtilityBone boneUtility)
	{
		base.ExplodePart(boneUtility);
		if (boneUtility == base.CachedHead)
		{
			Rigidbody rigidbody = base.CachedManager.CharacterRagdoll.GetRigidbody(base.CachedManager.GetUtilityBoneHead().boneName);
			if (rigidbody != null)
			{
				rigidbody.constraints = (RigidbodyConstraints)56;
				BreakAllJoint(rigidbody);
				rigidbody.transform.position = new Vector3(-10000f, -10000f, 0f);
				rigidbody.gameObject.SetActive(false);
			}
			base.CachedEffect.EmitBloodEffect(base.CachedManager.CurrentCharacter.CharacterType, base.CachedBody.transform, base.CachedHead.transform.position, base.CachedBody.transform.eulerAngles);
		}
	}

	protected override void CreateBloodEffects()
	{
	}
}
