using System.Collections;
using Spine;
using UnityEngine;

public class FatalitySpikySeed : FatalityBase
{
	private const string EXPLOSION_SOUND = "blood_fountain_loop";

	private const string GROW_UP_SKELETON = "grow_up_skeleton";

	private const string GROW_UP_SKELETON_BEEFY = "grow_up_skeleton_beefy";

	private const string GROW_UP_SKELETON_PLUMPY = "grow_up_skeleton_plumpy";

	private const string CHARACTER_BREAK_EVENT_NAME = "Break";

	[SerializeField]
	private SkeletonAnimation spikyTreeAnimation;

	[SerializeField]
	private float treeAppearDelay;

	[SerializeField]
	private float bloodEffectsDelay;

	private EventData cachedBreakCharacterEvent;

	private SkeletonAnimation spikyTree;

	private string AnimationName
	{
		get
		{
			string result = string.Empty;
			switch (base.CachedManager.CurrentCharacter.CharacterType)
			{
			case CharacterType.Normal:
				result = "grow_up_skeleton";
				break;
			case CharacterType.Beefy:
				result = "grow_up_skeleton_beefy";
				break;
			case CharacterType.Plumpy:
				result = "grow_up_skeleton_plumpy";
				break;
			}
			return result;
		}
	}

	private EventData CachedBreakCharacterEvent
	{
		get
		{
			if (cachedBreakCharacterEvent == null)
			{
				cachedBreakCharacterEvent = spikyTreeAnimation.Skeleton.Data.FindEvent("Break");
			}
			return cachedBreakCharacterEvent;
		}
	}

	public override void StartFatality(CollisionInfo infoCollision)
	{
		if (!base.IsActive && spikyTreeAnimation != null)
		{
			base.StartFatality(infoCollision);
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				Vector3 position = base.CachedManager.CurrentCharacter.transform.position;
				spikyTree = Object.Instantiate(spikyTreeAnimation.gameObject, position, Quaternion.identity, SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.transform).GetComponent<SkeletonAnimation>();
				spikyTree.State.SetAnimation(0, AnimationName, false);
				spikyTree.State.Event += SpikyTreeAnimation_State_Event;
				StartCoroutine(StartBloodEffects(position));
			}, treeAppearDelay);
			base.CachedManager.CurrentCharacter.SetAnimation(0, "finish_him", true);
		}
	}

	private IEnumerator StartBloodEffects(Vector3 characterPosition)
	{
		yield return new WaitForSeconds(bloodEffectsDelay);
		base.CachedEffect.EmitBloodEffect(base.CachedManager.CurrentCharacter.CharacterType, SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.transform, base.CachedManager.CurrentCharacter.HeadCollider.transform.position, Vector3.zero);
	}

	private void SpikyTreeAnimation_State_Event(Spine.AnimationState state, int trackIndex, Spine.Event e)
	{
		if (e.Data == CachedBreakCharacterEvent)
		{
			MeshRenderer[] componentsInChildren = base.CachedManager.CurrentCharacter.gameObject.GetComponentsInChildren<MeshRenderer>();
			MeshRenderer[] array = componentsInChildren;
			foreach (MeshRenderer meshRenderer in array)
			{
				meshRenderer.enabled = false;
			}
			AudioManager.PlayAudioWithKey("blood_fountain_loop");
		}
	}
}
