using System;

[Serializable]
public struct BoneParticleSystem
{
	public string boneName;

	[ResourceLink]
	public AssetLink particleEffectPrefab;
}
