using UnityEngine;

public class PoolableObjectInfo : MonoBehaviour
{
	public ObjectPool poolReference;

	public bool inPool;

	public float distanceToPutToPoolX;

	public float distanceToPutToPoolRightX;

	public float distanceToPutToPoolY;

	public float distanceToPutToPoolZ = -30f;

	public bool checkX;

	public bool checkRightX;

	public bool checkY;

	public bool checkZ = true;

	public bool isDisappearDistanceZFixed;

	public bool checkLifeTime;

	public float lifeTime;

	private float lifeTimeCounter;

	private Transform cachedTransform;

	private Transform CachedTransform
	{
		get
		{
			if (cachedTransform == null)
			{
				cachedTransform = base.transform;
			}
			return cachedTransform;
		}
	}

	public void ReturnToPool()
	{
		if (poolReference != null && !inPool)
		{
			poolReference.Push(base.gameObject);
		}
	}

	public ObjectPool GetPool()
	{
		CheckPool();
		return poolReference;
	}

	private void CheckPool()
	{
		if (poolReference == null)
		{
			poolReference = PoolManager.Instance.PoolForObject(base.gameObject);
		}
	}
}
