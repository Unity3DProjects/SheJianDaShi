using UnityEngine;

public class GameScreenWeapons : MonoBehaviour
{
	[SerializeField]
	private GUIControl controlWeapon;

	[SerializeField]
	private tk2dBaseSprite iconWeapon;

	[SerializeField]
	private GUILayoutCell iPhoneXCell;

	public GUIControl ControlWeapon
	{
		get
		{
			return controlWeapon;
		}
	}

	public tk2dBaseSprite IconWeapon
	{
		get
		{
			return iconWeapon;
		}
	}

	public GUILayoutCell IPhoneXCell
	{
		get
		{
			return iPhoneXCell;
		}
	}
}
