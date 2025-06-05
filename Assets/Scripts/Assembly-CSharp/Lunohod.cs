using Spine;
using UnityEngine;

public class Lunohod : MonoBehaviour
{
	[SerializeField]
	private EffectSpawner dustEffectSpawnerLeft;

	[SerializeField]
	private EffectSpawner dustEffectSpawnerRight;

	private SkeletonAnimation lunohodAnimation;

	private EventData cachedEventStartRight;

	private EventData cachedEventStartLeft;

	private EventData cachedEventStopRight;

	private EventData cachedEventStopLeft;

	private EventData CachedEventStartRight
	{
		get
		{
			if (cachedEventStartRight == null)
			{
				cachedEventStartRight = lunohodAnimation.Skeleton.Data.FindEvent("Start_right");
			}
			return cachedEventStartRight;
		}
	}

	private EventData CachedEventStartLeft
	{
		get
		{
			if (cachedEventStartLeft == null)
			{
				cachedEventStartLeft = lunohodAnimation.Skeleton.Data.FindEvent("Start_left");
			}
			return cachedEventStartLeft;
		}
	}

	private EventData CachedEventStopRight
	{
		get
		{
			if (cachedEventStopRight == null)
			{
				cachedEventStopRight = lunohodAnimation.Skeleton.Data.FindEvent("Stop_right");
			}
			return cachedEventStopRight;
		}
	}

	private EventData CachedEventStopLeft
	{
		get
		{
			if (cachedEventStopLeft == null)
			{
				cachedEventStopLeft = lunohodAnimation.Skeleton.Data.FindEvent("Stop_left");
			}
			return cachedEventStopLeft;
		}
	}

	private void Awake()
	{
		lunohodAnimation = GetComponent<SkeletonAnimation>();
		base.gameObject.SetActive(false);
		SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
		{
			base.gameObject.SetActive(true);
			StopEffect();
		}, 0f);
	}

	private void OnEnable()
	{
		if (lunohodAnimation != null)
		{
			lunohodAnimation.State.Event += LunohodAnimation_State_Event;
		}
	}

	private void OnDisable()
	{
		if (lunohodAnimation != null)
		{
			lunohodAnimation.State.Event -= LunohodAnimation_State_Event;
		}
	}

	private void StartEffect(bool isLeft)
	{
		if (isLeft && dustEffectSpawnerLeft != null)
		{
			dustEffectSpawnerLeft.gameObject.SetActive(true);
		}
		else if (!isLeft && dustEffectSpawnerRight != null)
		{
			dustEffectSpawnerRight.gameObject.SetActive(true);
		}
	}

	private void StopEffect()
	{
		if (dustEffectSpawnerLeft != null)
		{
			dustEffectSpawnerLeft.gameObject.SetActive(false);
		}
		if (dustEffectSpawnerRight != null)
		{
			dustEffectSpawnerRight.gameObject.SetActive(false);
		}
	}

	private void LunohodAnimation_State_Event(Spine.AnimationState state, int trackIndex, Spine.Event e)
	{
		if (e.Data == CachedEventStartRight)
		{
			StartEffect(false);
		}
		if (e.Data == CachedEventStartLeft)
		{
			StartEffect(true);
		}
		if (e.Data == CachedEventStopRight)
		{
			StopEffect();
		}
		if (e.Data == CachedEventStopLeft)
		{
			StopEffect();
		}
	}
}
