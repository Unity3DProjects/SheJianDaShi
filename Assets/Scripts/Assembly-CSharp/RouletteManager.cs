using System.Collections.Generic;
using Mit_Goliath_Native.Utils;
using UnityEngine;

public class RouletteManager : SingletonMonoBehaviour<RouletteManager>
{
	private const string NUMBER_KEYS = "Number_keys";

	[SerializeField]
	private TextAsset rouletteChanceWeight;

	[SerializeField]
	private TextAsset rouletteTournamentChanceWeight;

	[SerializeField]
	private TextAsset rouletteAllFreeCharactersBoughtChanceWeight;

	[SerializeField]
	private TextAsset rouletteUnfairUser;

	[SerializeField]
	private int smallCoinsCount = 500;

	[SerializeField]
	private int mediumCoinsCount = 750;

	[SerializeField]
	private int largeCoinsCount = 1000;

	[SerializeField]
	private int smallGemsCount = 1;

	[SerializeField]
	private int mediumGemsCount = 2;

	[SerializeField]
	private int largeGemsCount = 5;

	private List<RouletteRewardInfo> weightsInfo = new List<RouletteRewardInfo>();

	private RouletteRewardInfo currentRewardInfo;

	private List<ShopItemCharacter> ordinaryCharacters = new List<ShopItemCharacter>();

	private List<ShopItemCharacter> premiumCharacters = new List<ShopItemCharacter>();

	private Managers cachedManagers;

	public RouletteRewardInfo CurrentRewardInfo
	{
		get
		{
			return currentRewardInfo;
		}
	}

	public int KeysCount
	{
		get
		{
			return CustomPlayerPrefs.GetInt("Number_keys", 0);
		}
		set
		{
			CustomPlayerPrefs.SetInt("Number_keys", value);
		}
	}

	public bool IsAvalaiblePremiumCharacters
	{
		get
		{
			return premiumCharacters.Count > 0;
		}
	}

	public bool IsAvalaibleOrdinaryCharacters
	{
		get
		{
			return ordinaryCharacters.Count > 0;
		}
	}

	public int Coins1
	{
		get
		{
			return smallCoinsCount * ((!CachedManagers.CachedStatisticsManager.DoubleCoins) ? 1 : 2);
		}
	}

	public int Coins2
	{
		get
		{
			return mediumCoinsCount * ((!CachedManagers.CachedStatisticsManager.DoubleCoins) ? 1 : 2);
		}
	}

	public int Coins3
	{
		get
		{
			return largeCoinsCount * ((!CachedManagers.CachedStatisticsManager.DoubleCoins) ? 1 : 2);
		}
	}

	public int Gems1
	{
		get
		{
			return smallGemsCount;
		}
	}

	public int Gems2
	{
		get
		{
			return mediumGemsCount;
		}
	}

	public int Gems3
	{
		get
		{
			return largeGemsCount;
		}
	}

	public int LocksCount
	{
		get
		{
			return (!CachedManagers.CachedSubscriptionManager.IsSubscriptionActive) ? 1 : 3;
		}
	}

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

	protected override void Awake()
	{
		InitializeWeights();
		base.Awake();
	}

	public RouletteRewardInfo GenerateReward()
	{
		CheckCharacters();
		bool isTournamentCharacterReward = false;
		if ((float)CachedManagers.CachedStatisticsManager.MathesCount * 2.5f <= (float)CachedManagers.CachedStatisticsManager.FreeChestsOpened)
		{
			InitializeWeights(rouletteUnfairUser);
		}
		else if (SingletonMonoBehaviour<TournamentManager>.Instance.IsTournamentSuccessfullyEnded)
		{
			InitializeWeights(rouletteTournamentChanceWeight);
			isTournamentCharacterReward = SingletonMonoBehaviour<TournamentManager>.Instance.CurrentTournament.settings.difficultType == TournamentDifficultType.Hard;
		}
		else if (!IsAvalaibleOrdinaryCharacters)
		{
			InitializeWeights(rouletteAllFreeCharactersBoughtChanceWeight);
		}
		else
		{
			InitializeWeights();
		}
		CheckWeights(isTournamentCharacterReward);
		if (CachedManagers.CachedStatisticsManager.OpenedChestsCount == 0)
		{
			currentRewardInfo = weightsInfo[4];
			currentRewardInfo.archer = premiumCharacters.Find((ShopItemCharacter character) => character.name == "Magician");
			if (currentRewardInfo.archer == null)
			{
				currentRewardInfo.archer = premiumCharacters.RandomObject();
			}
			if (currentRewardInfo.archer == null)
			{
				currentRewardInfo.archer = ordinaryCharacters.RandomObject();
			}
			if (currentRewardInfo.archer == null)
			{
				currentRewardInfo = weightsInfo[2];
			}
		}
		else if (CachedManagers.CachedStatisticsManager.OpenedChestsCount == 1)
		{
			currentRewardInfo = weightsInfo[1];
		}
		else if (CachedManagers.CachedStatisticsManager.OpenedChestsCount == 2)
		{
			currentRewardInfo = weightsInfo[1];
		}
		else
		{
			currentRewardInfo = weightsInfo[0];
			int num = Random.Range(1, weightsInfo[weightsInfo.Count - 1].weight);
			for (int num2 = 0; num2 < weightsInfo.Count; num2++)
			{
				if (num <= weightsInfo[num2].weight)
				{
					currentRewardInfo = weightsInfo[num2];
					break;
				}
			}
			if (currentRewardInfo.type == RouletteRewardType.Ordinary && IsAvalaibleOrdinaryCharacters)
			{
				currentRewardInfo.archer = ordinaryCharacters.RandomObject();
			}
			else if (currentRewardInfo.type == RouletteRewardType.Premium && IsAvalaiblePremiumCharacters)
			{
				currentRewardInfo.archer = premiumCharacters.RandomObject();
			}
		}
		return currentRewardInfo;
	}

	public void OpenReward(RouletteRewardInfo info = null, int counter = 1)
	{
		if (info == null)
		{
			info = currentRewardInfo;
		}
		int num = 0;
		int num2 = 0;
		switch (info.type)
		{
		case RouletteRewardType.Coins1:
			num = smallCoinsCount * counter;
			CachedManagers.CachedStatisticsManager.AddCoins(num);
			break;
		case RouletteRewardType.Coins2:
			num = mediumCoinsCount * counter;
			CachedManagers.CachedStatisticsManager.AddCoins(num);
			break;
		case RouletteRewardType.Coins3:
			num = largeCoinsCount * counter;
			CachedManagers.CachedStatisticsManager.AddCoins(largeCoinsCount * counter);
			break;
		case RouletteRewardType.Gems1:
			num2 = smallGemsCount * counter;
			CachedManagers.CachedStatisticsManager.AddGems(smallGemsCount * counter);
			break;
		case RouletteRewardType.Gems2:
			num2 = mediumGemsCount * counter;
			CachedManagers.CachedStatisticsManager.AddGems(mediumGemsCount * counter);
			break;
		case RouletteRewardType.Gems3:
			num2 = largeGemsCount * counter;
			CachedManagers.CachedStatisticsManager.AddGems(largeGemsCount * counter);
			break;
		case RouletteRewardType.Ordinary:
		case RouletteRewardType.Premium:
			if ((bool)info.archer)
			{
				info.archer.OpenCharacter(true, true);
				GameGoliath.Instance.TrackCharacterAction(info.archer.GetCharacterID(), GoliathIds.Action.ACQUISITION, GoliathIds.Origin.CHEST_ROULETTE, string.Empty);
				SingletonMonoBehaviour<LevelsManager>.Instance.ObjectsSpawner.SetFirstSelectedCharacter(info.archer.Infos[0].characterNameType);
				SingletonMonoBehaviour<TutorialManager>.Instance.MatchesForCoinsReminder = 0;
			}
			break;
		}
		if (num > 0)
		{
			GameGoliath.Instance.TrackCurrencyFlow(GoliathIds.CurrencyMovement.EARNED, GoliathIds.Context.CHEST_ROULETTE, GoliathIds.CurrencyName.COINS, num, GoliathIds.Placement.END_GAME, SingletonMonoBehaviour<GameManager>.Instance.CachedManagers.CachedStatisticsManager.Coins - num);
		}
		if (num2 > 0)
		{
			GameGoliath.Instance.TrackCurrencyFlow(GoliathIds.CurrencyMovement.EARNED, GoliathIds.Context.CHEST_ROULETTE, GoliathIds.CurrencyName.GEMS, num, GoliathIds.Placement.END_GAME, SingletonMonoBehaviour<GameManager>.Instance.CachedManagers.CachedStatisticsManager.Gems - num2);
		}
		info = null;
		KeysCount = 0;
	}

	public RouletteRewardInfo GetRandomPremiumCharacter()
	{
		RouletteRewardInfo rouletteRewardInfo = null;
		if (IsAvalaiblePremiumCharacters)
		{
			rouletteRewardInfo = new RouletteRewardInfo(0, RouletteRewardType.Premium);
			rouletteRewardInfo.archer = premiumCharacters.RandomObject();
		}
		return rouletteRewardInfo;
	}

	public RouletteRewardInfo GetRandomOrdinaryCharacter()
	{
		RouletteRewardInfo rouletteRewardInfo = null;
		if (IsAvalaibleOrdinaryCharacters)
		{
			rouletteRewardInfo = new RouletteRewardInfo(0, RouletteRewardType.Ordinary);
			rouletteRewardInfo.archer = ordinaryCharacters.RandomObject();
		}
		return rouletteRewardInfo;
	}

	private void InitializeWeights(TextAsset forcedChanceWeight = null)
	{
		weightsInfo.Clear();
		int num = 0;
		TextAsset textAsset = ((!(forcedChanceWeight == null)) ? forcedChanceWeight : rouletteChanceWeight);
		string[] array = CSVReader.SplitCsvGridByRows(textAsset.text);
		for (int i = 0; i < array.Length; i++)
		{
			num += int.Parse(array[i]);
			weightsInfo.Add(new RouletteRewardInfo(num, (RouletteRewardType)i));
		}
		if (weightsInfo.Count <= 0)
		{
			weightsInfo.Add(new RouletteRewardInfo(1, RouletteRewardType.Coins1));
		}
		currentRewardInfo = weightsInfo[0];
	}

	private void CheckWeights(bool isTournamentCharacterReward)
	{
		if (!IsAvalaiblePremiumCharacters)
		{
			int weight = 0;
			int num = 0;
			int index = 0;
			int num2 = 0;
			int index2 = 0;
			for (int i = 0; i < weightsInfo.Count; i++)
			{
				if (weightsInfo[i].type == RouletteRewardType.Premium)
				{
					weight = weightsInfo[i].weight;
				}
				if (weightsInfo[i].type == RouletteRewardType.Ordinary)
				{
					num = weightsInfo[i].weight;
					index = i;
				}
				if (weightsInfo[i].type == RouletteRewardType.Coins3)
				{
					num2 = weightsInfo[i].weight;
					index2 = 3;
				}
			}
			weightsInfo[index].weight = weight;
			if (num == num2)
			{
				weightsInfo[index2].weight = weight;
			}
		}
		if (IsAvalaibleOrdinaryCharacters)
		{
			return;
		}
		int num3 = 0;
		int num4 = 0;
		int index3 = 0;
		int index4 = 0;
		for (int j = 0; j < weightsInfo.Count; j++)
		{
			if (weightsInfo[j].type == RouletteRewardType.Ordinary)
			{
				index4 = j;
				num3 = weightsInfo[j].weight;
			}
			if (weightsInfo[j].type == RouletteRewardType.Coins3)
			{
				index3 = j;
			}
			if (weightsInfo[j].type == RouletteRewardType.Premium)
			{
				num4 = weightsInfo[j].weight;
			}
		}
		if (!IsAvalaiblePremiumCharacters)
		{
			weightsInfo[index3].weight = num4;
			return;
		}
		weightsInfo[index3].weight = ((!isTournamentCharacterReward) ? num3 : (num4 - num3));
		weightsInfo[index4].weight = weightsInfo[index3].weight;
	}

	private void CheckCharacters()
	{
		List<ShopItemCharacter> availableCharacters = SingletonMonoBehaviour<ShopManager>.Instance.AvailableCharacters;
		ordinaryCharacters.Clear();
		premiumCharacters.Clear();
		for (int i = 0; i < availableCharacters.Count; i++)
		{
			ShopItemCharacter shopItemCharacter = availableCharacters[i];
			if (shopItemCharacter.Availible || !shopItemCharacter.CanBuyInShop)
			{
				continue;
			}
			if (shopItemCharacter.IsPremium)
			{
				if (!shopItemCharacter.IsWithoutRoulette)
				{
					premiumCharacters.Add(shopItemCharacter);
				}
			}
			else
			{
				ordinaryCharacters.Add(shopItemCharacter);
			}
		}
	}
}
