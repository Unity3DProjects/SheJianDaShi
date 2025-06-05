using System.Collections.Generic;
using UnityEngine;

public class LocaleSpriteChanger : MonoBehaviour
{
	[SerializeField]
	private tk2dSprite currentSprite;

	[SerializeField]
	private LanguageSprite defaultLanguagesSprite;

	[SerializeField]
	private List<LanguageSprite> otheLanguagesSprites;

	private LanguageSprite currentLanguageSprite;

	private Managers cachedManagers;

	private void Start()
	{
		RefreshSpriteFonts();
	}

	private void OnEnable()
	{
		RefreshSpriteFonts();
	}

	public void RefreshSpriteFonts()
	{
		if (currentLanguageSprite.languages == null || currentLanguageSprite.spritesNames == null)
		{
			currentLanguageSprite = defaultLanguagesSprite;
		}
		SystemLanguage currentLanguage = SingletonMonoBehaviour<StatisticsManager>.Instance.CurrentLanguage;
		if (defaultLanguagesSprite.languages.Contains(currentLanguage))
		{
			ChangeSprite(defaultLanguagesSprite);
			return;
		}
		for (int i = 0; i < otheLanguagesSprites.Count; i++)
		{
			if (otheLanguagesSprites[i].languages.Contains(currentLanguage))
			{
				ChangeSprite(otheLanguagesSprites[i]);
				break;
			}
		}
	}

	private void ChangeSprite(LanguageSprite languageSprite)
	{
		int num = currentLanguageSprite.spritesNames.IndexOf(currentSprite.CurrentSprite.name);
		if (num != -1 && currentLanguageSprite.spritesNames.Count == languageSprite.spritesNames.Count)
		{
			if (currentSprite.name != languageSprite.spritesNames[num])
			{
				currentSprite.SetSprite(languageSprite.spritesNames[num]);
			}
			currentLanguageSprite = languageSprite;
		}
	}
}
