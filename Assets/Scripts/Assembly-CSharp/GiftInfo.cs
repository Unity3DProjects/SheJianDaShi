using System;

[Serializable]
public struct GiftInfo
{
	public GiftType type;

	public int coinsValue;

	public int coinsType;

	public string spriteName;

	public CharacterNameType giftCharacter;

	public int CoinsValue
	{
		get
		{
			return coinsValue * ((!SingletonMonoBehaviour<StatisticsManager>.Instance.DoubleCoins) ? 1 : 2);
		}
	}
}
