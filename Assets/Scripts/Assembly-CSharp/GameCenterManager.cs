using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
// using GooglePlayGames;
// using GooglePlayGames.BasicApi;
// using GooglePlayGames.BasicApi.Multiplayer;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class GameCenterManager : SingletonMonoBehaviour<GameCenterManager>
{
	[Serializable]
	private class CloudSaveDataObject
	{
		public int coins;

		public int birdsKilled;

		public int highScoreBirdsKilled;

		public int fruitsSmashed;

		public int highscoreFruitsSmashed;

		public float totalDamageApplied;

		public int winsCount;

		public int losesCount;

		public int matchesCountVsHuman;

		public int matchesCount;

		public TimeSpan totalGameTime;

		public int openedChestsCount;

		public int freeChestsOpened;

		public int unlockedGameTypes;

		public string lastCharacterKey;

		public bool doubleCoinsKey;

		public int videosWatched;

		public int tournamentEasyWin;

		public int tournamentMediumWin;

		public int tournamentHardWin;

		public int charactersBoughtOnBucks;

		public int giftsOpened;

		public int spentBucks;

		public long defeatedCharacters;

		public int ratePending;

		public CloudSaveDataObject()
		{
			StatisticsManager cachedStatisticsManager = SingletonMonoBehaviour<GameManager>.Instance.CachedManagers.CachedStatisticsManager;
			coins = cachedStatisticsManager.Coins;
			birdsKilled = cachedStatisticsManager.BirdsKilled;
			highScoreBirdsKilled = cachedStatisticsManager.HighScoreBirdsKilled;
			fruitsSmashed = cachedStatisticsManager.FruitsSmashed;
			highscoreFruitsSmashed = cachedStatisticsManager.HighScoreFruitsSmashed;
			totalDamageApplied = cachedStatisticsManager.TotalDamageApplied;
			winsCount = cachedStatisticsManager.WinsCount;
			losesCount = cachedStatisticsManager.LosesCount;
			matchesCountVsHuman = cachedStatisticsManager.MatchesCountVSHuman;
			matchesCount = cachedStatisticsManager.MathesCount;
			totalGameTime = cachedStatisticsManager.TotalGameTime;
			openedChestsCount = cachedStatisticsManager.OpenedChestsCount;
			freeChestsOpened = cachedStatisticsManager.FreeChestsOpened;
			unlockedGameTypes = cachedStatisticsManager.UnlockedGameTypes;
			lastCharacterKey = cachedStatisticsManager.LastCharacterID;
			doubleCoinsKey = cachedStatisticsManager.DoubleCoins;
			videosWatched = cachedStatisticsManager.VideosWatched;
			tournamentEasyWin = cachedStatisticsManager.TournamentEasyWin;
			tournamentMediumWin = cachedStatisticsManager.TournamentMediumWin;
			tournamentHardWin = cachedStatisticsManager.TournamentHardWin;
			charactersBoughtOnBucks = cachedStatisticsManager.CharactersBoughtOnBucks;
			giftsOpened = cachedStatisticsManager.GiftsOpened;
			spentBucks = cachedStatisticsManager.SpentBucks;
		}

		public static CloudSaveDataObject FromBytes(byte[] bytes)
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			memoryStream.Write(bytes, 0, bytes.Length);
			memoryStream.Seek(0L, SeekOrigin.Begin);
			return binaryFormatter.Deserialize(memoryStream) as CloudSaveDataObject;
		}

		public byte[] ToBytes()
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			MemoryStream memoryStream = new MemoryStream();
			binaryFormatter.Serialize(memoryStream, this);
			return memoryStream.ToArray();
		}

		public void Save()
		{
			StatisticsManager cachedStatisticsManager = SingletonMonoBehaviour<GameManager>.Instance.CachedManagers.CachedStatisticsManager;
			if (cachedStatisticsManager.Coins < coins)
			{
				CustomPlayerPrefs.SetInt("Coins", coins);
			}
			if (cachedStatisticsManager.BirdsKilled < birdsKilled)
			{
				cachedStatisticsManager.BirdsKilled = birdsKilled;
			}
			if (cachedStatisticsManager.HighScoreBirdsKilled < highScoreBirdsKilled)
			{
				cachedStatisticsManager.HighScoreBirdsKilled = highScoreBirdsKilled;
			}
			if (cachedStatisticsManager.FruitsSmashed < fruitsSmashed)
			{
				cachedStatisticsManager.FruitsSmashed = fruitsSmashed;
			}
			if (cachedStatisticsManager.HighScoreFruitsSmashed < highscoreFruitsSmashed)
			{
				cachedStatisticsManager.HighScoreFruitsSmashed = highscoreFruitsSmashed;
			}
			if (cachedStatisticsManager.TotalDamageApplied < totalDamageApplied)
			{
				cachedStatisticsManager.TotalDamageApplied = totalDamageApplied;
			}
			if (cachedStatisticsManager.WinsCount < winsCount)
			{
				cachedStatisticsManager.WinsCount = winsCount;
			}
			if (cachedStatisticsManager.LosesCount < losesCount)
			{
				cachedStatisticsManager.LosesCount = losesCount;
			}
			if (cachedStatisticsManager.MatchesCountVSHuman < matchesCountVsHuman)
			{
				cachedStatisticsManager.MatchesCountVSHuman = matchesCountVsHuman;
			}
			if (cachedStatisticsManager.MathesCount < matchesCount)
			{
				cachedStatisticsManager.MathesCount = matchesCount;
			}
			if (cachedStatisticsManager.TotalGameTime < totalGameTime)
			{
				CustomPlayerPrefs.SetDateTime("total_game_time", DateTime.MinValue + totalGameTime);
			}
			if (cachedStatisticsManager.OpenedChestsCount < openedChestsCount)
			{
				cachedStatisticsManager.OpenedChestsCount = openedChestsCount;
			}
			if (cachedStatisticsManager.FreeChestsOpened < freeChestsOpened)
			{
				cachedStatisticsManager.FreeChestsOpened = freeChestsOpened;
			}
			if (cachedStatisticsManager.UnlockedGameTypes < unlockedGameTypes)
			{
				cachedStatisticsManager.UnlockedGameTypes = unlockedGameTypes;
			}
			cachedStatisticsManager.LastCharacterID = lastCharacterKey;
			if (!cachedStatisticsManager.DoubleCoins)
			{
				cachedStatisticsManager.DoubleCoins = doubleCoinsKey;
			}
			if (cachedStatisticsManager.VideosWatched < videosWatched)
			{
				CustomPlayerPrefs.SetInt("videos_watched", videosWatched);
			}
			if (cachedStatisticsManager.TournamentEasyWin < tournamentEasyWin)
			{
				cachedStatisticsManager.TournamentEasyWin = tournamentEasyWin;
			}
			if (cachedStatisticsManager.TournamentMediumWin < tournamentMediumWin)
			{
				cachedStatisticsManager.TournamentMediumWin = tournamentMediumWin;
			}
			if (cachedStatisticsManager.TournamentHardWin < tournamentHardWin)
			{
				cachedStatisticsManager.TournamentHardWin = tournamentHardWin;
			}
			if (cachedStatisticsManager.CharactersBoughtOnBucks < charactersBoughtOnBucks)
			{
				cachedStatisticsManager.CharactersBoughtOnBucks = charactersBoughtOnBucks;
			}
			if (cachedStatisticsManager.GiftsOpened < giftsOpened)
			{
				cachedStatisticsManager.GiftsOpened = giftsOpened;
			}
			if (cachedStatisticsManager.SpentBucks < spentBucks)
			{
				cachedStatisticsManager.SpentBucks = spentBucks;
			}
			PlayerPrefs.Save();
		}

		public override string ToString()
		{
			return string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}, {16}, {17}, {18}, {19}, {20}, {21}, {22}, {23}", coins, birdsKilled, highScoreBirdsKilled, fruitsSmashed, highscoreFruitsSmashed, totalDamageApplied, winsCount, losesCount, matchesCountVsHuman, matchesCount, totalGameTime, openedChestsCount, freeChestsOpened, unlockedGameTypes, lastCharacterKey, doubleCoinsKey, videosWatched, tournamentEasyWin, tournamentMediumWin, tournamentHardWin, charactersBoughtOnBucks, giftsOpened, spentBucks, defeatedCharacters);
		}
	}

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnWillAuthenticate;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<bool> m_OnAuthentication;

	private static string TRIED_MANUAL_LOGIN = "tried_manual_login";

	public bool needsToShowInvitation;

	// private Invitation postponedInvitation;

	private Action pendingAction;

	// public Invitation CurrentInvitation
	// {
	// 	get
	// 	{
	// 		return postponedInvitation;
	// 	}
	// }

	private string LeaderboardWinsID
	{
		get
		{
			return "CgkIu-OZuaoTEAIQHQ";
		}
	}

	private string LeaderboardApplesID
	{
		get
		{
			return "CgkIu-OZuaoTEAIQHg";
		}
	}

	private string LeaderboardBirdsID
	{
		get
		{
			return "CgkIu-OZuaoTEAIQHw";
		}
	}

	public bool isAuthenticated
	{
		get
		{
			ActivateClientConfiguration();
			return true;
		}
	}

	public static event Action OnWillAuthenticate
	{
		add
		{
			Action action = GameCenterManager.m_OnWillAuthenticate;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref GameCenterManager.m_OnWillAuthenticate, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = GameCenterManager.m_OnWillAuthenticate;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref GameCenterManager.m_OnWillAuthenticate, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<bool> OnAuthentication
	{
		add
		{
			Action<bool> action = GameCenterManager.m_OnAuthentication;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref GameCenterManager.m_OnAuthentication, (Action<bool>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<bool> action = GameCenterManager.m_OnAuthentication;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref GameCenterManager.m_OnAuthentication, (Action<bool>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private IEnumerator Start()
	{
		ActivateClientConfiguration();
		while (Application.internetReachability == NetworkReachability.NotReachable)
		{
			yield return new WaitForSeconds(30f);
		}
		if (!isAuthenticated)
		{
			Debug.Log("[Cloud] - Is not authenticated");
			// PlayGamesPlatform.Instance.Authenticate(delegate(bool success)
			// {
				int num = CustomPlayerPrefs.GetInt(TRIED_MANUAL_LOGIN);
				if (!true && num < 3)
				{
					Debug.Log("[Cloud] - Trying manual to login nº " + num);
					CustomPlayerPrefs.SetInt(TRIED_MANUAL_LOGIN, num + 1);
					LoginUser();
				}
				else if (true)
				{
					Debug.Log("[Cloud] - Logged in with silent login");
					CloudSaveManager.Instance.LoadFromCloud();
					CustomPlayerPrefs.SetInt(TRIED_MANUAL_LOGIN, 0);
				}
				else
				{
					Debug.Log("[Cloud] - Failed to login, too many attemps on manual logins");
				}
			// }, true);
		}
		else
		{
			CloudSaveManager.Instance.LoadFromCloud();
		}
		yield return new WaitForSeconds(0f);
	}

	public string UserName()
	{
		if (isAuthenticated)
		{
			return Social.localUser.userName;
		}
		return null;
	}

	public void OpenLeaderboard()
	{
		if (!isAuthenticated)
		{
			pendingAction = OpenLeaderboard;
			LoginUser();
		}
		else
		{
			// Social.ShowLeaderboardUI();
		}
	}

	public void OpenAchievements()
	{
		if (!isAuthenticated)
		{
			pendingAction = OpenAchievements;
			LoginUser();
		}
		else
		{
			// Social.ShowAchievementsUI();
		}
	}

	public void ReportWinsLeaderboardScore(int s)
	{
		if (isAuthenticated)
		{
			// Social.ReportScore(s, LeaderboardWinsID, delegate
			// {
			// });
		}
	}

	public void ReportApplesLeaderboardScore(int s)
	{
		if (isAuthenticated)
		{
			// Social.ReportScore(s, LeaderboardApplesID, delegate
			// {
			// });
		}
	}

	public void ReportBirdsLeaderboardScore(int s)
	{
		if (isAuthenticated)
		{
			// Social.ReportScore(s, LeaderboardBirdsID, delegate
			// {
			// });
		}
	}

	public void ReportAchievementProgress(string postfix)
	{
		if (isAuthenticated)
		{
			string achievementID = string.Empty;
			switch (postfix)
			{
			case "arrow1":
				achievementID = "CgkIu-OZuaoTEAIQAA";
				break;
			case "arrow2":
				achievementID = "CgkIu-OZuaoTEAIQAQ";
				break;
			case "arrow3":
				achievementID = "CgkIu-OZuaoTEAIQAg";
				break;
			case "arrow4":
				achievementID = "CgkIu-OZuaoTEAIQAw";
				break;
			case "arrow5":
				achievementID = "CgkIu-OZuaoTEAIQBA";
				break;
			case "arrow6":
				achievementID = "CgkIu-OZuaoTEAIQBQ";
				break;
			case "unite1":
				achievementID = "CgkIu-OZuaoTEAIQBg";
				break;
			case "unite2":
				achievementID = "CgkIu-OZuaoTEAIQBw";
				break;
			case "unite3":
				achievementID = "CgkIu-OZuaoTEAIQCA";
				break;
			case "unite4":
				achievementID = "CgkIu-OZuaoTEAIQCQ";
				break;
			case "unite5":
				achievementID = "CgkIu-OZuaoTEAIQCg";
				break;
			case "unite6":
				achievementID = "CgkIu-OZuaoTEAIQCw";
				break;
			case "unite7":
				achievementID = "CgkIu-OZuaoTEAIQDA";
				break;
			case "unite8":
				achievementID = "CgkIu-OZuaoTEAIQDQ";
				break;
			case "comeback1":
				achievementID = "CgkIu-OZuaoTEAIQDg";
				break;
			case "comeback2":
				achievementID = "CgkIu-OZuaoTEAIQDw";
				break;
			case "comeback3":
				achievementID = "CgkIu-OZuaoTEAIQEA";
				break;
			case "comeback4":
				achievementID = "CgkIu-OZuaoTEAIQEQ";
				break;
			case "comeback5":
				achievementID = "CgkIu-OZuaoTEAIQEg";
				break;
			case "comeback6":
				achievementID = "CgkIu-OZuaoTEAIQEw";
				break;
			case "comeback7":
				achievementID = "CgkIu-OZuaoTEAIQFA";
				break;
			case "searage":
				achievementID = "CgkIu-OZuaoTEAIQFQ";
				break;
			case "kingsgods":
				achievementID = "CgkIu-OZuaoTEAIQFg";
				break;
			case "heroesvillains":
				achievementID = "CgkIu-OZuaoTEAIQFw";
				break;
			case "greatestwarriors":
				achievementID = "CgkIu-OZuaoTEAIQGA";
				break;
			case "music":
				achievementID = "CgkIu-OZuaoTEAIQGQ";
				break;
			case "sport":
				achievementID = "CgkIu-OZuaoTEAIQGg";
				break;
			case "brutal":
				achievementID = "CgkIu-OZuaoTEAIQGw";
				break;
			case "girlspower":
				achievementID = "CgkIu-OZuaoTEAIQHA";
				break;
			}
			// Social.ReportProgress(achievementID, 100.0, delegate
			// {
			// });
		}
	}

	// public void OnInvitationReceived(Invitation invitation, bool shouldAutoAccept)
	// {
	// 	// postponedInvitation = invitation;
	// 	if (shouldAutoAccept)
	// 	{
	// 		LLMultiplayerManager.Instance.PlayerAcceptedInvitation(string.Empty);
	// 	}
	// 	else if (SingletonMonoBehaviour<GUIScreenManager>.Instance.ActiveScreen == GUIScreenType.Main)
	// 	{
	// 		SingletonMonoBehaviour<GUIPopupManager>.Instance.ShowPopup(PopupType.Invitation);
	// 	}
	// 	else
	// 	{
	// 		needsToShowInvitation = true;
	// 	}
	// }

	private void LoginUser()
	{
		GameCenterManager.m_OnWillAuthenticate?.Invoke();
		Social.localUser.Authenticate(delegate(bool success)
		{
			GameCenterManager.m_OnAuthentication?.Invoke(success);
			CloudSaveManager.Instance.LoadFromCloud();
			if (success && pendingAction != null)
			{
				pendingAction();
				pendingAction = null;
			}
		});
	}

	private void ActivateClientConfiguration()
	{
		// PlayGamesClientConfiguration configuration = new PlayGamesClientConfiguration.Builder().WithInvitationDelegate(OnInvitationReceived).EnableSavedGames().Build();
		// PlayGamesPlatform.InitializeInstance(configuration);
		// PlayGamesPlatform.Activate();
	}

	public CloudSaveJsonDTO loadSave(byte[] data)
	{
		CloudSaveJsonDTO cloudSaveDTO = null;
		if (!CloudSaveJsonDTO.TryFromBytes(out cloudSaveDTO, data))
		{
			Debug.Log("[CloudSave] Could not read save as a JSON, converting from binary...");
			try
			{
				cloudSaveDTO = convertOldToNew(CloudSaveDataObject.FromBytes(data));
			}
			catch (Exception ex)
			{
				Debug.Log("[CloudSave] Error converting save " + ex.ToString());
			}
		}
		else
		{
			Debug.Log("[CloudSave] Successefull load");
		}
		return cloudSaveDTO;
	}

	private CloudSaveJsonDTO convertOldToNew(CloudSaveDataObject cloudSaveDTO)
	{
		CloudSaveJsonDTO cloudSaveJsonDTO = new CloudSaveJsonDTO();
		cloudSaveJsonDTO.coins = cloudSaveDTO.coins;
		cloudSaveJsonDTO.birdsKilled = cloudSaveDTO.birdsKilled;
		cloudSaveJsonDTO.highScoreBirdsKilled = cloudSaveDTO.highScoreBirdsKilled;
		cloudSaveJsonDTO.fruitsSmashed = cloudSaveDTO.fruitsSmashed;
		cloudSaveJsonDTO.highscoreFruitsSmashed = cloudSaveDTO.highscoreFruitsSmashed;
		cloudSaveJsonDTO.totalDamageApplied = cloudSaveDTO.totalDamageApplied;
		cloudSaveJsonDTO.winsCount = cloudSaveDTO.winsCount;
		cloudSaveJsonDTO.losesCount = cloudSaveDTO.losesCount;
		cloudSaveJsonDTO.matchesCountVsHuman = cloudSaveDTO.matchesCountVsHuman;
		cloudSaveJsonDTO.matchesCount = cloudSaveDTO.matchesCount;
		cloudSaveJsonDTO.totalGameTime = cloudSaveDTO.totalGameTime.ToString();
		cloudSaveJsonDTO.openedChestsCount = cloudSaveDTO.openedChestsCount;
		cloudSaveJsonDTO.freeChestsOpened = cloudSaveDTO.freeChestsOpened;
		cloudSaveJsonDTO.unlockedGameTypes = cloudSaveDTO.unlockedGameTypes;
		cloudSaveJsonDTO.lastCharacterKey = cloudSaveDTO.lastCharacterKey;
		cloudSaveJsonDTO.doubleCoinsKey = cloudSaveDTO.doubleCoinsKey;
		cloudSaveJsonDTO.videosWatched = cloudSaveDTO.videosWatched;
		cloudSaveJsonDTO.tournamentEasyWin = cloudSaveDTO.tournamentEasyWin;
		cloudSaveJsonDTO.tournamentMediumWin = cloudSaveDTO.tournamentMediumWin;
		cloudSaveJsonDTO.tournamentHardWin = cloudSaveDTO.tournamentHardWin;
		cloudSaveJsonDTO.charactersBoughtOnBucks = cloudSaveDTO.charactersBoughtOnBucks;
		cloudSaveJsonDTO.giftsOpened = cloudSaveDTO.giftsOpened;
		cloudSaveJsonDTO.spentBucks = cloudSaveDTO.spentBucks;
		cloudSaveJsonDTO.defeatedCharacters = cloudSaveDTO.defeatedCharacters;
		return cloudSaveJsonDTO;
	}
}
