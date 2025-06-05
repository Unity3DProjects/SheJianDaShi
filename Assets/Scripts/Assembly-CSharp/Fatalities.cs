using System.Collections.Generic;
using UnityEngine;

public class Fatalities : MonoBehaviour
{
	[SerializeField]
	private List<FatalityBase> fatalityPrefabs;

	public List<FatalityBase> FatalityPrefabs
	{
		get
		{
			return fatalityPrefabs;
		}
	}
}
