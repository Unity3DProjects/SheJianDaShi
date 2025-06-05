using UnityEngine;

public static class tk2dUITime
{
	private static float _deltaTime;

	private static readonly float time_30fps = 1f / 30f;

	public static float deltaTime
	{
		get
		{
			return _deltaTime;
		}
	}

	public static void Init()
	{
		_deltaTime = Time.maximumDeltaTime;
	}

	public static void Update()
	{
		if (Time.timeScale < 0.001f)
		{
			_deltaTime = Mathf.Min(time_30fps, Time.unscaledDeltaTime);
		}
		else
		{
			_deltaTime = Time.unscaledDeltaTime;
		}
	}
}
