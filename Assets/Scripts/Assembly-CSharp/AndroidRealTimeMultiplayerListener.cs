using System;
using System.Text;
// using GooglePlayGames;
// using GooglePlayGames.BasicApi.Multiplayer;
using UnityEngine;

public class AndroidRealTimeMultiplayerListener
{
	private bool showingWaitingRoom;

	private static AndroidRealTimeMultiplayerListener instance;

	private long lastMessageTimeStamp;

	public static AndroidRealTimeMultiplayerListener Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new AndroidRealTimeMultiplayerListener();
			}

			return instance;
		}
	}
}

// private AndroidRealTimeMultiplayerListener()
	// {
	// }

	// void RealTimeMultiplayerListener.OnRoomSetupProgress(float percent)
	// {
	// 	if (!showingWaitingRoom)
	// 	{
	// 		showingWaitingRoom = true;
	// 		PlayGamesPlatform.Instance.RealTime.ShowWaitingRoomUI();
	// 	}
	// }

	// void RealTimeMultiplayerListener.OnRoomConnected(bool success)
	// {
	// 	SingletonMonoBehaviour<GUIPopupManager>.Instance.HidePopup(PopupType.MatchmakingWaiting);
	// 	LLMultiplayerManager.canCancelMatchmaking = false;
	// 	if (success)
	// 	{
	// 		LLMultiplayerManager.Instance.AllOpponentsFound(string.Empty);
	// 		return;
	// 	}
	// 	PlayGamesPlatform.Instance.RealTime.LeaveRoom();
	// 	LLMultiplayerManager.Instance.MatchmakerCancelled(string.Empty);
	// }

	// void RealTimeMultiplayerListener.OnLeftRoom()
	// {
	// }

	// void RealTimeMultiplayerListener.OnParticipantLeft(Participant participant)
	// {
	// }
	//
	// void RealTimeMultiplayerListener.OnPeersConnected(string[] participantIds)
	// {
	// }

// 	void RealTimeMultiplayerListener.OnPeersDisconnected(string[] participantIds)
// 	{
// 		bool value = false;
// 		foreach (string text in participantIds)
// 		{
// 			if (text == Social.localUser.id)
// 			{
// 				value = true;
// 				LLMultiplayerManager.canCancelMatchmaking = false;
// 			}
// 		}
// 		LLMultiplayerManager.Instance.PlayerDisconnected(Convert.ToInt32(value).ToString());
// 	}
//
// 	void RealTimeMultiplayerListener.OnRealTimeMessageReceived(bool isReliable, string senderId, byte[] data)
// 	{
// 		string text = Encoding.UTF8.GetString(data);
// 		if (isReliable)
// 		{
// 			LLMultiplayerManager.Instance.ReceiveMessageFromOpponent(text);
// 		}
// 		else
// 		{
// 			Debug.Log("OnRealTimeMessageReceived IGNORING UNRELIABLE MESSAGE " + text);
// 		}
// 	}
// }
