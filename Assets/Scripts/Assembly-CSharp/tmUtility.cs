using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class tmUtility
{
	public static string PlatformlessPath(string path)
	{
		if (!string.IsNullOrEmpty(path))
		{
			tmPlatform[] allPlatfrorms = tmSettings.allPlatfrorms;
			foreach (tmPlatform tmPlatform2 in allPlatfrorms)
			{
				path = path.Replace(tmPlatform2.postfix, string.Empty);
			}
		}
		return path;
	}

	public static string PathForPlatform(string path, tmPlatform platform)
	{
		if (!string.IsNullOrEmpty(path))
		{
			string text = Path.GetDirectoryName(path);
			if (!string.IsNullOrEmpty(text))
			{
				text += "/";
			}
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
			string extension = Path.GetExtension(path);
			fileNameWithoutExtension = PlatformlessPath(fileNameWithoutExtension);
			path = text + fileNameWithoutExtension + platform.postfix + extension;
		}
		return path;
	}

	public static string NewGUID()
	{
		return Guid.NewGuid().ToString().Replace("-", string.Empty);
	}

	public static tmResourceCollectionLink ResourceLinkByGUID(string guid)
	{
		string path = "tmResources/tm" + guid;
		return Resources.Load(path, typeof(tmResourceCollectionLink)) as tmResourceCollectionLink;
	}

	public static List<tmResourceCollectionLink> GetAllResourceLinks(tmPlatform platform)
	{
		tmResourceCollectionLink[] array = Resources.LoadAll<tmResourceCollectionLink>("tmResources/");
		HashSet<string> hashSet = new HashSet<string>();
		for (int i = 0; i < array.Length; i++)
		{
			string item = PlatformlessPath(array[i].name).Replace("tm", string.Empty);
			hashSet.Add(item);
		}
		List<tmResourceCollectionLink> list = new List<tmResourceCollectionLink>();
		foreach (string item2 in hashSet)
		{
			list.Add(ResourceLinkByGUID(item2 + platform.postfix));
		}
		return list;
	}

	public static List<tmResourceCollectionLink> GetAllResourceLinksFor(tmPlatform platform, List<tmResourceCollectionLink> _links)
	{
		HashSet<string> hashSet = new HashSet<string>();
		for (int i = 0; i < _links.Count; i++)
		{
			string item = PlatformlessPath(_links[i].name).Replace("tm", string.Empty);
			hashSet.Add(item);
		}
		List<tmResourceCollectionLink> list = new List<tmResourceCollectionLink>();
		foreach (string item2 in hashSet)
		{
			list.Add(ResourceLinkByGUID(item2 + platform.postfix));
		}
		return list;
	}

	public static void ValidateMesh(Mesh mesh)
	{
	}
}
