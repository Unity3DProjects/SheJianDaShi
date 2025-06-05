using UnityEngine;

public static class PathUtils
{
	public const string CSVExtention = "csv";

	public const string PNGExtention = "png";

	public const string CAFExtention = "caf";

	public const char CHAR_SLASH = '/';

	public const char CHAR_DOT = '.';

	public const string WWW_FILE_PREFIX = "file://";

	public const string WWW_JAR_PREFIX = "jar:";

	private const string FORMAT_PATH_ADDITION = "{0}{1}";

	private const string FORMAT_PATH_COMPONENT = "{0}/{1}";

	private const string FORMAT_PATH_EXTENTION = "{0}.{1}";

	private const string FORMAT_PATH_BASHSHELL = "\"{0}\"";

	public static string GetDataRoot()
	{
		string streamingAssetsPath = Application.streamingAssetsPath;
		return Application.streamingAssetsPath;
	}

	public static string GetStreamingAssetsFile(string fileName, string extention = null)
	{
		string streamingAssetsPath = Application.streamingAssetsPath;
		streamingAssetsPath = streamingAssetsPath.AppendPathComponent(fileName);
		if (extention != null)
		{
			streamingAssetsPath = streamingAssetsPath.AppendPathExtention(extention);
		}
		return streamingAssetsPath;
	}

	public static string GetStreamingAssetsCSV(string fileName)
	{
		return GetStreamingAssetsFile(fileName, "csv");
	}

	public static string GetStreamingAssetsPNG(string fileName)
	{
		return GetStreamingAssetsFile(fileName, "png");
	}

	public static string GetStreamingAssetsCAF(string fileName)
	{
		return GetStreamingAssetsFile(fileName, "caf");
	}

	public static string AppendPathComponent(this string str, string pathComponent)
	{
		return string.Format("{0}/{1}", str, pathComponent);
	}

	public static string AppendPathExtention(this string str, string pathExtention)
	{
		return string.Format("{0}.{1}", str, pathExtention);
	}

	public static string AppendWWWFilePrefix(this string str)
	{
		if (!str.Contains("file://"))
		{
			str = string.Format("{0}{1}", "file://", str);
		}
		return str;
	}

	public static string AppendWWWJARPrefix(this string str)
	{
		if (!str.Contains("jar:"))
		{
			str = string.Format("{0}{1}", "jar:", str);
		}
		return str;
	}

	public static string RemoveLastPathComponent(this string str)
	{
		if (str.Length >= 2)
		{
			int num = str.LastIndexOf('/', str.Length - 2);
			if (num > 0)
			{
				return str.Remove(num);
			}
		}
		return string.Empty;
	}

	public static string RemovePathExtention(this string str)
	{
		if (str.Length >= 2)
		{
			int num = str.LastIndexOf('.', str.Length - 2);
			if (num > 0)
			{
				return str.Remove(num);
			}
		}
		return string.Empty;
	}

	public static string ShellString(this string str)
	{
		return string.Format("\"{0}\"", str);
	}
}
