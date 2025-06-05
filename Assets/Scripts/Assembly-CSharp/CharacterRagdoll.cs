using System.Collections.Generic;
using UnityEngine;

public class CharacterRagdoll : MonoBehaviour
{
	[SpineBone("", "")]
	[SerializeField]
	private string startBone = "body";

	[SpineBone("", "")]
	[SerializeField]
	private List<string> necessaryBones = new List<string>
	{
		"body", "arm_l", "arm_r", "forearm_l", "forearm_r", "hand_l", "hand_r", "head", "thigh_l", "thigh_r",
		"shin_l", "shin_r"
	};

	[SerializeField]
	private float rotationLimit = 30f;

	[SerializeField]
	private float rootMass = 1600f;
}
