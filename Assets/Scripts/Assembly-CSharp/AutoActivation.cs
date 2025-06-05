using UnityEngine;

public class AutoActivation : MonoBehaviour
{
	[SerializeField]
	protected float activatePossibility = 0.5f;

	[SerializeField]
	protected ShootType activateShootType;

	protected bool isActivationEnabled;

	public virtual bool AutoActivationEnabled(ShootType shootType)
	{
		isActivationEnabled = activateShootType == ShootType.None || activateShootType == shootType;
		if (isActivationEnabled)
		{
			float num = Random.Range(0f, 1f);
			isActivationEnabled = num <= activatePossibility;
		}
		return isActivationEnabled;
	}

	public virtual void CalculateActivationTime(WeaponGeneral weapon, Level level, Vector3 startPosition, Vector3 enemyPosition)
	{
	}

	public virtual bool IsNeedActivate(Vector3 position, Vector3 enemyPosition, float deltaTime)
	{
		return false;
	}
}
