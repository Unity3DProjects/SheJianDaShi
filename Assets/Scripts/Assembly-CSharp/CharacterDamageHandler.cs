using System.Collections.Generic;
using UnityEngine;

public class CharacterDamageHandler : MonoBehaviour
{
	[SerializeField]
	private List<PeriodicalDamage> periodicalDamagePrefabs;

	private CharacterBase character;

	private Level level;

	private List<PeriodicalDamage> periodicalDamages = new List<PeriodicalDamage>();

	private List<PeriodicalDamage> removedPeriodicalDamages = new List<PeriodicalDamage>();

	private GameObject electricityEffectPrefab;

	private GameObject electricityThorEffectPrefab;

	public CharacterBase Character
	{
		get
		{
			return character;
		}
	}

	public bool IsPeriodicalDamageActive
	{
		get
		{
			return periodicalDamages.Count > 0;
		}
	}

	public GameObject ElectricityEffectPrefab
	{
		get
		{
			PeriodicalDamageType damageType = ((level.ActiveCharacter.CharacterNameType != CharacterNameType.ThorOdinson) ? PeriodicalDamageType.Electricity : PeriodicalDamageType.ThorElictricity);
			electricityEffectPrefab = (periodicalDamagePrefabs.Find((PeriodicalDamage obj) => obj.Type == damageType) as PeriodicalElectricityDamage).ElectricityEffectPrefab;
			return electricityEffectPrefab;
		}
	}

	public void Init(CharacterBase character, Level level)
	{
		this.character = character;
		this.level = level;
	}

	public void UpdateHandler(float deltaTime)
	{
		for (int num = periodicalDamages.Count - 1; num >= 0; num--)
		{
			PeriodicalDamage periodicalDamage = periodicalDamages[num];
			if (periodicalDamage != null)
			{
				periodicalDamage.UpdateDamage(deltaTime);
			}
			else
			{
				periodicalDamages.RemoveAt(num);
				level.TurnActionsChangeCount(-1);
			}
		}
		int count = removedPeriodicalDamages.Count;
		if (count <= 0)
		{
			return;
		}
		for (int i = 0; i < count; i++)
		{
			PeriodicalDamage periodicalDamage2 = removedPeriodicalDamages[i];
			if (periodicalDamage2 != null)
			{
				periodicalDamages.Remove(periodicalDamage2);
				Object.Destroy(periodicalDamage2.gameObject);
				if (periodicalDamages.Count == 0 && !character.IsFinishHim)
				{
					character.ApplyCharacterRagdoll(true);
					Vector3 force = Vector3.up * 2000f;
					character.RootRigidBody.AddForce(force, ForceMode.VelocityChange);
				}
				level.TurnActionsChangeCount(-1);
				break;
			}
		}
		removedPeriodicalDamages.Clear();
	}

	public void ApplyPeriodicalDamage(PeriodicalDamageInfo damageInfo, Collider affectedCollider)
	{
		PeriodicalDamage periodicalDamage = periodicalDamagePrefabs.Find((PeriodicalDamage obj) => obj.Type == damageInfo.type);
		if ((bool)periodicalDamage)
		{
			PeriodicalDamage periodicalDamage2 = Object.Instantiate(periodicalDamage);
			periodicalDamage2.ApplyPeriodicalDamage(this, damageInfo, affectedCollider);
			periodicalDamages.Add(periodicalDamage2);
			level.TurnActionsChangeCount(1);
		}
	}

	public void RemovePeriodicalDamage(PeriodicalDamage periodicalDamage)
	{
		removedPeriodicalDamages.Add(periodicalDamage);
	}
}
