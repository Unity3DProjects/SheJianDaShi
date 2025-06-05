using System;
using UnityEngine.SocialPlatforms;

public class LLScoreGPGS : IScore
{
	private string _leaderboardID;

	private long _value;

	private DateTime _date;

	private string _formattedValue;

	private string _userID;

	private int _rank;

	public string leaderboardID
	{
		get
		{
			return _leaderboardID;
		}
		set
		{
			_leaderboardID = value;
		}
	}

	public long value
	{
		get
		{
			return _value;
		}
		set
		{
			_value = value;
		}
	}

	public DateTime date
	{
		get
		{
			return _date;
		}
	}

	public string formattedValue
	{
		get
		{
			return _formattedValue;
		}
	}

	public string userID
	{
		get
		{
			return _userID;
		}
	}

	public int rank
	{
		get
		{
			return _rank;
		}
	}

	public LLScoreGPGS(string leaderboardId, LLNativeScore score)
	{
		_leaderboardID = leaderboardId;
		_value = score.score;
		_date = score.Date;
		_formattedValue = score.displayScore;
		_userID = score.playerId;
		_rank = (int)score.rank;
	}

	public void ReportScore(Action<bool> callback)
	{
		LLSocialGPGS.Instance.ReportScore(_value, _leaderboardID, callback);
	}
}
