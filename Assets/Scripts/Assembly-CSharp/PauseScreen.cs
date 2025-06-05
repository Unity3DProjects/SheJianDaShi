using System;
using UnityEngine;

public class PauseScreen : GUIScreen
{
	[SerializeField]
	private tk2dUIItem buttonMenu;

	[SerializeField]
	private tk2dUIItem buttonResume;

	[SerializeField]
	private tk2dUIItem buttonMusic;

	[SerializeField]
	private tk2dUIItem buttonSound;

	[SerializeField]
	private tk2dTextMesh labelMusic;

	[SerializeField]
	private tk2dTextMesh labelSound;

	[SerializeField]
	private Color menuButtonDisabledColor;

	[SerializeField]
	private tk2dBaseSprite menuButtonBack;

	[SerializeField]
	private Color menuButtonLabelDisabledColor;

	[SerializeField]
	private tk2dTextMesh menuButtonLabel;

	private float inGameCategoryVolume;

	private void OnEnable()
	{
		buttonMenu.OnClick += ButtonMenu_OnClick;
		buttonResume.OnClick += ButtonResume_OnClick;
		buttonMusic.OnClick += ButtonMusic_OnClick;
		buttonSound.OnClick += ButtonSound_OnClick;
		AudioManager.OnMusicChanged += AudioManager_OnMusicChanged;
		AudioManager.OnSoundChanged += AudioManager_OnSoundChanged;
		AudioManager.PlayAudioWithKey("pause_menu_appear");
		inGameCategoryVolume = SoundsManager.GetCategoryVolume("InGame");
		SoundsManager.SetCategoryVolume("InGame", 0f);
		SetVolumeForSound("theme_result_screen", 1f);
		SetVolumeForSound("theme_main", 0f);
		if (!base.CachedManagers.CachedTutorialManager.IsTutorialMatchesEnded)
		{
			menuButtonBack.color = menuButtonDisabledColor;
			menuButtonLabel.color = menuButtonLabelDisabledColor;
			buttonMenu.enabled = false;
		}
	}

	private void OnDisable()
	{
		buttonMenu.OnClick -= ButtonMenu_OnClick;
		buttonResume.OnClick -= ButtonResume_OnClick;
		buttonMusic.OnClick -= ButtonMusic_OnClick;
		buttonSound.OnClick -= ButtonSound_OnClick;
		AudioManager.OnMusicChanged -= AudioManager_OnMusicChanged;
		AudioManager.OnSoundChanged -= AudioManager_OnSoundChanged;
		if (SingletonMonoBehaviour<SoundsManager>.InstanceIfExist != null)
		{
			SoundsManager.SetCategoryVolume("InGame", inGameCategoryVolume);
			SetVolumeForSound("theme_result_screen", 0f);
			SetVolumeForSound("theme_main", 1f);
		}
	}

	public override void Show(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		SetMusicText();
		base.Show(onFinished, immediately, delay, callInvokes);
	}

	public void ButtonSound_OnClick()
	{
		AudioManager.PlayAudioWithKey("button");
		if (base.CachedManagers.CachedAudioManager.IsSoundsEnabled)
		{
			base.CachedManagers.CachedAudioManager.DisableSounds();
			inGameCategoryVolume = 0f;
		}
		else
		{
			base.CachedManagers.CachedAudioManager.EnableSounds();
			inGameCategoryVolume = 1f;
		}
	}

	public void ButtonMusic_OnClick()
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

	public void ButtonResume_OnClick()
	{
		GameType type = base.CachedManagers.CachedGameManager.CurrentGameType;
		base.CachedManagers.CachedGUIScreenManager.HideCurrentScreen(true, delegate
		{
			if (type == GameType.BirdsHunt)
			{
				base.CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.Timer);
			}
			else
			{
				base.CachedManagers.CachedGameManager.IsGamePaused = false;
			}
			base.CachedManagers.CachedGUIScreenManager.ShowScreen(GUIScreenType.Game);
		}, false);
	}

	public void ButtonMenu_OnClick()
	{
		switch (base.CachedManagers.CachedGameManager.CurrentGameType)
		{
		case GameType.Tournament:
		{
			GUIPopup gUIPopup2 = base.CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.LeavingTournament);
			gUIPopup2.OnPopupHide = (Action<GUIPopup>)Delegate.Combine(gUIPopup2.OnPopupHide, new Action<GUIPopup>(LeavingMatch_OnPopupHide));
			break;
		}
		case GameType.Multiplayer:
		{
			GUIPopup gUIPopup = base.CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.LeavingBattle);
			gUIPopup.OnPopupHide = (Action<GUIPopup>)Delegate.Combine(gUIPopup.OnPopupHide, new Action<GUIPopup>(LeavingMatch_OnPopupHide));
			break;
		}
		default:
			LeaveGame();
			break;
		}
	}

	private void SetVolumeForSound(string soundName, float volume)
	{
		SoundObject[] playingAudioObjects = SoundsManager.GetPlayingAudioObjects(soundName);
		if (playingAudioObjects.Length == 0)
		{
			AudioManager.PlayAudioWithKey(soundName);
			playingAudioObjects = SoundsManager.GetPlayingAudioObjects(soundName);
		}
		SoundObject[] array = playingAudioObjects;
		foreach (SoundObject soundObject in array)
		{
			soundObject.Volume = volume;
		}
	}

	private void LeaveGame()
	{
		base.CachedManagers.CachedGUIScreenManager.HideCurrentScreen(true, delegate
		{
			base.CachedManagers.CachedGameManager.IsGamePaused = false;
			base.CachedManagers.CachedGameManager.StopGame();
			base.CachedManagers.CachedGUIScreenManager.ShowScreen(GUIScreenType.Main);
			SingletonMonoBehaviour<AdvertisingManager>.Instance.ShowInterstitial();
		});
	}

	private void SetMusicText()
	{
		labelMusic.text = LocalisationManager.LocalizedStringOrSource("ui.audio.music") + LocalisationManager.LocalizedStringOrSource((!base.CachedManagers.CachedAudioManager.IsMusicEnabled) ? "ui.audio.off" : "ui.audio.on");
		labelSound.text = LocalisationManager.LocalizedStringOrSource("ui.audio.sound") + LocalisationManager.LocalizedStringOrSource((!base.CachedManagers.CachedAudioManager.IsSoundsEnabled) ? "ui.audio.off" : "ui.audio.on");
	}

	private void AudioManager_OnMusicChanged(bool result)
	{
		labelMusic.text = LocalisationManager.LocalizedStringOrSource("ui.audio.music") + LocalisationManager.LocalizedStringOrSource((!result) ? "ui.audio.off" : "ui.audio.on");
	}

	private void AudioManager_OnSoundChanged(bool result)
	{
		labelSound.text = LocalisationManager.LocalizedStringOrSource("ui.audio.sound") + LocalisationManager.LocalizedStringOrSource((!result) ? "ui.audio.off" : "ui.audio.on");
	}

	private void LeavingMatch_OnPopupHide(GUIPopup popup)
	{
		popup.OnPopupHide = (Action<GUIPopup>)Delegate.Remove(popup.OnPopupHide, new Action<GUIPopup>(LeavingMatch_OnPopupHide));
		if ((popup as LeavingTournamentPopup).IsLeave)
		{
			SingletonMonoBehaviour<TournamentManager>.Instance.ResetTournament();
			LeaveGame();
		}
	}

	public override void BackButtonPressed()
	{
		ButtonResume_OnClick();
	}
}
