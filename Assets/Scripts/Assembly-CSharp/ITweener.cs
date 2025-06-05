public interface ITweener : ITweenerMini
{
	string TargetName { get; }

	bool IsRun { get; }

	void AddOnFinishedDelegate(TweenCallback del);

	void RemoveOnFinishedDelegate(TweenCallback del);

	void SetOnFinishedDelegate(TweenCallback del);
}
