using System.Collections.Generic;
using UnityEngine;

public static class ArrayExtention
{
	public static T RandomObject<T>(this IList<T> list)
	{
		return (list.Count <= 0) ? default(T) : list[Random.Range(0, list.Count)];
	}

	public static T LastObject<T>(this IList<T> list)
	{
		return (list.Count <= 0) ? default(T) : list[list.Count - 1];
	}

	public static T FirstObject<T>(this IList<T> list)
	{
		return (list.Count <= 0) ? default(T) : list[0];
	}

	public static List<T> Diff<T>(IList<T> array1, IList<T> array2)
	{
		List<T> list = new List<T>();
		foreach (T item in array1)
		{
			if (!array2.Contains(item))
			{
				list.Add(item);
			}
		}
		foreach (T item2 in array2)
		{
			if (!array1.Contains(item2))
			{
				list.Add(item2);
			}
		}
		return list;
	}

	public static bool ArrayEqual<T>(this IList<T> array1, IList<T> array2) where T : Object
	{
		if (array2 == null || array1 == null)
		{
			return false;
		}
		bool flag = array1.Count == array2.Count;
		if (flag)
		{
			for (int i = 0; i < array1.Count; i++)
			{
				if (!flag)
				{
					break;
				}
				if ((bool)array1[i])
				{
					flag &= array1[i] == array2[i];
				}
			}
		}
		return flag;
	}

	public static T[] EnsureLength<T>(T[] array, int desiredLength, bool ensureAtLeast = false)
	{
		if (array == null || ((!ensureAtLeast) ? (array.Length != desiredLength) : (array.Length < desiredLength)))
		{
			return new T[desiredLength];
		}
		return array;
	}

	public static bool ArrayContains<T>(this T[] array, T value) where T : Object
	{
		bool flag = array != null && value != null;
		if (flag)
		{
			flag = false;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == value)
				{
					flag = true;
					break;
				}
			}
		}
		return flag;
	}
}
