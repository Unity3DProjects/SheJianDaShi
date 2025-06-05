using UnityEngine;

public class EmittedSystemInfo : MonoBehaviour
{
	[SerializeField]
	private WeaponActivationType activationType;

	[SerializeField]
	private bool isEmitAtLocaleSpace;

	[SerializeField]
	private EmitType emitType;

	public WeaponActivationType ActivationType
	{
		get
		{
			return activationType;
		}
	}

	public bool IsEmitAtLocaleSpace
	{
		get
		{
			return isEmitAtLocaleSpace;
		}
	}

	public EmitType EmitType
	{
		get
		{
			return emitType;
		}
	}
}
