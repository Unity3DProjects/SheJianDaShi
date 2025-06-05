using System;

[Serializable]
public struct MicroachievementInfo
{
	private const string ACHIEVEMENT_KEY = "achievemnt_";

	private const string VALUE_KEY = "_value";

	private const string COUNT_KEY = "_key";

	private const string ACHIEVEMENT_AWARD_PREFIX = "+";

	public MicroachievementType type;

	public string name;

	public int coinsAward;

	public int gemsAward;

	public int priority;

	public bool isRepeating;

	public int currentValue;

	public int[] targetValues;

	public string localisationName;

	public string localisationDescription;

	public string[] iconSpriteName;

	public bool isEveryTimeAchieved;

	public string achievementPostfix;

	public bool isRestoredAchievement;

	public int ReachedCount
	{
		get
		{
			return CustomPlayerPrefs.GetInt("achievemnt_" + type.ToString() + "_key", 0);
		}
		set
		{
			CustomPlayerPrefs.SetInt("achievemnt_" + type.ToString() + "_key", value);
		}
	}

	public int ReachedValue
	{
		get
		{
			return CustomPlayerPrefs.GetInt("achievemnt_" + type.ToString() + "_value", 0);
		}
		set
		{
			CustomPlayerPrefs.SetInt("achievemnt_" + type.ToString() + "_value", value);
		}
	}

	public int AchievementLevel
	{
		get
		{
			int result = 0;
			for (int i = 0; i < targetValues.Length; i++)
			{
				if (currentValue >= targetValues[i])
				{
					result = i;
				}
			}
			return result;
		}
	}

	public string CurrentSpriteName()
	{
		return iconSpriteName[AchievementLevel];
	}

	public bool IsSavedAchievement()
	{
		return ReachedCount > 0;
	}

	public void SaveAchievement()
	{
		ReachedValue = currentValue;
		if (isRepeating)
		{
			ReachedCount++;
		}
		else
		{
			ReachedCount = 1;
		}
	}

	public string AwardString()
	{
		int num = coinsAward;
		if (SingletonMonoBehaviour<ProposalManager>.Instance.DoubleCoins != null && SingletonMonoBehaviour<ProposalManager>.Instance.DoubleCoins.Bought)
		{
			num *= 2;
		}
		return string.Format("{0}{1}", "+", num.ToString());
	}

	public string LocalisationName(int curAchievementLevel = 0)
	{
		string text = SingletonMonoBehaviour<LocalisationManager>.Instance.GetTextByKey(localisationName);
		if (targetValues.Length > 0)
		{
			text = text + " " + GUIAchievement.ToRoman((curAchievementLevel != 0) ? curAchievementLevel : (AchievementLevel + 1));
		}
		return text;
	}
}
