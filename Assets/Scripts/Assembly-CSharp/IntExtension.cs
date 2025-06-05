public static class IntExtension
{
	public static int RoundInt(this int i)
	{
		if (i > 10)
		{
			int num = i % 10;
			int num2 = i / 10;
			return num2 * 10 + ((num >= 5) ? 10 : 0);
		}
		return i;
	}
}
