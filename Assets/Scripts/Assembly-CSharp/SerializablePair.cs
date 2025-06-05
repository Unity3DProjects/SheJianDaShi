using System;
using UnityEngine;

[Serializable]
public class SerializablePair<TKey, TValue> : SerializablePairBase<TKey, TValue> where TKey : IComparable
{
	[SerializeField]
	private TKey pairKey;

	[SerializeField]
	private TValue pairValue;

	public override TKey PairKey
	{
		get
		{
			return pairKey;
		}
		set
		{
			pairKey = value;
		}
	}

	public override TValue PairValue
	{
		get
		{
			return pairValue;
		}
		set
		{
			pairValue = value;
		}
	}
}
