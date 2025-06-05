using UnityEngine;

public class MusicColumn : MonoBehaviour
{
	private const string TWERK_ANIMATION = "twerk";

	[SerializeField]
	private SkeletonAnimation skeletonAnimation;

	public SkeletonAnimation SkeletonAnimation
	{
		get
		{
			return skeletonAnimation;
		}
	}

	private void Start()
	{
		skeletonAnimation.skeleton.SetToSetupPose();
	}

	public void ApplyMusic()
	{
		skeletonAnimation.state.SetAnimation(0, "twerk", true);
	}
}
