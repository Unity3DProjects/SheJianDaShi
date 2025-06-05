using UnityEngine;

public class Decal : MonoBehaviour
{
	[SerializeField]
	private MaskedSpine maskedFill;

	[SerializeField]
	private MaskedSpine maskedCut;

	[SerializeField]
	private DecalType decalType;

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

	public MaskedSpine MaskedFill
	{
		get
		{
			return maskedFill;
		}
	}

	public MaskedSpine MaskedCut
	{
		get
		{
			return maskedCut;
		}
	}

	public DecalType DecalType
	{
		get
		{
			return decalType;
		}
	}

	public bool IsCut
	{
		get
		{
			return maskedCut != null;
		}
	}

	public Bounds GetBounds()
	{
		Bounds result = default(Bounds);
		if ((bool)maskedFill)
		{
			result.Encapsulate(maskedFill.GetBounds());
		}
		if ((bool)maskedCut)
		{
			result.Encapsulate(maskedCut.GetBounds());
		}
		return result;
	}

	public void InitDecal()
	{
		if ((bool)maskedFill && (bool)maskedFill.CachedSprite && (bool)maskedFill.CachedSprite.Collection)
		{
			maskedFill.CachedSprite.Collection.Init();
		}
		if ((bool)maskedCut && (bool)maskedCut.CachedSprite && (bool)maskedCut.CachedSprite.Collection)
		{
			maskedCut.CachedSprite.Collection.Init();
		}
	}
}
