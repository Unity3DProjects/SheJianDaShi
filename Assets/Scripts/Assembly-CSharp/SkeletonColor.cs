using UnityEngine;

public class SkeletonColor : MonoBehaviour, ITweenColor
{
	private SkeletonAnimation cachedSkeleton;

	private SkeletonAnimation CachedSkeleton
	{
		get
		{
			if (cachedSkeleton == null)
			{
				cachedSkeleton = GetComponent<SkeletonAnimation>();
			}
			return cachedSkeleton;
		}
	}

	public Color Color
	{
		get
		{
			Color white = Color.white;
			if (CachedSkeleton != null && CachedSkeleton.Skeleton != null)
			{
				white.a = CachedSkeleton.Skeleton.A;
				white.r = CachedSkeleton.Skeleton.R;
				white.b = CachedSkeleton.Skeleton.B;
				white.g = CachedSkeleton.Skeleton.G;
			}
			return white;
		}
		set
		{
			if (CachedSkeleton != null && CachedSkeleton.Skeleton != null)
			{
				CachedSkeleton.Skeleton.SetColor(value);
			}
		}
	}
}
