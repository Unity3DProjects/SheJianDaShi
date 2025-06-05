using UnityEngine;

public class MultiplayerNames : MonoBehaviour
{
	[SerializeField]
	private tk2dSprite firstPlayerFlagSprite;

	[SerializeField]
	private tk2dSprite secondPlayerFlagSprite;

	[SerializeField]
	private tk2dTextMesh firstPlayerName;

	[SerializeField]
	private tk2dTextMesh secondPlayerName;

	[SerializeField]
	private GUILayoutCell iPhoneXPortraitCell;

	[SerializeField]
	private GUILayoutCell[] iPhoneXCells;

	public GUILayoutCell IPhoneXPortraitCell
	{
		get
		{
			return iPhoneXPortraitCell;
		}
	}

	public GUILayoutCell[] IPhoneXCells
	{
		get
		{
			return iPhoneXCells;
		}
	}

	private void Start()
	{
		string nickname = SingletonMonoBehaviour<MultiplayerManager>.Instance.LeftPlayerSettings.nickname;
		string nickname2 = SingletonMonoBehaviour<MultiplayerManager>.Instance.RightPlayerSettings.nickname;
		if (!string.IsNullOrEmpty(nickname))
		{
			firstPlayerName.text = nickname;
		}
		if (!string.IsNullOrEmpty(nickname2))
		{
			secondPlayerName.text = nickname2;
		}
		if (firstPlayerFlagSprite.Collection.inst.GetSpriteDefinition(SingletonMonoBehaviour<MultiplayerManager>.Instance.LeftPlayerSettings.countryCode) != null)
		{
			firstPlayerFlagSprite.SetSprite(SingletonMonoBehaviour<MultiplayerManager>.Instance.LeftPlayerSettings.countryCode);
		}
		else
		{
			firstPlayerFlagSprite.SetSprite("XX");
		}
		if (secondPlayerFlagSprite.Collection.inst.GetSpriteDefinition(SingletonMonoBehaviour<MultiplayerManager>.Instance.RightPlayerSettings.countryCode) != null)
		{
			secondPlayerFlagSprite.SetSprite(SingletonMonoBehaviour<MultiplayerManager>.Instance.RightPlayerSettings.countryCode);
		}
		else
		{
			secondPlayerFlagSprite.SetSprite("XX");
		}
	}
}
