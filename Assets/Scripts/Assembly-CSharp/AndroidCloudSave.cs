using System;
// using GooglePlayGames;
// using GooglePlayGames.BasicApi;
// using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine;

public class AndroidCloudSave : CloudSaveImplementation
{
	private bool hasLoadedFromCloud;

	private bool isWorking;

	private static string saveName = "BowmastersSaveSlotName";

	private static string loadingTextKey = "ui.toast.save.loading";

	public void LoadFromCloud()
	{
		if (CanAccessCloud() && !isWorking && !hasLoadedFromCloud)
		{
			isWorking = true;
			MiniclipUtils.SendToast(SingletonMonoBehaviour<LocalisationManager>.Instance.GetTextByKey(loadingTextKey));
			PerformLoadFromCloud();
		}
	}

	public void SaveToCloud()
	{
		if (CanAccessCloud() && !isWorking)
		{
			if (hasLoadedFromCloud)
			{
				isWorking = true;
				PerformSaveToCloud();
			}
			else
			{
				LoadFromCloud();
			}
		}
	}

	public void PerformLoadFromCloud()
	{
		// ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution(saveName, DataSource.ReadNetworkOnly, ConflictResolutionStrategy.UseLongestPlaytime, delegate(SavedGameRequestStatus requestStatus, ISavedGameMetadata game)
		// {
		// 	if (requestStatus != SavedGameRequestStatus.Success)
		// 	{
		// 		isWorking = false;
		// 		Log(string.Format("Load to cloud callback not succeeded - Status[{0}]", requestStatus));
		// 	}
		// 	else
		// 	{
		// 		((PlayGamesPlatform)Social.Active).SavedGame.ReadBinaryData(game, LoadFromCloudCallback);
		// 	}
		// });
	}

	public void PerformSaveToCloud()
	{
		// ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution(saveName, DataSource.ReadNetworkOnly, ConflictResolutionStrategy.UseLongestPlaytime, delegate(SavedGameRequestStatus requestStatus, ISavedGameMetadata game)
		// {
		// 	if (requestStatus != SavedGameRequestStatus.Success)
		// 	{
		// 		isWorking = false;
		// 		Log(string.Format("Save to cloud callback not succeeded - Status[{0}]", requestStatus));
		// 	}
		// 	else
		// 	{
		// 		SavedGameMetadataUpdate updateForMetadata = default(SavedGameMetadataUpdate.Builder).Build();
		// 		CloudSaveJsonDTO cloudSaveJsonDTO = new CloudSaveJsonDTO();
		// 		Log("DATA TO BE SAVED: " + cloudSaveJsonDTO.ToString());
		// 		byte[] updatedBinaryData = cloudSaveJsonDTO.ToBytes();
		// 		((PlayGamesPlatform)Social.Active).SavedGame.CommitUpdate(game, updateForMetadata, updatedBinaryData, SaveToCloudCallback);
		// 	}
		// });
	}

	// private void LoadFromCloudCallback(SavedGameRequestStatus status, byte[] data)
	// {
	// 	isWorking = false;
	// 	if (status != SavedGameRequestStatus.Success)
	// 	{
	// 		Log(string.Format("ReadBinaryData failed - Status[{0}]", status));
	// 		return;
	// 	}
	// 	hasLoadedFromCloud = true;
	// 	CloudSaveJsonDTO cloudSaveJsonDTO = SingletonMonoBehaviour<GameCenterManager>.Instance.loadSave(data);
	// 	if (cloudSaveJsonDTO != null)
	// 	{
	// 		Log("LOADED DATA: " + cloudSaveJsonDTO.ToString());
	// 		ApplySaveToGame(cloudSaveJsonDTO);
	// 		CloudSaveManager.SaveLoadedCallback();
	// 	}
	// }

	// private void SaveToCloudCallback(SavedGameRequestStatus status, ISavedGameMetadata game)
	// {
	// 	isWorking = false;
	// 	if (status != SavedGameRequestStatus.Success)
	// 	{
	// 		Log(string.Format("CommitUpdate failed - Status[{0}]", status));
	// 	}
	// }

	public bool CanAccessCloud()
	{
		return SingletonMonoBehaviour<GameCenterManager>.Instance.isAuthenticated && MiniclipUtils.isNetworkAvailable();
	}

	public void ApplySaveToGame(CloudSaveJsonDTO saveData)
	{
		StatisticsManager cachedStatisticsManager = SingletonMonoBehaviour<GameManager>.Instance.CachedManagers.CachedStatisticsManager;
		TutorialManager instance = SingletonMonoBehaviour<TutorialManager>.Instance;
		if (cachedStatisticsManager.Coins < saveData.coins)
		{
			cachedStatisticsManager.Coins = saveData.coins;
		}
		if (cachedStatisticsManager.Gems < saveData.gems)
		{
			cachedStatisticsManager.Gems = saveData.gems;
		}
		cachedStatisticsManager.FreeGemsReceived = saveData.freeGemsReceived;
		if (cachedStatisticsManager.BirdsKilled < saveData.birdsKilled)
		{
			cachedStatisticsManager.BirdsKilled = saveData.birdsKilled;
		}
		if (cachedStatisticsManager.HighScoreBirdsKilled < saveData.highScoreBirdsKilled)
		{
			cachedStatisticsManager.HighScoreBirdsKilled = saveData.highScoreBirdsKilled;
		}
		if (cachedStatisticsManager.FruitsSmashed < saveData.fruitsSmashed)
		{
			cachedStatisticsManager.FruitsSmashed = saveData.fruitsSmashed;
		}
		if (cachedStatisticsManager.HighScoreFruitsSmashed < saveData.highscoreFruitsSmashed)
		{
			cachedStatisticsManager.HighScoreFruitsSmashed = saveData.highscoreFruitsSmashed;
		}
		if (cachedStatisticsManager.TotalDamageApplied < saveData.totalDamageApplied)
		{
			cachedStatisticsManager.TotalDamageApplied = saveData.totalDamageApplied;
		}
		if (cachedStatisticsManager.WinsCount < saveData.winsCount)
		{
			cachedStatisticsManager.WinsCount = saveData.winsCount;
		}
		if (cachedStatisticsManager.LosesCount < saveData.losesCount)
		{
			cachedStatisticsManager.LosesCount = saveData.losesCount;
		}
		if (cachedStatisticsManager.MatchesCountVSHuman < saveData.matchesCountVsHuman)
		{
			cachedStatisticsManager.MatchesCountVSHuman = saveData.matchesCountVsHuman;
		}
		if (cachedStatisticsManager.MathesCount < saveData.matchesCount)
		{
			cachedStatisticsManager.MathesCount = saveData.matchesCount;
		}
		TimeSpan timeSpan = TimeSpan.Parse(saveData.totalGameTime);
		if (cachedStatisticsManager.TotalGameTime < timeSpan)
		{
			CustomPlayerPrefs.SetDateTime("total_game_time", DateTime.MinValue + timeSpan);
		}
		if (cachedStatisticsManager.OpenedChestsCount < saveData.openedChestsCount)
		{
			cachedStatisticsManager.OpenedChestsCount = saveData.openedChestsCount;
		}
		if (cachedStatisticsManager.FreeChestsOpened < saveData.freeChestsOpened)
		{
			cachedStatisticsManager.FreeChestsOpened = saveData.freeChestsOpened;
		}
		if (cachedStatisticsManager.UnlockedGameTypes < saveData.unlockedGameTypes)
		{
			cachedStatisticsManager.UnlockedGameTypes = saveData.unlockedGameTypes;
		}
		cachedStatisticsManager.LastCharacterID = saveData.lastCharacterKey;
		if (!cachedStatisticsManager.DoubleCoins)
		{
			cachedStatisticsManager.DoubleCoins = saveData.doubleCoinsKey;
		}
		if (cachedStatisticsManager.VideosWatched < saveData.videosWatched)
		{
			CustomPlayerPrefs.SetInt("videos_watched", saveData.videosWatched);
		}
		if (cachedStatisticsManager.TournamentEasyWin < saveData.tournamentEasyWin)
		{
			cachedStatisticsManager.TournamentEasyWin = saveData.tournamentEasyWin;
		}
		if (cachedStatisticsManager.TournamentMediumWin < saveData.tournamentMediumWin)
		{
			cachedStatisticsManager.TournamentMediumWin = saveData.tournamentMediumWin;
		}
		if (cachedStatisticsManager.TournamentHardWin < saveData.tournamentHardWin)
		{
			cachedStatisticsManager.TournamentHardWin = saveData.tournamentHardWin;
		}
		if (cachedStatisticsManager.CharactersBoughtOnBucks < saveData.charactersBoughtOnBucks)
		{
			cachedStatisticsManager.CharactersBoughtOnBucks = saveData.charactersBoughtOnBucks;
		}
		if (cachedStatisticsManager.GiftsOpened < saveData.giftsOpened)
		{
			cachedStatisticsManager.GiftsOpened = saveData.giftsOpened;
		}
		if (cachedStatisticsManager.SpentBucks < saveData.spentBucks)
		{
			cachedStatisticsManager.SpentBucks = saveData.spentBucks;
		}
		if (!instance.IsVSCompTutorialFinished)
		{
			instance.IsVSCompTutorialInProgress = saveData.isVSCompTutorialFinished;
		}
		if (!instance.IsVSHumanTutorialFinished)
		{
			instance.IsVSCompTutorialInProgress = saveData.isVSHumanTutorialFinished;
		}
		if (!instance.IsAppleGameTutorialFinished)
		{
			instance.IsAppleGameTutorialInProgress = saveData.isAppleGameTutorialFinished;
		}
		if (SingletonMonoBehaviour<SubscriptionManager>.Instance.LastSubscriptionRewardDate < DateTime.Parse(saveData.lastSubscriptionRewardDate))
		{
			SingletonMonoBehaviour<SubscriptionManager>.Instance.LastSubscriptionRewardDate = DateTime.Parse(saveData.lastSubscriptionRewardDate);
		}
		DateTime dateLastGift = SingletonMonoBehaviour<GiftManager>.Instance.DateLastGift;
		DateTime dateTime = DateTime.FromBinary(saveData.lastDailyGift);
		if (dateLastGift.Equals(DateTime.MinValue) || dateTime > dateLastGift)
		{
			SingletonMonoBehaviour<GiftManager>.Instance.DateLastGift = DateTime.FromBinary(saveData.lastDailyGift);
		}
		if (saveData.unlockedCharacters != null && saveData.unlockedCharacters.Length > 0)
		{
			ShopItemCharacter[] array = SingletonMonoBehaviour<ShopManager>.Instance.AvailableCharacters.FindAll((ShopItemCharacter x) => Array.IndexOf(saveData.unlockedCharacters, x.ProductID) != -1).ToArray();
			ShopItemCharacter[] array2 = array;
			foreach (ShopItemCharacter shopItemCharacter in array2)
			{
				int savedUpgradeLevel = saveData.GetSavedUpgradeLevel(shopItemCharacter);
				int levelAddition = savedUpgradeLevel - shopItemCharacter.CharacterUpgradeInfo.upgradeLevel;
				shopItemCharacter.UpgradeCharacterLevel(levelAddition);
				shopItemCharacter.OpenCharacter(true, false, false);
				if (SingletonMonoBehaviour<GUIScreenManager>.Instance.ActiveScreen == GUIScreenType.MainScrollView)
				{
					GUICharacter gUICharacter = SingletonMonoBehaviour<GameManager>.Instance.CachedManagers.CachedLevelsManager.ObjectsSpawner.GetGUICharacter(shopItemCharacter.Infos[0].characterNameType);
					gUICharacter.CheckBought();
				}
			}
		}
		if (SingletonMonoBehaviour<Managers>.Instance.CachedGUIScreenManager.MenuScreen != null)
		{
			SingletonMonoBehaviour<Managers>.Instance.CachedTutorialManager.LoadBuyCharacterOnCoinsTutorial(SingletonMonoBehaviour<Managers>.Instance.CachedGUIScreenManager.MenuScreen.MenuScreenScrollView);
		}
		if (saveData.unlockedActivations != null)
		{
			for (int num2 = 0; num2 < saveData.unlockedActivations.Length; num2++)
			{
				TutorialType tutorialType = (TutorialType)saveData.unlockedActivations[num2];
				instance.AddWeaponActivation(tutorialType);
			}
		}
		CustomPlayerPrefs.Save();
	}

	private void Log(string message)
	{
	}
}
