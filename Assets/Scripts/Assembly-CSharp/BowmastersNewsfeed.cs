using System.Collections.Generic;
using Mit_McServices.Newsfeed;
using Mit_McServices.Newsfeed.Settings;
using UnityEngine;

public class BowmastersNewsfeed : NewsfeedManager
{
	private bool messagesAvailability;

	private uint nrOfMessagesAvailable;

	private bool isNewsfeedShowing;

	public static BowmastersNewsfeed Instance
	{
		get
		{
			return (BowmastersNewsfeed)NewsfeedManager.instance;
		}
	}

	public bool IsNewsfeedShowing
	{
		get
		{
			return isNewsfeedShowing;
		}
	}

	public bool MessagesAvailability
	{
		get
		{
			return messagesAvailability;
		}
	}

	public uint NrOfMessagesAvailable
	{
		get
		{
			return nrOfMessagesAvailable;
		}
	}

	private void Start()
	{
		SetUrgentMessagesDelay(60);
	}

	private void OnEnable()
	{
		GUIOrientationManger.OnDeviceOrientationChanged += GUIOrientationManger_OnDeviceOrientationChanged;
	}

	private void OnDisable()
	{
		GUIOrientationManger.OnDeviceOrientationChanged -= GUIOrientationManger_OnDeviceOrientationChanged;
	}

	public override NewsfeedSettings Game_GetSettings()
	{
		NewsfeedSettings newsfeedSettings = new NewsfeedSettings();
		newsfeedSettings.ApplicationId = Application.identifier;
		newsfeedSettings.IsSandbox = Debug.isDebugBuild;
		newsfeedSettings.HasToShowBadge = false;
		newsfeedSettings.MinimumSessionDuration = 20;
		return newsfeedSettings;
	}

	public override void Game_OnBoardWillAppear()
	{
		Debug.Log("Game_OnBoardWillAppear");
	}

	public override void Game_OnBoardDidAppear()
	{
		Debug.Log("Game_OnBoardDidAppear");
		isNewsfeedShowing = true;
	}

	public override void Game_OnBoardWillDisappear()
	{
		Debug.Log("Game_OnBoardWillDisappear");
	}

	public override void Game_OnBoardDidDisappear()
	{
		Debug.Log("Game_OnBoardDidDisappear");
		isNewsfeedShowing = false;
	}

	public override void Game_OnAvailabilityChanged(uint availability)
	{
		Debug.Log("Game_OnAvailabilityChanged: " + availability);
		messagesAvailability = availability == 1;
		if (GetMenuScreenScript() == null)
		{
			return;
		}
		List<NotificationButton> notificationButtons = GetMenuScreenScript().NotificationButtons;
		if (notificationButtons == null)
		{
			Debug.Log("Failed to get buttons from MenuScreen");
			return;
		}
		foreach (NotificationButton item in notificationButtons)
		{
			item.show(availability == 1);
		}
	}

	public override void Game_OnUnreadMessagesCountChanged(uint currentUnreadMessagesCount)
	{
		Debug.Log("Game_OnUnreadMessagesCountChanged: " + currentUnreadMessagesCount);
		if (GetMenuScreenScript() == null)
		{
			return;
		}
		nrOfMessagesAvailable = currentUnreadMessagesCount;
		List<NotificationButton> notificationButtons = GetMenuScreenScript().NotificationButtons;
		if (notificationButtons == null)
		{
			Debug.Log("Failed to get buttons from MenuScreen");
			return;
		}
		foreach (NotificationButton item in notificationButtons)
		{
			item.setUnreadMessagesCounter(currentUnreadMessagesCount);
		}
	}

	public bool AreThereShowableUrgents()
	{
		return GetUnreadUrgentMessagesCount() > 0 && CanShowUrgentBoard();
	}

	private MenuScreen GetMenuScreenScript()
	{
		GameObject gameObject = GameObject.Find("GUI");
		GameObject gameObject2 = gameObject.transform.Find("ScreenManager").gameObject;
		if (gameObject2 == null)
		{
			return null;
		}
		return gameObject2.GetComponentInChildren<MenuScreen>();
	}

	private void GUIOrientationManger_OnDeviceOrientationChanged(GUILayouterRotationType orientation)
	{
		if (orientation == GUILayouterRotationType.Portrait && base.IsShowingNewsfeed)
		{
			DismissBoard();
		}
	}
}
