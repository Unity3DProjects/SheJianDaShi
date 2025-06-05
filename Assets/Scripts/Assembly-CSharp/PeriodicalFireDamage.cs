using System.Collections.Generic;
using UnityEngine;

public class PeriodicalFireDamage : PeriodicalDamage
{
	[SerializeField]
	private float spreadMinDelay = 0.3f;

	[SerializeField]
	private float spreadMaxDelay = 0.5f;

	[SerializeField]
	private EffectType effectType;

	private GameObject particleSystemParent;

	private EffectsManager cachedEffectsManager;

	private float spreadDelay = -1f;

	private EffectsManager CachedEffectsManager
	{
		get
		{
			if (cachedEffectsManager == null)
			{
				cachedEffectsManager = SingletonMonoBehaviour<EffectsManager>.Instance;
			}
			return cachedEffectsManager;
		}
	}

	public override void UpdateDamage(float deltaTime)
	{
		base.UpdateDamage(deltaTime);
		if (damageInfo.isSpreadEnabled && spreadDelay > 0f && currentTime > spreadDelay)
		{
			spreadDelay = -1f;
			SpreadPeriodicalDamage();
		}
	}

	public override void ApplyPeriodicalDamage(CharacterDamageHandler damageHandler, PeriodicalDamageInfo damageInfo, Collider affectedCollider)
	{
		base.ApplyPeriodicalDamage(damageHandler, damageInfo, affectedCollider);
		damageHandler.Character.StartFireAnimation();
		spreadDelay = Random.Range(spreadMinDelay, spreadMaxDelay);
		SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
		{
			if (effectType != EffectType.None)
			{
				particleSystemParent = Object.Instantiate(CachedEffectsManager.EffectPrefabByType(effectType));
				particleSystemParent.transform.parent = affectedCollider.transform;
				particleSystemParent.transform.localPosition = Vector3.zero;
			}
		}, 0f);
	}

	protected override void Remove()
	{
		if ((bool)particleSystemParent)
		{
			particleSystemParent.StopFX(false);
		}
		base.Remove();
	}

	protected virtual void SpreadPeriodicalDamage()
	{
		List<Collider> list = new List<Collider>();
		SkeletonUtility component = damageHandler.Character.SkeletonAnimation.GetComponent<SkeletonUtility>();
		List<SkeletonUtilityBone> utilityBones = component.utilityBones;
		SkeletonUtilityBone currentBone = utilityBones.Find((SkeletonUtilityBone obj) => obj.boneName == affectedCollider.transform.parent.name);
		SkeletonUtilityBone skeletonUtilityBone = utilityBones.Find((SkeletonUtilityBone obj) => currentBone.bone.Parent != null && obj.bone == currentBone.bone.Parent);
		if ((bool)skeletonUtilityBone)
		{
			Collider[] componentsInChildren = skeletonUtilityBone.GetComponentsInChildren<Collider>();
			Collider[] array = componentsInChildren;
			foreach (Collider collider in array)
			{
				if (collider.transform.parent == skeletonUtilityBone.transform)
				{
					list.Add(collider);
				}
			}
		}
		List<SkeletonUtilityBone> list2 = utilityBones.FindAll((SkeletonUtilityBone obj) => currentBone.bone.Children.Contains(obj.bone));
		foreach (SkeletonUtilityBone item in list2)
		{
			Collider[] componentsInChildren2 = item.GetComponentsInChildren<Collider>();
			Collider[] array2 = componentsInChildren2;
			foreach (Collider collider2 in array2)
			{
				if (collider2.transform.parent == item.transform)
				{
					list.Add(collider2);
				}
			}
		}
		for (int num3 = 0; num3 < list.Count; num3++)
		{
			damageInfo.isSpreadEnabled = false;
			damageHandler.ApplyPeriodicalDamage(damageInfo, list[num3]);
		}
	}
}
