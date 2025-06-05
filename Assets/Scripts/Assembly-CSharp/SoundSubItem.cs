using System;
using UnityEngine;

[Serializable]
public class SoundSubItem
{
	public AudioClip clip;

	public AssetLink clipref;

	public float Probability = 1f;

	public float Volume = 1f;

	public float PitchShift;

	public float Pan2D;

	public float Delay;

	public float RandomPitch;

	public float RandomVolume;

	public float RandomDelay;

	public float FadeIn;

	public float FadeOut;

	public bool RandomStartPosition;

	[NonSerialized]
	public SoundItem Item;

	private float _summedProbability = -1f;

	public string Name
	{
		get
		{
			return clipref.name;
		}
	}

	public SoundClipType SubItemType
	{
		get
		{
			if (clip != null)
			{
				return SoundClipType.Clip;
			}
			if (clipref != null && clipref.StreamingAssetPath.Length > 0)
			{
				return SoundClipType.StreamingClip;
			}
			return SoundClipType.Unknown;
		}
		set
		{
		}
	}

	public UnityEngine.Object Clip
	{
		get
		{
			if (SubItemType == SoundClipType.Clip)
			{
				return clip;
			}
			if (clipref != null)
			{
				return clipref.GetAsset();
			}
			return null;
		}
		set
		{
			clip = value as AudioClip;
			clipref = new AssetLink(value);
		}
	}

	internal float SummedProbability
	{
		get
		{
			return _summedProbability;
		}
		set
		{
			_summedProbability = value;
		}
	}

	internal float GetPitch()
	{
		float num = SoundUtility.TransformPitch(PitchShift);
		if (RandomPitch != 0f)
		{
			num *= SoundUtility.TransformPitch(UnityEngine.Random.Range(0f - RandomPitch, RandomPitch));
		}
		return num;
	}

	internal void Initialize(SoundItem item)
	{
		Item = item;
	}

	public override string ToString()
	{
		return base.ToString();
	}
}
