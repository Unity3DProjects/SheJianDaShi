using System.Collections;
using UnityEngine;

public class AutoDestroyObject : ObjectFromPool
{
	[SerializeField]
	private float lifeTime;

	private void OnEnable()
	{
		StartCoroutine("MoveToPool");
	}

	private IEnumerator MoveToPool()
	{
		yield return new WaitForSeconds(lifeTime);
		ReturnToPool();
	}
}
