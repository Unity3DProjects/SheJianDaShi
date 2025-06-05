public class RouletteRewardInfo
{
	private const string WIN_REWARD_COINS = "ui.roulette.labelwin.postfix_coins";

	private const string WIN_REWARD_GEMS = "ui.roulette.labelwin.postfix_gems";

	private const string WIN_REWARD_ARCHER = "ui.roulette.labelwin.postfix_archer";

	public int weight;

	public RouletteRewardType type;

	public ShopItemCharacter archer;

	public RouletteRewardInfo(int weightReward, RouletteRewardType typeReward)
	{
		weight = weightReward;
		type = typeReward;
	}

	public string WinDescription(int counter = 1)
	{
		string result = null;
		LocalisationManager instance = SingletonMonoBehaviour<LocalisationManager>.Instance;
		RouletteManager instance2 = SingletonMonoBehaviour<RouletteManager>.Instance;
		switch (type)
		{
		case RouletteRewardType.Coins1:
			result = instance2.Coins1 * counter + instance.GetTextByKey("ui.roulette.labelwin.postfix_coins");
			break;
		case RouletteRewardType.Coins2:
			result = instance2.Coins2 * counter + instance.GetTextByKey("ui.roulette.labelwin.postfix_coins");
			break;
		case RouletteRewardType.Coins3:
			result = instance2.Coins3 * counter + instance.GetTextByKey("ui.roulette.labelwin.postfix_coins");
			break;
		case RouletteRewardType.Ordinary:
			if ((bool)archer)
			{
				result = SingletonMonoBehaviour<LevelsManager>.Instance.ObjectsSpawner.GetCharacterLocalizedName(archer.Infos[0].characterNameType) + instance.GetTextByKey("ui.roulette.labelwin.postfix_archer");
			}
			break;
		case RouletteRewardType.Premium:
			if ((bool)archer)
			{
				result = SingletonMonoBehaviour<LevelsManager>.Instance.ObjectsSpawner.GetCharacterLocalizedName(archer.Infos[0].characterNameType) + instance.GetTextByKey("ui.roulette.labelwin.postfix_archer");
			}
			break;
		case RouletteRewardType.Gems1:
			result = instance2.Gems1 * counter + instance.GetTextByKey("ui.roulette.labelwin.postfix_gems");
			break;
		case RouletteRewardType.Gems2:
			result = instance2.Gems2 * counter + instance.GetTextByKey("ui.roulette.labelwin.postfix_gems");
			break;
		case RouletteRewardType.Gems3:
			result = instance2.Gems3 * counter + instance.GetTextByKey("ui.roulette.labelwin.postfix_gems");
			break;
		}
		return result;
	}
}
