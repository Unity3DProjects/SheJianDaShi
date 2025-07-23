using System;

public class QGBaseActionOption<T>
{
	public Action<T> success;

	public Action<T> fail;

	public Action<T> complete;

	public void InvokeSuccess(object option)
	{
		success?.Invoke((T)option);
	}

	public void InvokeFail(object option)
	{
		fail?.Invoke((T)option);
	}

	public void InvokeComplete(object option)
	{
		complete?.Invoke((T)option);
	}
}

public class QGBaseActionOption<S, C>
{
	public Action<S> success;

	public Action<C> fail;

	public Action<C> complete;

	public void InvokeSuccess(object option)
	{
		success?.Invoke((S)option);
	}

	public void InvokeFail(object option)
	{
		fail?.Invoke((C)option);
	}

	public void InvokeComplete(object option)
	{
		complete?.Invoke((C)option);
	}
}

public class QGBaseActionOption<S, F, C>
{
	public Action<S> success;

	public Action<F> fail;

	public Action<C> complete;

	public void InvokeSuccess(object option)
	{
		success?.Invoke((S)option);
	}

	public void InvokeFail(object option)
	{
		fail?.Invoke((F)option);
	}

	public void InvokeComplete(object option)
	{
		complete?.Invoke((C)option);
	}
}

