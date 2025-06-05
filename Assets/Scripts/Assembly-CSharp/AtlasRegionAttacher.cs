using System;
using System.Collections;
using Spine;
using UnityEngine;

public class AtlasRegionAttacher : MonoBehaviour
{
	[Serializable]
	public class SlotRegionPair
	{
		[SpineSlot("", "", false)]
		public string slot;

		[SpineAtlasRegion]
		public string region;
	}

	public AtlasAsset atlasAsset;

	public SlotRegionPair[] attachments;

	private Atlas atlas;

	private void Awake()
	{
		SkeletonRenderer component = GetComponent<SkeletonRenderer>();
		component.OnReset = (SkeletonRenderer.SkeletonRendererDelegate)Delegate.Combine(component.OnReset, new SkeletonRenderer.SkeletonRendererDelegate(Apply));
	}

	private void Apply(SkeletonRenderer skeletonRenderer)
	{
		atlas = atlasAsset.GetAtlas();
		AtlasAttachmentLoader atlasAttachmentLoader = new AtlasAttachmentLoader(atlas);
		float scale = skeletonRenderer.SkeletonDataAsset.Scale;
		IEnumerator enumerator = attachments.GetEnumerator();
		while (enumerator.MoveNext())
		{
			SlotRegionPair slotRegionPair = (SlotRegionPair)enumerator.Current;
			RegionAttachment regionAttachment = atlasAttachmentLoader.NewRegionAttachment(null, slotRegionPair.region, slotRegionPair.region);
			regionAttachment.Width = regionAttachment.RegionOriginalWidth * scale;
			regionAttachment.Height = regionAttachment.RegionOriginalHeight * scale;
			regionAttachment.SetColor(new Color(1f, 1f, 1f, 1f));
			regionAttachment.UpdateOffset();
			Slot slot = skeletonRenderer.skeleton.FindSlot(slotRegionPair.slot);
			slot.Attachment = regionAttachment;
		}
	}
}
