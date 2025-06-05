using UnityEngine;

public class PrivacyPolicyPopup : GDPRPopup
{
	[SerializeField]
	private tk2dUIItem PrivacyPolicyButton;

	protected override void OnEnable()
	{
		base.OnEnable();
		PrivacyPolicyButton.OnClick += OnButtonCheckPrivacyClicked;
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		PrivacyPolicyButton.OnClick -= OnButtonCheckPrivacyClicked;
	}

	public virtual void ChangePopupText()
	{
	}

	public override void OnButtonAgreedClicked()
	{
		GDPR.SetUserAnswer(GDPR.ConsentType.PrivacyPolicy, true);
		SingletonMonoBehaviour<GDPRManager>.Instance.PrivacyPolicyAccepted = true;
		base.OnButtonAgreedClicked();
		Hide();
	}

	public override void OnButtonCheckPrivacyClicked()
	{
		MCServices.ShowTermsPopupWebView();
		base.OnButtonCheckPrivacyClicked();
		Hide();
	}
}
