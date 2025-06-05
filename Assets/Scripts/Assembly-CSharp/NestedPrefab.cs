using System;
using UnityEngine;

public class NestedPrefab : MonoBehaviour
{
	public string prefabGuid;

	private void Reset()
	{
		GenerateNewGUID();
	}

	public void GenerateNewGUID()
	{
		prefabGuid = Guid.NewGuid().ToString();
	}
}
