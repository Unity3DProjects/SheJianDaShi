using System.Collections.Generic;
using UnityEngine;

public class EmissionTrashCan : Emission
{
	[SerializeField]
	private int minWeaponCount = 2;

	[SerializeField]
	private int maxWeaponCount = 4;

	[SerializeField]
	private int maxWeaponSpeed = 1500;

	[SerializeField]
	private float emittedWeaponsDamagePerent = 33f;

	private List<int> emittedWeaponsNumbers;

	public override void EmitObjects(Level level, Vector3 position, float angle, float velocity)
	{
		emittedWeaponsNumbers = new List<int>();
		List<EmissionSettings> emissionSettings = base.CreateEmissionObjectsSettings(position, angle, velocity);
		CreateEmittedObjects(level, emissionSettings);
		EmissionMultiplayerMessage(level, emissionSettings, emittedWeaponsNumbers);
	}

	public override void CreateEmittedObjects(Level level, List<EmissionSettings> emissionSettings)
	{
		if (level.GameType != GameType.Multiplayer || SingletonMonoBehaviour<MultiplayerManager>.Instance.IsMainPlayer)
		{
			CreateEmittedTrashCanObjects(level, emittedObjectsPrefabs, emissionSettings, null);
			return;
		}
		LevelMultiplayer levelMultiplayer = level as LevelMultiplayer;
		levelMultiplayer.LastEmittedObjectsPrefabs = base.EmittedObjectsPrefabs;
		levelMultiplayer.IncreaseTurnCountTemporally(2f);
	}

	public void CreateEmittedTrashCanObjects(Level level, LevelObject[] prefabs, List<EmissionSettings> emissionSettings, List<int> emittedObjectsNumbers)
	{
		if (prefabs.Length != emissionSettings.Count)
		{
			return;
		}
		int num = maxWeaponCount;
		int min = minWeaponCount;
		int num2 = Random.Range(min, num + 1);
		bool flag = true;
		if (emittedObjectsNumbers != null)
		{
			num2 = emittedObjectsNumbers.Count;
			flag = false;
		}
		for (int i = 0; i < num2; i++)
		{
			int num3 = Random.Range(0, prefabs.Length);
			if (!flag)
			{
				num3 = emittedObjectsNumbers[i];
			}
			else
			{
				emittedWeaponsNumbers.Add(num3);
			}
			if (prefabs[num3] is WeaponGeneral)
			{
				WeaponGeneral weaponGeneral = Object.Instantiate(prefabs[num3]) as WeaponGeneral;
				weaponGeneral.ForceMaxSpeed(maxWeaponSpeed);
				weaponGeneral.DamagePercent = emittedWeaponsDamagePerent / 100f;
				level.AddEmittedWeapon(weaponGeneral);
				ShootingInfo shootingInfo = new ShootingInfo(emissionSettings[num3].emissionAngle, emissionSettings[num3].emissionVelocity);
				weaponGeneral.SetShootingInfo(emissionSettings[num3].emissionPosition, shootingInfo, true);
				weaponGeneral.Shoot();
			}
			else if (prefabs[num3] is EmittedObject)
			{
				EmittedObject emittedObject = Object.Instantiate(prefabs[num3]) as EmittedObject;
				level.AddEmittedObject(emittedObject);
				emittedObject.transform.position = emissionSettings[num3].emissionPosition;
				emittedObject.Init(level, Quaternion.AngleAxis(emissionSettings[num3].emissionAngle, Vector3.forward) * new Vector3(emissionSettings[num3].emissionVelocity, 0f));
			}
		}
	}
}
