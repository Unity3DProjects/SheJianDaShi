using System;
using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	private const float SCREEN_PHYSICAL_WIDTH = 1136f;

	private const float SMOOTH_DAMP_TIME_WEAPONS = 0.04f;

	private const float SMOOTH_DAMP_MAX_SPEED = 600000f;

	[SerializeField]
	private tk2dCamera tkCamera;

	[SerializeField]
	private float scaleMinSpeed = 800f;

	[SerializeField]
	private float scaleMaxSpeed = 2000f;

	[SerializeField]
	private AnimationCurve curveScale;

	[SerializeField]
	private GameObject blurPrefab;

	[SerializeField]
	private float blurMaxAlpha = 0.5f;

	[SerializeField]
	private float blurAppearDuration = 1f;

	[SerializeField]
	private AnimationCurve blurAppearCurve;

	[SerializeField]
	private float blurDisappearDuration = 1f;

	[SerializeField]
	private AnimationCurve blurDisappearCurve;

	private Action positionUpdatedAction;

	private Transform cachedTransform;

	private LevelCameraSettings levelSettings;

	private float prevScale = 1f;

	private float scale = 1f;

	private float currentScaleTime;

	private float targetScaleTime;

	private float deltaScale;

	private bool isScaleAnimated;

	private bool isFollowingTarget;

	private Transform target;

	private Vector2 currentCameraVelocity;

	private Vector2 cameraStartPosition;

	private Vector2 targetOffset;

	private float blendTime;

	private float blendDuration = 1f;

	private float targetScale;

	private float shakeDuration;

	private float shakeTime;

	private float shakeAmplitude;

	private bool isRestoringAfterShake;

	private GameObject blur;

	private tk2dSprite cachedBlurSprite;

	private TweenColor cachedBlurTweener;

	private SoundObject cachedMainTheme;

	private float changingVolumeTime;

	private float defaultOrtho;

	private float targetResolutionX;

	private float targetResolutionY;

	private bool isSmoothEnabled;

	private GUIOrientationManger cachedGUIOrientationManager;

	private TweenPosition cachedTweenPosition;

	private bool isHorizontalOnlyShake;

	private bool isVerticalOnlyShake;

	private float oneSideShakeTime;

	private float currentOneSideShakeTime;

	private bool isFirstMovement;

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

	private GUIOrientationManger CachedGUIOrientationManager
	{
		get
		{
			if (cachedGUIOrientationManager == null)
			{
				cachedGUIOrientationManager = SingletonMonoBehaviour<GUIOrientationManger>.Instance;
			}
			return cachedGUIOrientationManager;
		}
	}

	private TweenPosition CachedTweenPosition
	{
		get
		{
			if (cachedTweenPosition == null)
			{
				cachedTweenPosition = GetComponent<TweenPosition>();
			}
			return cachedTweenPosition;
		}
	}

	public LevelCameraSettings LevelSettings
	{
		get
		{
			return levelSettings;
		}
		set
		{
			levelSettings = value;
		}
	}

	public Vector2 Position
	{
		get
		{
			return CachedTransform.position;
		}
		set
		{
			CachedTransform.position = ConstrainedPosition(value);
		}
	}

	public tk2dCamera CachedCamera
	{
		get
		{
			if (tkCamera == null)
			{
				tkCamera = GetComponent<tk2dCamera>();
			}
			return tkCamera;
		}
	}

	public float Scale
	{
		get
		{
			return scale;
		}
		private set
		{
			if (scale != value)
			{
				scale = value;
				RecalculateSize();
			}
		}
	}

	private float TargetScale
	{
		get
		{
			return targetScale;
		}
		set
		{
			if (value != targetScale)
			{
				targetScale = value;
			}
		}
	}

	public Transform Target
	{
		get
		{
			return target;
		}
	}

	private bool IsShaking
	{
		get
		{
			return shakeTime <= shakeDuration;
		}
	}

	private tk2dSprite CachedBlurSprite
	{
		get
		{
			if (cachedBlurSprite == null)
			{
				if (blur == null)
				{
					CreateBlur();
				}
				cachedBlurSprite = blur.GetComponent<tk2dSprite>();
			}
			return cachedBlurSprite;
		}
	}

	private TweenColor CachedBlurTweener
	{
		get
		{
			if (cachedBlurTweener == null)
			{
				if (blur == null)
				{
					CreateBlur();
				}
				cachedBlurTweener = blur.GetComponent<TweenColor>();
			}
			return cachedBlurTweener;
		}
	}

	private SoundObject CachedMainTheme
	{
		get
		{
			if (cachedMainTheme == null)
			{
				SoundObject[] playingAudioObjects = SoundsManager.GetPlayingAudioObjects("theme_main");
				if (playingAudioObjects.Length > 0)
				{
					cachedMainTheme = playingAudioObjects[0];
				}
			}
			return cachedMainTheme;
		}
	}

	private void Awake()
	{
		defaultOrtho = CachedCamera.CameraSettings.orthographicSize;
		CachedTransform.position = new Vector3(0f, 0f, -500f);
		RecalculateSize();
	}

	private void OnEnable()
	{
		GUIOrientationManger.OnDeviceOrientationChanged += GUIOrientationManger_OnDeviceOrientationChanged;
	}

	private void OnDisable()
	{
		GUIOrientationManger.OnDeviceOrientationChanged -= GUIOrientationManger_OnDeviceOrientationChanged;
	}

	public void UpdateCameraController(float deltaTime)
	{
		if (isFollowingTarget && target != null)
		{
			FollowTarget(deltaTime);
		}
		if (shakeTime <= shakeDuration)
		{
			Vector2 vector = Vector2.zero;
			if ((!isHorizontalOnlyShake && !isVerticalOnlyShake) || (isHorizontalOnlyShake && isVerticalOnlyShake))
			{
				vector = UnityEngine.Random.insideUnitCircle * shakeAmplitude;
			}
			else if (isHorizontalOnlyShake)
			{
				vector = ((!isFirstMovement) ? (Vector2.left * shakeAmplitude) : (Vector2.right * shakeAmplitude));
			}
			else if (isVerticalOnlyShake)
			{
				vector = ((!isFirstMovement) ? (Vector2.down * shakeAmplitude) : (Vector2.up * shakeAmplitude));
			}
			Position += vector;
			currentOneSideShakeTime -= deltaTime;
			if (currentOneSideShakeTime <= 0f)
			{
				isFirstMovement = !isFirstMovement;
				currentOneSideShakeTime = oneSideShakeTime;
			}
			shakeTime += deltaTime;
			if (shakeTime > shakeDuration && isRestoringAfterShake)
			{
				TweenMoveToPosition(cameraStartPosition, 0.5f);
			}
		}
		if (isScaleAnimated)
		{
			UpdateScale(deltaTime);
		}
	}

	public void SetScaleImmediately(float size)
	{
		Scale = size;
		TargetScale = Scale;
		isScaleAnimated = false;
		currentScaleTime = 0f;
		targetScaleTime = -1f;
	}

	public void SetBeginScale()
	{
		SetScaleImmediately(levelSettings.minScale);
	}

	public void SetScaleWithDuration(float targetScale, float duration)
	{
		currentScaleTime = 0f;
		targetScaleTime = duration;
		deltaScale = targetScale - scale;
		prevScale = scale;
		TargetScale = targetScale;
		isScaleAnimated = true;
	}

	public void UpdateScaleWithWeaponSpeed(float speed)
	{
		float num = Mathf.Clamp(speed, scaleMinSpeed, scaleMaxSpeed);
		num = (num - scaleMinSpeed) / scaleMaxSpeed;
		TargetScale = levelSettings.minScale + num * (levelSettings.maxScale - levelSettings.minScale);
		isScaleAnimated = true;
	}

	public void OnUpdatePosition()
	{
		if (positionUpdatedAction != null)
		{
			positionUpdatedAction();
		}
		positionUpdatedAction = null;
	}

	public void ApplyShakeWithDuration(float duration = 0.5f, float amplitude = 5f, bool isRestoreAfterShake = false, bool isHorizontal = false, bool isVertical = false, float oneSideDuration = 0.1f)
	{
		shakeDuration = duration;
		shakeAmplitude = amplitude;
		shakeTime = 0f;
		isRestoringAfterShake = isRestoreAfterShake;
		if (isRestoringAfterShake)
		{
			cameraStartPosition = Position;
		}
		isHorizontalOnlyShake = isHorizontal;
		isVerticalOnlyShake = isVertical;
		oneSideShakeTime = oneSideDuration;
		currentOneSideShakeTime = oneSideShakeTime;
		isFirstMovement = false;
	}

	public void TweenMoveToPosition(Vector2 targetPos, float duration, Action action = null, AnimationCurve tweenCurve = null)
	{
		isFollowingTarget = false;
		positionUpdatedAction = action;
		Vector3 endPosition = ConstrainedPosition(targetPos);
		if (!CachedTweenPosition.isActiveAndEnabled)
		{
			CachedTweenPosition.beginPosition = new Vector3(Position.x, Position.y, -500f);
			CachedTweenPosition.duration = duration;
			if (tweenCurve == null)
			{
				CachedTweenPosition.SwitchOffCurve();
				CachedTweenPosition.method = Method.Linear;
			}
			else
			{
				CachedTweenPosition.SwitchOnCurve();
				CachedTweenPosition.animationCurve = tweenCurve;
			}
			CachedTweenPosition.SetBeginStateImmediately();
		}
		CachedTweenPosition.endPosition = endPosition;
		CachedTweenPosition.SetEndState();
	}

	public void SetTarget(Transform targetTrasform, Vector2 startVelocity, Vector2 offset, float blendDur = 0.5f, bool isNeedSmooth = true)
	{
		if (target != targetTrasform)
		{
			target = targetTrasform;
			cameraStartPosition = Position;
			currentCameraVelocity = startVelocity * Time.deltaTime;
			targetOffset = offset;
			isFollowingTarget = true;
			blendTime = 0f;
			blendDuration = blendDur;
			isSmoothEnabled = isNeedSmooth;
		}
	}

	public void StopTweenMove()
	{
		TweenPosition component = GetComponent<TweenPosition>();
		component.SetEndStateImmediately();
	}

	public void StartBlur()
	{
		float duration = Mathf.Abs(blurMaxAlpha - CachedBlurTweener.CurrentColor.a) / blurMaxAlpha * blurAppearDuration;
		EnableBlurTweenColor(CachedBlurTweener.CurrentColor, new Color(0f, 0f, 0f, blurMaxAlpha), duration, blurAppearCurve);
		CalculateBlurSize();
	}

	public void RemoveBlur()
	{
		float duration = CachedBlurTweener.CurrentColor.a / blurMaxAlpha * blurDisappearDuration;
		EnableBlurTweenColor(CachedBlurTweener.CurrentColor, Color.clear, duration, blurDisappearCurve);
	}

	public void RemoveBlurImmediately()
	{
		if (blur != null)
		{
			UnityEngine.Object.Destroy(blur);
			blur = null;
		}
	}

	private void RecalculateSize()
	{
		int pixelWidth = CachedCamera.ScreenCamera.pixelWidth;
		int pixelHeight = CachedCamera.ScreenCamera.pixelHeight;
		bool flag = CachedGUIOrientationManager.CurrentRotationType == GUILayouterRotationType.Portrait;
		targetResolutionX = ((!flag) ? Mathf.Max(pixelWidth, pixelHeight) : Mathf.Min(pixelWidth, pixelHeight));
		targetResolutionY = ((!flag) ? Mathf.Min(pixelWidth, pixelHeight) : Mathf.Max(pixelWidth, pixelHeight));
		float num = ((!flag) ? 1f : (targetResolutionY / targetResolutionX));
		CachedCamera.ZoomFactor = num / scale;
		float num2 = Mathf.Max(targetResolutionX, targetResolutionY);
		float orthographicSize = defaultOrtho * 1136f / num2;
		CachedCamera.CameraSettings.orthographicSize = orthographicSize;
	}

	private Vector3 ConstrainedPosition(Vector2 position, bool isHorizontalOnly = false, bool isVerticalOnly = false)
	{
		return new Vector3(Mathf.Clamp(position.x, levelSettings.minX, levelSettings.maxX), Mathf.Clamp(position.y, levelSettings.minY, levelSettings.maxY), -500f);
	}

	private void FollowTarget(float deltaTime)
	{
		Vector2 vector = (Vector2)target.position + targetOffset;
		Vector2 vector2 = vector - Position;
		blendTime += deltaTime / blendDuration;
		blendTime = Mathf.Min(blendTime, 1f);
		float num = vector2.magnitude * blendTime;
		Vector2 vector3 = vector2.normalized * num;
		Vector2 vector4 = currentCameraVelocity + vector3;
		if (isSmoothEnabled)
		{
			Vector2 currentVelocity = Vector2.zero;
			Position = Vector2.SmoothDamp(Position, cameraStartPosition + vector4, ref currentVelocity, 0.04f, 600000f, deltaTime);
		}
		else
		{
			Position = cameraStartPosition + vector4;
		}
		currentCameraVelocity = Position - cameraStartPosition;
		if (CachedTweenPosition.isActiveAndEnabled)
		{
			CachedTweenPosition.enabled = false;
		}
	}

	private void CreateBlur()
	{
		if (blur == null)
		{
			blur = UnityEngine.Object.Instantiate(blurPrefab);
			blur.transform.position = new Vector3(Position.x, Position.y, -10f);
			blur.transform.parent = base.transform;
			CalculateBlurSize();
		}
	}

	private void CalculateBlurSize()
	{
		if (blur != null)
		{
			float maxScale = levelSettings.maxScale;
			CachedBlurSprite.scale = new Vector3((float)ScreenDimentions.Width * maxScale, (float)ScreenDimentions.Height * maxScale, 0f);
		}
	}

	private void EnableBlurTweenColor(Color startColor, Color endColor, float duration, AnimationCurve blurCurve)
	{
		CachedBlurTweener.beginColor = startColor;
		cachedBlurTweener.endColor = endColor;
		CachedBlurTweener.duration = duration;
		CachedBlurTweener.useCurve = true;
		CachedBlurTweener.animationCurve = blurCurve;
		CachedBlurTweener.ResetTween();
		CachedBlurTweener.enabled = true;
	}

	private void UpdateScale(float deltaTime)
	{
		if (currentScaleTime < targetScaleTime)
		{
			currentScaleTime += deltaTime;
			Scale = prevScale + deltaScale * Mathf.Clamp01(currentScaleTime / targetScaleTime);
			if (currentScaleTime >= targetScaleTime)
			{
				isScaleAnimated = false;
				currentScaleTime = 0f;
				targetScaleTime = -1f;
				Scale = TargetScale;
			}
		}
		else
		{
			Scale = Mathf.Lerp(Scale, TargetScale, deltaTime / blendDuration);
		}
	}

	private IEnumerator ChangeVolume(SoundObject sound, float startVolume, float endVolume, float duration)
	{
		if (duration <= 0f || startVolume == endVolume)
		{
			sound.Volume = endVolume;
			yield return false;
		}
		else
		{
			while (changingVolumeTime < duration)
			{
				changingVolumeTime += Time.unscaledDeltaTime;
				sound.Volume = startVolume + (endVolume - startVolume) * Mathf.Clamp01(changingVolumeTime / duration);
				yield return null;
			}
		}
		changingVolumeTime = 0f;
	}

	private void GUIOrientationManger_OnDeviceOrientationChanged(GUILayouterRotationType rt)
	{
		RecalculateSize();
		CalculateBlurSize();
	}
}
