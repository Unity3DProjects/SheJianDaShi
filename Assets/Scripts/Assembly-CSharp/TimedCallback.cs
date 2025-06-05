public struct TimedCallback
{
	public TweenCallback callback;

	public float time;

	public TimedCallback(TweenCallback _callback, float _time)
	{
		this = default(TimedCallback);
		callback = _callback;
		time = _time;
	}
}
