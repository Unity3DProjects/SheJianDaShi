using System;
using System.Diagnostics;
using Mit_McServices.Rate;
using Mit_McServices.Rate.Settings;

public class GameRate : RateManager
{
	private new const bool LOGS_ARE_ENABLED = false;

	private bool willUseNativePopup;

	private string rateTitle;

	private string rateMsg;

	private string rateBtnLater;

	private string rateBtnOk;

	private float minimumActiveTimePerSession;

	private int questionLimitPerVersion;

	private int minimumSessionStarts;

	private float minimumDaysInterval;

	private int minimumPositiveExperiences;

	private bool shouldUseIOSNativePopup;

	public static GameRate Instance
	{
		get
		{
			return (GameRate)RateManager.instance;
		}
	}

	protected new void Awake()
	{
		base.Awake();
		SetupTestSettings();
		Initialize();
	}

	private void SetupTestSettings()
	{
		willUseNativePopup = true;
		rateTitle = "rate_descr";
		rateMsg = "rate_invite";
		rateBtnLater = "rate_later";
		rateBtnOk = "rate_ok";
		minimumActiveTimePerSession = 100f;
		questionLimitPerVersion = 1;
		minimumSessionStarts = 2;
		minimumDaysInterval = 0f;
		minimumPositiveExperiences = 0;
		shouldUseIOSNativePopup = false;
	}

	public override RateSettings GetSettings()
	{
		RateTexts rateTexts = new RateTexts(rateTitle, rateMsg, rateBtnLater, rateBtnOk);
		RateSettings rateSettings = new RateSettings();
		rateSettings.WillUseNativePopup = willUseNativePopup;
		rateSettings.Texts = new RateTexts(rateTitle, rateMsg, rateBtnLater, rateBtnOk);
		rateSettings.MinimumActiveTimePerSession = minimumActiveTimePerSession;
		rateSettings.QuestionLimitPerVersion = questionLimitPerVersion;
		rateSettings.MinimumSessionStarts = minimumSessionStarts;
		rateSettings.MinimumDaysInterval = minimumDaysInterval;
		rateSettings.MinimumPositiveExperiences = minimumPositiveExperiences;
		rateSettings.ShouldUseIOSNativePopup = shouldUseIOSNativePopup;
		return rateSettings;
	}

	protected override void CustomRatePopupBehaviour(Action rateBehaviour, Action cancelBehaviour)
	{
	}

	protected override bool RateIsEnabled()
	{
		return true;
	}

	protected override bool ConditionsAreMet()
	{
		return true;
	}

	protected override string GetVersionString()
	{
		return "versionString";
	}

	protected override void OnPopupShown()
	{
	}

	protected override void OnPopupDismissed()
	{
	}

	[Conditional("UNITY_EDITOR")]
	[Conditional("MC_DEBUG")]
	private static void Log(string logStr)
	{
	}
}
