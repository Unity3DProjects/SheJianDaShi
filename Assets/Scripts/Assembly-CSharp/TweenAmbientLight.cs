using UnityEngine;

[AddComponentMenu("Inventain/Tween/TweenAmbientLight")]
public class TweenAmbientLight : Tweener
{
	public Color beginColor;

	public Color endColor;

	protected override void TweenUpdateRuntime(float factor, bool isFinished)
	{
		RenderSettings.ambientLight = Color.Lerp(beginColor, endColor, factor);
	}

	public static TweenAmbientLight SetGlobalAmbientColor(GameObject go, Color color, float duration = 1f)
	{
		TweenAmbientLight tweenAmbientLight = Tweener.InitGO<TweenAmbientLight>(go, duration);
		tweenAmbientLight.beginColor = RenderSettings.ambientLight;
		tweenAmbientLight.endColor = color;
		tweenAmbientLight.Play(true);
		return tweenAmbientLight;
	}
}
