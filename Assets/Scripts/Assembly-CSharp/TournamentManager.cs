using System.Collections.Generic;
using MiniJSON;
using UnityEngine;

public class TournamentManager : SingletonMonoBehaviour<TournamentManager>
{
	private const string TOURNAMENT_KEY = "tournament";

	[SerializeField]
	private List<CharacterNameType> easyEnemies;

	[SerializeField]
	private HittingPobability[] easyEnemiesHittingProbability;

	[SerializeField]
	private List<CharacterNameType> mediumEnemies;

	[SerializeField]
	private HittingPobability[] mediumEnemiesHittingProbability;

	[SerializeField]
	private List<CharacterNameType> hardEnemies;

	[SerializeField]
	private HittingPobability[] hardEnemiesHittingProbability;

	[SerializeField]
	private List<CharacterNameType> bossEnemies;

	[SerializeField]
	private HittingPobability[] bossEnemiesHittingProbability;

	[SerializeField]
	private TournamentSettings[] availableTournamentSettings;

	[SerializeField]
	[Range(0f, 1f)]
	private float mediumTournamentHittingOffset;

	[SerializeField]
	[Range(0f, 1f)]
	private float mediumTournamentHittingOffsetProbability;

	[SerializeField]
	[Range(0f, 1f)]
	private float hardTournamentHittingOffset;

	[SerializeField]
	[Range(0f, 1f)]
	private float hardTournamentHittingOffsetProbability;

	[SerializeField]
	private int easyWinsForMediumUnlock = 1;

	[SerializeField]
	private int mediumWinsForHardUnlock = 1;

	private TournamentDescription currentTournament;

	private CharacterNameType playerCharacter;

	private List<CharacterNameType> enemies;

	public TournamentDescription CurrentTournament
	{
		get
		{
			return currentTournament;
		}
	}

	public bool IsMediumTournamentUnlocked
	{
		get
		{
			return SingletonMonoBehaviour<StatisticsManager>.Instance.TournamentEasyWin >= easyWinsForMediumUnlock;
		}
	}

	public bool IsHardTournamentUnlocked
	{
		get
		{
			return SingletonMonoBehaviour<StatisticsManager>.Instance.TournamentMediumWin >= mediumWinsForHardUnlock;
		}
	}

	public float CurrentEnemyHealth
	{
		get
		{
			return currentTournament.settings.health * Mathf.Pow(currentTournament.settings.healthIncreaseCoeff, currentTournament.currentEnemyNumber);
		}
	}

	public HittingPobability[] CurrentEnemyHittingProbability
	{
		get
		{
			HittingPobability[] result = easyEnemiesHittingProbability;
			if (mediumEnemies.Contains(EnemyCharacter))
			{
				result = mediumEnemiesHittingProbability;
			}
			else if (hardEnemies.Contains(EnemyCharacter))
			{
				result = hardEnemiesHittingProbability;
			}
			else if (bossEnemies.Contains(EnemyCharacter))
			{
				result = bossEnemiesHittingProbability;
			}
			return result;
		}
	}

	public bool IsTournamentSuccessfullyEnded
	{
		get
		{
			return currentTournament.currentEnemyNumber == Enemies.Count && currentTournament.currentEnemyNumber > 0;
		}
	}

	public CharacterNameType PlayerCharacter
	{
		get
		{
			if (playerCharacter == CharacterNameType.None)
			{
				if (currentTournament.playerCharacterID is int || currentTournament.playerCharacterID is CharacterNameType)
				{
					playerCharacter = (CharacterNameType)currentTournament.playerCharacterID;
				}
				else if (currentTournament.playerCharacterID is string)
				{
					playerCharacter = SingletonMonoBehaviour<LevelsManager>.Instance.ObjectsSpawner.CharacterNameTypeByProductID((string)currentTournament.playerCharacterID);
				}
			}
			return playerCharacter;
		}
	}

	public CharacterNameType EnemyCharacter
	{
		get
		{
			return Enemies[currentTournament.currentEnemyNumber];
		}
	}

	public List<CharacterNameType> Enemies
	{
		get
		{
			if (enemies == null || enemies.Count == 0)
			{
				enemies = new List<CharacterNameType>();
				if (currentTournament.enemiesID != null)
				{
					for (int i = 0; i < currentTournament.enemiesID.Count; i++)
					{
						CharacterNameType item = CharacterNameType.None;
						object obj = currentTournament.enemiesID[i];
						if (obj is int || obj is CharacterNameType)
						{
							item = (CharacterNameType)obj;
						}
						else if (obj is string)
						{
							item = SingletonMonoBehaviour<LevelsManager>.Instance.ObjectsSpawner.CharacterNameTypeByProductID((string)obj);
						}
						enemies.Add(item);
					}
				}
			}
			return enemies;
		}
	}

	public float HittingOffset
	{
		get
		{
			float result = 1f;
			if (CurrentTournament.settings.difficultType == TournamentDifficultType.Medium)
			{
				result = mediumTournamentHittingOffset;
			}
			else if (CurrentTournament.settings.difficultType == TournamentDifficultType.Hard)
			{
				result = hardTournamentHittingOffset;
			}
			return result;
		}
	}

	public float HittingOffsetProbability
	{
		get
		{
			float result = 0f;
			if (CurrentTournament.settings.difficultType == TournamentDifficultType.Medium)
			{
				result = mediumTournamentHittingOffsetProbability;
			}
			else if (CurrentTournament.settings.difficultType == TournamentDifficultType.Hard)
			{
				result = hardTournamentHittingOffsetProbability;
			}
			return result;
		}
	}

	public List<CharacterNameType> EasyCharacters
	{
		get
		{
			return easyEnemies;
		}
	}

	public List<CharacterNameType> MediumCharacters
	{
		get
		{
			return mediumEnemies;
		}
	}

	public List<CharacterNameType> HardCharacters
	{
		get
		{
			return hardEnemies;
		}
	}

	public List<CharacterNameType> BossCharacters
	{
		get
		{
			return bossEnemies;
		}
	}

	public string TournamentCoinsAward(TournamentDifficultType difficult)
	{
		string result = null;
		MicroachievementsManager instance = SingletonMonoBehaviour<MicroachievementsManager>.Instance;
		switch (difficult)
		{
		case TournamentDifficultType.Easy:
			result = instance.InfoByType(MicroachievementType.TournamentWinEasy).AwardString();
			break;
		case TournamentDifficultType.Medium:
			result = instance.InfoByType(MicroachievementType.TournamentWinMedium).AwardString();
			break;
		case TournamentDifficultType.Hard:
			result = instance.InfoByType(MicroachievementType.TournamentWinHard).AwardString();
			break;
		}
		return result;
	}

	public bool LoadTournament()
	{
		string text = CustomPlayerPrefs.GetString("tournament");
		if (!string.IsNullOrEmpty(text))
		{
			currentTournament = Json.Deserialize<TournamentDescription>(text);
			return currentTournament.canBeContinued;
		}
		currentTournament.settings.difficultType = TournamentDifficultType.None;
		return false;
	}

	public void ResetTournament()
	{
		currentTournament.canBeContinued = false;
		currentTournament.currentEnemyNumber = 0;
		SaveTournament();
	}

	public void GenerateTournament(TournamentDifficultType difficult, CharacterNameType currentCharacter)
	{
		if (currentTournament.enemiesID != null)
		{
			currentTournament.enemiesID.Clear();
		}
		else
		{
			currentTournament.enemiesID = new List<object>();
		}
		if (difficult == TournamentDifficultType.None)
		{
			currentTournament.settings.difficultType = difficult;
			return;
		}
		for (int i = 0; i < availableTournamentSettings.Length; i++)
		{
			if (availableTournamentSettings[i].difficultType == difficult)
			{
				currentTournament.settings = availableTournamentSettings[i];
				break;
			}
		}
		currentTournament.playerCharacterID = currentCharacter;
		playerCharacter = CharacterNameType.None;
		if (enemies != null)
		{
			enemies.Clear();
		}
		GenerateEnemies(easyEnemies, currentTournament.settings.easyEnemiesCount);
		GenerateEnemies(mediumEnemies, currentTournament.settings.mediumEnemiesCount);
		GenerateEnemies(hardEnemies, currentTournament.settings.hardEnemiesCount);
		GenerateEnemies(bossEnemies, currentTournament.settings.bossEnemiesCount);
		currentTournament.currentEnemyNumber = 0;
		SaveTournament();
	}

	public void StartTournamentLevel()
	{
		currentTournament.canBeContinued = true;
		SaveTournament();
	}

	public void EndTournamentLevel(bool isWin)
	{
		if (isWin)
		{
			currentTournament.currentEnemyNumber++;
			return;
		}
		currentTournament.canBeContinued = false;
		SaveTournament();
	}

	public void TournamentFinished()
	{
		AddStatistics();
		ResetTournament();
	}

	public void SetCanBeContinued(bool canBeContinued)
	{
		currentTournament.canBeContinued = canBeContinued;
		SaveTournament();
	}

	public void SaveTournament()
	{
		CustomPlayerPrefs.SetString("tournament", Json.Serialize(currentTournament));
	}

	private void GenerateEnemies(List<CharacterNameType> sourceList, int count)
	{
		if (count > sourceList.Count - 1)
		{
			Debug.LogError("Incorrect tournament settings for characters count!");
		}
		int[] array = GenerateRandomPermutation(sourceList.Count);
		for (int i = 0; i < array.Length; i++)
		{
			if (count <= 0)
			{
				break;
			}
			if (sourceList[array[i]] != PlayerCharacter)
			{
				currentTournament.enemiesID.Add(sourceList[array[i]]);
				count--;
			}
		}
	}

	private int[] GenerateRandomPermutation(int count)
	{
		int[] array = new int[count];
		for (int i = 0; i < count; i++)
		{
			array[i] = i;
		}
		for (int j = 0; j < count - 1; j++)
		{
			int num = Random.Range(j, count);
			int num2 = array[j];
			array[j] = array[num];
			array[num] = num2;
		}
		return array;
	}

	private void AddStatistics()
	{
		TournamentDifficultType difficultType = currentTournament.settings.difficultType;
		StatisticsManager instance = SingletonMonoBehaviour<StatisticsManager>.Instance;
		switch (difficultType)
		{
		case TournamentDifficultType.Easy:
			instance.TournamentEasyWin++;
			break;
		case TournamentDifficultType.Medium:
			instance.TournamentMediumWin++;
			break;
		case TournamentDifficultType.Hard:
			instance.TournamentHardWin++;
			break;
		}
	}
}
