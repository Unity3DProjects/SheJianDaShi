using UnityEngine;

public class TwerkingWoman : MonoBehaviour
{
	private const string TWERK_ANIMATION = "twerk";

	[SerializeField]
	private SkeletonAnimation skeletonAnimation;

	public SkeletonDataAsset SkeletonDataAsset
	{
		get
		{
			return skeletonAnimation.SkeletonDataAsset;
		}
	}

	public void ApplyTwerking()
	{
		skeletonAnimation.State.SetAnimation(0, "twerk", true);
		skeletonAnimation.Update();
	}
}
