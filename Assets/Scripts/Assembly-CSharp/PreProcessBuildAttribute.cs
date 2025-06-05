using System;

[AttributeUsage(AttributeTargets.Method)]
public sealed class PreProcessBuildAttribute : Attribute
{
	private int m_callbackOrder;

	public int callbackOrder
	{
		get
		{
			return m_callbackOrder;
		}
	}

	public PreProcessBuildAttribute()
	{
		m_callbackOrder = 1;
	}

	public PreProcessBuildAttribute(int callbackOrder)
	{
		m_callbackOrder = callbackOrder;
	}
}
