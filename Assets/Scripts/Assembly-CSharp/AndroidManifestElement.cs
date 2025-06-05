using System;
using System.Text;
using UnityEngine;

[Serializable]
public class AndroidManifestElement
{
	public enum Parent
	{
		Manifest = 0,
		Application = 1,
		MainActivity = 2
	}

	private const string UNICODE_FORMAT = "\\u{0:X4}";

	public Parent parent;

	[TextArea(2, 8)]
	public string contentFormat;

	private string content;

	public string Content
	{
		get
		{
			if (string.IsNullOrEmpty(content))
			{
				return contentFormat;
			}
			return content;
		}
	}

	public void SetValues(params string[] values)
	{
		if (values == null || values.Length == 0)
		{
			CustomDebug.Log("Values is null");
			return;
		}
		for (int i = 0; i < values.Length; i++)
		{
			if (IsDigitsOnly(values[i]))
			{
				values[i] = ConvertToUnicode(values[i]);
			}
		}
		content = string.Format(contentFormat, values);
	}

	private string ConvertToUnicode(string value)
	{
		string text = string.Empty;
		byte[] bytes = Encoding.Unicode.GetBytes(value);
		for (int i = 0; i < bytes.Length; i++)
		{
			if (bytes[i] != 0)
			{
				text += string.Format("\\u{0:X4}", bytes[i]);
			}
		}
		return text;
	}

	private bool IsDigitsOnly(string str)
	{
		foreach (char c in str)
		{
			if (c < '0' || c > '9')
			{
				return false;
			}
		}
		return true;
	}
}
