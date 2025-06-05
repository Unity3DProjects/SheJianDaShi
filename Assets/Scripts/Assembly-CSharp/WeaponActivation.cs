using UnityEngine;

public class WeaponActivation : MonoBehaviour
{
	[SerializeField]
	private WeaponActivationType type;

	[SerializeField]
	private CollideObjectType collideType;

	[SerializeField]
	private MonoBehaviour targetScript;

	[SerializeField]
	private string activateMethod;

	[SerializeField]
	private bool isAutoRemoved;

	[SerializeField]
	private float activationTime = -1f;

	private float time;

	public WeaponActivationType Type
	{
		get
		{
			return type;
		}
	}

	public float ActivationTime
	{
		get
		{
			return activationTime;
		}
		set
		{
			activationTime = value;
		}
	}

	public bool IsAutoRemoved
	{
		get
		{
			return isAutoRemoved;
		}
		set
		{
			isAutoRemoved = value;
		}
	}

	public void Init(WeaponActivationType type, CollideObjectType collideType, MonoBehaviour targetScript, string activateMethod)
	{
		this.type = type;
		this.collideType = collideType;
		this.targetScript = targetScript;
		this.activateMethod = activateMethod;
	}

	public virtual bool IsActivationEnabled(WeaponActivationType activationType, ICollidable collidedObject)
	{
		bool result = false;
		if (type == WeaponActivationType.Hit)
		{
			if (collidedObject != null)
			{
				switch (collideType)
				{
				case CollideObjectType.All:
					result = true;
					break;
				case CollideObjectType.Character:
					result = collidedObject is Character;
					break;
				case CollideObjectType.Ground:
					result = collidedObject is Ground;
					break;
				}
			}
		}
		else if (type == WeaponActivationType.Tap && type == activationType)
		{
			result = true;
			if (base.isActiveAndEnabled)
			{
				SingletonMonoBehaviour<GameManager>.Instance.CameraController.ApplyShakeWithDuration();
			}
		}
		return result;
	}

	public void Activate(WeaponGeneral weapon, WeaponActivationType activationType, ICollidable collidedObject)
	{
		if (!string.IsNullOrEmpty(activateMethod))
		{
			targetScript.Invoke(activateMethod, 0f);
		}
		if (IsAutoRemoved)
		{
			weapon.RemoveActivation(this);
		}
	}

	public virtual void UpdateActivation(float deltaTime, WeaponGeneral weapon, WeaponActivationType activationType, ICollidable collidedObject)
	{
		if (type == WeaponActivationType.Timer)
		{
			time += deltaTime;
			if (time >= ActivationTime)
			{
				Activate(weapon, activationType, collidedObject);
			}
		}
	}
}
