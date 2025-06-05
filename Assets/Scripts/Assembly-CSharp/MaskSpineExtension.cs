public static class MaskSpineExtension
{
	public static bool ContainsMasked(this MaskedSpine[] masked, MaskedSpine value)
	{
		bool result = false;
		for (int i = 0; i < masked.Length; i++)
		{
			if (masked[i] == value)
			{
				result = true;
				break;
			}
		}
		return result;
	}
}
