using System;
using Spine;
using UnityEngine;

public class CustomSkin : MonoBehaviour
{
	[Serializable]
	public class SkinPair
	{
		[SpineAttachment(false, true, false, "", "skinSource")]
		public string sourceAttachment;

		[SpineSlot("", "", false)]
		public string targetSlot;

		[SpineAttachment(true, false, true, "", "")]
		public string targetAttachment;
	}

	public SkeletonDataAsset skinSource;

	public SkinPair[] skinning;

	public Skin customSkin;

	private SkeletonRenderer skeletonRenderer;

	private void Start()
	{
		skeletonRenderer = GetComponent<SkeletonRenderer>();
		Skeleton skeleton = skeletonRenderer.skeleton;
		customSkin = new Skin("CustomSkin");
		SkinPair[] array = skinning;
		foreach (SkinPair skinPair in array)
		{
			Attachment attachment = SpineAttachment.GetAttachment(skinPair.sourceAttachment, skinSource);
			customSkin.AddAttachment(skeleton.FindSlotIndex(skinPair.targetSlot), skinPair.targetAttachment, attachment);
		}
		skeleton.SetSkin(customSkin);
	}
}
