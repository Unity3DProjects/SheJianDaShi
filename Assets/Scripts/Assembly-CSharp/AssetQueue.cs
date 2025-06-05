using System;
using System.Collections.Generic;

public class AssetQueue
{
	private readonly List<Asset> queue;

	public int Count
	{
		get
		{
			return queue.Count;
		}
	}

	public bool IsEmpty
	{
		get
		{
			return queue.Count == 0;
		}
	}

	public bool IsReady
	{
		get
		{
			return Count > 0 && queue.Peek().IsLoaded;
		}
	}

	public AssetQueue()
	{
		queue = new List<Asset>();
	}

	public AssetQueue(int capacity)
	{
		if (capacity < 0)
		{
			throw new ArgumentOutOfRangeException("capacity");
		}
		queue = new List<Asset>(capacity);
	}

	public void Clear()
	{
		queue.Clear();
	}

	public void Enqueue(Asset asset)
	{
		queue.Enqueue(asset);
	}

	public Asset Dequeue()
	{
		if (IsReady)
		{
			return queue.Dequeue();
		}
		return null;
	}
}
