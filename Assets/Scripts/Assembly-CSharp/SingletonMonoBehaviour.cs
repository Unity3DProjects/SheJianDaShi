using InternalSingleton;
using UnityEngine;

public abstract class SingletonMonoBehaviour<T> : MonoBehaviour, ISingletonMonoBehaviour where T : MonoBehaviour
{
	public static T Instance
	{
		get
		{
			return UnitySingleton<T>.GetSingleton(true);
		}
	}

	public static T InstanceIfExist
	{
		get
		{
			return UnitySingleton<T>.GetSingleton(false);
		}
	}

	public virtual bool IsSingletonObject
	{
		get
		{
			return true;
		}
	}

	protected virtual void Awake()
	{
		if (IsSingletonObject)
		{
			UnitySingleton<T>.InternalAwake(this as T);
		}
	}

	protected virtual void OnDestroy()
	{
		if (IsSingletonObject)
		{
			UnitySingleton<T>.InternalDestroy();
		}
	}
}
