using UnityEngine;

public class PartyMastersAnnouncer : MonoBehaviour
{
	[SerializeField]
	private SkeletonAnimation animationLolVain;

	private void Start()
	{
		if (animationLolVain != null)
		{
			animationLolVain.gameObject.transform.localScale *= ((!tk2dSystem.IsRetina) ? 1f : 2f);
			animationLolVain.Skeleton.SetAttachment("snapback", "hat_promo");
			animationLolVain.Skeleton.SetAttachment("eyes", "glasses_promo");
			animationLolVain.Skeleton.SetAttachment("head", "head_promo");
			animationLolVain.Skeleton.SetAttachment("body", "body_promo");
			animationLolVain.Skeleton.SetAttachment("arm_r", "shoulder_r_promo");
			animationLolVain.Skeleton.SetAttachment("arm_l", "shoulder_l_promo");
			animationLolVain.Skeleton.SetAttachment("forearm_r", "elbow_r_promo");
			animationLolVain.Skeleton.SetAttachment("forearm_l", "elbow_l_promo");
			animationLolVain.Skeleton.SetAttachment("hand_r", "hand_r_promo");
			animationLolVain.Skeleton.SetAttachment("hand_l", "hand_l_promo");
			animationLolVain.Skeleton.SetAttachment("thigh_r", "thigh_r_promo");
			animationLolVain.Skeleton.SetAttachment("thigh_l", "thigh_l_promo");
			animationLolVain.Skeleton.SetAttachment("shin_r", "shin_r_promo");
			animationLolVain.Skeleton.SetAttachment("shin_l", "shin_l_promo");
			animationLolVain.Skeleton.SetAttachment("foot_r", "foot_r_promo");
			animationLolVain.Skeleton.SetAttachment("foot_l", "foot_l_promo");
		}
	}
}
