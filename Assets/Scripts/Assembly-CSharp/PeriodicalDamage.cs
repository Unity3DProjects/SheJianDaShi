using UnityEngine;

public class PeriodicalDamage : MonoBehaviour
{
	[SerializeField]
	private PeriodicalDamageType type;

	protected CharacterBase character;

	protected CharacterDamageHandler damageHandler;

	protected PeriodicalDamageInfo damageInfo;

	protected Collider affectedCollider;

	protected float currentTime;

	protected float currentPeriodicalDamage;

	protected float periodicalDamageTick;

	protected float currentPeriodicalDamageTime;

	public PeriodicalDamageType Type
	{
		get
		{
			return type;
		}
	}

	public void Init(CharacterBase character)
	{
		this.character = character;
	}

	public virtual void UpdateDamage(float deltaTime)
	{
		currentTime += deltaTime;
		if (currentTime < damageInfo.duration)
		{
			currentPeriodicalDamageTime -= deltaTime;
			if (currentPeriodicalDamageTime <= 0f && currentPeriodicalDamage > 0f)
			{
				PeriodicalDamageInfo periodicalDamageInfo = damageInfo;
				periodicalDamageInfo.position = base.transform.position;
				periodicalDamageInfo.damage = currentPeriodicalDamage;
				damageHandler.Character.ApplyPeriodicalDamage(periodicalDamageInfo);
				currentPeriodicalDamage -= damageInfo.damage / (damageInfo.frequency * damageInfo.duration);
				currentPeriodicalDamageTime += periodicalDamageTick;
			}
		}
		else
		{
			Remove();
		}
	}

	public virtual void ApplyPeriodicalDamage(CharacterDamageHandler damageHandler, PeriodicalDamageInfo damageInfo, Collider affectedCollider)
	{
		this.damageHandler = damageHandler;
		this.damageInfo = damageInfo;
		this.affectedCollider = affectedCollider;
		base.transform.parent = affectedCollider.transform;
		currentPeriodicalDamage = damageInfo.damage;
		periodicalDamageTick = 1f / damageInfo.frequency;
		currentPeriodicalDamageTime = 0f;
	}

	protected virtual void Remove()
	{
		damageHandler.RemovePeriodicalDamage(this);
	}
}
