using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolRoot : MonoBehaviour
{
	public GameObject prefab;

	private Stack<ObjectFromPool> stack = new Stack<ObjectFromPool>();

	private Vector3 GO_POOL_POSITION = new Vector3(0f, 0f, -1000f);

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

	public void Push(ObjectFromPool go)
	{
		go.inPool = true;
		stack.Push(go);
		go.OnReturnToPool();
		go.OnPush();
		go.CachedTransform.position = GO_POOL_POSITION;
		go.CachedTransform.parent = CachedTransform;
		go.gameObject.SetActive(false);
	}

	public ObjectFromPool Pop()
	{
		ObjectFromPool objectFromPool = null;
		objectFromPool = ((stack.Count != 0) ? stack.Pop() : CreateNewObject());
		objectFromPool.gameObject.SetActive(true);
		objectFromPool.inPool = false;
		objectFromPool.OnCreateFromPool();
		objectFromPool.OnPop();
		return objectFromPool;
	}

	private ObjectFromPool CreateNewObject()
	{
		GameObject gameObject = Object.Instantiate(prefab);
		gameObject.name = prefab.name;
		return gameObject.GetComponent<ObjectFromPool>();
	}
}
