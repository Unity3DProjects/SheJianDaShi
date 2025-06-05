using System;
using UnityEngine;

[Serializable]
public struct AnimationSettings
{
	[SpineAnimation("", "subscribeCatAnimationPortrait")]
	[SerializeField]
	public string animationName;

	public int repeatTimes;
}
