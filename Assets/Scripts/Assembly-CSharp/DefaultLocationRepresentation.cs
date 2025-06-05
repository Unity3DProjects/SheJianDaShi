using UnityEngine;

public class DefaultLocationRepresentation : MonoBehaviour
{
	[SerializeField]
	private float scalePadPortrait = 6350f;

	[SerializeField]
	private float scalePadLandscape = 6050f;

	[SerializeField]
	private float scalePhonePortrait = 6200f;

	[SerializeField]
	private float scalePhoneLandscape = 5550f;

	private bool IsIPad
	{
		get
		{
			return SystemInfo.deviceModel.Contains("iPad");
		}
	}

	private bool IsWideScreenAspectRatio
	{
		get
		{
			float aspect = Camera.main.aspect;
			return aspect >= 2f || (double)aspect <= 0.5;
		}
	}

	private float Scale
	{
		get
		{
			return base.transform.localScale.x;
		}
		set
		{
			base.transform.localScale = new Vector3(value, value, 1f);
		}
	}

	private void Awake()
	{
		GUIOrientationManger_OnDeviceOrientationChanged(SingletonMonoBehaviour<GUIOrientationManger>.Instance.CurrentRotationType);
		GUIOrientationManger.OnDeviceOrientationChanged += GUIOrientationManger_OnDeviceOrientationChanged;
	}

	private void OnDestroy()
	{
		GUIOrientationManger.OnDeviceOrientationChanged -= GUIOrientationManger_OnDeviceOrientationChanged;
	}

	private void GUIOrientationManger_OnDeviceOrientationChanged(GUILayouterRotationType obj)
	{
		if (IsIPad || IsWideScreenAspectRatio)
		{
			Scale = ((obj != GUILayouterRotationType.Landscape) ? scalePadPortrait : scalePadLandscape);
		}
		else
		{
			Scale = ((obj != GUILayouterRotationType.Landscape) ? scalePhonePortrait : scalePhoneLandscape);
		}
		if (tk2dSystem.IsRetina)
		{
			Scale *= 2f;
		}
	}
}
