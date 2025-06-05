public sealed class LLPromoFetcher
{
	private static bool isInitialized;

	public static void Initialize()
	{
		if (!isInitialized)
		{
			isInitialized = true;
		}
	}

	public static LLPromoFetcherUnit PromoFetcherActiveUnit()
	{
		return default(LLPromoFetcherUnit);
	}

	public static void visitActiveUnit(LLPromoVisitType visitType)
	{
	}
}
