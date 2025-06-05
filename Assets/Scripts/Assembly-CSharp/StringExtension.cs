using System.Text;

public static class StringExtension
{
	public static char ASCII_NL = '\n';

	public static char ASCII_WS = ' ';

	public static string GetShortNumberForm(this string s)
	{
		StringBuilder stringBuilder = new StringBuilder(s);
		int num = 0;
		for (int num2 = stringBuilder.Length - 1; num2 >= 0; num2--)
		{
			bool flag = false;
			char c = stringBuilder[num2];
			if (c >= '0' && c <= '9')
			{
				flag = true;
				num++;
			}
			if (!flag || (flag && num2 == 0))
			{
				if (num > 3)
				{
					int num3 = ((!flag) ? (num2 + 1) : num2);
					int num4 = ((num <= 6) ? 3 : 6);
					string value = ((num4 != 6) ? "k" : "M");
					int num5 = num3 + num;
					int num6 = num5 - num4;
					int num7 = num6;
					if (stringBuilder[num7] != '0' && num - num4 <= 2)
					{
						stringBuilder.Insert(num6, ".");
						num7 += 2;
						num4--;
					}
					stringBuilder.Remove(num7, num4);
					stringBuilder.Insert(num7, value);
				}
				num = 0;
			}
		}
		return stringBuilder.ToString();
	}

	public static string RemoveCharsControl(this string s)
	{
		if (s != null)
		{
			int length = s.Length;
			char[] array = s.ToCharArray();
			int length2 = 0;
			for (int i = 0; i < length; i++)
			{
				char c = array[i];
				if (c >= ASCII_WS || c == ASCII_NL)
				{
					array[length2++] = c;
				}
			}
			s = new string(array, 0, length2);
		}
		return s;
	}

	public static string RemoveCharsNewline(this string s)
	{
		return s.RemoveChars(ASCII_NL);
	}

	public static string RemoveCharsWhiteSpace(this string s)
	{
		return s.RemoveChars(ASCII_WS);
	}

	public static string RemoveChars(this string s, char removeChar)
	{
		if (s != null)
		{
			int length = s.Length;
			char[] array = s.ToCharArray();
			int length2 = 0;
			for (int i = 0; i < length; i++)
			{
				char c = array[i];
				if (c != removeChar)
				{
					array[length2++] = c;
				}
			}
			s = new string(array, 0, length2);
		}
		return s;
	}
}
