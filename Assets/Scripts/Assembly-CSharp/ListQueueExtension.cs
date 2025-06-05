using System.Collections.Generic;

public static class ListQueueExtension
{
	public static T Dequeue<T>(this List<T> list)
	{
		T result = list[0];
		list.RemoveAt(0);
		return result;
	}

	public static void Enqueue<T>(this List<T> list, T obj)
	{
		list.Add(obj);
	}

	public static T Peek<T>(this List<T> list)
	{
		return list[0];
	}
}
