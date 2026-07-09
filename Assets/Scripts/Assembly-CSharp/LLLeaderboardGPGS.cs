using System;
using UnityEngine.SocialPlatforms;

public class LLLeaderboardGPGS : ILeaderboard
{
	public const string DEFAULT_TITLE = "Default";

	public const int DEFAULT_MAX_RANGE = 25;

	public const LLLeaderboardType DEFAULT_LEADERBOARD_TYPE = LLLeaderboardType.CurrentPlayer;

	public const TimeScope DEFAULT_TIME_SCOPE = TimeScope.AllTime;

	private string _id;

	private string _title;

	private uint _maxRange;

	private TimeScope _timeScope;

	private bool _loading;

	private IScore _localUserScore;

	private IScore[] _scores;

	private LLLeaderboardType leaderboardType;

	public LLLeaderboardType LeaderboardType
	{
		get
		{
			return leaderboardType;
		}
		set
		{
			leaderboardType = value;
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

	public string title
	{
		get
		{
			return _title;
		}
	}

	public uint maxRange
	{
		get
		{
			return _maxRange;
		}
	}

	public TimeScope timeScope
	{
		get
		{
			return _timeScope;
		}
		set
		{
			_timeScope = value;
		}
	}

	public bool loading
	{
		get
		{
			return _loading;
		}
	}

	public IScore localUserScore
	{
		get
		{
			return _localUserScore;
		}
	}

	public IScore[] scores
	{
		get
		{
			return _scores;
		}
	}

	public UnityEngine.SocialPlatforms.Range range
	{
		get
		{
			throw new NotImplementedException();
		}
		set
		{
			throw new NotImplementedException();
		}
	}

	public UserScope userScope
	{
		get
		{
			throw new NotImplementedException();
		}
		set
		{
			throw new NotImplementedException();
		}
	}

	public LLLeaderboardGPGS()
	{
		_title = "Default";
		_maxRange = 25u;
		_timeScope = TimeScope.AllTime;
		leaderboardType = LLLeaderboardType.CurrentPlayer;
	}

	public LLLeaderboardGPGS(LLLeaderboardType leaderboardType, TimeScope timeScope)
		: this()
	{
		_timeScope = timeScope;
		this.leaderboardType = leaderboardType;
	}

	public void SetUserFilter(string[] userIDs)
	{
		throw new NotImplementedException();
	}

	public void LoadScores(Action<bool> callback)
	{
		_loading = true;
		Action<IScore[]> callback2 = HandlerUploadedScores(callback);
		LLSocialGPGS.Instance.LoadScores(_id, LeaderboardType, _timeScope, (int)_maxRange, callback2);
	}

	public void LoadLocalUserScore(Action<bool> callback)
	{
		_loading = true;
		Action<IScore[]> callback2 = HandlerUploadedUserScore(callback);
		LLSocialGPGS.Instance.LoadScores(_id, LLLeaderboardType.CurrentPlayer, _timeScope, -1, callback2);
	}

	private Action<IScore[]> HandlerUploadedScores(Action<bool> callback)
	{
		return delegate(IScore[] loadedScores)
		{
			if (loadedScores != null && loadedScores.Length > 0)
			{
				_scores = loadedScores;
				if (leaderboardType == LLLeaderboardType.CurrentPlayer)
				{
					_loading = false;
					if (callback != null)
					{
						callback(true);
					}
				}
				else
				{
					LoadLocalUserScore(callback);
				}
			}
			else
			{
				_loading = false;
				if (callback != null)
				{
					callback(false);
				}
			}
		};
	}

	private Action<IScore[]> HandlerUploadedUserScore(Action<bool> callback)
	{
		return delegate(IScore[] loadedScore)
		{
			_loading = false;
			if (scores != null && scores.Length > 0)
			{
				_localUserScore = loadedScore[0];
				if (callback != null)
				{
					callback(true);
				}
			}
			else if (callback != null)
			{
				callback(false);
			}
		};
	}
}
