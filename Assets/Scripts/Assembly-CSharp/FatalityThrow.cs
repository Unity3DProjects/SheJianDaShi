using System.Collections.Generic;
using UnityEngine;

public class FatalityThrow : FatalityBase
{
	private List<Collider> currentThrowParts = new List<Collider>();

	public override void StartFatality(CollisionInfo infoCollision)
	{
		if (base.IsActive)
		{
			if (currentThrowParts.Contains(infoCollision.otherCollider) || base.CachedManager.CharacterRagdoll.IsActive)
			{
				return;
			}
			Rigidbody[] rigidbodyArray = base.CachedManager.CharacterRagdoll.GetRigidbodyArray();
			if (rigidbodyArray == null)
			{
				return;
			}
			for (int i = 0; i < rigidbodyArray.Length; i++)
			{
				if (rigidbodyArray[i] != null)
				{
					Collider component = rigidbodyArray[i].GetComponent<Collider>();
					if (component == infoCollision.otherCollider)
					{
						rigidbodyArray[i].constraints = (RigidbodyConstraints)56;
						BreakAllJoint(rigidbodyArray[i]);
						currentThrowParts.Add(component);
						break;
					}
				}
			}
			return;
		}
		base.StartFatality(infoCollision);
		if (!base.CachedManager.CharacterRagdoll.IsActive)
		{
			base.CachedManager.CharacterRagdoll.Apply();
		}
		SkeletonUtilityBone utilityBone = base.CachedManager.GetUtilityBone(infoCollision.otherCollider);
		if (!(utilityBone != null))
		{
			return;
		}
		Rigidbody rigidbody = base.CachedManager.CharacterRagdoll.GetRigidbody(utilityBone.boneName);
		if ((bool)rigidbody)
		{
			Collider component2 = rigidbody.GetComponent<Collider>();
			if ((bool)component2)
			{
				currentThrowParts.Add(component2);
			}
			rigidbody.constraints = (RigidbodyConstraints)56;
			BreakAllJoint(rigidbody);
		}
		base.CachedManager.CurrentCharacter.SetCollidersEnabled(false, true);
	}

	public override void StopFatality()
	{
		base.StopFatality();
		currentThrowParts.Clear();
		base.CachedManager.CurrentCharacter.PartsCollidersEnabled = true;
	}
}
