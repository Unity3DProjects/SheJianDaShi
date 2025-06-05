using System.Collections.Generic;
using UnityEngine;

public class FatalityBurningBall : FatalityBreak
{
	[SerializeField]
	private List<BoneParticleSystem> boneSystems;

	private List<GameObject> particleSystems = new List<GameObject>();

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
		if (base.IsActive)
		{
			return;
		}
		base.StartFatality(infoCollision);
		base.CachedManager.CurrentCharacter.ApplyCharacterRagdoll();
		SkeletonUtilityBone utilityBone = base.CachedManager.GetUtilityBone(infoCollision.otherCollider);
		if (utilityBone == CachedHead)
		{
			Collider collider = base.CachedManager.GetCollider(utilityBone);
			BreakPart(collider, utilityBone);
			return;
		}
		List<SkeletonUtilityBone> list = new List<SkeletonUtilityBone>();
		list.Add(base.CachedManager.GetUtilityBoneHead());
		list.AddRange(base.CachedManager.GetUtilityBonesArms());
		list.AddRange(base.CachedManager.GetUtilityBonesLegs());
		foreach (SkeletonUtilityBone item in list)
		{
			Collider collider2 = base.CachedManager.GetCollider(utilityBone);
			BreakPart(collider2, item);
		}
		base.CachedEffect.EmitGutsEffect(base.CachedManager.CurrentCharacter.CharacterType, base.transform, base.CachedBody.transform.position, base.CachedBody.transform.eulerAngles);
		base.CachedEffect.EmitGuts(base.transform, base.CachedBody.transform.position, Vector3.up);
	}

	public override void StopFatality()
	{
		foreach (GameObject particleSystem in particleSystems)
		{
			particleSystem.StopFX(true);
		}
		base.StopFatality();
	}

	protected override void BreakPart(Collider partCollider, SkeletonUtilityBone utilityBone)
	{
		base.BreakPart(partCollider, utilityBone);
		if (boneSystems.Exists((BoneParticleSystem obj) => obj.boneName == utilityBone.boneName))
		{
			BoneParticleSystem boneParticleSystem = boneSystems.Find((BoneParticleSystem obj) => obj.boneName == utilityBone.boneName);
			if (boneParticleSystem.particleEffectPrefab != null && !string.IsNullOrEmpty(boneParticleSystem.particleEffectPrefab.assetGUID))
			{
				GameObject gameObject = (GameObject)boneParticleSystem.particleEffectPrefab.GetInstance();
				gameObject.transform.parent = utilityBone.transform;
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localRotation = Quaternion.identity;
				gameObject.PlayFX();
				particleSystems.Add(gameObject);
			}
		}
	}
}
