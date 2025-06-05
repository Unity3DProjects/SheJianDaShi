using System;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class LLAchievementGPGS : IAchievementDescription, IAchievement
{
	private string _id;

	private double _percentCompleted;

	private DateTime _lastReportedDate;

	private string _title;

	private string _achievedDescription;

	private int _points;

	private Texture2D _image;

	private int currentSteps;

	private int totalSteps;

	private LLAchievementState state;

	private LLAchievementType type;

	public bool IsIncremental
	{
		get
		{
			return type == LLAchievementType.Incremental;
		}
	}

	public string id
	{
		get
		{
			return _id;
		}
		set
		{
			_id = value;
		}
	}

	public bool hidden
	{
		get
		{
			return state == LLAchievementState.Hidden;
		}
	}

	public string title
	{
		get
		{
			return _title;
		}
	}

	public string achievedDescription
	{
		get
		{
			return _achievedDescription;
		}
	}

	public string unachievedDescription
	{
		get
		{
			return _achievedDescription;
		}
	}

	public int points
	{
		get
		{
			return _points;
		}
	}

	public Texture2D image
	{
		get
		{
			return _image;
		}
	}

	public double percentCompleted
	{
		get
		{
			return _percentCompleted;
		}
		set
		{
			_percentCompleted = value;
		}
	}

	public bool completed
	{
		get
		{
			return state == LLAchievementState.Unlocked;
		}
	}

	public DateTime lastReportedDate
	{
		get
		{
			return _lastReportedDate;
		}
	}

	public LLAchievementGPGS()
	{
	}

	public LLAchievementGPGS(LLNativeAchievement achievement)
	{
		state = achievement.state;
		type = achievement.type;
		currentSteps = achievement.currentSteps;
		totalSteps = achievement.totalSteps;
		_id = achievement.id;
		_percentCompleted = CalculatePercentCompleted();
		_lastReportedDate = achievement.LastReportedDate;
		_title = achievement.name;
		_achievedDescription = achievement.description;
		_points = (int)achievement.xpValue;
		_image = null;
	}

	public void ReportProgress(Action<bool> callback)
	{
		if (type == LLAchievementType.Standart)
		{
			LLSocialGPGS.Instance.ReportProgress(_id, _percentCompleted, callback);
			return;
		}
		throw new NotImplementedException();
	}

	private double CalculatePercentCompleted()
	{
		if (IsIncremental)
		{
			if (totalSteps > 0)
			{
				return (double)currentSteps / (double)totalSteps * 100.0;
			}
			return 0.0;
		}
		return (!completed) ? 0.0 : 100.0;
	}
}
