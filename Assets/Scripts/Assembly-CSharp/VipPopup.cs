public class VipPopup : UpgradePopup
{
	protected override void InitializeShopItemVariable()
	{
		shopItem = SingletonMonoBehaviour<ProposalManager>.Instance.VipPack;
	}
}
