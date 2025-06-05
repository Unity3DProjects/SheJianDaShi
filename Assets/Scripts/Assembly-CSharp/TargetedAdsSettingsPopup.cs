using Mit_Goliath_Native.Utils;
using UnityEngine;

public class TargetedAdsSettingsPopup : GDPRPopup
{
	[SerializeField]
	private tk2dUIItem NoThanksButton;

	[SerializeField]
	public tk2dTextMesh NoThanksText;

	protected override void OnEnable()
	{
		base.OnEnable();
		NoThanksButton.OnClick += OnButtonRefusedClicked;
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		NoThanksButton.OnClick -= OnButtonRefusedClicked;
	}

	public virtual void ChangePopupText()
	{
		AgreeText.text = "Keep this permission";
		NoThanksText.text = "Revoke";
	}

	public override void OnButtonAgreedClicked()
	{
		GameGoliath.Instance.TrackPrivacyPolicyConsent(GoliathIds.GDPRStep.CLICK, GoliathIds.GDPRContext.SETTINGS_POPUP, GoliathIds.GDPRClickResult.GAVE_AD_PERMISSION);
		GDPR.SetUserAnswer(GDPR.ConsentType.TargetedAds, true);
		SingletonMonoBehaviour<GDPRManager>.Instance.TargetedAdsConsent = true;
		base.OnButtonAgreedClicked();
		Hide();
	}

	public override void OnButtonRefusedClicked()
	{
		GameGoliath.Instance.TrackPrivacyPolicyConsent(GoliathIds.GDPRStep.CLICK, GoliathIds.GDPRContext.SETTINGS_POPUP, GoliathIds.GDPRClickResult.DENIED_AD_PERMISSION);
		GDPR.SetUserAnswer(GDPR.ConsentType.TargetedAds, false);
		SingletonMonoBehaviour<GDPRManager>.Instance.TargetedAdsConsent = false;
		base.OnButtonRefusedClicked();
		Hide();
	}
}
