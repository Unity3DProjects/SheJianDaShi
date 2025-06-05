using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class ICloudManager : SingletonMonoBehaviour<ICloudManager>
{
	[Serializable]
	public struct UpgradedCharactersData
	{
		public string characterID;

		public int upgradeLevel;
	}

	[Serializable]
	public struct SavedProgressData
	{
		public List<string> boughtCharactersID;

		public List<UpgradedCharactersData> upgradedCharactersData;

		public int coinsProgress;

		public int gemsProgress;

		public int freeGemsReceived;

		public int matchesCount;

		public int winsCount;

		public int losesCount;

		public int matchesCountVSHuman;

		public int matchesCountMultiplayer;

		public int matchesWinsMultiplayer;

		public int tournamentEasyWinsCount;

		public int tournamentMediumWinsCount;

		public int tournamentHardWinsCount;

		public bool noAdsBought;

		public bool allCharactersBought;
	}

	private enum ICloudInteractionState
	{
		None = 0,
		Save = 1,
		Restore = 2,
		Read = 3
	}

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnProgressDownloaded;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<int, int, int, bool> m_OnProgressReaded;

	private ICloudInteractionState iCloudInteractionState;

	private Managers cachedManagers;

	private bool isSavingDataAvailable;

	private bool isLoaderShowed = true;

	private bool isStorePopupInSettingsShowed;

	public bool IsProgressInDownload { get; set; }

	public int AutosavedCharacters { get; private set; }

	public int AutosavedCoins { get; private set; }

	public int AutosavedGems { get; private set; }

	public bool AutosavedAllIncludedBought { get; private set; }

	public bool IsSavingDataAvailable
	{
		get
		{
			return isSavingDataAvailable;
		}
		set
		{
			isSavingDataAvailable = value;
		}
	}

	public bool IsStorePopupInSettingsShowed
	{
		get
		{
			return isStorePopupInSettingsShowed;
		}
		set
		{
			isStorePopupInSettingsShowed = value;
		}
	}

	public bool IsLoginPopupActivated { get; set; }

	private Managers CachedManagers
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

	public ChoiceType BadChoiceType { get; set; }

	public static event Action OnProgressDownloaded
	{
		add
		{
			Action action = ICloudManager.m_OnProgressDownloaded;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref ICloudManager.m_OnProgressDownloaded, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = ICloudManager.m_OnProgressDownloaded;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref ICloudManager.m_OnProgressDownloaded, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<int, int, int, bool> OnProgressReaded
	{
		add
		{
			Action<int, int, int, bool> action = ICloudManager.m_OnProgressReaded;
			Action<int, int, int, bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref ICloudManager.m_OnProgressReaded, (Action<int, int, int, bool>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<int, int, int, bool> action = ICloudManager.m_OnProgressReaded;
			Action<int, int, int, bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref ICloudManager.m_OnProgressReaded, (Action<int, int, int, bool>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private void Start()
	{
		LLICloudManager.Initialize();
	}

	private void OnEnable()
	{
		LLICloudManager.OnProgressDownloaded += LLICloudManager_OnProgressDownloaded;
		LLICloudManager.OnICloudLoginFailed += LLICloudManager_OnICloudLoginFailed;
		LLICloudManager.OnProgressDownloadFailed += LLICloudManager_OnProgressDownloadFailed;
		LLICloudManager.OnProgressUploaded += LLICloudManager_OnProgressUploaded;
		TapToStartScreen.OnLoaderRemoved += TapToStartScreen_OnLoaderRemoved;
		TutorialManager.OnTutorialMatchesEnded += TutorialManager_OnTutorialMatchesEnded;
		StoreWarningPopup.OnStoreProgressResumed += StoreWarningPopup_OnStoreProgressResumed;
		ICloudLoaderPopup.OnRestoreProgressAgreed += ICloudLoaderPopup_OnRestoreProgressAgreed;
	}

	private void OnDisable()
	{
		LLICloudManager.OnProgressDownloaded -= LLICloudManager_OnProgressDownloaded;
		LLICloudManager.OnICloudLoginFailed -= LLICloudManager_OnICloudLoginFailed;
		LLICloudManager.OnProgressDownloadFailed -= LLICloudManager_OnProgressDownloadFailed;
		LLICloudManager.OnProgressUploaded -= LLICloudManager_OnProgressUploaded;
		TapToStartScreen.OnLoaderRemoved -= TapToStartScreen_OnLoaderRemoved;
		TutorialManager.OnTutorialMatchesEnded -= TutorialManager_OnTutorialMatchesEnded;
		StoreWarningPopup.OnStoreProgressResumed -= StoreWarningPopup_OnStoreProgressResumed;
		ICloudLoaderPopup.OnRestoreProgressAgreed -= ICloudLoaderPopup_OnRestoreProgressAgreed;
	}

	public void SaveProgressToICloud()
	{
		if (CachedManagers.CachedStatisticsManager.ICloudStore != ICLoudSaveType.NeededAndRestored || !IsSavingDataAvailable)
		{
			return;
		}
		iCloudInteractionState = ICloudInteractionState.Save;
		if (!LLICloudManager.IsLocalPlayerAuthenticated)
		{
			return;
		}
		SavedProgressData savedProgressData = new SavedProgressData
		{
			boughtCharactersID = new List<string>(),
			upgradedCharactersData = new List<UpgradedCharactersData>()
		};
		List<ShopItemCharacter> boughtCharacters = CachedManagers.CachedShopManager.BoughtCharacters;
		for (int i = 0; i < boughtCharacters.Count; i++)
		{
			savedProgressData.boughtCharactersID.Add(boughtCharacters[i].shopItemID);
		}
		List<ShopItemCharacter> list = boughtCharacters;
		for (int j = 0; j < CachedManagers.CachedShopManager.DefaultCharacters.Count; j++)
		{
			list.Add(CachedManagers.CachedShopManager.DefaultCharacters[j]);
		}
		for (int k = 0; k < list.Count; k++)
		{
			if (list[k].CharacterUpgradeInfo.upgradeLevel > 0)
			{
				UpgradedCharactersData item = new UpgradedCharactersData
				{
					upgradeLevel = list[k].CharacterUpgradeInfo.upgradeLevel,
					characterID = list[k].ShopItemID
				};
				savedProgressData.upgradedCharactersData.Add(item);
			}
		}
		savedProgressData.coinsProgress = CachedManagers.CachedStatisticsManager.Coins;
		savedProgressData.gemsProgress = CachedManagers.CachedStatisticsManager.Gems;
		savedProgressData.freeGemsReceived = CachedManagers.CachedStatisticsManager.FreeGemsReceived;
		savedProgressData.matchesCount = CachedManagers.CachedStatisticsManager.MathesCount;
		savedProgressData.winsCount = CachedManagers.CachedStatisticsManager.WinsCount;
		savedProgressData.losesCount = CachedManagers.CachedStatisticsManager.LosesCount;
		savedProgressData.matchesCountVSHuman = CachedManagers.CachedStatisticsManager.MatchesCountVSHuman;
		savedProgressData.matchesCountMultiplayer = CachedManagers.CachedStatisticsManager.MatchesCountMultiplayer;
		savedProgressData.matchesWinsMultiplayer = CachedManagers.CachedStatisticsManager.MatchesWinsMultiplayer;
		savedProgressData.tournamentEasyWinsCount = CachedManagers.CachedStatisticsManager.TournamentEasyWin;
		savedProgressData.tournamentMediumWinsCount = CachedManagers.CachedStatisticsManager.TournamentMediumWin;
		savedProgressData.tournamentHardWinsCount = CachedManagers.CachedStatisticsManager.TournamentHardWin;
		savedProgressData.noAdsBought = CachedManagers.CachedShopManager.NoAdsBought;
		savedProgressData.allCharactersBought = CachedManagers.CachedShopManager.AllCharactersBought;
		LLICloudManager.UploadData(JsonConvert.SerializeObject(savedProgressData));
	}

	public void ReadProgressFromICloud(bool isForRestore)
	{
		if (IsSavingDataAvailable && !IsProgressInDownload)
		{
			iCloudInteractionState = ((!isForRestore) ? ICloudInteractionState.Read : ICloudInteractionState.Restore);
			if (LLICloudManager.IsLocalPlayerAuthenticated)
			{
				LLICloudManager.DownloadData();
				IsProgressInDownload = true;
			}
			else
			{
				OpenLoginCenter(PopupType.GameCenterLogin);
			}
		}
	}

	public void OpenLoginCenter(PopupType popupType)
	{
		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.NoInternet, true, null, true);
		}
		else
		{
			CachedManagers.CachedGUIPopupManager.ShowPopup(popupType, true, null, true);
		}
	}

	private void LLICloudManager_OnProgressDownloaded(string savedProgress)
	{
		if (!string.IsNullOrEmpty(savedProgress))
		{
			SavedProgressData savedProgressData = JsonConvert.DeserializeObject<SavedProgressData>(savedProgress);
			if (!savedProgressData.IsNull())
			{
				if (iCloudInteractionState == ICloudInteractionState.Restore)
				{
					List<string> boughtCharactersID = savedProgressData.boughtCharactersID;
					List<ShopItemCharacter> availableCharacters = CachedManagers.CachedShopManager.AvailableCharacters;
					for (int i = 0; i < boughtCharactersID.Count; i++)
					{
						if (CachedManagers.CachedShopManager.GetShopItemCharacter(boughtCharactersID[i]) != null)
						{
							ShopItemCharacter shopItemCharacter = CachedManagers.CachedShopManager.GetShopItemCharacter(boughtCharactersID[i]);
							if (shopItemCharacter != null && !shopItemCharacter.Bought)
							{
								shopItemCharacter.OpenCharacter(false, false);
							}
						}
					}
					if (iCloudInteractionState == ICloudInteractionState.Restore)
					{
						List<ShopItemCharacter> boughtCharacters = CachedManagers.CachedShopManager.BoughtCharacters;
						for (int j = 0; j < boughtCharacters.Count; j++)
						{
							if (!boughtCharactersID.Contains(boughtCharacters[j].ShopItemID) && boughtCharacters[j] != null)
							{
								CustomPlayerPrefs.SetInt(boughtCharacters[j].ProductID, 0);
							}
						}
					}
					CachedManagers.CachedStatisticsManager.AddCoins(savedProgressData.coinsProgress - CachedManagers.CachedStatisticsManager.Coins, false);
					CachedManagers.CachedStatisticsManager.AddGems(savedProgressData.gemsProgress - CachedManagers.CachedStatisticsManager.Gems);
					CachedManagers.CachedStatisticsManager.FreeGemsReceived = Mathf.Max(savedProgressData.freeGemsReceived, CachedManagers.CachedStatisticsManager.FreeGemsReceived);
					if (savedProgressData.matchesCount >= CachedManagers.CachedStatisticsManager.MathesCount && savedProgressData.winsCount >= CachedManagers.CachedStatisticsManager.WinsCount && savedProgressData.matchesCountVSHuman >= CachedManagers.CachedStatisticsManager.MatchesCountVSHuman && savedProgressData.tournamentEasyWinsCount >= CachedManagers.CachedStatisticsManager.TournamentEasyWin && savedProgressData.tournamentMediumWinsCount >= CachedManagers.CachedStatisticsManager.TournamentMediumWin && savedProgressData.tournamentHardWinsCount >= CachedManagers.CachedStatisticsManager.TournamentHardWin)
					{
						CachedManagers.CachedStatisticsManager.WinsCount = savedProgressData.winsCount;
						CachedManagers.CachedStatisticsManager.LosesCount = savedProgressData.losesCount;
						CachedManagers.CachedStatisticsManager.MatchesCountVSHuman = savedProgressData.matchesCountVSHuman;
						CachedManagers.CachedStatisticsManager.MatchesCountMultiplayer = savedProgressData.matchesCountMultiplayer;
						CachedManagers.CachedStatisticsManager.MatchesWinsMultiplayer = savedProgressData.matchesWinsMultiplayer;
						CachedManagers.CachedStatisticsManager.TournamentEasyWin = savedProgressData.tournamentEasyWinsCount;
						CachedManagers.CachedStatisticsManager.TournamentMediumWin = savedProgressData.tournamentMediumWinsCount;
						CachedManagers.CachedStatisticsManager.TournamentHardWin = savedProgressData.tournamentHardWinsCount;
						CachedManagers.CachedStatisticsManager.MathesCount = savedProgressData.matchesCount;
					}
					CachedManagers.CachedShopManager.NoAdsBought = CachedManagers.CachedShopManager.NoAdsBought || savedProgressData.noAdsBought;
					CachedManagers.CachedShopManager.AllCharactersBought = CachedManagers.CachedShopManager.AllCharactersBought || savedProgressData.allCharactersBought;
					if (CachedManagers.CachedShopManager.AllIncludedBought)
					{
						for (int k = 0; k < availableCharacters.Count; k++)
						{
							if (!availableCharacters[k].Bought && (!availableCharacters[k].IsLockedByTimer || (availableCharacters[k].IsLockedByTimer && availableCharacters[k].IsTimeEnded)))
							{
								availableCharacters[k].OpenCharacter(false, false);
							}
						}
					}
					List<UpgradedCharactersData> upgradedCharactersData = savedProgressData.upgradedCharactersData;
					if (upgradedCharactersData != null)
					{
						for (int l = 0; l < upgradedCharactersData.Count; l++)
						{
							if (CachedManagers.CachedShopManager.GetShopItemCharacter(upgradedCharactersData[l].characterID) != null)
							{
								ShopItemCharacter shopItemCharacter2 = CachedManagers.CachedShopManager.GetShopItemCharacter(upgradedCharactersData[l].characterID);
								int num = upgradedCharactersData[l].upgradeLevel - shopItemCharacter2.CharacterUpgradeInfo.upgradeLevel;
								if (shopItemCharacter2 != null && (shopItemCharacter2.Bought || shopItemCharacter2.DefaultCharacter) && num > 0)
								{
									shopItemCharacter2.UpgradeCharacterLevel(num);
								}
							}
						}
					}
					for (int m = 0; m < availableCharacters.Count; m++)
					{
						if (availableCharacters[m].UpgradeInfo.Count == 0)
						{
							continue;
						}
						bool flag = true;
						if (upgradedCharactersData != null)
						{
							for (int n = 0; n < upgradedCharactersData.Count; n++)
							{
								if (CachedManagers.CachedShopManager.GetShopItemCharacter(upgradedCharactersData[n].characterID) != null && upgradedCharactersData[n].characterID == availableCharacters[m].ShopItemID && (availableCharacters[m].Bought || availableCharacters[m].DefaultCharacter))
								{
									flag = false;
								}
							}
						}
						if (flag)
						{
							availableCharacters[m].ClearCharacterLevel();
						}
					}
					AutosavedCharacters = savedProgressData.boughtCharactersID.Count;
					AutosavedCoins = savedProgressData.coinsProgress;
					AutosavedGems = savedProgressData.gemsProgress;
					AutosavedAllIncludedBought = savedProgressData.allCharactersBought;
					CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.RestoreProgress, true, null, true);
					SaveProgressToICloud();
					ICloudManager.m_OnProgressDownloaded?.Invoke();
					MenuScreen menuScreen = CachedManagers.CachedGUIScreenManager.MenuScreen;
					if (menuScreen != null)
					{
						menuScreen.UpdateMainMenuScreen(true);
						CachedManagers.CachedTutorialManager.StopBuyCharacterOnCoinsTutorial();
						CachedManagers.CachedTutorialManager.LoadBuyCharacterOnCoinsTutorial(menuScreen.MenuScreenScrollView);
					}
				}
				if (iCloudInteractionState == ICloudInteractionState.Read)
				{
					if (!isLoaderShowed)
					{
						CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.ICloudLoader, true, null, true);
						isLoaderShowed = true;
					}
					ICloudManager.m_OnProgressReaded?.Invoke(savedProgressData.coinsProgress, savedProgressData.boughtCharactersID.Count, savedProgressData.gemsProgress, savedProgressData.allCharactersBought);
				}
			}
		}
		IsProgressInDownload = false;
		CachedManagers.CachedTK2DUIManager.InputEnabled = true;
	}

	private void LLICloudManager_OnProgressDownloadFailed()
	{
		if (!isLoaderShowed)
		{
			CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.ICloudLoader, true, null, true);
			isLoaderShowed = true;
		}
		else
		{
			CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.NoProgress, true, null, true);
		}
		IsProgressInDownload = false;
	}

	private void LLICloudManager_OnICloudLoginFailed()
	{
		if (!isLoaderShowed)
		{
			CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.ICloudLoader, true, null, true);
			isLoaderShowed = true;
		}
		else if (iCloudInteractionState == ICloudInteractionState.Restore || iCloudInteractionState == ICloudInteractionState.Read)
		{
			OpenLoginCenter(PopupType.ICloudLogin);
		}
		IsProgressInDownload = false;
	}

	private void LLICloudManager_OnProgressUploaded()
	{
		ReadProgressFromICloud(false);
	}

	private void TapToStartScreen_OnLoaderRemoved()
	{
		CachedManagers.CachedScheduler.CallMethodWithDelay(this, delegate
		{
			if (CachedManagers.CachedTutorialManager.IsTutorialMatchesEnded)
			{
				TutorialManager_OnTutorialMatchesEnded();
			}
		}, 0.5f);
	}

	private void TutorialManager_OnTutorialMatchesEnded()
	{
		StoreWarningPopup_OnStoreProgressResumed();
	}

	private void ICloudLoaderPopup_OnRestoreProgressAgreed()
	{
		if ((CachedManagers.CachedGUIScreenManager.ActiveScreen == GUIScreenType.Settings || CachedManagers.CachedGUIScreenManager.ActiveScreen == GUIScreenType.Main) && CachedManagers.CachedShopManager.CanMakePurchase())
		{
			CachedManagers.CachedTK2DUIManager.InputEnabled = false;
			ReadProgressFromICloud(true);
		}
	}

	private void StoreWarningPopup_OnStoreProgressResumed()
	{
		IsSavingDataAvailable = true;
		if (CachedManagers.CachedStatisticsManager.ICloudStore == ICLoudSaveType.Needed)
		{
			CachedManagers.CachedStatisticsManager.ICloudStore = ICLoudSaveType.NeededAndRestored;
		}
	}
}
