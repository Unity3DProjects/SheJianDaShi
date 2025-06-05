using System;
using System.Collections.Generic;

public class CallbackCache<T>
{
	private Dictionary<string, List<Action<T>>> callbacks = new Dictionary<string, List<Action<T>>>();

	public void AddCallback(string key, Action<T> callback)
	{
		List<Action<T>> value;
		if (!callbacks.TryGetValue(key, out value))
		{
			value = new List<Action<T>>();
			callbacks.Add(key, value);
		}
		if (!value.Contains(callback))
		{
			value.Add(callback);
		}
	}

	public bool Call(string key, T arg)
	{
		List<Action<T>> value;
		if (callbacks.TryGetValue(key, out value))
		{
			foreach (Action<T> item in value)
			{
				item(arg);
			}
			callbacks.Remove(key);
			return true;
		}
		return false;
	}

	public bool DoesCallersExist(string key)
	{
		return callbacks.ContainsKey(key);
	}

	public void RemoveAll(string key)
	{
		callbacks.Remove(key);
	}
}
