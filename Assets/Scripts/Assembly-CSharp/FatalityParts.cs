public enum FatalityParts
{
	None = 0,
	Head = 1,
	Arms = 2,
	Legs = 4,
	Body = 8,
	AllExceptBody = 7,
	All = 15
}

public static class GoreContentFilter
{
	public static readonly bool AllowDismemberment = false;

	public static readonly bool AllowInternalOrgans = false;

	public static readonly bool AllowCutMasks = false;
}
