using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class BoneEffectClass : MonoBehaviour
{
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<GameObject> m_OnCollided;

	private bool isGroundCollided;

	private GameObject flyingEffect;

	public static event Action<GameObject> OnCollided
	{
		add
		{
			Action<GameObject> action = BoneEffectClass.m_OnCollided;
			Action<GameObject> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref BoneEffectClass.m_OnCollided, (Action<GameObject>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<GameObject> action = BoneEffectClass.m_OnCollided;
			Action<GameObject> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref BoneEffectClass.m_OnCollided, (Action<GameObject>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (!isGroundCollided && collision.collider.GetComponent<Ground>() != null)
		{
			isGroundCollided = true;
			if (BoneEffectClass.OnCollided != null)
			{
				BoneEffectClass.OnCollided(base.gameObject);
			}
			flyingEffect.StopFX(true);
			flyingEffect = null;
		}
	}

	public void SetFlyingEffect(GameObject effect)
	{
		flyingEffect = effect;
	}
}
