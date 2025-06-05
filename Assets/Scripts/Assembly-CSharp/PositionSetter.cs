using UnityEngine;

[AddComponentMenu("Inventain/Size/PositionSetter")]
public class PositionSetter : SizeSetter
{
	public SizeFactor positionFactorType = SizeFactor.MinFactor;

	protected override void UpdateSize()
	{
		SizeHelper.RecalculatePosition(base.transform, positionFactorType, roundFloatPreference);
	}
}
