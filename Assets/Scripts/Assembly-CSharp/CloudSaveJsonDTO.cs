using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class CloudSaveJsonDTO
{
	public struct CharacterUpgradesData
	{
		public string productID;

		public int upgradeLevel;
	}

	public int coins;

	public int gems;

	public int freeGemsReceived;

	public int birdsKilled;

	public int highScoreBirdsKilled;

	public int fruitsSmashed;

	public int highscoreFruitsSmashed;

	public float totalDamageApplied;

	public int winsCount;

	public int losesCount;

	public int matchesCountVsHuman;

	public int matchesCount;

	public string totalGameTime;

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

	public string[] unlockedCharacters;

	public CharacterUpgradesData[] unlockedUpgrades;

	public bool isVSCompTutorialFinished;

	public bool isVSHumanTutorialFinished;

	public bool isAppleGameTutorialFinished;

	public int[] unlockedActivations;

	public long lastDailyGift;

	public string lastSubscriptionRewardDate;

	public CloudSaveJsonDTO()
	{
		Debug.Log("CloudSave::CloudSaveJsonDTO");
		StatisticsManager cachedStatisticsManager = SingletonMonoBehaviour<GameManager>.Instance.CachedManagers.CachedStatisticsManager;
		TutorialManager instance = SingletonMonoBehaviour<TutorialManager>.Instance;
		coins = cachedStatisticsManager.Coins;
		gems = cachedStatisticsManager.Gems;
		freeGemsReceived = cachedStatisticsManager.FreeGemsReceived;
		birdsKilled = cachedStatisticsManager.BirdsKilled;
		highScoreBirdsKilled = cachedStatisticsManager.HighScoreBirdsKilled;
		fruitsSmashed = cachedStatisticsManager.FruitsSmashed;
		highscoreFruitsSmashed = cachedStatisticsManager.HighScoreFruitsSmashed;
		totalDamageApplied = cachedStatisticsManager.TotalDamageApplied;
		winsCount = cachedStatisticsManager.WinsCount;
		losesCount = cachedStatisticsManager.LosesCount;
		matchesCountVsHuman = cachedStatisticsManager.MatchesCountVSHuman;
		matchesCount = cachedStatisticsManager.MathesCount;
		totalGameTime = cachedStatisticsManager.TotalGameTime.ToString();
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
		unlockedCharacters = SingletonMonoBehaviour<ShopManager>.Instance.UnlockedCharacters.ConvertAll((ShopItemCharacter input) => input.ProductID).ToArray();
		unlockedUpgrades = SingletonMonoBehaviour<ShopManager>.Instance.UnlockedCharacters.ConvertAll((ShopItemCharacter input) => GetCharacterUpgradesData(input)).ToArray();
		isVSCompTutorialFinished = instance.IsVSCompTutorialFinished;
		isVSHumanTutorialFinished = instance.IsVSHumanTutorialFinished;
		isAppleGameTutorialFinished = instance.IsAppleGameTutorialFinished;
		unlockedActivations = instance.UnlockedActivations.ConvertAll((TutorialType tutorialType) => (int)tutorialType).ToArray();
		lastDailyGift = SingletonMonoBehaviour<GiftManager>.Instance.DateLastGift.ToBinary();
		lastSubscriptionRewardDate = SingletonMonoBehaviour<SubscriptionManager>.Instance.LastSubscriptionRewardDate.ToString();
	}

	public CharacterUpgradesData GetCharacterUpgradesData(ShopItemCharacter item)
	{
		CharacterUpgradesData result = default(CharacterUpgradesData);
		result.productID = item.ProductID;
		result.upgradeLevel = item.UpgradeLevel;
		return result;
	}

	public int GetSavedUpgradeLevel(ShopItemCharacter item)
	{
		CharacterUpgradesData[] array = unlockedUpgrades;
		for (int i = 0; i < array.Length; i++)
		{
			CharacterUpgradesData characterUpgradesData = array[i];
			if (characterUpgradesData.productID == item.ProductID)
			{
				return characterUpgradesData.upgradeLevel;
			}
		}
		return 0;
	}

	public static bool TryFromBytes(out CloudSaveJsonDTO cloudSaveDTO, byte[] bytes)
	{
		cloudSaveDTO = FromBytes(bytes);
		return cloudSaveDTO != null;
	}

	public static CloudSaveJsonDTO FromBytes(byte[] bytes)
	{
		CloudSaveJsonDTO result = null;
		try
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			memoryStream.Write(bytes, 0, bytes.Length);
			memoryStream.Seek(0L, SeekOrigin.Begin);
			string serialize = binaryFormatter.Deserialize(memoryStream) as string;
			result = JsonConvert.DeserializeObject<CloudSaveJsonDTO>(serialize);
		}
		catch (Exception)
		{
		}
		return result;
	}

	public byte[] ToBytes()
	{
		try
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			MemoryStream memoryStream = new MemoryStream();
			binaryFormatter.Serialize(memoryStream, ToString());
			return memoryStream.ToArray();
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
		return null;
	}

	public new string ToString()
	{
		return JsonConvert.SerializeObject(this).ToString();
	}
}
