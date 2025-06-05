using System;

[Serializable]
public struct BrutalityEffectInfo
{
	public BrutalityEffectType type;

	[ResourceLink]
	public AssetLink effectPrefab;
}
