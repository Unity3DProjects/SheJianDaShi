public class PackProposalMenu : TimerProposalMenu
{
	public override void Activate()
	{
		base.Activate();
		if (SingletonMonoBehaviour<ProposalManager>.Instance.BuyPack != null)
		{
			iconProposal.spriteId = SingletonMonoBehaviour<ProposalManager>.Instance.BuyPack.IconSpriteID;
		}
	}
}
