using UnityEngine;

public class GUIEffectPlayOnAwake : MonoBehaviour
{
	private void Awake()
	{
		base.gameObject.PlayFX();
	}
}
