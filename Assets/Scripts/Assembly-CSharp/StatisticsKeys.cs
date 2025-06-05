using System.Runtime.InteropServices;

[StructLayout((LayoutKind)0, Size = 1)]
public struct StatisticsKeys
{
	[StructLayout((LayoutKind)0, Size = 1)]
	public struct BodyCollisions
	{
		public const string HEAD = "head_collisions";

		public const string BODY = "body_collisions";
	}
}
