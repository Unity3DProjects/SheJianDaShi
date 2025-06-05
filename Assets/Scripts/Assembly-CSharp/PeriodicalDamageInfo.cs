using System;
using UnityEngine;

[Serializable]
public struct PeriodicalDamageInfo
{
	[HideInInspector]
	public Vector3 position;

	[HideInInspector]
	public WeaponType weaponType;

	public float frequency;

	public float radius;

	public float damage;

	public float duration;

	public bool isSpreadEnabled;

	public PeriodicalDamageType type;
}
