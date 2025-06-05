using UnityEngine;

public class ParticleSystemAutoDestroy : MonoBehaviour
{
	[SerializeField]
	private ParticleSystem systemRoot;

	private ParticleSystem CachedSystemRoot
	{
		get
		{
			if (systemRoot == null)
			{
				systemRoot = GetComponent<ParticleSystem>();
				if (!systemRoot)
				{
					systemRoot = GetComponentInChildren<ParticleSystem>();
				}
			}
			return systemRoot;
		}
	}

	private void Update()
	{
		if ((bool)CachedSystemRoot && !CachedSystemRoot.IsAlive())
		{
			Object.Destroy(base.gameObject);
		}
	}
}
