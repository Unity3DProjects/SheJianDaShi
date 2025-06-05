using UnityEngine;

public class DailyGiftProposalMenu : GUIProposalMenu
{
	[SerializeField]
	private SkeletonAnimation idleAnimation;

	public override void Activate()
	{
		base.Activate();
		if (idleAnimation != null)
		{
			if (currentEffect != null)
			{
				Object.Destroy(currentEffect);
				currentEffect = null;
			}
			if (effectType != EffectType.None)
			{
				currentEffect = Object.Instantiate(SingletonMonoBehaviour<EffectsManager>.Instance.EffectPrefabByType(effectType));
				currentEffect.transform.parent = idleAnimation.transform;
				currentEffect.transform.localPosition = Vector3.zero;
			}
		}
	}
}
