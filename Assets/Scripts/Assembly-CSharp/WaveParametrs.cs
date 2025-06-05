using System;

[Serializable]
public struct WaveParametrs
{
	public int birdsCountMin;

	public int birdsCountMax;

	public float birdsDelayMin;

	public float birdsDelayMax;

	public float timeSpawnPowerUpMin;

	public float timeSpawnPowerUpMax;

	public static WaveParametrs WaveDefault
	{
		get
		{
			return new WaveParametrs
			{
				birdsCountMin = 1,
				birdsCountMax = 2,
				birdsDelayMin = 0.1f,
				birdsDelayMax = 0.2f,
				timeSpawnPowerUpMin = 1f,
				timeSpawnPowerUpMax = 2f
			};
		}
	}
}
