using System;
using System.Collections;
using UnityEngine;

public class UIButtonScaler : MonoBehaviour
{
	public tk2dUIItem myItem;

	public float downDuration = 0.1f;

	public float upDuration = 0.5f;

	public Vector3 downScale = new Vector3(0.9f, 0.9f, 0.9f);

	public Vector3 upScale = Vector3.one;

	public bool cachedUpScaleOnAwake = true;

	public Method downMethod;

	public bool useDownCurve = true;

	public AnimationCurve downCurve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));

	public Method upMethod;

	public bool useUpCurve = true;

	public AnimationCurve upCurve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));

	public bool useOnReleaseInsteadOfOnUp;

	public bool isIgnoringTimeScale;

	public bool isScaleBoxCollider;

	private Transform cachedTransform;

	private BoxCollider cachedBoxCollider;

	private Vector3 cachedBoxColliderOriginSize;

	private Vector3 toScale = Vector3.one;

	private Vector3 fromScale = Vector3.one;

	private float elapsedTime;

	public bool UseOnReleaseInsteadOfOnUp
	{
		get
		{
			return useOnReleaseInsteadOfOnUp;
		}
	}

	public bool IsIgnoringTimeScale
	{
		get
		{
			return isIgnoringTimeScale;
		}
	}

	public Transform CachedTransform
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

	public tk2dUIItem MyItem
	{
		get
		{
			if (myItem == null)
			{
				myItem = GetComponent<tk2dUIItem>();
			}
			return myItem;
		}
	}

	public BoxCollider CachedBoxCollider
	{
		get
		{
			if (cachedBoxCollider == null)
			{
				cachedBoxCollider = GetComponent<BoxCollider>();
				if (cachedBoxCollider != null)
				{
					cachedBoxColliderOriginSize = cachedBoxCollider.size;
					if (tk2dSystem.IsRetina)
					{
						cachedBoxColliderOriginSize *= 2f;
					}
				}
			}
			return cachedBoxCollider;
		}
	}

	public Vector3 LocalScale
	{
		get
		{
			return CachedTransform.localScale;
		}
		set
		{
			CachedTransform.localScale = value;
			if (!isScaleBoxCollider && CachedBoxCollider != null)
			{
				float num = ((value.x != 0f) ? Mathf.Abs(value.x) : 1f);
				CachedBoxCollider.size = cachedBoxColliderOriginSize / num;
			}
		}
	}

	private void Awake()
	{
		if (cachedUpScaleOnAwake)
		{
			ScaleSetter component = GetComponent<ScaleSetter>();
			if (component != null)
			{
				component.Init();
			}
			upScale = LocalScale;
		}
	}

	private void OnEnable()
	{
		if (MyItem != null)
		{
			MyItem.OnDown += ButtonDown;
			if (useOnReleaseInsteadOfOnUp)
			{
				MyItem.OnRelease += ButtonUp;
			}
			else
			{
				MyItem.OnUp += ButtonUp;
			}
		}
		elapsedTime = 0f;
		LocalScale = upScale;
	}

	private void OnDisable()
	{
		if (MyItem != null)
		{
			MyItem.OnDown -= ButtonDown;
			if (useOnReleaseInsteadOfOnUp)
			{
				MyItem.OnRelease -= ButtonUp;
			}
			else
			{
				MyItem.OnUp -= ButtonUp;
			}
		}
	}

	private void ButtonDown()
	{
		StopCoroutine("UpScaleTween");
		if (downDuration > 0f)
		{
			toScale = downScale;
			fromScale = LocalScale;
			StartCoroutine("DownScaleTween");
		}
		else
		{
			LocalScale = downScale;
		}
	}

	private void ButtonUp()
	{
		StopCoroutine("DownScaleTween");
		if (upDuration > 0f)
		{
			toScale = upScale;
			fromScale = LocalScale;
			StartCoroutine("UpScaleTween");
		}
		else
		{
			LocalScale = upScale;
		}
	}

	private float ApplyMethod(float value, Method method)
	{
		if (value < 0f)
		{
			value = 0f;
		}
		if (value > 1f)
		{
			value = 1f;
		}
		switch (method)
		{
		case Method.EaseIn:
			value = 1f - Mathf.Sin((float)Math.PI / 2f * (1f - value));
			value *= value;
			break;
		case Method.EaseOut:
			value = Mathf.Sin((float)Math.PI / 2f * value);
			value = 1f - value;
			value = 1f - value * value;
			break;
		case Method.EaseInOut:
		{
			value -= Mathf.Sin(value * ((float)Math.PI * 2f)) / ((float)Math.PI * 2f);
			value = value * 2f - 1f;
			float num = ((!(value < 0f)) ? 1f : (-1f));
			value = 1f - num * value;
			value = 1f - value * value;
			value = num * value * 0.5f + 0.5f;
			break;
		}
		}
		return value;
	}

	private IEnumerator DownScaleTween()
	{
		elapsedTime = 0f;
		while (elapsedTime < downDuration)
		{
			float percent = elapsedTime / downDuration;
			percent = ((!(percent > 1f)) ? percent : 1f);
			percent = ApplyMethod(percent, downMethod);
			percent = ((!useDownCurve || downCurve == null) ? percent : downCurve.Evaluate(percent));
			LocalScale = fromScale * (1f - percent) + toScale * percent;
			yield return null;
			float deltaTime = ((!isIgnoringTimeScale) ? Time.deltaTime : Time.unscaledDeltaTime);
			elapsedTime += deltaTime;
		}
		LocalScale = toScale;
	}

	private IEnumerator UpScaleTween()
	{
		elapsedTime = 0f;
		while (elapsedTime < upDuration)
		{
			float percent = elapsedTime / upDuration;
			percent = ((!(percent > 1f)) ? percent : 1f);
			percent = ApplyMethod(percent, upMethod);
			percent = ((!useUpCurve || upCurve == null) ? percent : upCurve.Evaluate(percent));
			LocalScale = fromScale * (1f - percent) + toScale * percent;
			yield return null;
			float deltaTime = ((!isIgnoringTimeScale) ? Time.deltaTime : Time.unscaledDeltaTime);
			elapsedTime += deltaTime;
		}
		LocalScale = toScale;
	}

	private void Reset()
	{
		downDuration = 0.15f;
		upDuration = 0.7f;
		downScale = new Vector3(0.85f, 0.85f, 1f);
		upScale = Vector3.one;
		cachedUpScaleOnAwake = false;
		downMethod = Method.EaseOut;
		useDownCurve = false;
		downCurve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));
		upMethod = Method.EaseOut;
		useUpCurve = true;
		upCurve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));
		useOnReleaseInsteadOfOnUp = false;
	}
}
