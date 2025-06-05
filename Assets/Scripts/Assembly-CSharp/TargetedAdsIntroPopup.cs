using Mit_Goliath_Native.Utils;
using UnityEngine;

public class TargetedAdsIntroPopup : GDPRPopup
{
	[SerializeField]
	private tk2dUIItem PrivacyPolicyButton;

	[SerializeField]
	private tk2dUIItem NoThanksButton;

	[SerializeField]
	public tk2dTextMesh NoThanksText;

	protected override void OnEnable()
	{
		base.OnEnable();
		PrivacyPolicyButton.OnClick += OnButtonCheckPrivacyClicked;
		NoThanksButton.OnClick += OnButtonRefusedClicked;
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		PrivacyPolicyButton.OnClick -= OnButtonCheckPrivacyClicked;
		NoThanksButton.OnClick -= OnButtonRefusedClicked;
	}

	public override void OnButtonAgreedClicked()
	{
		GameGoliath.Instance.TrackPrivacyPolicyConsent(GoliathIds.GDPRStep.CLICK, GoliathIds.GDPRContext.PRIVACY_POLICY_WITH_AD_CONSENT, GoliathIds.GDPRClickResult.GAVE_AD_PERMISSION);
		GDPR.SetUserAnswer(GDPR.ConsentType.TargetedAds, true);
		SingletonMonoBehaviour<GDPRManager>.Instance.TargetedAdsConsent = true;
		base.OnButtonAgreedClicked();
		Hide();
	}

	public override void OnButtonRefusedClicked()
	{
		GameGoliath.Instance.TrackPrivacyPolicyConsent(GoliathIds.GDPRStep.CLICK, GoliathIds.GDPRContext.PRIVACY_POLICY_WITH_AD_CONSENT, GoliathIds.GDPRClickResult.DENIED_AD_PERMISSION);
		GDPR.SetUserAnswer(GDPR.ConsentType.TargetedAds, false);
		SingletonMonoBehaviour<GDPRManager>.Instance.TargetedAdsConsent = false;
		base.OnButtonRefusedClicked();
		Hide();
	}

	public override void OnButtonCheckPrivacyClicked()
	{
		GameGoliath.Instance.TrackPrivacyPolicyConsent(GoliathIds.GDPRStep.CLICK, GoliathIds.GDPRContext.PRIVACY_POLICY_WITH_AD_CONSENT, GoliathIds.GDPRClickResult.PRIVACY_POLICY);
		MCServices.ShowTermsPopupWebView();
		base.OnButtonCheckPrivacyClicked();
		Hide();
	}
}
