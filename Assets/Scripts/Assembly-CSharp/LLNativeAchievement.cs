using System;

public class LLNativeAchievement
{
	private const string LOG_FORMAT_STRING = "[LLNativeAchievement] id={0}, name={1}, desc={2}, xpValue={3}, state={4}, type={5}, lastUpdatedTimestamp={6}, steps={7}/{8}, revealedImageUri={9}, unlockedImageUri={10}";

	private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

	public string id;

	public string name;

	public string description;

	public long xpValue;

	public LLAchievementState state;

	public LLAchievementType type;

	public long lastUpdatedTimestamp;

	public int currentSteps;

	public int totalSteps;

	public string revealedImageUri;

	public string unlockedImageUri;

	public DateTime LastReportedDate
	{
		get
		{
			return UnixEpoch.AddMilliseconds(lastUpdatedTimestamp);
		}
	}

	public override string ToString()
	{
		return string.Format("[LLNativeAchievement] id={0}, name={1}, desc={2}, xpValue={3}, state={4}, type={5}, lastUpdatedTimestamp={6}, steps={7}/{8}, revealedImageUri={9}, unlockedImageUri={10}", id, name, description, xpValue, state, type, lastUpdatedTimestamp, currentSteps, totalSteps, revealedImageUri, unlockedImageUri);
	}
}
