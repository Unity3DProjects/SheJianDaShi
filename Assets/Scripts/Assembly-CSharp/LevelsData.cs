using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelsData : MonoBehaviour
{
	[SerializeField]
	private Level[] levelsPrefabs;

	private Dictionary<GameType, Level> levelsMap = new Dictionary<GameType, Level>();

	public void Init()
	{
		if (levelsPrefabs.Length == levelsMap.Count)
		{
			return;
		}
		Level[] array = levelsPrefabs;
		foreach (Level level in array)
		{
			if (!levelsMap.ContainsKey(level.GameType))
			{
				levelsMap.Add(level.GameType, level);
			}
			else
			{
				Debug.LogError("Duplicate level type");
			}
		}
	}

	public Level GetLevel(GameType type)
	{
		Level value;
		if (levelsMap.TryGetValue(type, out value))
		{
			return value;
		}
		throw new Exception("Missing level with TYPE : " + type);
	}

	public Level CreateLevel(GameType type)
	{
		GameObject original = GetLevel(type).gameObject;
		GameObject gameObject = UnityEngine.Object.Instantiate(original);
		return gameObject.GetComponent<Level>();
	}
}
