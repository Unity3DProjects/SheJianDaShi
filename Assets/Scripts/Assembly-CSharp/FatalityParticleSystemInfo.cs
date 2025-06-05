using System;

[Serializable]
public struct FatalityParticleSystemInfo
{
	public CharacterType characterType;

	[ResourceLink]
	public AssetLink effectPrefab;
}
