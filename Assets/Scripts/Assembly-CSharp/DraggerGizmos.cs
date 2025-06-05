using UnityEngine;

public class DraggerGizmos : MonoBehaviour
{
	[SerializeField]
	private GameObject rootObjectLabels;

	[SerializeField]
	private tk2dTextMesh strengthlabel;

	[SerializeField]
	private tk2dTextMesh anglelabel;

	[SerializeField]
	private tk2dTextMesh strengthText;

	[SerializeField]
	private tk2dTextMesh angleText;

	[SerializeField]
	private tk2dSlicedSprite backSprite;

	private Transform cachedTransform;

	private tk2dCamera cachedUICamera;

	private Transform CachedTransform
	{
		get
		{
			if (cachedTransform == null)
			{
				cachedTransform = base.transform;
			}
			return cachedTransform;
		}
	}

	private tk2dCamera CachedUICamera
	{
		get
		{
			if (cachedUICamera == null)
			{
				cachedUICamera = SingletonMonoBehaviour<GUIScreenManager>.Instance.UICamera;
			}
			return cachedUICamera;
		}
	}

	private void Awake()
	{
		if (tk2dSystem.IsRetina)
		{
			Vector3 localPosition = rootObjectLabels.transform.localPosition;
			rootObjectLabels.transform.localPosition = new Vector3(localPosition.x * 2f, localPosition.y * 2f, localPosition.z);
			localPosition = strengthlabel.transform.localPosition;
			strengthlabel.transform.localPosition = new Vector3(localPosition.x * 2f, localPosition.y * 2f, localPosition.z);
			localPosition = anglelabel.transform.localPosition;
			anglelabel.transform.localPosition = new Vector3(localPosition.x * 2f, localPosition.y * 2f, localPosition.z);
			strengthText.transform.localPosition *= 2f;
			angleText.transform.localPosition *= 2f;
			backSprite.dimensions *= 2f;
		}
	}

	public void SetPosition(Vector3 screenPosition)
	{
		screenPosition.x = Mathf.Clamp(screenPosition.x, (float)CachedUICamera.ScreenCamera.pixelWidth * 0.2f, (float)CachedUICamera.ScreenCamera.pixelWidth * 0.8f);
		screenPosition.y = Mathf.Clamp(screenPosition.y, 0f, (float)CachedUICamera.ScreenCamera.pixelHeight * 0.9f);
		Vector3 position = CachedUICamera.ScreenCamera.ScreenToWorldPoint(screenPosition);
		position.z = CachedTransform.position.z;
		CachedTransform.position = position;
	}

	public void SetParametrs(int ratio, int angle)
	{
		if (SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.ActiveCharacter == SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.CharacterSecond)
		{
			angle = 180 - angle;
		}
		else if (angle > 180)
		{
			angle -= 360;
		}
		strengthlabel.text = ratio + "%";
		anglelabel.text = angle + "°";
	}

	public void SetColor(Color color)
	{
		strengthlabel.color = color;
		anglelabel.color = color;
		strengthText.color = color;
		angleText.color = color;
	}
}
