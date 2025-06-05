using System;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;

public class ReflectionUtils<T> where T : class, new()
{
	private readonly T _Instance;

	public ReflectionUtils(T instance)
	{
		_Instance = instance;
	}

	public string GetFieldName<U>(Expression<Func<T, U>> fieldAccess)
	{
		MemberExpression memberExpression = fieldAccess.Body as MemberExpression;
		if (memberExpression != null)
		{
			return memberExpression.Member.Name;
		}
		throw new InvalidOperationException("Member expression expected");
	}

	public FieldInfo GetField(string fieldName)
	{
		return typeof(T).GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
	}

	public A GetAttribute<A>(FieldInfo field) where A : Attribute
	{
		return (A)Attribute.GetCustomAttribute(field, typeof(A));
	}

	public void ConstrainMin<U>(Expression<Func<T, U>> fieldAccess, float value)
	{
		FieldInfo field = GetField(GetFieldName(fieldAccess));
		field.SetValue(_Instance, Mathf.Max(value, GetAttribute<MinAttribute>(field).min));
	}

	public void ConstrainMin<U>(Expression<Func<T, U>> fieldAccess, int value)
	{
		FieldInfo field = GetField(GetFieldName(fieldAccess));
		field.SetValue(_Instance, (int)Mathf.Max(value, GetAttribute<MinAttribute>(field).min));
	}

	public void ConstrainRange<U>(Expression<Func<T, U>> fieldAccess, float value)
	{
		FieldInfo field = GetField(GetFieldName(fieldAccess));
		RangeAttribute attribute = GetAttribute<RangeAttribute>(field);
		field.SetValue(_Instance, Mathf.Clamp(value, attribute.min, attribute.max));
	}

	public void ConstrainRange<U>(Expression<Func<T, U>> fieldAccess, int value)
	{
		FieldInfo field = GetField(GetFieldName(fieldAccess));
		RangeAttribute attribute = GetAttribute<RangeAttribute>(field);
		field.SetValue(_Instance, (int)Mathf.Clamp(value, attribute.min, attribute.max));
	}
}
