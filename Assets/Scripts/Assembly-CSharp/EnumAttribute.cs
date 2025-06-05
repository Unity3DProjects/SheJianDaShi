using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class EnumAttribute : PropertyAttribute
{
	private string[] listForEnum;

	private Type listSourceClass;

	private string listSourceMethod;

	public string[] ListForEnum
	{
		get
		{
			if (listSourceClass != null)
			{
				if (!string.IsNullOrEmpty(listSourceMethod))
				{
					UpdateMethod();
				}
				else
				{
					UpdateFields();
				}
			}
			return listForEnum;
		}
	}

	private EnumAttribute()
	{
	}

	public EnumAttribute(Type sourceClassConstFields)
	{
		if (sourceClassConstFields != null)
		{
			listSourceClass = sourceClassConstFields;
			listSourceMethod = null;
			UpdateFields();
		}
	}

	public EnumAttribute(Type sourceClassList, string sourceMethodList)
	{
		if (sourceClassList != null)
		{
			listSourceClass = sourceClassList;
			if (!string.IsNullOrEmpty(sourceMethodList))
			{
				listSourceMethod = sourceMethodList;
				UpdateMethod();
			}
			else
			{
				listSourceMethod = null;
			}
		}
	}

	public EnumAttribute(params object[] list)
	{
		if (list.Length > 0)
		{
			listForEnum = new string[list.Length];
			for (int i = 0; i < list.Length; i++)
			{
				listForEnum[i] = list[i].ToString();
			}
		}
	}

	public EnumAttribute(string[] list)
	{
		if (list.Length > 0)
		{
			listForEnum = new string[list.Length];
			for (int i = 0; i < list.Length; i++)
			{
				listForEnum[i] = list[i];
			}
		}
	}

	private void UpdateMethod()
	{
		object[] array = listSourceClass.GetMethod(listSourceMethod).Invoke(null, null) as object[];
		if (array.Length <= 0)
		{
			return;
		}
		listForEnum = new string[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] != null)
			{
				listForEnum[i] = array[i].ToString();
			}
		}
	}

	private void UpdateFields()
	{
		List<string> list = new List<string>();
		FieldInfo[] fields = listSourceClass.GetFields(BindingFlags.Static | BindingFlags.Public);
		foreach (FieldInfo fieldInfo in fields)
		{
			if (fieldInfo.IsLiteral && !fieldInfo.IsInitOnly)
			{
				object rawConstantValue = fieldInfo.GetRawConstantValue();
				if (rawConstantValue != null)
				{
					list.Add(rawConstantValue.ToString());
				}
			}
		}
		listForEnum = list.ToArray();
	}
}
