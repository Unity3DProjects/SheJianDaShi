public class LevelTournament : LevelVSComputer
{
	private TournamentManager cachedTournamentManager;

	private TournamentManager CachedTournamentManager
	{
		get
		{
			if (cachedTournamentManager == null)
			{
				cachedTournamentManager = SingletonMonoBehaviour<TournamentManager>.Instance;
			}
			return cachedTournamentManager;
		}
	}

	protected override FloorSettings[] LevelFloorSettings
	{
		get
		{
			return CachedTournamentManager.CurrentTournament.settings.floorSettings;
		}
	}

	private void OnApplicationPause(bool paused)
	{
		SingletonMonoBehaviour<TournamentManager>.Instance.SetCanBeContinued(!paused);
	}

	public override void Init(ObjectsSpawner objectsSpawner, CameraController cameraController)
	{
		base.Init(objectsSpawner, cameraController);
		characterSecond.HittingProbabilities = CachedTournamentManager.CurrentEnemyHittingProbability;
		CachedTournamentManager.StartTournamentLevel();
	}

	protected override CharacterBase CreateBaseCharacter(bool flipped, CharacterBase selectedCharacter, float forcedHealth = -1f)
	{
		return base.CreateBaseCharacter(flipped, selectedCharacter, CachedTournamentManager.CurrentEnemyHealth);
	}

	protected override void EndLevel(Character loseCharacter)
	{
		CachedTournamentManager.EndTournamentLevel(loseCharacter != base.CharacterFirst);
		base.EndLevel(loseCharacter);
	}
}
