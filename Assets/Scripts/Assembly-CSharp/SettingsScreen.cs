using System;
using UnityEngine;

public class SettingsScreen : GUIScreen
{
	public const string MUSIC_KEY = "ui.audio.music";

	public const string SOUND_KEY = "ui.audio.sound";

	public const string NOTIFY_KEY = "ui.settings.notify";

	public const string ON_STATE_KEY = "ui.audio.on";

	public const string OFF_STATE_KEY = "ui.audio.off";

	public const string LANGUAGE_KEY = "ui.settings.language";

	private MiniclipSettingsButtons miniclipSettingsButtonScript;

	private int consumeCounter;

	[SerializeField]
	private tk2dUIItem soundButton;

	[SerializeField]
	private tk2dUIItem musicButton;

	[SerializeField]
	private tk2dUIItem notifyButton;

	[SerializeField]
	private tk2dUIItem restorePurchasesButton;

	[SerializeField]
	private tk2dUIItem restoreProgressButton;

	[SerializeField]
	private tk2dUIItem buttonLanguage;

	[SerializeField]
	private tk2dUIItem[] buttonResumes;

	[SerializeField]
	private tk2dTextMesh labelSettingSound;

	[SerializeField]
	private tk2dTextMesh labelSettingMusic;

	[SerializeField]
	private tk2dTextMesh labelSettingNotify;

	[SerializeField]
	private tk2dTextMesh labelSettingLanguage;

	[SerializeField]
	private tk2dTextMesh[] labelsCoins;

	[SerializeField]
	private tk2dTextMesh[] labelsGems;

	[Header("iPhone X")]
	[SerializeField]
	private float portraitTopOffset;

	[SerializeField]
	private GUILayoutCell portraitTopOffsetCell;

	[SerializeField]
	private float rightButtonsIPhoneXOffset;

	[SerializeField]
	private GUILayoutCell leftButtonsCell;

	[SerializeField]
	private GUILayoutCell rightButtonsCell;

	[SerializeField]
	private float bottomIPhoneXOffset;

	[SerializeField]
	private GUILayoutCell topButtonsCell;

	[SerializeField]
	private GUILayoutCell bottomButtonsCell;

	[Button]
	[SerializeField]
	private string resetPrefs = "ResetPrefs";

	private void Awake()
	{
		miniclipSettingsButtonScript = UnityEngine.Object.Instantiate(MiniclipPrefabs.Instance.settingsButtons).GetComponent<MiniclipSettingsButtons>();
		miniclipSettingsButtonScript.VerticalLayoutTransform = soundButton.transform.parent.parent;
		miniclipSettingsButtonScript.ButtonsCell = soundButton.transform.parent.parent.parent.GetComponent<GUILayoutCell>();
		miniclipSettingsButtonScript.setupButtons();
		restorePurchasesButton.transform.parent.parent = null;
		restorePurchasesButton.transform.parent.gameObject.SetActive(false);
		restoreProgressButton.transform.parent.parent = null;
		restoreProgressButton.transform.parent.gameObject.SetActive(false);
	}

	private void Start()
	{
	}

	private void OnEnable()
	{
		soundButton.OnClick += SoundButton_OnClick;
		musicButton.OnClick += MusicButton_OnClick;
		notifyButton.OnClick += NotifyButton_OnClick;
		notifyButton.gameObject.SetActive(false);
		tk2dUIItem[] array = buttonResumes;
		foreach (tk2dUIItem tk2dUIItem2 in array)
		{
			tk2dUIItem2.OnClick += ButtonResume_OnClick;
		}
		StatisticsManager.OnCoinsChanged += StatisticsManager_OnCoinsChanged;
		StatisticsManager.OnGemsChanged += StatisticsManager_OnGemsChanged;
		restorePurchasesButton.OnClick += OnRestorePurchasesButtonClicked;
		restoreProgressButton.OnClick += OnRestoreProgressButtonClicked;
		AudioManager.OnMusicChanged += AudioManager_OnMusicChanged;
		AudioManager.OnSoundChanged += AudioManager_OnSoundChanged;
		CustomGUI.OnDebugGUI += CustomGUI_OnDebugGUI;
		GUIScreenManager.OnLanguageFontsChanged += GUIScreenManager_OnLanguageFontsChanged;
		buttonLanguage.OnClick += ButtonLanguage_OnClick;
		ChangeLanguagePopup.OnLanguageChanged = (Action)Delegate.Combine(ChangeLanguagePopup.OnLanguageChanged, new Action(ChangeLanguagePopup_OnLanguageChanged));
		miniclipSettingsButtonScript.onEnable();
	}

	private void OnDisable()
	{
		soundButton.OnClick -= SoundButton_OnClick;
		musicButton.OnClick -= MusicButton_OnClick;
		notifyButton.OnClick -= NotifyButton_OnClick;
		tk2dUIItem[] array = buttonResumes;
		foreach (tk2dUIItem tk2dUIItem2 in array)
		{
			tk2dUIItem2.OnClick -= ButtonResume_OnClick;
		}
		restorePurchasesButton.OnClick -= OnRestorePurchasesButtonClicked;
		restoreProgressButton.OnClick -= OnRestoreProgressButtonClicked;
		StatisticsManager.OnCoinsChanged -= StatisticsManager_OnCoinsChanged;
		StatisticsManager.OnGemsChanged -= StatisticsManager_OnGemsChanged;
		AudioManager.OnMusicChanged -= AudioManager_OnMusicChanged;
		AudioManager.OnSoundChanged -= AudioManager_OnSoundChanged;
		CustomGUI.OnDebugGUI -= CustomGUI_OnDebugGUI;
		GUIScreenManager.OnLanguageFontsChanged -= GUIScreenManager_OnLanguageFontsChanged;
		buttonLanguage.OnClick -= ButtonLanguage_OnClick;
		ChangeLanguagePopup.OnLanguageChanged = (Action)Delegate.Remove(ChangeLanguagePopup.OnLanguageChanged, new Action(ChangeLanguagePopup_OnLanguageChanged));
		miniclipSettingsButtonScript.onDisable();
	}

	public override void Show(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		labelSettingMusic.text = LocalisationManager.LocalizedStringOrSource("ui.audio.music") + LocalisationManager.LocalizedStringOrSource((!base.CachedManagers.CachedAudioManager.IsMusicEnabled) ? "ui.audio.off" : "ui.audio.on");
		labelSettingSound.text = LocalisationManager.LocalizedStringOrSource("ui.audio.sound") + LocalisationManager.LocalizedStringOrSource((!base.CachedManagers.CachedAudioManager.IsSoundsEnabled) ? "ui.audio.off" : "ui.audio.on");
		labelSettingLanguage.text = LocalisationManager.LocalizedStringOrSource("ui.settings.language") + SingletonMonoBehaviour<LocalisationManager>.Instance.CurrentLanguageAssetLanguageName;
		UpdateLabelSettingsNotify(base.CachedManagers.CachedNotifyManager.IsLocalNotificationsEnable);
		StatisticsManager_OnCoinsChanged();
		StatisticsManager_OnGemsChanged();
		base.Show(onFinished, immediately, delay, callInvokes);
	}

	public override void Hide(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		Action<GUIControl> onFinished2 = delegate(GUIControl control)
		{
			base.CachedManagers.CachedProposalManager.CountCloseSettingsScreen++;
			if (onFinished != null)
			{
				onFinished(control);
			}
			base.CachedManagers.CachedProposalManager.ShowInterstitialMenu(true);
		};
		restorePurchasesButton.transform.parent.parent = musicButton.transform.parent.parent;
		base.Hide(onFinished2, immediately, delay, callInvokes);
	}

	private void UpdateLabelSettingsNotify(bool isEnable)
	{
		labelSettingNotify.text = string.Format("{0} {1}", LocalisationManager.LocalizedStringOrSource("ui.settings.notify"), LocalisationManager.LocalizedStringOrSource((!isEnable) ? "ui.audio.off" : "ui.audio.on"));
	}

	private void SetMusicText()
	{
		labelSettingMusic.text = LocalisationManager.LocalizedStringOrSource("ui.audio.music") + LocalisationManager.LocalizedStringOrSource((!base.CachedManagers.CachedAudioManager.IsMusicEnabled) ? "ui.audio.off" : "ui.audio.on");
		labelSettingSound.text = LocalisationManager.LocalizedStringOrSource("ui.audio.sound") + LocalisationManager.LocalizedStringOrSource((!base.CachedManagers.CachedAudioManager.IsSoundsEnabled) ? "ui.audio.off" : "ui.audio.on");
		UpdateLabelSettingsNotify(base.CachedManagers.CachedNotifyManager.IsLocalNotificationsEnable);
	}

	private void SoundButton_OnClick()
	{
		AudioManager.PlayAudioWithKey("button");
		if (base.CachedManagers.CachedAudioManager.IsSoundsEnabled)
		{
			base.CachedManagers.CachedAudioManager.DisableSounds();
		}
		else
		{
			base.CachedManagers.CachedAudioManager.EnableSounds();
		}
	}

	private void MusicButton_OnClick()
	{
		AudioManager.PlayAudioWithKey("button");
		if (base.CachedManagers.CachedAudioManager.IsMusicEnabled)
		{
			base.CachedManagers.CachedAudioManager.DisableMusic();
		}
		else
		{
			base.CachedManagers.CachedAudioManager.EnableMusic();
		}
	}

	private void NotifyButton_OnClick()
	{
		AudioManager.PlayAudioWithKey("button");
		bool flag = !base.CachedManagers.CachedNotifyManager.IsLocalNotificationsEnable;
		base.CachedManagers.CachedNotifyManager.IsLocalNotificationsEnable = flag;
		UpdateLabelSettingsNotify(flag);
	}

	private void ButtonResume_OnClick()
	{
		base.CachedManagers.CachedGUIScreenManager.ShowPreviousScreen();
	}

	private void ButtonMoreGames_OnClick()
	{
		MCServices.ShowMoreGamesWebView();
	}

	private void ButtonTerms_OnClick()
	{
		MCServices.ShowTermsPopupWebView();
	}

	private void StatisticsManager_OnCoinsChanged(int obj = 0)
	{
		tk2dTextMesh[] array = labelsCoins;
		foreach (tk2dTextMesh tk2dTextMesh2 in array)
		{
			tk2dTextMesh2.text = base.CachedManagers.CachedStatisticsManager.Coins.ToString();
		}
	}

	private void StatisticsManager_OnGemsChanged(int obj = 0)
	{
		tk2dTextMesh[] array = labelsGems;
		foreach (tk2dTextMesh tk2dTextMesh2 in array)
		{
			tk2dTextMesh2.text = base.CachedManagers.CachedStatisticsManager.Gems.ToString();
		}
	}

	private void OnRestorePurchasesButtonClicked()
	{
		AudioManager.PlayAudioWithKey("button");
		if (base.CachedManagers.CachedShopManager.CanMakePurchase())
		{
			base.CachedManagers.CachedTK2DUIManager.InputEnabled = false;
			base.CachedManagers.CachedShopManager.RestorePurchases(ShopManager_RestoreTransactions);
		}
	}

	private void OnRestoreProgressButtonClicked()
	{
		AudioManager.PlayAudioWithKey("button");
		base.CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.ICloudLoader, true, null, true);
	}

	private void AudioManager_OnMusicChanged(bool result)
	{
		labelSettingMusic.text = LocalisationManager.LocalizedStringOrSource("ui.audio.music") + LocalisationManager.LocalizedStringOrSource((!result) ? "ui.audio.off" : "ui.audio.on");
	}

	private void AudioManager_OnSoundChanged(bool result)
	{
		labelSettingSound.text = LocalisationManager.LocalizedStringOrSource("ui.audio.sound") + LocalisationManager.LocalizedStringOrSource((!result) ? "ui.audio.off" : "ui.audio.on");
	}

	private void ShopManager_RestoreTransactions(bool result)
	{
		base.CachedManagers.CachedTK2DUIManager.InputEnabled = true;
		MenuScreen menuScreen = base.CachedManagers.CachedGUIScreenManager.MenuScreen;
		if (menuScreen != null)
		{
			menuScreen.MenuScreenScrollView.CheckBoughtCharacters();
		}
		if (result)
		{
			base.CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.RestorePurchase, true, null, true);
		}
	}

	private void GUIScreenManager_OnLanguageFontsChanged()
	{
		SetMusicText();
	}

	private void ButtonLanguage_OnClick()
	{
		AudioManager.PlayAudioWithKey("button");
		SingletonMonoBehaviour<GUIPopupManager>.Instance.ShowPopup(PopupType.ChangeLanguage);
	}

	private void ChangeLanguagePopup_OnLanguageChanged()
	{
		labelSettingLanguage.text = LocalisationManager.LocalizedStringOrSource("ui.settings.language") + SingletonMonoBehaviour<LocalisationManager>.Instance.CurrentLanguageAssetLanguageName;
	}

	private void BankButton_OnClick()
	{
		base.CachedManagers.CachedGUIScreenManager.HideCurrentScreen(true, delegate
		{
			base.CachedManagers.CachedGUIScreenManager.ShowScreen(GUIScreenType.Shop);
		});
		base.CachedManagers.CachedGUIScreenManager.BeforeShopScreenType = GUIScreenType.Settings;
	}

	private void CustomGUI_OnDebugGUI()
	{
		if (GUI.Button(new Rect(0f, 100f, 100f, 100f), "Reset Prefs") && !string.IsNullOrEmpty(resetPrefs))
		{
			CustomPlayerPrefs.DeleteAll();
		}
		if (GUI.Button(new Rect(Screen.width - 150, Screen.height - 350, 150f, 50f), "English"))
		{
			SingletonMonoBehaviour<StatisticsManager>.Instance.CurrentLanguage = SystemLanguage.English;
			SingletonMonoBehaviour<LocalisationManager>.Instance.ChangeLanguage(SingletonMonoBehaviour<StatisticsManager>.Instance.CurrentLanguage);
		}
		if (GUI.Button(new Rect(Screen.width - 150, Screen.height - 400, 150f, 50f), "Chinese"))
		{
			SingletonMonoBehaviour<StatisticsManager>.Instance.CurrentLanguage = SystemLanguage.Chinese;
			SingletonMonoBehaviour<LocalisationManager>.Instance.ChangeLanguage(SingletonMonoBehaviour<StatisticsManager>.Instance.CurrentLanguage);
		}
		if (GUI.Button(new Rect(0f, 300f, 100f, 100f), "CLOUD SAVE"))
		{
			CloudSaveManager.Instance.SaveToCloud();
		}
		if (GUI.Button(new Rect(0f, 400f, 100f, 100f), "CLOUD LOAD"))
		{
			CloudSaveManager.Instance.LoadFromCloud();
		}
		if (GUI.Button(new Rect(0f, 500f, 100f, 100f), "CLOUD LOAD!!!"))
		{
			CloudSaveManager.Instance.PerformLoadFromCloud();
		}
		if (GUI.Button(new Rect(0f, 600f, 100f, 100f), "CLOUD SAVE!!!"))
		{
			CloudSaveManager.Instance.PerformSaveToCloud();
		}
		if (GUI.Button(new Rect(0f, 700f, 100f, 100f), (consumeCounter == 0) ? "Clear purchases" : ((consumeCounter == 1) ? "Are you sure?" : ((consumeCounter != 2) ? "Last time" : "Really?"))))
		{
		}
		if (!GUI.Button(new Rect(0f, 800f, 100f, 100f), "Unlock Chars"))
		{
			return;
		}
		foreach (ShopItemCharacter availableCharacter in SingletonMonoBehaviour<ShopManager>.Instance.AvailableCharacters)
		{
			availableCharacter.OpenCharacter(true, true);
			GUICharacter gUICharacter = base.ObjectsSpawner.GetGUICharacter(availableCharacter.Infos[0].characterNameType);
			gUICharacter.CheckBought();
			SingletonMonoBehaviour<TutorialManager>.Instance.WasCharacterBoughtOnCoins = true;
		}
	}

	public override void BackButtonPressed()
	{
		ButtonResume_OnClick();
	}
}
