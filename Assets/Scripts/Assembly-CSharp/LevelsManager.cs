using UnityEngine;

public class LevelsManager : SingletonMonoBehaviour<LevelsManager>
{
	[SerializeField]
	private ObjectsSpawner objectsSpawnerPrefab;

	[SerializeField]
	private LevelsData levelsData;

	private ObjectsSpawner objectsSpawner;

	private Level currentLevel;

	public ObjectsSpawner ObjectsSpawner
	{
		get
		{
			if (objectsSpawner == null)
			{
				objectsSpawner = Object.Instantiate(objectsSpawnerPrefab);
				objectsSpawner.transform.parent = base.transform;
				objectsSpawner.Init();
			}
			return objectsSpawner;
		}
	}

	public Level CurrentLevel
	{
		get
		{
			return currentLevel;
		}
	}

	public int IndexSelectedCharacter { get; set; }

	public bool IsFirstCharacterDoubleChosen { get; set; }

	public bool IsSecondCharacterDoubleChosen { get; set; }

	public bool IsDoubleAvatarActive { get; set; }

	protected override void Awake()
	{
		base.Awake();
		levelsData.Init();
		SingletonMonoBehaviour<Managers>.Instance.CachedLocationsManager.Init();
	}

	public void CreateLevel(GameType type, CameraController cameraController)
	{
		currentLevel = levelsData.CreateLevel(type);
		currentLevel.transform.parent = base.transform;
		currentLevel.Init(ObjectsSpawner, cameraController);
	}

	public void UpdateLevel(float deltaTime)
	{
		if (currentLevel != null)
		{
			currentLevel.UpdateLevel(deltaTime);
		}
	}

	public void UpdateLevelLocation(float deltaTime)
	{
		if (currentLevel != null)
		{
			currentLevel.UpdateLevelLocation(deltaTime);
		}
	}

	public void RemoveLevel()
	{
		if (currentLevel != null)
		{
			Object.Destroy(currentLevel.gameObject);
			currentLevel = null;
		}
	}
}
