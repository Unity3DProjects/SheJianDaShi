using UnityEngine;

public class ObjectFromPool : MonoBehaviour
{
	private Transform cachedTransform;

	private ObjectPoolRoot poolReference;

	private ObjectPoolManager cachedPoolManager;

	public bool inPool { get; set; }

	public Transform CachedTransform
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

	private ObjectPoolRoot PoolReference
	{
		get
		{
			if (poolReference == null)
			{
				poolReference = CachedPoolManager.PoolForObject(this);
			}
			return poolReference;
		}
	}

	private ObjectPoolManager CachedPoolManager
	{
		get
		{
			if (cachedPoolManager == null)
			{
				cachedPoolManager = SingletonMonoBehaviour<ObjectPoolManager>.Instance;
			}
			return cachedPoolManager;
		}
	}

	public virtual void OnCreateFromPool()
	{
	}

	public virtual void OnReturnToPool()
	{
	}

	public virtual void OnPop()
	{
	}

	public virtual void OnPush()
	{
	}

	public void ReturnToPool()
	{
		if (PoolReference != null && !inPool)
		{
			PoolReference.Push(this);
		}
	}
}
