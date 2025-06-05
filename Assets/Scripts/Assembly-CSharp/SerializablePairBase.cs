using System;

[Serializable]
public abstract class SerializablePairBase<TKey, TValue> where TKey : IComparable
{
	public abstract TKey PairKey { get; set; }

	public abstract TValue PairValue { get; set; }
}
