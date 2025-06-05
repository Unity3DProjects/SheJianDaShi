using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class MicroachievementsManager : SingletonMonoBehaviour<MicroachievementsManager>
{
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<Queue<MicroachievementInfo>> m_OnReachedAchievement;

	[SerializeField]
	private MicroachievementInfo[] achievements;

	private ArrayList reachedLevelAchievements = new ArrayList();

	private Queue<MicroachievementInfo> queueAhievements = new Queue<MicroachievementInfo>();

	public Queue<MicroachievementInfo> QueueAhievements
	{
		get
		{
			return queueAhievements;
		}
	}

	public int CoinsFromAchievementsQueue
	{
		get
		{
			int num = 0;
			foreach (MicroachievementInfo queueAhievement in queueAhievements)
			{
				num += queueAhievement.coinsAward;
			}
			if (SingletonMonoBehaviour<StatisticsManager>.Instance.DoubleCoins)
			{
				num *= 2;
			}
			return num;
		}
	}

	public static event Action<Queue<MicroachievementInfo>> OnReachedAchievement
	{
		add
		{
			Action<Queue<MicroachievementInfo>> action = MicroachievementsManager.m_OnReachedAchievement;
			Action<Queue<MicroachievementInfo>> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MicroachievementsManager.m_OnReachedAchievement, (Action<Queue<MicroachievementInfo>>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<Queue<MicroachievementInfo>> action = MicroachievementsManager.m_OnReachedAchievement;
			Action<Queue<MicroachievementInfo>> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MicroachievementsManager.m_OnReachedAchievement, (Action<Queue<MicroachievementInfo>>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private void OnEnable()
	{
		CustomGUI.OnDebugGUI += CustomGUI_OnDebugGUI;
	}

	private void OnDisable()
	{
		CustomGUI.OnDebugGUI -= CustomGUI_OnDebugGUI;
	}

	public void ResetReachedLevelAchievements()
	{
		reachedLevelAchievements.Clear();
	}

	public void CheckPackAchievementsReaching(ShopItemCharacter restoredCharacter)
	{
		List<ShopItemPack> availablePacks = SingletonMonoBehaviour<ShopManager>.Instance.AvailablePacks;
		foreach (ShopItemPack item in availablePacks)
		{
			bool flag = true;
			ShopItemCharacter[] characters = item.Characters;
			foreach (ShopItemCharacter shopItemCharacter in characters)
			{
				flag &= shopItemCharacter.Bought;
			}
			if (flag)
			{
				MicroachievementInfo achievement = InfoByType(item.AchievementType);
				achievement.isRestoredAchievement = restoredCharacter != null;
				ReachLevelAchievement(achievement);
			}
		}
	}

	public void CheckLevelAchievementReaching(MicroachievementType type, int value = 0, ShopItemCharacter restoredCharacter = null)
	{
		Level currentLevel = SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel;
		if (!(currentLevel == null) && (currentLevel.ActiveCharacter.IsAI || currentLevel.ActiveCharacter.IsMultiplayerEnemy || currentLevel.GameType == GameType.VSHuman || currentLevel.GameType == GameType.Multiplayer))
		{
			return;
		}
		MicroachievementInfo achievement = InfoByType(type);
		achievement.isRestoredAchievement = restoredCharacter != null;
		if (achievement.type == MicroachievementType.None)
		{
			return;
		}
		bool flag = value == 0;
		int currentValue = value;
		if (!flag)
		{
			for (int i = 0; i < achievement.targetValues.Length; i++)
			{
				int num = achievement.targetValues[i];
				if (value >= num)
				{
					currentValue = num;
					flag = true;
				}
			}
		}
		if (flag)
		{
			achievement.currentValue = currentValue;
			ReachLevelAchievement(achievement);
		}
	}

	public void ReachLevelAchievement(MicroachievementInfo achievement)
	{
		ReachAchievement(achievement, reachedLevelAchievements);
	}

	public MicroachievementInfo InfoByType(MicroachievementType type)
	{
		MicroachievementInfo[] array = achievements;
		for (int i = 0; i < array.Length; i++)
		{
			MicroachievementInfo result = array[i];
			if (result.type == type)
			{
				return result;
			}
		}
		return default(MicroachievementInfo);
	}

	private void ReachAchievement(MicroachievementInfo achievement, ArrayList array)
	{
		bool flag = false;
		if (achievement.isRepeating)
		{
			bool flag2 = false;
			for (int num = array.Count - 1; num >= 0; num--)
			{
				MicroachievementInfo microachievementInfo = (MicroachievementInfo)array[num];
				if (microachievementInfo.type == achievement.type)
				{
					flag2 = true;
					if (achievement.currentValue > microachievementInfo.currentValue || microachievementInfo.isEveryTimeAchieved)
					{
						array.RemoveAt(num);
						flag2 = false;
					}
					break;
				}
			}
			array.Add(achievement);
			flag = !flag2;
		}
		else if (!achievement.IsSavedAchievement() || achievement.currentValue > achievement.ReachedValue)
		{
			flag = true;
		}
		if (flag)
		{
			achievement.SaveAchievement();
			Notificate(achievement);
			if (achievement.achievementPostfix != string.Empty)
			{
				int num2 = achievement.AchievementLevel + 1;
				string postfix = ((achievement.targetValues.Length <= 0) ? achievement.achievementPostfix : (achievement.achievementPostfix + num2));
				SingletonMonoBehaviour<GameCenterManager>.Instance.ReportAchievementProgress(postfix);
			}
		}
	}

	private void Notificate(MicroachievementInfo info)
	{
		queueAhievements.Enqueue(info);
		MicroachievementsManager.m_OnReachedAchievement?.Invoke(queueAhievements);
	}

	private void CustomGUI_OnDebugGUI()
	{
		if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height - 50, 50f, 50f), "ach"))
		{
			for (int i = 0; i < 33; i++)
			{
				CheckLevelAchievementReaching((MicroachievementType)i, 20000);
				CheckLevelAchievementReaching((MicroachievementType)i);
			}
		}
	}
}
