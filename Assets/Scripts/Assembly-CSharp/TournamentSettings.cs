using System;

[Serializable]
public struct TournamentSettings
{
	public TournamentDifficultType difficultType;

	public FloorSettings[] floorSettings;

	public int easyEnemiesCount;

	public int mediumEnemiesCount;

	public int hardEnemiesCount;

	public int bossEnemiesCount;

	public float health;

	public float healthIncreaseCoeff;

	public int CharactersCount
	{
		get
		{
			return easyEnemiesCount + mediumEnemiesCount + hardEnemiesCount + bossEnemiesCount;
		}
	}
}
