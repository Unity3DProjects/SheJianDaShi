using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RagdollParams
{
	public List<Vector3> ragdollPositions;

	public List<Vector3> ragdollRotations;

	public bool isLeftCharacter;
}
