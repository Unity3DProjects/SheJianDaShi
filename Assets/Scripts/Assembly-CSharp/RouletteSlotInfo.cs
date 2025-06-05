using System;

[Serializable]
public struct RouletteSlotInfo
{
	public RouletteRewardType rewardType;

	public SkeletonUtilityBone parentBoneLandscape;

	public SkeletonUtilityBone parentBonePortrait;

	public bool isWinnerSlot;

	public bool isWinnerNeighbourSlot;
}
