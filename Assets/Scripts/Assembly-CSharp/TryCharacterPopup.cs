using System;
using System.Collections.Generic;
using UnityEngine.Analytics;

public class TryCharacterPopup : TryCharacterProposalPopup
{
	public override void Hide(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		base.CachedManagers.CachedProposalManager.GenerateProposalMenu(true);
		if (item != null)
		{
			base.ObjectsSpawner.SetFirstSelectedCharacter(item.Infos[0].characterNameType);
		}
		MenuScreen menuScreen = base.CachedManagers.CachedGUIScreenManager.MenuScreen;
		if (menuScreen != null)
		{
			MenuScreenScrollView menuScreenScrollView = menuScreen.MenuScreenScrollView;
			menuScreenScrollView.CheckBoughtCharacters();
			menuScreenScrollView.UpdateScrollViewPosition();
		}
		base.Hide(onFinished, immediately, delay, callInvokes);
	}

	protected override void TryButton_OnClick()
	{
		StatisticsManager cachedStatisticsManager = base.CachedManagers.CachedStatisticsManager;
		Analytics.CustomEvent("Try_Character_Accepted", new Dictionary<string, object>
		{
			{
				"GameTimeMinutes",
				cachedStatisticsManager.TotalGameTime.Minutes
			},
			{ "DaysSinceFirstLaunch", cachedStatisticsManager.DaysSinceFirstLaunch }
		});
		Hide();
	}
}
