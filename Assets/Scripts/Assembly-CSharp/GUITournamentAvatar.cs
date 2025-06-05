using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUITournamentAvatar : MonoBehaviour
{
	private const string DISABLE_SHADER = "tk2d/BlendVertexColorGrayscaleColorize";

	private const string DISABLE_GRAYSCALE_PROPERTY = "_GrayscaleFactor";

	private const string ANIMATION_HIGHLIGHT = "highlight";

	[SerializeField]
	private GUILayouter internalLayouter;

	[SerializeField]
	private tk2dBaseSprite avatarSprite;

	[SerializeField]
	private tk2dTextMesh numberLabel;

	[SerializeField]
	private tk2dTextMesh[] nameLabel;

	[SerializeField]
	private GameObject topLabelRoot;

	[SerializeField]
	private GameObject bottomLabelRoot;

	[SerializeField]
	private TweenColor tweenColor;

	[SerializeField]
	private Color enableColor;

	[SerializeField]
	private float enableDuration;

	[SerializeField]
	private SkeletonAnimation enableAnimation;

	[SerializeField]
	private AnimationCurve enableCurve;

	[SerializeField]
	private Color inactiveColor;

	[SerializeField]
	private Color defeatedColor;

	[SerializeField]
	private float defeatedDuration;

	[SerializeField]
	private AnimationCurve defeatedCurve;

	[SerializeField]
	private TweenScale defeatedCrossAppearTween;

	[SerializeField]
	private float defeatedCrossAppearDuration;

	[SerializeField]
	private AnimationCurve defeatedCrossAppearCurve;

	[SerializeField]
	private TweenScale[] flagAppearTween;

	[SerializeField]
	private float flagAppearDuration;

	[SerializeField]
	private AnimationCurve flagAppearCurve;

	[SerializeField]
	private float flagDisappearDuration;

	[SerializeField]
	private AnimationCurve flagDisappearCurve;

	private float grayscaleTimeCounter;

	private List<Tweener> tweeners = new List<Tweener>();

	private bool isIncreaseGrayscaleProperty;

	private ObjectsSpawner objectsSpawner;

	public List<Tweener> Tweeners
	{
		get
		{
			if (tweeners.Count == 0)
			{
				tweeners.Add(tweenColor);
				tweeners.Add(defeatedCrossAppearTween);
				for (int i = 0; i < flagAppearTween.Length; i++)
				{
					if (flagAppearTween[i].gameObject.activeSelf)
					{
						tweeners.Add(flagAppearTween[i]);
					}
				}
			}
			return tweeners;
		}
	}

	private float GrayscaleProperty
	{
		get
		{
			return avatarSprite.CachedRenderer.material.GetFloat("_GrayscaleFactor");
		}
		set
		{
			avatarSprite.CachedRenderer.material.SetFloat("_GrayscaleFactor", value);
		}
	}

	private ObjectsSpawner ObjectsSpawner
	{
		get
		{
			if (objectsSpawner == null)
			{
				objectsSpawner = SingletonMonoBehaviour<LevelsManager>.Instance.ObjectsSpawner;
			}
			return objectsSpawner;
		}
	}

	private void OnDisable()
	{
		if (GrayscaleProperty > 0f && GrayscaleProperty < 1f)
		{
			GrayscaleProperty = ((!isIncreaseGrayscaleProperty) ? 0f : 1f);
		}
	}

	public void Init(GUILayoutCell parentCell, CharacterNameType character, int number, bool isNumberActive = true, bool isBottomLabelActive = true)
	{
		GUICharacter gUICharacter = ObjectsSpawner.GetGUICharacter(character);
		avatarSprite.SetSprite(gUICharacter.IconAvatarName);
		numberLabel.transform.parent.gameObject.SetActive(isNumberActive);
		if (isNumberActive)
		{
			numberLabel.text = (SingletonMonoBehaviour<TournamentManager>.Instance.Enemies.Count - number).ToString();
			avatarSprite.transform.localScale = new Vector3(-1f, 1f, 1f);
		}
		for (int i = 0; i < nameLabel.Length; i++)
		{
			nameLabel[i].text = ObjectsSpawner.GetCharacterLocalizedName(character);
		}
		topLabelRoot.SetActive(!isBottomLabelActive);
		bottomLabelRoot.SetActive(isBottomLabelActive);
		parentCell.LayoutHandlerObjects.Add(internalLayouter.gameObject);
		Material material = new Material(Shader.Find("tk2d/BlendVertexColorGrayscaleColorize"));
		material.mainTexture = avatarSprite.CurrentSprite.materialInst.mainTexture;
		avatarSprite.CachedRenderer.material = material;
		if (!isNumberActive)
		{
			EnableAvatar(true);
			return;
		}
		int num = SingletonMonoBehaviour<TournamentManager>.Instance.CurrentTournament.currentEnemyNumber - number;
		if (num > 1)
		{
			DefeatAvatar(true);
		}
		else if (num < 1)
		{
			avatarSprite.color = inactiveColor;
			ChangeGrayscaleProperty(1f, 0f);
		}
		else if (num == 1)
		{
			EnableAvatar(true);
		}
	}

	public void EnableAvatar(bool isImmediately = false, Action callback = null)
	{
		if (isImmediately)
		{
			avatarSprite.color = enableColor;
			ChangeGrayscaleProperty(0f, 0f);
			ChangeFlagVisibility(true, null, true);
			if (callback != null)
			{
				callback();
			}
			return;
		}
		tweenColor.beginColor = tweenColor.CurrentColor;
		tweenColor.ResetTween();
		tweenColor.endColor = enableColor;
		tweenColor.animationCurve = enableCurve;
		tweenColor.duration = enableDuration;
		tweenColor.SetOnFinishedDelegate(delegate
		{
			ChangeFlagVisibility(true, delegate
			{
				if (callback != null)
				{
					callback();
				}
			}, isImmediately);
		});
		tweenColor.enabled = true;
		ChangeGrayscaleProperty(0f, enableDuration);
		if (base.gameObject.activeInHierarchy)
		{
			enableAnimation.State.SetAnimation(0, "highlight", false);
			AudioManager.PlayAudioWithKey("vs_screen_open_avatar");
		}
	}

	public void DefeatAvatar(bool isImmediately = false, Action callback = null)
	{
		if (isImmediately)
		{
			avatarSprite.color = defeatedColor;
			defeatedCrossAppearTween.SetEndStateImmediately();
			ChangeFlagVisibility(false, null, true);
			if (callback != null)
			{
				callback();
			}
			return;
		}
		tweenColor.beginColor = tweenColor.CurrentColor;
		tweenColor.ResetTween();
		tweenColor.endColor = defeatedColor;
		tweenColor.animationCurve = defeatedCurve;
		tweenColor.duration = defeatedDuration;
		tweenColor.SetOnFinishedDelegate(delegate
		{
			defeatedCrossAppearTween.animationCurve = defeatedCrossAppearCurve;
			defeatedCrossAppearTween.duration = defeatedCrossAppearDuration;
			defeatedCrossAppearTween.SetOnFinishedDelegate(delegate
			{
				ChangeFlagVisibility(false, delegate
				{
					if (callback != null)
					{
						callback();
					}
				}, isImmediately);
			});
			defeatedCrossAppearTween.enabled = true;
			if (base.gameObject.activeInHierarchy)
			{
				AudioManager.PlayAudioWithKey("vs_screen_defeat_avatar");
			}
		});
		tweenColor.enabled = true;
	}

	private void ChangeGrayscaleProperty(float finishValue, float duration)
	{
		StopAllCoroutines();
		finishValue = Mathf.Clamp01(finishValue);
		grayscaleTimeCounter = 0f;
		if (duration <= 0f || !base.gameObject.activeInHierarchy)
		{
			GrayscaleProperty = finishValue;
			return;
		}
		isIncreaseGrayscaleProperty = GrayscaleProperty < finishValue;
		StartCoroutine(ChangeGrayscaleCoroutine(GrayscaleProperty, finishValue, duration));
	}

	private void ChangeFlagVisibility(bool isVisible, Action onFinish, bool isImmediately = false)
	{
		for (int i = 0; i < flagAppearTween.Length; i++)
		{
			TweenScale tweenScale = flagAppearTween[i];
			if (isImmediately)
			{
				if ((isVisible && tweenScale.IsReversed) || (!isVisible && !tweenScale.IsReversed))
				{
					tweenScale.SetBeginStateImmediately();
				}
				else
				{
					tweenScale.SetEndStateImmediately();
				}
				if (onFinish != null)
				{
					onFinish();
				}
				continue;
			}
			tweenScale.IsReversed = !isVisible;
			tweenScale.ResetTween();
			tweenScale.duration = ((!isVisible) ? flagDisappearDuration : flagAppearDuration);
			tweenScale.animationCurve = ((!isVisible) ? flagDisappearCurve : flagAppearCurve);
			tweenScale.SetOnFinishedDelegate(delegate
			{
				if (onFinish != null)
				{
					onFinish();
				}
			});
			tweenScale.enabled = true;
		}
	}

	private IEnumerator ChangeGrayscaleCoroutine(float startValue, float finishValue, float duration)
	{
		while (grayscaleTimeCounter < duration)
		{
			grayscaleTimeCounter += Time.deltaTime;
			float coeff = Mathf.Clamp01(grayscaleTimeCounter / duration);
			GrayscaleProperty = startValue + (finishValue - startValue) * coeff;
			yield return null;
		}
	}
}
