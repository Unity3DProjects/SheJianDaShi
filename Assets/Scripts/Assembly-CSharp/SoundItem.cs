using System;
using UnityEngine;

[Serializable]
public class SoundItem
{
	public const float MIN_TIME_BETWEEN_PLAY_CALLS = 0.05f;

	public string Name;

	public SoundLoopMode Loop;

	public int loopSequenceCount;

	public bool DestroyOnLoad = true;

	public float Volume = 1f;

	public SoundPickSubItemMode SubItemPickMode = SoundPickSubItemMode.RandomNotSameTwice;

	public int MaxInstanceCount;

	public bool SkipWhenExeeded;

	public float Delay;

	public float OverlapTime;

	public float MinTimeBetweenPlays = 0.05f;

	public bool overrideAudioSourceSettings;

	public float audioSource_MinDistance = 1f;

	public float audioSource_MaxDistance = 500f;

	[SerializeField]
	public SoundSettings SoundSettingsOverride;

	public SoundSubItem[] subItems;

	internal int lastChosen = -1;

	internal double lastPlayedTime = -1.0;

	[NonSerialized]
	private SoundsCategory category;

	public SoundsCategory Category
	{
		get
		{
			return category;
		}
		private set
		{
			category = value;
		}
	}

	private void Awake()
	{
		lastChosen = -1;
	}

	internal void Initialize(SoundsCategory categ)
	{
		Category = categ;
		for (int i = 0; i < subItems.Length; i++)
		{
			subItems[i].Initialize(this);
		}
		ComputeNormalizedProbability();
	}

	private void ComputeNormalizedProbability()
	{
		float num = 0f;
		int num2 = 0;
		for (int i = 0; i < subItems.Length; i++)
		{
			SoundSubItem soundSubItem = subItems[i];
			num += soundSubItem.Probability;
			num2++;
		}
		float num3 = 0f;
		for (int j = 0; j < subItems.Length; j++)
		{
			SoundSubItem soundSubItem2 = subItems[j];
			num3 = (soundSubItem2.SummedProbability = num3 + soundSubItem2.Probability / num);
		}
	}

	public void UnloadAudioClip()
	{
	}

	public SoundSubItem[] ChooseSubItems(SoundPickSubItemMode pickMode)
	{
		if (subItems == null)
		{
			return null;
		}
		int num = subItems.Length;
		if (num == 0)
		{
			return null;
		}
		int num2 = 0;
		if (num > 1)
		{
			switch (pickMode)
			{
			case SoundPickSubItemMode.Disabled:
				return null;
			case SoundPickSubItemMode.Random:
				num2 = ChooseRandomSubitem(true);
				break;
			case SoundPickSubItemMode.RandomNotSameTwice:
				num2 = ChooseRandomSubitem(false);
				break;
			}
		}
		lastChosen = num2;
		return new SoundSubItem[1] { subItems[num2] };
	}

	public int ChooseRandomSubitem(bool allowSameElementTwiceInRow)
	{
		int num = subItems.Length;
		int result = 0;
		float num2 = 0f;
		float max;
		if (!allowSameElementTwiceInRow)
		{
			if (lastChosen >= 0)
			{
				num2 = subItems[lastChosen].SummedProbability;
				if (lastChosen >= 1)
				{
					num2 -= subItems[lastChosen - 1].SummedProbability;
				}
			}
			else
			{
				num2 = 0f;
			}
			max = 1f - num2;
		}
		else
		{
			max = 1f;
		}
		float num3 = UnityEngine.Random.Range(0f, max);
		int i;
		for (i = 0; i < num - 1; i++)
		{
			float num4 = subItems[i].SummedProbability;
			if (!allowSameElementTwiceInRow)
			{
				if (i == lastChosen && num4 != 1f)
				{
					continue;
				}
				if (i > lastChosen)
				{
					num4 -= num2;
				}
			}
			if (num4 > num3)
			{
				result = i;
				break;
			}
		}
		if (i == num - 1)
		{
			result = num - 1;
		}
		return result;
	}
}
