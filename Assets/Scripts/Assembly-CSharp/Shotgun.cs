using UnityEngine;

public class Shotgun : WeaponGeneral
{
	private const string ACTIVATION_SOUND = "shotgun_activation";

	[SerializeField]
	private int maxTapCount;

	[SerializeField]
	private float bulletSpeed;

	[SerializeField]
	[ResourceLink]
	private AssetLink shootEffectPrefab;

	private int currentTapCount;

	protected override void EmitObjects()
	{
		Emission component = base.gameObject.GetComponent<Emission>();
		if ((bool)component)
		{
			if (base.Level.ActiveCharacter == base.Level.CharacterSecond)
			{
				component.EmitObjects(level, base.CachedTransform.position, 180f - base.transform.rotation.eulerAngles.z, bulletSpeed);
			}
			else
			{
				component.EmitObjects(level, base.CachedTransform.position, base.transform.rotation.eulerAngles.z, bulletSpeed);
			}
		}
	}

	private void OnTap()
	{
		if (currentTapCount < maxTapCount)
		{
			EmitObjects();
			currentTapCount++;
			if (shootEffectPrefab != null && !string.IsNullOrEmpty(shootEffectPrefab.assetGUID))
			{
				Object.Instantiate(shootEffectPrefab.GetAsset(), base.CachedTransform.position, base.CachedTransform.rotation, base.CachedTransform);
			}
			AudioManager.PlayAudioWithKey("shotgun_activation");
		}
	}
}
