using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FatalityDollarsPiles : FatalityBricks
{
	[SerializeField]
	private FatalityBrick anotherKindOfPile;

	[SerializeField]
	[ResourceLink]
	private AssetLink moneyFlightEffectPrefab;

	[SerializeField]
	private int pilesCountX;

	[SerializeField]
	private int pilesCountY;

	[SerializeField]
	private float offsetBetweenPiles;

	[SerializeField]
	private float delayBeforeMainPile;

	[SerializeField]
	private float delayBetweenMainPile;

	[SerializeField]
	private float explodeCharacterForce;

	private bool isOnceCollided;

	public override void StartFatalityBrick(GameObject bricksParent)
	{
		StartCoroutine(StartMoneyFalling(bricksParent));
	}

	private IEnumerator StartMoneyFalling(GameObject bricksParent)
	{
		yield return new WaitForSeconds(delayBeforeMainPile);
		if (moneyFlightEffectPrefab != null && !string.IsNullOrEmpty(moneyFlightEffectPrefab.assetGUID))
		{
			GameObject gameObject = (GameObject)moneyFlightEffectPrefab.GetInstance();
			gameObject.transform.parent = base.transform;
			gameObject.transform.localPosition = Vector3.up * 500f;
		}
		yield return new WaitForSeconds(delayBetweenMainPile);
		_003CStartFatalityBrick_003E__BaseCallProxy0(bricksParent);
	}

	private void ObserverPhysics_EventCollisionEnter(Collision coll)
	{
		if (!isOnceCollided)
		{
			AudioManager.PlayAudioWithKey("mjolnir_ground_collision");
			isOnceCollided = true;
		}
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private void _003CStartFatalityBrick_003E__BaseCallProxy0(GameObject bricksParent)
	{
		base.StartFatalityBrick(bricksParent);
	}
}
