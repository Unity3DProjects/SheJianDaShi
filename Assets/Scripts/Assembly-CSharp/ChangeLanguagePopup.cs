using System;
using UnityEngine;

public class ChangeLanguagePopup : GUIPopup
{
	public static Action OnLanguageChanged;

	[SerializeField]
	private tk2dUIItem englishButton;

	[SerializeField]
	private tk2dUIItem chineseButton;

	[SerializeField]
	private tk2dSprite englishSprite;

	[SerializeField]
	private tk2dSprite chineseSprite;

	private SystemLanguage currentLanguage;

	private SystemLanguage CurrentLanguage
	{
		get
		{
			return currentLanguage;
		}
		set
		{
			if (currentLanguage != value)
			{
				currentLanguage = value;
				bool flag = currentLanguage == SystemLanguage.Chinese || currentLanguage == SystemLanguage.ChineseSimplified || currentLanguage == SystemLanguage.ChineseTraditional;
				englishSprite.gameObject.SetActive(!flag);
				chineseSprite.gameObject.SetActive(flag);
				SingletonMonoBehaviour<StatisticsManager>.Instance.CurrentLanguage = currentLanguage;
			}
		}
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		englishButton.OnClick += EnglishButton_OnClick;
		chineseButton.OnClick += ChineseButton_OnClick;
		CurrentLanguage = SingletonMonoBehaviour<StatisticsManager>.Instance.CurrentLanguage;
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		englishButton.OnClick -= EnglishButton_OnClick;
		chineseButton.OnClick -= ChineseButton_OnClick;
	}

	private void ChangeLanguage(SystemLanguage selectedLanguage)
	{
		if (!SingletonMonoBehaviour<LocalisationManager>.Instance.CurrentLanguageAssetLanguages.Contains(selectedLanguage))
		{
			CurrentLanguage = selectedLanguage;
			SingletonMonoBehaviour<LocalisationManager>.Instance.ChangeLanguage(selectedLanguage);
			if (OnLanguageChanged != null)
			{
				OnLanguageChanged();
			}
		}
	}

	private void EnglishButton_OnClick()
	{
		ChangeLanguage(SystemLanguage.English);
	}

	private void ChineseButton_OnClick()
	{
		ChangeLanguage(SystemLanguage.Chinese);
	}
}
