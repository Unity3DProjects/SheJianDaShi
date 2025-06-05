using UnityEngine;

public class VipPackProposalMenu : TimerProposalMenu
{
	[SerializeField]
	private string iconNameArcher;

	protected override void OnEnable()
	{
		base.OnEnable();
		if (!string.IsNullOrEmpty(iconNameArcher) && iconProposal != null)
		{
			string sprite = iconNameArcher;
			iconProposal.SetSprite(sprite);
		}
	}
}
