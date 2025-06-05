using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AsyncOperationExtension
{
	private static AsyncOperationBehaviour behaviour = null;

	private static List<Coroutine> allCoroutines = new List<Coroutine>();

	public static Coroutine StartCoroutine(this IEnumerator iterator, Action finished = null)
	{
		Initialize();
		Coroutine coroutine = behaviour.StartCoroutine(RunTaskInner(iterator, finished));
		if (coroutine != null)
		{
			allCoroutines.Add(coroutine);
		}
		return coroutine;
	}

	public static Coroutine StartCoroutine(this AsyncOperation task, Action finished = null)
	{
		Initialize();
		Coroutine coroutine = behaviour.StartCoroutine(RunTaskAsyncInner(task, finished));
		if (coroutine != null)
		{
			allCoroutines.Add(coroutine);
		}
		return coroutine;
	}

	public static Coroutine StartCoroutine(this WWW task, Action finished = null)
	{
		Initialize();
		Coroutine coroutine = behaviour.StartCoroutine(RunTaskWWWInner(task, finished));
		if (coroutine != null)
		{
			allCoroutines.Add(coroutine);
		}
		return coroutine;
	}

	public static void StopCoroutine(this Coroutine coroutine)
	{
		if (coroutine != null && (bool)behaviour && allCoroutines.Contains(coroutine))
		{
			allCoroutines.Remove(coroutine);
			behaviour.StopCoroutine(coroutine);
		}
	}

	private static void Initialize()
	{
		if (behaviour == null)
		{
			GameObject gameObject = new GameObject();
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			gameObject.name = "AsyncOperationExtension_Couroutine";
			gameObject.hideFlags = HideFlags.HideAndDontSave;
			behaviour = gameObject.AddComponent<AsyncOperationBehaviour>();
		}
	}

	private static IEnumerator RunTaskInner(IEnumerator task, Action finished = null)
	{
		while (task.MoveNext())
		{
			yield return null;
		}
		if (finished != null)
		{
			finished();
		}
	}

	private static IEnumerator RunTaskAsyncInner(AsyncOperation task, Action finished = null)
	{
		while (!task.isDone)
		{
			yield return null;
		}
		if (finished != null)
		{
			finished();
		}
	}

	private static IEnumerator RunTaskWWWInner(WWW task, Action finished = null)
	{
		while (!task.isDone)
		{
			yield return null;
		}
		if (finished != null)
		{
			finished();
		}
	}
}
