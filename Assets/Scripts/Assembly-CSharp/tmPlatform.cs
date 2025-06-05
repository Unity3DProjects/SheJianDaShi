using System;
using UnityEngine;

[Serializable]
public class tmPlatform
{
	public string name = string.Empty;

	public string postfix = string.Empty;

	public float scale = 1f;

	[SerializeField]
	private string guid;

	public tmPlatform(string name, string postfix, float scale, string guid)
	{
		this.name = name;
		this.postfix = postfix;
		this.scale = scale;
		this.guid = guid;
	}

	public override bool Equals(object obj)
	{
		tmPlatform tmPlatform2 = obj as tmPlatform;
		if (tmPlatform2 != null)
		{
			return guid.Equals(tmPlatform2.guid);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return guid.GetHashCode();
	}
}
