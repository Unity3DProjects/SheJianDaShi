using System;
using UnityEngine;

public static class ObjectPoolHelper
{
	public static void ReturnToPool(this GameObject t)
	{
		PoolableObjectInfo component = t.GetComponent<PoolableObjectInfo>();
		if (component != null)
		{
			component.ReturnToPool();
		}
		else
		{
			UnityEngine.Object.Destroy(t);
		}
	}

	public static GameObject Clone(this GameObject t, bool autocreatepool = false, Action<GameObject> preAction = null)
	{
		GameObject gameObject = null;
		PoolableObjectInfo component = t.GetComponent<PoolableObjectInfo>();
		if (Application.isPlaying)
		{
			ObjectPool objectPool = null;
			if (component == null)
			{
				if (autocreatepool)
				{
					objectPool = PoolManager.Instance.PoolForObject(t);
				}
			}
			else
			{
				objectPool = component.GetPool();
			}
			if (objectPool != null)
			{
				gameObject = objectPool.Pop(preAction);
			}
		}
		if (gameObject == null)
		{
			gameObject = UnityEngine.Object.Instantiate(t);
			gameObject.name = t.name;
		}
		return gameObject;
	}
}
