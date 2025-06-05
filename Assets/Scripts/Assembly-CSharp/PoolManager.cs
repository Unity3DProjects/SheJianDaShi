using System.Collections.Generic;
using UnityEngine;

public class PoolManager : SingletonMonoBehaviour<PoolManager>
{
	public List<ObjectPool> pools = new List<ObjectPool>();

	public Dictionary<string, ObjectPool> poolMap = new Dictionary<string, ObjectPool>();

	private Transform poolObjectsRoot;

	public new static PoolManager Instance
	{
		get
		{
			PoolManager poolManager = SingletonMonoBehaviour<PoolManager>.InstanceIfExist;
			if (poolManager == null)
			{
				GameObject gameObject = new GameObject("PoolManager");
				poolManager = gameObject.AddComponent<PoolManager>();
			}
			return poolManager;
		}
	}

	public Transform PoolObjectsRoot
	{
		get
		{
			if (poolObjectsRoot == null)
			{
				GameObject gameObject = new GameObject("PoolObjectsRoot");
				poolObjectsRoot = gameObject.transform;
			}
			return poolObjectsRoot;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		base.gameObject.GetComponentsInChildren(pools);
		foreach (ObjectPool pool in pools)
		{
			if (pool.prefab == null)
			{
				CustomDebug.LogError("Missing prefab in pool : " + pool.name, pool);
			}
			else if (!poolMap.ContainsKey(pool.prefab.name))
			{
				poolMap.Add(pool.prefab.name, pool);
			}
			else
			{
				CustomDebug.LogError("Duplicate : " + pool.prefab.name, pool);
			}
		}
	}

	public ObjectPool FindPool(Object prefab)
	{
		ObjectPool value = null;
		if (!poolMap.TryGetValue(prefab.name, out value))
		{
		}
		return value;
	}

	public ObjectPool PoolForObject(GameObject prefab)
	{
		ObjectPool objectPool = FindPool(prefab);
		if (objectPool == null)
		{
			GameObject gameObject = new GameObject();
			gameObject.name = prefab.name + "Pool";
			gameObject.transform.position = Vector3.zero;
			gameObject.transform.parent = base.transform;
			objectPool = gameObject.AddComponent<ObjectPool>();
			objectPool.prefab = prefab;
			objectPool.autoExtend = true;
			pools.Add(objectPool);
			poolMap.Add(prefab.name, objectPool);
		}
		return objectPool;
	}

	public void RemoveObjectPool(ObjectPool pool)
	{
		if (pool != null)
		{
			pools.Remove(pool);
			poolMap.Remove(pool.prefab.name);
		}
	}
}
