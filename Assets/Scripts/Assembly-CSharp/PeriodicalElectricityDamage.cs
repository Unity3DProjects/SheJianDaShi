using UnityEngine;

public class PeriodicalElectricityDamage : PeriodicalDamage
{
	[SerializeField]
	private EffectType effectType;

	private GameObject particleSystemParentHead;

	private GameObject particleSystemParentBody;

	private EffectsManager cachedEffectsManager;

	public GameObject ElectricityEffectPrefab
	{
		get
		{
			return CachedEffectsManager.EffectPrefabByType(effectType);
		}
	}

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

	public override void ApplyPeriodicalDamage(CharacterDamageHandler damageHandler, PeriodicalDamageInfo damageInfo, Collider affectedCollider)
	{
		base.ApplyPeriodicalDamage(damageHandler, damageInfo, affectedCollider);
		damageHandler.Character.StartElectricityAnimation();
		damageHandler.Character.StartElectricityEffect();
	}

	protected override void Remove()
	{
		damageHandler.Character.StopElectricityEffect(0f);
		base.Remove();
	}
}
