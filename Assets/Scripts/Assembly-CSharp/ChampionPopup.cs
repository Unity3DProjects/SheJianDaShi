using System;

public class ChampionPopup : UpgradePopup
{
	private void Start()
	{
		SingletonMonoBehaviour<ProposalManager>.Instance.LastChampionPack = DateTime.Now;
	}

	protected override void InitializeShopItemVariable()
	{
		shopItem = SingletonMonoBehaviour<ProposalManager>.Instance.ChampionNew;
	}
}
