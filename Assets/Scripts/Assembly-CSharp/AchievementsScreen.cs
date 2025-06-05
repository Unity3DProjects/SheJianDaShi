using System;
using System.Collections.Generic;
using UnityEngine;

public class AchievementsScreen : GUIScreen
{
	[Serializable]
	private struct AchievementInfo
	{
		public MicroachievementType typeAchievement;

		public int reachedLevelAchiviement;
	}

	[SerializeField]
	private tk2dUIScrollView scrollView;

	[SerializeField]
	private tk2dTextMesh[] coinsLabels;

	[SerializeField]
	private tk2dTextMesh[] gemsLabels;

	[SerializeField]
	private tk2dUIItem[] backButtons;

	[SerializeField]
	private tk2dTextMesh[] countLabels;

	[SerializeField]
	private GUILayoutCell parentCellAchievement;

	[SerializeField]
	private GUIAchievement prefabAchievement;

	[SerializeField]
	private float cellsInterval;

	[SerializeField]
	private float landscapeOffset;

	[SerializeField]
	private float portraitOffset;

	[SerializeField]
	private List<AchievementInfo> infoAchievements;

	private tk2dUIItem[] buttonAchievements;

	[Header("iPhone X")]
	[SerializeField]
	private GUILayoutCell leftButtonsCell;

	[SerializeField]
	private GUILayoutCell rightButtonsCell;

	[SerializeField]
	private float rightButtonsIPhoneXOffset;

	[SerializeField]
	private GUILayoutCell[] bottomBarCells;

	[SerializeField]
	private float bottomBarIPhoneXSize;

	[SerializeField]
	private GUILayoutCell barRootCell;

	[SerializeField]
	private float barRootCellIPhoneXSize;

	[SerializeField]
	private GUILayoutCell[] topBarCells;

	[SerializeField]
	private float topBarIPhoneXSize;

	private bool isAchievementInitialized;

	private List<GUIAchievement> achievements = new List<GUIAchievement>();

	private void Awake()
	{
		buttonAchievements = new tk2dUIItem[2];
		for (int i = 0; i < backButtons.Length; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(backButtons[i].gameObject, backButtons[i].transform.parent);
			gameObject.transform.SetLocalPositionY(-95f);
			tk2dSpriteCollection component = MiniclipPrefabs.Instance.spriteCollection.GetComponent<tk2dSpriteCollection>();
			tk2dSpriteCollectionData spriteCollection = component.spriteCollection;
			tk2dSprite componentInChildren = gameObject.GetComponentInChildren<tk2dSprite>();
			componentInChildren.SetSprite(spriteCollection, "games_controller_white");
			buttonAchievements[i] = gameObject.GetComponentInChildren<tk2dUIItem>();
		}
	}

	private void OnEnable()
	{
		tk2dUIItem[] array = buttonAchievements;
		foreach (tk2dUIItem tk2dUIItem2 in array)
		{
			tk2dUIItem2.OnClick += Achievements_OnClick;
		}
		for (int j = 0; j < backButtons.Length; j++)
		{
			backButtons[j].OnClick += BackButton_OnClick;
		}
		StatisticsManager.OnCoinsChanged += StatisticsManager_OnCoinsChanged;
		StatisticsManager.OnGemsChanged += StatisticsManager_OnCoinsChanged;
		GUIOrientationManger.OnDeviceOrientationChanged += GUIOrientationManger_OnDeviceOrientationChanged;
	}

	private void OnDisable()
	{
		tk2dUIItem[] array = buttonAchievements;
		foreach (tk2dUIItem tk2dUIItem2 in array)
		{
			tk2dUIItem2.OnClick -= Achievements_OnClick;
		}
		for (int j = 0; j < backButtons.Length; j++)
		{
			backButtons[j].OnClick -= BackButton_OnClick;
		}
		StatisticsManager.OnCoinsChanged -= StatisticsManager_OnCoinsChanged;
		StatisticsManager.OnGemsChanged -= StatisticsManager_OnCoinsChanged;
		GUIOrientationManger.OnDeviceOrientationChanged -= GUIOrientationManger_OnDeviceOrientationChanged;
	}

	public override void Show(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		StatisticsManager_OnCoinsChanged();
		StatisticsManager_OnGemsChanged();
		InitializeCharacters();
		base.Show(onFinished, immediately, delay, callInvokes);
	}

	public override void Hide(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		Action<GUIControl> onFinished2 = delegate(GUIControl control)
		{
			base.CachedManagers.CachedProposalManager.CountCloseAchievementsScreen++;
			if (onFinished != null)
			{
				onFinished(control);
			}
			base.CachedManagers.CachedProposalManager.ShowInterstitialMenu(false);
		};
		base.Hide(onFinished2, immediately, delay, callInvokes);
	}

	private void InitializeCharacters()
	{
		int num = 0;
		if (!isAchievementInitialized)
		{
			achievements.Clear();
			for (int i = 0; i < infoAchievements.Count; i++)
			{
				GUIAchievement gUIAchievement = UnityEngine.Object.Instantiate(prefabAchievement);
				gUIAchievement.Init(infoAchievements[i].typeAchievement, infoAchievements[i].reachedLevelAchiviement, parentCellAchievement);
				gUIAchievement.transform.parent = scrollView.contentContainer;
				achievements.Add(gUIAchievement);
				if (gUIAchievement.IsReached)
				{
					num++;
				}
			}
			isAchievementInitialized = true;
		}
		GUIOrientationManger_OnDeviceOrientationChanged();
		for (int j = 0; j < countLabels.Length; j++)
		{
			countLabels[j].text = string.Format("{0}/{1}", num.ToString(), achievements.Count.ToString());
		}
	}

	private void BackButton_OnClick()
	{
		base.CachedManagers.CachedGUIScreenManager.ShowPreviousScreen();
	}

	private void Achievements_OnClick()
	{
		SingletonMonoBehaviour<GameCenterManager>.Instance.OpenAchievements();
	}

	private void StatisticsManager_OnCoinsChanged(int obj = 0)
	{
		for (int i = 0; i < countLabels.Length; i++)
		{
			coinsLabels[i].text = base.CachedManagers.CachedStatisticsManager.Coins.ToString();
		}
	}

	private void StatisticsManager_OnGemsChanged(int gemsCount = 0)
	{
		for (int i = 0; i < countLabels.Length; i++)
		{
			gemsLabels[i].text = base.CachedManagers.CachedStatisticsManager.Gems.ToString();
		}
	}

	private void GUIOrientationManger_OnDeviceOrientationChanged(GUILayouterRotationType obj = GUILayouterRotationType.Both)
	{
		switch (base.CachedManagers.CachedGUIOrientationManager.CurrentRotationType)
		{
		case GUILayouterRotationType.Landscape:
		{
			for (int j = 0; j < achievements.Count; j++)
			{
				GUIAchievement gUIAchievement2 = achievements[j];
				if (gUIAchievement2 != null)
				{
					gUIAchievement2.transform.localPosition = (landscapeOffset + cellsInterval * (float)j - ((float)Screen.height * GUIScreenManager.GuiScale - cellsInterval) / 2f) * Vector3.down;
				}
			}
			scrollView.contentLength = landscapeOffset + (float)(achievements.Count + 1) * cellsInterval - (float)Screen.height * GUIScreenManager.GuiScale;
			break;
		}
		case GUILayouterRotationType.Portrait:
		{
			for (int i = 0; i < achievements.Count; i++)
			{
				GUIAchievement gUIAchievement = achievements[i];
				if (gUIAchievement != null)
				{
					gUIAchievement.transform.localPosition = (portraitOffset + cellsInterval * (float)i - ((float)Screen.height * GUIScreenManager.GuiScale - cellsInterval) / 2f) * Vector3.down;
				}
				gUIAchievement.transform.localPosition += Vector3.down * ((!GameManager.IsIPhoneX) ? 0f : (topBarIPhoneXSize * 4f));
			}
			scrollView.contentLength = portraitOffset + (float)(achievements.Count + 1) * cellsInterval - (float)Screen.height * GUIScreenManager.GuiScale;
			scrollView.contentLength += ((!GameManager.IsIPhoneX) ? 0f : (topBarIPhoneXSize * 4f));
			break;
		}
		}
		scrollView.ResetContentPosition();
	}

	public override void BackButtonPressed()
	{
		BackButton_OnClick();
	}
}
