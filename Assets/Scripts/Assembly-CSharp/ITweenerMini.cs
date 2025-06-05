public interface ITweenerMini
{
	bool IsBeginStateSet { get; }

	bool IsEndStateSet { get; }

	void SetBeginState(float delay = 0f, TweenCallback del = null);

	void SetEndState(float delay = 0f, TweenCallback del = null);

	void SetBeginStateImmediately();

	void SetEndStateImmediately();
}
