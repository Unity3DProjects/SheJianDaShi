using System;
using System.Collections.Generic;
using Spine;
using UnityEngine;

[ExecuteInEditMode]
public class BoundingBoxFollower : MonoBehaviour
{
	public SkeletonRenderer skeletonRenderer;

	[SpineSlot("", "skeletonRenderer", true)]
	public string slotName;

	[Tooltip("LOL JK, Someone else do it!")]
	public bool use3DMeshCollider;

	private Slot slot;

	private BoundingBoxAttachment currentAttachment;

	private PolygonCollider2D currentCollider;

	private string currentAttachmentName;

	private bool valid;

	private bool hasReset;

	public Dictionary<BoundingBoxAttachment, PolygonCollider2D> colliderTable = new Dictionary<BoundingBoxAttachment, PolygonCollider2D>();

	public Dictionary<BoundingBoxAttachment, string> attachmentNameTable = new Dictionary<BoundingBoxAttachment, string>();

	public string CurrentAttachmentName
	{
		get
		{
			return currentAttachmentName;
		}
	}

	public BoundingBoxAttachment CurrentAttachment
	{
		get
		{
			return currentAttachment;
		}
	}

	public PolygonCollider2D CurrentCollider
	{
		get
		{
			return currentCollider;
		}
	}

	public Slot Slot
	{
		get
		{
			return slot;
		}
	}

	private void OnEnable()
	{
		ClearColliders();
		if (skeletonRenderer == null)
		{
			skeletonRenderer = GetComponentInParent<SkeletonRenderer>();
		}
		if (skeletonRenderer != null)
		{
			SkeletonRenderer obj = skeletonRenderer;
			obj.OnReset = (SkeletonRenderer.SkeletonRendererDelegate)Delegate.Remove(obj.OnReset, new SkeletonRenderer.SkeletonRendererDelegate(HandleReset));
			SkeletonRenderer obj2 = skeletonRenderer;
			obj2.OnReset = (SkeletonRenderer.SkeletonRendererDelegate)Delegate.Combine(obj2.OnReset, new SkeletonRenderer.SkeletonRendererDelegate(HandleReset));
			if (hasReset)
			{
				HandleReset(skeletonRenderer);
			}
		}
	}

	private void OnDisable()
	{
		SkeletonRenderer obj = skeletonRenderer;
		obj.OnReset = (SkeletonRenderer.SkeletonRendererDelegate)Delegate.Remove(obj.OnReset, new SkeletonRenderer.SkeletonRendererDelegate(HandleReset));
	}

	private void Start()
	{
		if (!hasReset && skeletonRenderer != null)
		{
			HandleReset(skeletonRenderer);
		}
	}

	public void HandleReset(SkeletonRenderer renderer)
	{
		if (slotName == null || slotName == string.Empty)
		{
			return;
		}
		hasReset = true;
		ClearColliders();
		colliderTable.Clear();
		if (skeletonRenderer.skeleton == null)
		{
			SkeletonRenderer obj = skeletonRenderer;
			obj.OnReset = (SkeletonRenderer.SkeletonRendererDelegate)Delegate.Remove(obj.OnReset, new SkeletonRenderer.SkeletonRendererDelegate(HandleReset));
			skeletonRenderer.Reset();
			SkeletonRenderer obj2 = skeletonRenderer;
			obj2.OnReset = (SkeletonRenderer.SkeletonRendererDelegate)Delegate.Combine(obj2.OnReset, new SkeletonRenderer.SkeletonRendererDelegate(HandleReset));
		}
		Skeleton skeleton = skeletonRenderer.skeleton;
		slot = skeleton.FindSlot(slotName);
		int slotIndex = skeleton.FindSlotIndex(slotName);
		foreach (Skin skin in skeleton.Data.Skins)
		{
			List<string> list = new List<string>();
			skin.FindNamesForSlot(slotIndex, list);
			foreach (string item in list)
			{
				Attachment attachment = skin.GetAttachment(slotIndex, item);
				if (attachment is BoundingBoxAttachment)
				{
					PolygonCollider2D polygonCollider2D = SkeletonUtility.AddBoundingBox2DAsComponent((BoundingBoxAttachment)attachment, base.gameObject);
					polygonCollider2D.enabled = false;
					polygonCollider2D.hideFlags = HideFlags.HideInInspector;
					colliderTable.Add((BoundingBoxAttachment)attachment, polygonCollider2D);
					attachmentNameTable.Add((BoundingBoxAttachment)attachment, item);
				}
			}
		}
		if (colliderTable.Count == 0)
		{
			valid = false;
		}
		else
		{
			valid = true;
		}
		if (!valid)
		{
			Debug.LogWarning("Bounding Box Follower not valid! Slot [" + slotName + "] does not contain any Bounding Box Attachments!");
		}
	}

	private void ClearColliders()
	{
		PolygonCollider2D[] components = GetComponents<PolygonCollider2D>();
		if (Application.isPlaying)
		{
			PolygonCollider2D[] array = components;
			foreach (PolygonCollider2D obj in array)
			{
				UnityEngine.Object.Destroy(obj);
			}
		}
		else
		{
			PolygonCollider2D[] array2 = components;
			foreach (PolygonCollider2D obj2 in array2)
			{
				UnityEngine.Object.DestroyImmediate(obj2);
			}
		}
		colliderTable.Clear();
		attachmentNameTable.Clear();
	}

	private void LateUpdate()
	{
		if (skeletonRenderer.valid && slot != null && slot.Attachment != currentAttachment)
		{
			SetCurrent((BoundingBoxAttachment)slot.Attachment);
		}
	}

	private void SetCurrent(BoundingBoxAttachment attachment)
	{
		if ((bool)currentCollider)
		{
			currentCollider.enabled = false;
		}
		if (attachment != null)
		{
			currentCollider = colliderTable[attachment];
			currentCollider.enabled = true;
		}
		else
		{
			currentCollider = null;
		}
		currentAttachment = attachment;
		currentAttachmentName = ((currentAttachment != null) ? attachmentNameTable[attachment] : null);
	}
}
