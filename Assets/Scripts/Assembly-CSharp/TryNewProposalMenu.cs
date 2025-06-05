using UnityEngine;

public class TryNewProposalMenu : GUIProposalMenu
{
	[SerializeField]
	private tk2dSprite iconCharacter;

	public override void Activate()
	{
		base.Activate();
		ShopItemCharacter tryCharacter = SingletonMonoBehaviour<ProposalManager>.Instance.TryCharacter;
		if (tryCharacter != null)
		{
			GUICharacter gUICharacter = SingletonMonoBehaviour<LevelsManager>.Instance.ObjectsSpawner.GetGUICharacter(tryCharacter.Infos[0].characterNameType);
			if (gUICharacter != null)
			{
				iconCharacter.SetSprite(gUICharacter.IconShopName);
			}
		}
	}
}
