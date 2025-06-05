using System.Collections.Generic;
using UnityEngine;

public class StackRandomizer
{
	public int stackSize;

	private readonly List<int> objects = new List<int>();

	private List<int> stack = new List<int>();

	private List<int> copyArray = new List<int>();

	public StackRandomizer(int count, int stackSize)
	{
		int num = 0;
		this.stackSize = ((count > stackSize) ? stackSize : (count - 1));
		for (int i = 0; i < count; i++)
		{
			objects.Add(num);
			num++;
		}
	}

	public StackRandomizer(int startValue, int count, int stackSize)
	{
		this.stackSize = ((count > stackSize) ? stackSize : (count - 1));
		for (int i = 0; i < count; i++)
		{
			objects.Add(startValue);
			startValue++;
		}
	}

	public int GetRandomIndex()
	{
		copyArray.Clear();
		for (int i = 0; i < objects.Count; i++)
		{
			if (!stack.Contains(objects[i]))
			{
				copyArray.Add(objects[i]);
			}
		}
		int num = copyArray[Random.Range(0, copyArray.Count)];
		stack.Add(num);
		if (stack.Count > stackSize)
		{
			stack.RemoveAt(0);
		}
		return num;
	}
}
