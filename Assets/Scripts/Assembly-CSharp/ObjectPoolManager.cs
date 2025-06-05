using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : SingletonMonoBehaviour<ObjectPoolManager>
{
	private Dictionary<string, ObjectPoolRoot> poolRoots = new Dictionary<string, ObjectPoolRoot>();

	public ObjectPoolRoot PoolForObject(ObjectFromPool prefab)
	{
		ObjectPoolRoot objectPoolRoot = FindPool(prefab);
		if (objectPoolRoot == null)
		{
			GameObject gameObject = new GameObject();
			gameObject.name = prefab.name;
			gameObject.transform.position = Vector3.zero;
			gameObject.transform.parent = base.transform;
			objectPoolRoot = gameObject.AddComponent<ObjectPoolRoot>();
			objectPoolRoot.prefab = prefab.gameObject;
			poolRoots.Add(prefab.name, objectPoolRoot);
		}
		return objectPoolRoot;
	}

	private ObjectPoolRoot FindPool(Object prefab)
	{
		ObjectPoolRoot value = null;
		if (!poolRoots.TryGetValue(prefab.name, out value))
		{
		}
		return value;
	}
}
