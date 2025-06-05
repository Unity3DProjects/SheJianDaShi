using System.Collections.Generic;
using UnityEngine;

public class LocationsManager : SingletonMonoBehaviour<LocationsManager>
{
	private readonly List<LocationType> locationTypes = new List<LocationType>
	{
		LocationType.Desert,
		LocationType.Brooklyn,
		LocationType.Cemetry,
		LocationType.City,
		LocationType.Space
	};

	[SerializeField]
	private Location[] locations;

	[SerializeField]
	private int minFightsCount = 1;

	[SerializeField]
	private int maxFightsCount = 2;

	[SerializeField]
	private List<ForcedLocation> forcedLocations;

	[SerializeField]
	private int matchesWithoutForcedLocationsForNewUsers;

	private int currentFightsCount;

	private LocationType currentLocation;

	private LocationType lastLocation;

	private ForcedLocation currentForcedLocation;

	private List<LocationType> possibleLocations;

	private List<LocationType> forcedLocationsTypes;

	private List<ForcedLocation> currentForcedLocations = new List<ForcedLocation>();

	public LocationType CurrentLocation
	{
		get
		{
			if (currentLocation == LocationType.None)
			{
				if (!IsForcedLocationsShowed)
				{
					int index = 0;
					if (currentForcedLocations.Count > 1)
					{
						for (int i = 1; i < currentForcedLocations.Count; i++)
						{
							if (currentForcedLocations[i].forcedShowTimes > currentForcedLocations[i - 1].forcedShowTimes && (currentForcedLocations[i].forcedLocationType != lastLocation || currentForcedLocations.Count == 1))
							{
								index = i;
							}
							else if (currentForcedLocations[i].forcedShowTimes == currentForcedLocations[i - 1].forcedShowTimes)
							{
								index = ((Random.value > 0.5f) ? ((currentForcedLocations[i].forcedLocationType == lastLocation) ? (i - 1) : i) : ((currentForcedLocations[i - 1].forcedLocationType == lastLocation) ? i : (i - 1)));
							}
						}
					}
					currentForcedLocation.forcedLocationType = currentForcedLocations[index].forcedLocationType;
					currentForcedLocation.forcedShowTimes = currentForcedLocations[index].forcedShowTimes - 1;
					currentLocation = currentForcedLocation.forcedLocationType;
					currentForcedLocations.Remove(currentForcedLocations[index]);
					if (currentForcedLocation.forcedShowTimes > 0)
					{
						currentForcedLocations.Add(currentForcedLocation);
					}
					if (currentForcedLocations.Count == 0)
					{
						CustomPlayerPrefs.SetString("forced_locations_key", Application.version);
					}
				}
				else
				{
					if (possibleLocations == null)
					{
						if (SingletonMonoBehaviour<StatisticsManager>.Instance.TotalSeesionCount == 1)
						{
							possibleLocations = new List<LocationType>(locationTypes);
						}
						else
						{
							possibleLocations = new List<LocationType>();
						}
					}
					if (SingletonMonoBehaviour<StatisticsManager>.Instance.TotalSeesionCount == 1)
					{
						currentLocation = possibleLocations.RandomObject();
						possibleLocations.Remove(currentLocation);
						if (possibleLocations.Count == 0)
						{
							possibleLocations.AddRange(locationTypes);
						}
					}
					else if (currentFightsCount >= Random.Range(minFightsCount, maxFightsCount + 1) || currentLocation == LocationType.None)
					{
						List<LocationType> list = locationTypes.FindAll((LocationType location) => !possibleLocations.Contains(location));
						currentLocation = list.RandomObject();
						possibleLocations.Add(currentLocation);
						if (possibleLocations.Count > 2)
						{
							possibleLocations.RemoveAt(0);
						}
						currentFightsCount = 0;
					}
				}
			}
			lastLocation = currentLocation;
			return currentLocation;
		}
	}

	private List<LocationType> ForcedLocationsTypes
	{
		get
		{
			if (forcedLocationsTypes == null)
			{
				forcedLocationsTypes = new List<LocationType>();
				for (int i = 0; i < forcedLocations.Count; i++)
				{
					forcedLocationsTypes.Add(forcedLocations[i].forcedLocationType);
				}
			}
			return forcedLocationsTypes;
		}
	}

	private bool IsForcedLocationsShowed
	{
		get
		{
			return CustomPlayerPrefs.GetString("forced_locations_key") == Application.version || currentForcedLocations.Count == 0;
		}
	}

	private bool IsNotForcedLocationsForNewUsersShowed
	{
		get
		{
			return matchesWithoutForcedLocationsForNewUsers <= SingletonMonoBehaviour<StatisticsManager>.Instance.MathesCount;
		}
	}

	public void Init()
	{
		if (possibleLocations != null)
		{
			possibleLocations.Clear();
		}
		if (currentForcedLocations != null)
		{
			currentForcedLocations.Clear();
		}
		for (int i = 0; i < forcedLocations.Count; i++)
		{
			if (forcedLocations[i].forcedLocationType != LocationType.None && forcedLocations[i].forcedShowTimes != 0)
			{
				currentForcedLocations.Add(forcedLocations[i]);
			}
		}
		if (!IsNotForcedLocationsForNewUsersShowed)
		{
			CustomPlayerPrefs.SetString("forced_locations_key", Application.version);
		}
	}

	public Location GetLocation()
	{
		Location locationPrefab = GetLocationPrefab();
		currentLocation = LocationType.None;
		if (!IsForcedLocationsShowed)
		{
			currentFightsCount++;
		}
		return Object.Instantiate(locationPrefab);
	}

	public Location GetLocationPrefab()
	{
		LocationType locationType = LocationType.None;
		locationType = ((SingletonMonoBehaviour<GameManager>.Instance.CurrentGameType != GameType.Multiplayer) ? CurrentLocation : SingletonMonoBehaviour<MultiplayerManager>.Instance.MatchSettings.levelSettings.locationType);
		if (forcedLocations != null && !IsNotForcedLocationsForNewUsersShowed && forcedLocations.Count < locations.Length)
		{
			while (ForcedLocationsTypes.Contains(locationType))
			{
				currentLocation = LocationType.None;
				locationType = CurrentLocation;
			}
		}
		Location location = null;
		for (int i = 0; i < locations.Length; i++)
		{
			if (locations[i].Type == locationType)
			{
				location = locations[i];
				break;
			}
		}
		if (location == null && locations != null && locations.Length > 0)
		{
			location = locations[0];
		}
		return location;
	}
}
