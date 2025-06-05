using System;
using System.IO;
using Spine;
using UnityEngine;

public class SkeletonDataAsset : ScriptableObject
{
	public AtlasAsset[] atlasAssets;

	public tk2dSpriteCollectionData[] spriteCollections;

	public TextAsset skeletonJSON;

	[SerializeField]
	private float scale = 1f;

	public PlatformScaleType platformScaleType;

	public string[] fromAnimation;

	public string[] toAnimation;

	public float[] duration;

	public float defaultMix;

	public RuntimeAnimatorController controller;

	private SkeletonData skeletonData;

	private AnimationStateData stateData;

	public float Scale
	{
		get
		{
			float num = scale;
			if (platformScaleType != PlatformScaleType.None)
			{
				if ((tk2dSystem.IsRetina && platformScaleType == PlatformScaleType.RetinaUpScale) || (!tk2dSystem.IsRetina && platformScaleType == PlatformScaleType.NonRetinaUpScale))
				{
					num *= 2f;
				}
				else if ((tk2dSystem.IsRetina && platformScaleType == PlatformScaleType.RetinaDownScale) || (!tk2dSystem.IsRetina && platformScaleType == PlatformScaleType.NonRetinaDownScale) || platformScaleType == PlatformScaleType.BothDownScale)
				{
					num *= 0.5f;
				}
			}
			return num;
		}
		set
		{
			scale = value;
		}
	}

	public void Reset()
	{
		skeletonData = null;
		stateData = null;
	}

	public SkeletonData GetSkeletonData(bool quiet)
	{
		if (atlasAssets == null)
		{
			atlasAssets = new AtlasAsset[0];
			if (!quiet)
			{
				Debug.LogError("Atlas not set for SkeletonData asset: " + base.name, this);
			}
			Reset();
			return null;
		}
		if (spriteCollections == null)
		{
			spriteCollections = new tk2dSpriteCollectionData[0];
			if (!quiet)
			{
				Debug.LogError("Atlas not set for SkeletonData asset: " + base.name, this);
			}
			Reset();
			return null;
		}
		if (skeletonJSON == null)
		{
			if (!quiet)
			{
				Debug.LogError("Skeleton JSON file not set for SkeletonData asset: " + base.name, this);
			}
			Reset();
			return null;
		}
		if (atlasAssets.Length == 0 && spriteCollections.Length == 0)
		{
			Reset();
			return null;
		}
		Atlas[] array = new Atlas[atlasAssets.Length];
		for (int i = 0; i < atlasAssets.Length; i++)
		{
			if (atlasAssets[i] == null)
			{
				Reset();
				return null;
			}
			array[i] = atlasAssets[i].GetAtlas();
			if (array[i] == null)
			{
				Reset();
				return null;
			}
		}
		if (skeletonData != null && skeletonData.isValid())
		{
			return skeletonData;
		}
		AttachmentLoader attachmentLoader;
		float num;
		if (spriteCollections.Length > 0)
		{
			attachmentLoader = new SpriteCollectionAttachmentLoader(spriteCollections);
			num = 1f / (spriteCollections[0].inst.invOrthoSize * spriteCollections[0].inst.halfTargetHeight) * Scale;
		}
		else
		{
			if (array.Length == 0)
			{
				Reset();
				if (!quiet)
				{
					Debug.LogError("Atlas not set for SkeletonData asset: " + base.name, this);
				}
				return null;
			}
			attachmentLoader = new AtlasAttachmentLoader(array);
			num = Scale;
		}
		try
		{
			if (skeletonJSON.name.ToLower().Contains(".skel"))
			{
				MemoryStream input = new MemoryStream(skeletonJSON.bytes);
				SkeletonBinary skeletonBinary = new SkeletonBinary(attachmentLoader);
				skeletonBinary.Scale = num;
				skeletonData = skeletonBinary.ReadSkeletonData(input);
			}
			else
			{
				StringReader reader = new StringReader(skeletonJSON.text);
				SkeletonJson skeletonJson = new SkeletonJson(attachmentLoader);
				skeletonJson.Scale = num;
				skeletonData = skeletonJson.ReadSkeletonData(reader);
			}
		}
		catch (Exception ex)
		{
			if (!quiet)
			{
				Debug.LogError("Error reading skeleton JSON file for SkeletonData asset: " + base.name + "\n" + ex.Message + "\n" + ex.StackTrace, this);
			}
			return null;
		}
		stateData = new AnimationStateData(skeletonData);
		FillStateData();
		return skeletonData;
	}

	public void FillStateData()
	{
		if (stateData == null)
		{
			return;
		}
		stateData.DefaultMix = defaultMix;
		int i = 0;
		for (int num = fromAnimation.Length; i < num; i++)
		{
			if (fromAnimation[i].Length != 0 && toAnimation[i].Length != 0)
			{
				stateData.SetMix(fromAnimation[i], toAnimation[i], duration[i]);
			}
		}
	}

	public AnimationStateData GetAnimationStateData()
	{
		if (stateData != null)
		{
			return stateData;
		}
		GetSkeletonData(false);
		return stateData;
	}
}
