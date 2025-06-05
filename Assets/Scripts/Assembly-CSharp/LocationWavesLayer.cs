using UnityEngine;

public class LocationWavesLayer : LocationLayer
{
	[SerializeField]
	private tk2dTiledSprite wavesFrontPart;

	[SerializeField]
	private tk2dTiledSprite wavesTiled;

	[SerializeField]
	private Vector3 wavesSpeed;

	private Vector3 startPosition = Vector3.zero;

	private Transform wavesFrontTransform;

	private Transform wavesTiledTransform;

	private float wavesFrontSpriteXSize;

	private Vector3 wavesSizeOffset;

	public override void Init(float locationWidth, float locationHeight)
	{
		base.Init(locationWidth, locationHeight);
		float num = ((!tk2dSystem.IsRetina) ? 1f : 0.5f);
		wavesFrontPart.transform.localPosition = (startPosition = new Vector3((0f - locationWidth) * 0.5f, wavesFrontPart.transform.localPosition.y, wavesFrontPart.transform.localPosition.z));
		wavesFrontPart.scale *= num;
		wavesFrontPart.dimensions = new Vector3(locationWidth / num, wavesFrontPart.dimensions.y / num);
		wavesTiled.scale = new Vector3(locationWidth, wavesTiled.scale.y, 1f);
		wavesTiled.transform.localPosition = new Vector3((0f - locationWidth) * 0.5f, wavesTiled.transform.localPosition.y, wavesTiled.transform.localPosition.z);
		wavesFrontTransform = wavesFrontPart.CachedTransform;
		wavesTiledTransform = wavesTiled.CachedTransform;
		wavesFrontSpriteXSize = wavesFrontPart.CurrentSprite.GetBounds().size.x;
		wavesSizeOffset = Vector3.left * Mathf.Sign(wavesSpeed.x) * wavesFrontSpriteXSize;
	}

	public override void UpdateLayer(float deltaTime)
	{
		base.UpdateLayer(deltaTime);
		Vector3 vector = Vector3.right * wavesSpeed.x * deltaTime;
		wavesFrontTransform.localPosition += vector;
		wavesTiledTransform.localPosition += vector;
		if (Mathf.Abs(wavesFrontTransform.localPosition.x - startPosition.x) >= wavesFrontSpriteXSize)
		{
			wavesFrontTransform.localPosition += wavesSizeOffset;
			wavesTiledTransform.localPosition += wavesSizeOffset;
			startPosition = wavesFrontTransform.localPosition;
		}
	}
}
