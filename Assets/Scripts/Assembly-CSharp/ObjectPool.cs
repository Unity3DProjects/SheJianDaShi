using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
	public GameObject prefab;

	public Transform Parent;

	public int preInstantiateCount;

	public bool autoExtend;

	public int currentCount;

	protected Stack<GameObject> stack = new Stack<GameObject>();

	private Vector3 GO_POOL_POSITION = new Vector3(0f, 0f, -1000f);

	private int countInPool;

	public int GetStackLenght()
	{
		return stack.Count;
	}

	public virtual void Push(GameObject go)
	{
		go.GetComponent<PoolableObjectInfo>().inPool = true;
		PushObject(go);
		Component[] componentsInChildren = go.GetComponentsInChildren(typeof(IPoolCallback));
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			IPoolCallback poolCallback = (IPoolCallback)componentsInChildren[i];
			poolCallback.OnReturnToPool();
		}
		Component[] components = go.GetComponents(typeof(IPoolCallback));
		for (int j = 0; j < components.Length; j++)
		{
			IPoolCallback poolCallback2 = (IPoolCallback)components[j];
			poolCallback2.OnPush();
		}
		Transform transform = go.transform;
		transform.position = GO_POOL_POSITION;
		if (transform.parent != Parent)
		{
			transform.SetParent(Parent);
		}
		if (Parent == null)
		{
			transform.SetParent(PoolManager.Instance.PoolObjectsRoot);
		}
		go.SetActive(false);
		currentCount = GetObjectsCount();
	}

	public virtual GameObject Pop(Action<GameObject> preAction = null)
	{
		GameObject gameObject = null;
		if (GetObjectsCount() == 0)
		{
			if (!autoExtend)
			{
				return null;
			}
			gameObject = CreateNewObject();
		}
		else
		{
			gameObject = PopObject();
		}
		gameObject.SetActive(true);
		gameObject.GetComponent<PoolableObjectInfo>().inPool = false;
		if (preAction != null)
		{
			preAction(gameObject);
		}
		Component[] componentsInChildren = gameObject.GetComponentsInChildren(typeof(IPoolCallback));
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			IPoolCallback poolCallback = (IPoolCallback)componentsInChildren[i];
			poolCallback.OnCreateFromPool();
		}
		Component[] components = gameObject.GetComponents(typeof(IPoolCallback));
		for (int j = 0; j < components.Length; j++)
		{
			IPoolCallback poolCallback2 = (IPoolCallback)components[j];
			poolCallback2.OnPop();
		}
		currentCount = GetObjectsCount();
		return gameObject;
	}

	protected virtual void Start()
	{
		Parent = base.transform;
		for (int i = 0; i < preInstantiateCount; i++)
		{
			GameObject go = CreateNewObject();
			Push(go);
		}
	}

	protected virtual int GetObjectsCount()
	{
		return stack.Count;
	}

	protected virtual void PushObject(GameObject go)
	{
		stack.Push(go);
	}

	protected virtual GameObject PopObject()
	{
		return stack.Pop();
	}

	public GameObject PopObjectForUnload()
	{
		return PopObject();
	}

	protected virtual GameObject CreateNewObject()
	{
		if (!prefab)
		{
			throw new Exception("Missing prefab in pool : " + base.gameObject.name);
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(prefab);
		gameObject.name = prefab.name;
		PoolableObjectInfo poolableObjectInfo = gameObject.GetComponent<PoolableObjectInfo>();
		if (poolableObjectInfo == null)
		{
			poolableObjectInfo = gameObject.AddComponent<PoolableObjectInfo>();
		}
		poolableObjectInfo.poolReference = this;
		return gameObject;
	}
}
