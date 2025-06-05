using UnityEngine;

public abstract class SizeSetter : MonoBehaviour
{
	public bool destroyAfterInit;

	public RoundFloatEnum roundFloatPreference = RoundFloatEnum.DontRoundFloat;

	private bool start;

	private void Awake()
	{
		if (!start)
		{
			start = true;
			UpdateSize();
			if (destroyAfterInit)
			{
				Object.Destroy(this);
			}
		}
	}

	public void Init()
	{
		Awake();
	}

	public void ForceInit()
	{
		UpdateSize();
		start = true;
	}

	protected abstract void UpdateSize();
}
