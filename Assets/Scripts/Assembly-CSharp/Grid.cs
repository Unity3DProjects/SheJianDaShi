using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

[ExecuteInEditMode]
public class Grid : MonoBehaviour
{
	public enum Arrangement
	{
		Horizontal = 0,
		Vertical = 1
	}

	public Arrangement arrangement;

	public int maxPerLine;

	public float cellWidth = 200f;

	public float cellHeight = 200f;

	public bool repositionNow;

	public bool sorted;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action m_OnRepositionEnded;

	private Transform cachedTransform;

	public Transform CachedTransform
	{
		get
		{
			if (cachedTransform == null)
			{
				cachedTransform = base.transform;
			}
			return cachedTransform;
		}
	}

	public event Action OnRepositionEnded
	{
		add
		{
			Action action = this.m_OnRepositionEnded;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnRepositionEnded, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = this.m_OnRepositionEnded;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnRepositionEnded, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private void Start()
	{
		Reposition();
	}

	private void Update()
	{
		if (repositionNow)
		{
			repositionNow = false;
			Reposition();
		}
	}

	public Transform[] GetChilds()
	{
		List<Transform> list = new List<Transform>();
		foreach (Transform item in CachedTransform)
		{
			list.Add(item);
		}
		if (sorted)
		{
			list.Sort((Transform a, Transform b) => string.Compare(a.name, b.name));
		}
		return list.ToArray();
	}

	public void Add(Transform newChild)
	{
		newChild.parent = CachedTransform;
	}

	public void Remove(Transform newChild)
	{
		newChild.parent = null;
	}

	public void Reposition()
	{
		int num = 0;
		int num2 = 0;
		Transform[] childs = GetChilds();
		for (int i = 0; i < childs.Length; i++)
		{
			Vector3 localPosition = ((arrangement != Arrangement.Horizontal) ? new Vector3(cellWidth * (float)num2, (0f - cellHeight) * (float)num, 0f) : new Vector3(cellWidth * (float)num, (0f - cellHeight) * (float)num2, 0f));
			childs[i].localPosition = localPosition;
			if (++num >= maxPerLine && maxPerLine > 0)
			{
				num = 0;
				num2++;
			}
		}
		if (this.OnRepositionEnded != null)
		{
			this.OnRepositionEnded();
		}
	}
}
