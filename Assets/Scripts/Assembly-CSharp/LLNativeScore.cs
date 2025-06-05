using System;

public struct LLNativeScore
{
	private const string LOG_FORMAT_STRING = "[LLNativeScore] score = {0}, timestampMillis = {1}, displayScore = {2}, playerId = {3}, rank = {4}";

	private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

	public long score;

	public long timestampMillis;

	public string displayScore;

	public string playerId;

	public long rank;

	public DateTime Date
	{
		get
		{
			return UnixEpoch.AddMilliseconds(timestampMillis);
		}
	}

	public override string ToString()
	{
		return string.Format("[LLNativeScore] score = {0}, timestampMillis = {1}, displayScore = {2}, playerId = {3}, rank = {4}", score, timestampMillis, displayScore, playerId, rank);
	}
}
