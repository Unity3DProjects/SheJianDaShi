using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class tk2dColoredText
{
	private abstract class ColorCommand
	{
		public abstract void Apply(tk2dTextGeomGen.InlineStyler styler);

		public static ColorCommand Parse(string input, out int endPosition)
		{
			ColorCommand colorCommand = null;
			int num = 0;
			if (input.Length >= 1)
			{
				char c = input[0];
				if (c >= '0' && c <= '9')
				{
					colorCommand = SetTexUCommand.Parse(c);
					num = 1;
				}
				else
				{
					string input2 = input.Substring(1);
					int endPosition2 = 0;
					switch (c)
					{
					case 'c':
						colorCommand = SetColorCommand.Parse(input2, false, false, out endPosition2);
						break;
					case 'C':
						colorCommand = SetColorCommand.Parse(input2, false, true, out endPosition2);
						break;
					case 'g':
						colorCommand = SetColorCommand.Parse(input2, true, false, out endPosition2);
						break;
					case 'G':
						colorCommand = SetColorCommand.Parse(input2, true, true, out endPosition2);
						break;
					}
					num = 1 + endPosition2;
				}
			}
			if (colorCommand != null)
			{
				endPosition = num;
			}
			else
			{
				endPosition = 0;
			}
			return colorCommand;
		}
	}

	private class SetColorCommand : ColorCommand
	{
		private static readonly Color32 ErrorColor = new Color32(byte.MaxValue, 0, byte.MaxValue, byte.MaxValue);

		private Color32 topColor;

		private Color32 bottomColor;

		public SetColorCommand(Color32 _color)
		{
			topColor = (bottomColor = _color);
		}

		public SetColorCommand(Color32 _topColor, Color32 _bottomColor)
		{
			topColor = _topColor;
			bottomColor = _bottomColor;
		}

		public override void Apply(tk2dTextGeomGen.InlineStyler styler)
		{
			styler.meshTopColor = topColor;
			styler.meshBottomColor = bottomColor;
		}

		public static SetColorCommand Parse(string input, bool twoColors, bool fullHex, out int endPosition)
		{
			int num = ((!twoColors) ? 1 : 2) * ((!fullHex) ? 4 : 8);
			bool flag = false;
			Color32 color = ErrorColor;
			Color32 color2 = ErrorColor;
			if (input.Length >= num)
			{
				if (!TryGetStyleHexColor(input, fullHex, out color))
				{
					flag = true;
				}
				if (twoColors)
				{
					string str = input.Substring((!fullHex) ? 4 : 8);
					if (!TryGetStyleHexColor(str, fullHex, out color2))
					{
						flag = true;
					}
				}
				else
				{
					color2 = color;
				}
			}
			else
			{
				flag = true;
			}
			endPosition = num;
			if (flag)
			{
				return new SetColorCommand(ErrorColor);
			}
			return new SetColorCommand(color, color2);
		}

		private static bool TryGetStyleHexColor(string str, bool fullHex, out Color32 color)
		{
			color = ErrorColor;
			int num;
			int num2;
			int num3;
			int num4;
			if (fullHex)
			{
				if (str.Length < 8)
				{
					return false;
				}
				num = GetFullHexColorComponent(str[0], str[1]);
				num2 = GetFullHexColorComponent(str[2], str[3]);
				num3 = GetFullHexColorComponent(str[4], str[5]);
				num4 = GetFullHexColorComponent(str[6], str[7]);
			}
			else
			{
				if (str.Length < 4)
				{
					return false;
				}
				num = GetCompactHexColorComponent(str[0]);
				num2 = GetCompactHexColorComponent(str[1]);
				num3 = GetCompactHexColorComponent(str[2]);
				num4 = GetCompactHexColorComponent(str[3]);
			}
			if (num == -1 || num2 == -1 || num3 == -1 || num4 == -1)
			{
				return false;
			}
			color = new Color32((byte)num, (byte)num2, (byte)num3, (byte)num4);
			return true;
		}

		private static int GetFullHexColorComponent(int c1, int c2)
		{
			int num = 0;
			if (c1 >= 48 && c1 <= 57)
			{
				num += (c1 - 48) * 16;
			}
			else if (c1 >= 97 && c1 <= 102)
			{
				num += (10 + c1 - 97) * 16;
			}
			else
			{
				if (c1 < 65 || c1 > 70)
				{
					return -1;
				}
				num += (10 + c1 - 65) * 16;
			}
			if (c2 >= 48 && c2 <= 57)
			{
				return num + (c2 - 48);
			}
			if (c2 >= 97 && c2 <= 102)
			{
				return num + (10 + c2 - 97);
			}
			if (c2 >= 65 && c2 <= 70)
			{
				return num + (10 + c2 - 65);
			}
			return -1;
		}

		private static int GetCompactHexColorComponent(int c)
		{
			if (c >= 48 && c <= 57)
			{
				return (c - 48) * 17;
			}
			if (c >= 97 && c <= 102)
			{
				return (10 + c - 97) * 17;
			}
			if (c >= 65 && c <= 70)
			{
				return (10 + c - 65) * 17;
			}
			return -1;
		}
	}

	private class SetTexUCommand : ColorCommand
	{
		private float texU;

		public SetTexUCommand(float _texU)
		{
			texU = _texU;
		}

		public override void Apply(tk2dTextGeomGen.InlineStyler styler)
		{
			styler.meshGradientTexU = texU / (float)((styler.curGradientCount <= 0) ? 1 : styler.curGradientCount);
		}

		public static SetTexUCommand Parse(char inputSymbol)
		{
			return new SetTexUCommand(inputSymbol - 48);
		}
	}

	private class ColorCommandSet
	{
		private Dictionary<int, ColorCommand> colorCommands;

		private Dictionary<int, ColorCommand> ColorCommands
		{
			get
			{
				if (colorCommands == null)
				{
					colorCommands = new Dictionary<int, ColorCommand>();
				}
				return colorCommands;
			}
		}

		public ColorCommand GetCommandAtIndex(int index)
		{
			if (colorCommands != null && ColorCommands.ContainsKey(index))
			{
				return ColorCommands[index];
			}
			return null;
		}

		public void MoveCommandsBy(int startIndex, int offset)
		{
			if (colorCommands == null)
			{
				return;
			}
			Dictionary<int, ColorCommand> dictionary = new Dictionary<int, ColorCommand>(ColorCommands.Count);
			foreach (KeyValuePair<int, ColorCommand> colorCommand in ColorCommands)
			{
				dictionary.Add(colorCommand.Key + ((colorCommand.Key >= startIndex) ? offset : 0), colorCommand.Value);
			}
			colorCommands = dictionary;
		}

		public void Add(int index, ColorCommand command)
		{
			ColorCommands.Remove(index);
			ColorCommands.Add(index, command);
		}

		public void Remove(int index, int count)
		{
			for (int i = index; i < index + count; i++)
			{
				ColorCommands.Remove(i);
			}
		}
	}

	private enum ColorType : byte
	{
		unk = 0,
		rgb = 1,
		hex = 2,
		flt = 3
	}

	private StringBuilder text;

	private ColorCommandSet commandSet;

	public int Length
	{
		get
		{
			return text.Length;
		}
	}

	public char this[int idx]
	{
		get
		{
			return text[idx];
		}
		set
		{
			text = text.Remove(idx, 1);
			text = text.Insert(idx, new string(value, 1));
		}
	}

	public tk2dColoredText(string source, Color defaultColor)
	{
		text = new StringBuilder(RemoveColorCommands(Colorize(source, defaultColor), out commandSet));
	}

	private static string rgb2fmtclr(string[] values)
	{
		int num = int.Parse(values[0]);
		int num2 = int.Parse(values[1]);
		int num3 = int.Parse(values[2]);
		return string.Format("^C{0}{1}{2}FF", num.ToString("X2"), num2.ToString("X2"), num3.ToString("X2"));
	}

	private static string hex2fmtclr(string[] values)
	{
		return string.Format("^C{0}{1}{2}FF", values[0], values[1], values[2]);
	}

	private static string flt2fmtclr(string[] values)
	{
		int num = (int)(double.Parse(values[0]) * 255.0);
		int num2 = (int)(double.Parse(values[1]) * 255.0);
		int num3 = (int)(double.Parse(values[2]) * 255.0);
		string[] values2 = new string[3]
		{
			num.ToString("d"),
			num2.ToString("d"),
			num3.ToString("d")
		};
		return rgb2fmtclr(values2);
	}

	private static string ParceColor(string color)
	{
		ColorType colorType = ColorType.unk;
		int num = color.IndexOf("=") + 1;
		int num2 = color.IndexOf("(");
		switch (color.Substring(num, num2 - num))
		{
		case "rgb":
			colorType = ColorType.rgb;
			break;
		case "hex":
			colorType = ColorType.hex;
			break;
		case "flt":
			colorType = ColorType.flt;
			break;
		}
		int num3 = num2 + 1;
		int num4 = color.IndexOf(")", num3);
		string[] values = color.Substring(num3, num4 - num3).Split(';');
		string result = string.Empty;
		switch (colorType)
		{
		case ColorType.rgb:
			result = rgb2fmtclr(values);
			break;
		case ColorType.hex:
			result = hex2fmtclr(values);
			break;
		case ColorType.flt:
			result = flt2fmtclr(values);
			break;
		}
		return result;
	}

	private static string Colorize(string formattedString, Color defaultColor)
	{
		string[] array = formattedString.Split('<', '>');
		if (array.Length <= 1)
		{
			return formattedString;
		}
		string[] values = new string[3]
		{
			defaultColor.r.ToString("f"),
			defaultColor.g.ToString("f"),
			defaultColor.b.ToString("f")
		};
		string text = flt2fmtclr(values);
		string text2 = text;
		foreach (string text3 in array)
		{
			text2 = ((!text3.StartsWith("color=")) ? ((!text3.Equals("/color")) ? (text2 + text3) : (text2 + text)) : (text2 + ParceColor(text3)));
		}
		return text2;
	}

	private static string RemoveColorCommands(string formattedString, out ColorCommandSet colorCommandSet)
	{
		colorCommandSet = new ColorCommandSet();
		StringBuilder stringBuilder = new StringBuilder(formattedString.Length);
		for (int i = 0; i < formattedString.Length; i++)
		{
			char c = formattedString[i];
			if (c == '^')
			{
				if (i + 1 >= formattedString.Length)
				{
					continue;
				}
				if (formattedString[i + 1] == '^')
				{
					stringBuilder.Append('^');
					i++;
					continue;
				}
				int endPosition = 0;
				ColorCommand colorCommand = ColorCommand.Parse(formattedString.Substring(i + 1), out endPosition);
				if (colorCommand != null)
				{
					colorCommandSet.Add(stringBuilder.Length, colorCommand);
					i += endPosition;
				}
			}
			else
			{
				stringBuilder.Append(c);
			}
		}
		return stringBuilder.ToString();
	}

	public void ApplyColorCommand(tk2dTextGeomGen.InlineStyler styler, int index)
	{
		ColorCommand commandAtIndex = commandSet.GetCommandAtIndex(index);
		if (commandAtIndex != null)
		{
			commandAtIndex.Apply(styler);
		}
	}

	public void Insert(int idx, string toInsert)
	{
		text = text.Insert(idx, toInsert);
		commandSet.MoveCommandsBy(idx, toInsert.Length);
	}

	public void Remove(int startIdx, int count = 1)
	{
		text = text.Remove(startIdx, count);
		commandSet.Remove(startIdx, count);
		commandSet.MoveCommandsBy(startIdx + count, -count);
	}

	public void Append(string strToAppend)
	{
		text.Append(strToAppend);
	}

	public void Replace(char oldChar, char newChar)
	{
		text = text.Replace(oldChar, newChar);
	}

	public int IndexOf(char charToFind)
	{
		return text.ToString().IndexOf(charToFind);
	}

	public int IndexOf(char charToFind, int startIdx)
	{
		return text.ToString().IndexOf(charToFind, startIdx);
	}

	public void ApplyShortNumber()
	{
		string shortNumberForm = text.ToString().GetShortNumberForm();
		text.Length = 0;
		text.Append(shortNumberForm);
	}

	public override string ToString()
	{
		return text.ToString();
	}
}
