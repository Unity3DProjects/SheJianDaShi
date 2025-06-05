using UnityEngine;

public class LocationGroundLayer : LocationLayer
{
	[SerializeField]
	private tk2dTiledSprite ground;

	public override void Init(float locationWidth, float locationHeight)
	{
		base.Init(locationWidth, locationHeight);
		ground.scale = new Vector3(locationWidth, ground.scale.y, 1f);
		ground.transform.localPosition = new Vector3((0f - locationWidth) * 0.5f, ground.transform.localPosition.y, ground.transform.localPosition.z);
	}
}
