using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableDictionary<TPair, TKey, TValue> where TPair : SerializablePairBase<TKey, TValue>, new() where TKey : IComparable
{
	[SerializeField]
	private TPair[] pairData;

	private Dictionary<TKey, TValue> cachedDictionary;

	private Dictionary<TKey, TValue> CachedDictionary
	{
		get
		{
			if (cachedDictionary == null)
			{
				cachedDictionary = new Dictionary<TKey, TValue>(Count);
				if (pairData != null)
				{
					TPair[] array = pairData;
					for (int i = 0; i < array.Length; i++)
					{
						TPair val = array[i];
						if (cachedDictionary.ContainsKey(val.PairKey))
						{
							CustomDebug.LogError("Duplicate keys in serializable dicitonary");
						}
						cachedDictionary[val.PairKey] = val.PairValue;
					}
				}
			}
			return cachedDictionary;
		}
	}

	public Dictionary<TKey, TValue>.KeyCollection Keys
	{
		get
		{
			return CachedDictionary.Keys;
		}
	}

	public int Count
	{
		get
		{
			return pairData.Length;
		}
	}

	private bool TryGetIndexOfKey(TKey _key, out int index)
	{
		bool result = false;
		index = -1;
		for (int i = 0; i < Count; i++)
		{
			if (_key.Equals(pairData[i].PairKey))
			{
				index = i;
				result = true;
			}
		}
		return result;
	}

	public bool TryGetValue(TKey _key, out TValue outValue)
	{
		return CachedDictionary.TryGetValue(_key, out outValue);
	}
}
