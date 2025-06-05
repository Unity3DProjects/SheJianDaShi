using UnityEngine;

public abstract class ScriptableSingleton<T> : ScriptableObject where T : ScriptableSingleton<T>
{
	private static T cachedInstance;

	protected static string FileName
	{
		get
		{
			return typeof(T).Name;
		}
	}

	public static T Instance
	{
		get
		{
			if (cachedInstance == null)
			{
				cachedInstance = Resources.Load(FileName) as T;
			}
			if (cachedInstance == null)
			{
				CustomDebug.LogWarning("No instance of " + FileName + " found, using default values");
				cachedInstance = ScriptableObject.CreateInstance<T>();
			}
			return cachedInstance;
		}
	}

	public static bool DoesInstanceExist
	{
		get
		{
			if (cachedInstance == null)
			{
				cachedInstance = Resources.Load(FileName) as T;
			}
			return cachedInstance != null;
		}
	}
}
