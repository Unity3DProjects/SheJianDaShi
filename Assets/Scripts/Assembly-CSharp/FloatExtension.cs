using UnityEngine;

public static class FloatExtension
{
	public static string GetShortNumberValue(this float value)
	{
		int num = 0;
		int num2 = 0;
		float num3 = value;
		while (num3 > 1000f)
		{
			num3 /= 1000f;
			num++;
		}
		if (num > 0 && num % 2 == 0)
		{
			num2 = num / 2;
			num -= num2 * 2;
		}
		string text = ((num == 0 && num2 == 0) ? Mathf.FloorToInt(num3).ToString() : ((!(num3 > 99f)) ? num3.ToString("F1") : num3.ToString("F0")));
		if (num2 > 0)
		{
			for (int i = 0; i < num2; i++)
			{
				text += "M";
			}
		}
		else
		{
			for (int j = 0; j < num; j++)
			{
				text += "K";
			}
		}
		return text;
	}
}
