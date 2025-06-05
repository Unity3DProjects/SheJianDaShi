using UnityEngine;

public class GUIScreen : GUIControl
{
	[SerializeField]
	private GUIScreenType type;

	private Managers cachedManagers;

	private ObjectsSpawner objectsSpawner;

	public GUIScreenType Type
	{
		get
		{
			return type;
		}
	}

	protected Managers CachedManagers
	{
		get
		{
			if (cachedManagers == null)
			{
				cachedManagers = SingletonMonoBehaviour<Managers>.Instance;
			}
			return cachedManagers;
		}
	}

	protected ObjectsSpawner ObjectsSpawner
	{
		get
		{
			if (objectsSpawner == null)
			{
				objectsSpawner = CachedManagers.CachedLevelsManager.ObjectsSpawner;
			}
			return objectsSpawner;
		}
	}
}
