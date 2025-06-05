using System;
using UnityEngine;

public class ResourceLinkAttribute : PropertyAttribute
{
	public Type resourceType;

	public ResourceLinkAttribute()
		: this(typeof(GameObject))
	{
	}

	public ResourceLinkAttribute(Type resourceType)
	{
		this.resourceType = resourceType;
	}
}
