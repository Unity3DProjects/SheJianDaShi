using System;
using System.Collections.Generic;
using System.Linq;
using Mit_Goliath_Native.Utils;
using UnityEngine;

public class MiniclipSettingsButtons : MonoBehaviour
{
	public const string TARGETED_ADS_KEY = "ui.settings.targeted_ads";

	public const string ON_STATE_KEY = "ui.audio.on";

	public const string OFF_STATE_KEY = "ui.audio.off";

	public GameObject buttonTargetedAds;

	public GameObject buttonCustomerSupport;

	public GameObject buttonMoreGames;

	public GameObject buttonTermsConditions;

	[SerializeField]
	private tk2dTextMesh labelTargetedAds;

	private List<GameObject> buttons;

	private List<Action> buttonsCallbacks;

	private List<GameObject> addedFixedSpaces = new List<GameObject>();

	private GUILayoutCell bottomFlexSpace;

	private GUILayoutCell buttonsCell;

	private Transform verticalLayoutTransform;

	private GUILayouterRotationType currentRotation;

	public GUILayoutCell ButtonsCell
	{
		set
		{
			buttonsCell = value;
		}
	}

	public Transform VerticalLayoutTransform
	{
		set
		{
			verticalLayoutTransform = value;
			bottomFlexSpace = verticalLayoutTransform.Find("__FlexSpace").GetComponent<GUILayoutCell>();
		}
	}

	private void Awake()
	{
		if (SingletonMonoBehaviour<GDPRManager>.Instance.IsRelevant)
		{
			buttonsCallbacks = new List<Action> { ButtonTargetedAds_OnClick, ButtonCustomerSupport_OnClick, ButtonMoreGames_OnClick, ButtonTerms_OnClick };
			buttons = new List<GameObject> { buttonTargetedAds, buttonCustomerSupport, buttonMoreGames, buttonTermsConditions };
			buttonTargetedAds.gameObject.SetActive(true);
		}
		else
		{
			UnityEngine.Object.Destroy(buttonTargetedAds);
			buttonsCallbacks = new List<Action> { ButtonCustomerSupport_OnClick, ButtonMoreGames_OnClick, ButtonTerms_OnClick };
			buttons = new List<GameObject> { buttonCustomerSupport, buttonMoreGames, buttonTermsConditions };
			buttonTargetedAds.gameObject.SetActive(false);
		}
		currentRotation = SingletonMonoBehaviour<GUIOrientationManger>.Instance.CurrentRotationType;
		GUIOrientationManger.OnDeviceOrientationChanged += GUIOrientationManger_OnDeviceOrientationChanged;
	}

	public void onEnable()
	{
		for (int i = 0; i < buttons.Count; i++)
		{
			Action value = buttonsCallbacks.ElementAt(i);
			buttons.ElementAt(i).transform.GetComponentInChildren<tk2dUIItem>().OnClick += value;
		}
		ChangeLanguagePopup.OnLanguageChanged = (Action)Delegate.Combine(ChangeLanguagePopup.OnLanguageChanged, new Action(ChangeLanguagePopup_OnLanguageChanged));
		UpdateLabelTargetedAds(SingletonMonoBehaviour<GDPRManager>.Instance.TargetedAdsConsent);
	}

	public void onDisable()
	{
		for (int i = 0; i < buttons.Count; i++)
		{
			Action value = buttonsCallbacks.ElementAt(i);
			buttons.ElementAt(i).transform.GetComponentInChildren<tk2dUIItem>().OnClick -= value;
		}
		ChangeLanguagePopup.OnLanguageChanged = (Action)Delegate.Remove(ChangeLanguagePopup.OnLanguageChanged, new Action(ChangeLanguagePopup_OnLanguageChanged));
	}

	public void ChangeLanguagePopup_OnLanguageChanged()
	{
		UpdateLabelTargetedAds(SingletonMonoBehaviour<GDPRManager>.Instance.TargetedAdsConsent);
	}

	public void setupButtons()
	{
		adjustToPortraitLayout();
		if (currentRotation != GUILayouterRotationType.Landscape)
		{
		}
	}

	private void adjustToLandscapeLayout()
	{
		base.transform.parent = verticalLayoutTransform.transform;
		buttons.ForEach(delegate(GameObject b)
		{
			b.transform.parent = base.transform;
		});
		bottomFlexSpace.Type = GUILayoutCellType.FixedSize;
		bottomFlexSpace.SizeValue = 50f;
		base.transform.SetLocalPositionX(0f);
		float width = buttonMoreGames.GetComponent<GUILayoutCell>().RecievedRect.width;
		buttonMoreGames.transform.SetLocalPositionX((0f - width) * 1.2f);
		buttonTermsConditions.transform.SetLocalPositionX(width * 1.2f);
	}

	private void adjustToPortraitLayout()
	{
		Transform fixedSpaceObj = verticalLayoutTransform.Find("__FixedSpace");
		buttons.ForEach(delegate(GameObject b)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(fixedSpaceObj.gameObject);
			addedFixedSpaces.Add(gameObject);
			gameObject.transform.parent = verticalLayoutTransform;
			b.transform.parent = verticalLayoutTransform;
			gameObject.transform.SetAsFirstSibling();
			b.transform.SetAsFirstSibling();
			bottomFlexSpace.transform.SetAsFirstSibling();
			gameObject.transform.SetLocalPositionX(0f);
			b.transform.SetLocalPositionX(0f);
			bottomFlexSpace.transform.SetLocalPositionX(0f);
		});
	}

	private void GUIOrientationManger_OnDeviceOrientationChanged(GUILayouterRotationType newRotation)
	{
	}

	private void TargetedAdsSettingsPopup_OnRefusedClicked()
	{
		UpdateLabelTargetedAds(false);
	}

	private void UpdateLabelTargetedAds(bool isEnable)
	{
		labelTargetedAds.text = string.Format("{0} {1}", LocalisationManager.LocalizedStringOrSource("ui.settings.targeted_ads"), LocalisationManager.LocalizedStringOrSource((!isEnable) ? "ui.audio.off" : "ui.audio.on"));
	}

	private void ButtonTargetedAds_OnClick()
	{
		AudioManager.PlayAudioWithKey("button");
		bool flag = !SingletonMonoBehaviour<GDPRManager>.Instance.TargetedAdsConsent;
		SingletonMonoBehaviour<GDPRManager>.Instance.TargetedAdsConsent = flag;
		if (!flag)
		{
			GDPRPopup gDPRPopup = SingletonMonoBehaviour<GDPRManager>.Instance.ShowSettingsTargetedAdsPopup();
			gDPRPopup.OnRefusedClicked += TargetedAdsSettingsPopup_OnRefusedClicked;
		}
		else
		{
			GameGoliath.Instance.TrackPrivacyPolicyConsent(GoliathIds.GDPRStep.CLICK, GoliathIds.GDPRContext.SETTINGS, GoliathIds.GDPRClickResult.GAVE_AD_PERMISSION);
			UpdateLabelTargetedAds(flag);
		}
	}

	private void ButtonMoreGames_OnClick()
	{
		MCServices.ShowMoreGamesWebView();
	}

	private void ButtonTerms_OnClick()
	{
		MCServices.ShowTermsPopupWebView();
	}

	private void ButtonCustomerSupport_OnClick()
	{
		MCServices.ShowCustomerSupportWebView();
	}
}
