using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Spine;
using UnityEngine;

[RequireComponent(typeof(ISkeletonAnimation))]
[ExecuteInEditMode]
public class SkeletonUtility : MonoBehaviour
{
	public delegate void SkeletonUtilityDelegate();

	private const float MIN_THICKNESS_3D = 0.001f;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private SkeletonUtilityDelegate m_OnReset;

	public Transform boneRoot;

	[HideInInspector]
	public SkeletonRenderer skeletonRenderer;

	[HideInInspector]
	public ISkeletonAnimation skeletonAnimation;

	[NonSerialized]
	public List<SkeletonUtilityBone> utilityBones = new List<SkeletonUtilityBone>();

	[NonSerialized]
	public List<SkeletonUtilityConstraint> utilityConstraints = new List<SkeletonUtilityConstraint>();

	[SerializeField]
	private float thickness3DCollider = 0.125f;

	protected bool hasTransformBones;

	protected bool hasUtilityConstraints;

	protected bool needToReprocessBones;

	public float Thickness3DCollider
	{
		get
		{
			return thickness3DCollider;
		}
		set
		{
			thickness3DCollider = ((!(value > 0f)) ? 0.001f : value);
		}
	}

	public event SkeletonUtilityDelegate OnReset
	{
		add
		{
			SkeletonUtilityDelegate skeletonUtilityDelegate = this.m_OnReset;
			SkeletonUtilityDelegate skeletonUtilityDelegate2;
			do
			{
				skeletonUtilityDelegate2 = skeletonUtilityDelegate;
				skeletonUtilityDelegate = Interlocked.CompareExchange(ref this.m_OnReset, (SkeletonUtilityDelegate)Delegate.Combine(skeletonUtilityDelegate2, value), skeletonUtilityDelegate);
			}
			while (skeletonUtilityDelegate != skeletonUtilityDelegate2);
		}
		remove
		{
			SkeletonUtilityDelegate skeletonUtilityDelegate = this.m_OnReset;
			SkeletonUtilityDelegate skeletonUtilityDelegate2;
			do
			{
				skeletonUtilityDelegate2 = skeletonUtilityDelegate;
				skeletonUtilityDelegate = Interlocked.CompareExchange(ref this.m_OnReset, (SkeletonUtilityDelegate)Delegate.Remove(skeletonUtilityDelegate2, value), skeletonUtilityDelegate);
			}
			while (skeletonUtilityDelegate != skeletonUtilityDelegate2);
		}
	}

	public static T GetInParent<T>(Transform origin) where T : Component
	{
		return origin.GetComponentInParent<T>();
	}

	public static PolygonCollider2D AddBoundingBox2D(Skeleton skeleton, string skinName, string slotName, string attachmentName, Transform parent, bool isTrigger = true)
	{
		if (skinName == string.Empty)
		{
			skinName = skeleton.Data.DefaultSkin.Name;
		}
		Skin skin = skeleton.Data.FindSkin(skinName);
		if (skin == null)
		{
			Debug.LogError("Skin " + skinName + " not found!");
			return null;
		}
		Attachment attachment = skin.GetAttachment(skeleton.FindSlotIndex(slotName), attachmentName);
		if (attachment is BoundingBoxAttachment)
		{
			GameObject gameObject = new GameObject("[BoundingBox]" + attachmentName);
			gameObject.transform.parent = parent;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localScale = Vector3.one;
			PolygonCollider2D polygonCollider2D = gameObject.AddComponent<PolygonCollider2D>();
			polygonCollider2D.isTrigger = isTrigger;
			BoundingBoxAttachment boundingBoxAttachment = (BoundingBoxAttachment)attachment;
			float[] vertices = boundingBoxAttachment.Vertices;
			int num = vertices.Length;
			int num2 = num / 2;
			Vector2[] array = new Vector2[num2];
			int num3 = 0;
			int num4 = 0;
			while (num4 < num)
			{
				array[num3].x = vertices[num4];
				array[num3].y = vertices[num4 + 1];
				num4 += 2;
				num3++;
			}
			polygonCollider2D.SetPath(0, array);
			return polygonCollider2D;
		}
		return null;
	}

	public static SkeletonUtilityMeshCollider AddBoundingBox3D(Skeleton skeleton, string skinName, string slotName, string attachmentName, Transform parent, bool isTrigger = true, float depth = 0.125f)
	{
		if (skinName == string.Empty)
		{
			skinName = skeleton.Data.DefaultSkin.Name;
		}
		Skin skin = skeleton.Data.FindSkin(skinName);
		if (skin == null)
		{
			Debug.LogError("Skin " + skinName + " not found!");
			return null;
		}
		Attachment attachment = skin.GetAttachment(skeleton.FindSlotIndex(slotName), attachmentName);
		if (attachment is BoundingBoxAttachment)
		{
			GameObject gameObject = new GameObject("[BoundingBox3D]" + attachmentName);
			gameObject.transform.parent = parent;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localScale = Vector3.one;
			SkeletonUtilityMeshCollider skeletonUtilityMeshCollider = gameObject.AddComponent<SkeletonUtilityMeshCollider>();
			skeletonUtilityMeshCollider.IsTrigger = isTrigger;
			BoundingBoxAttachment boundingBoxAttachment = (BoundingBoxAttachment)attachment;
			float[] vertices = boundingBoxAttachment.Vertices;
			int num = vertices.Length;
			int num2 = num / 2;
			Vector2[] array = new Vector2[num2];
			int num3 = 0;
			int num4 = 0;
			while (num4 < num)
			{
				array[num3].x = vertices[num4];
				array[num3].y = vertices[num4 + 1];
				num4 += 2;
				num3++;
			}
			skeletonUtilityMeshCollider.CachedMesh = SkeletonUtilityTriangulator.CreateMesh(array, depth);
			return skeletonUtilityMeshCollider;
		}
		return null;
	}

	public static SkeletonUtilityMeshCollider AddBoundingBox3DAsComponent(BoundingBoxAttachment boundingBox, GameObject gameObject, bool isTrigger = true, float depth = 0.125f)
	{
		if (boundingBox == null)
		{
			return null;
		}
		SkeletonUtilityMeshCollider skeletonUtilityMeshCollider = gameObject.AddComponent<SkeletonUtilityMeshCollider>();
		skeletonUtilityMeshCollider.IsTrigger = isTrigger;
		float[] vertices = boundingBox.Vertices;
		int num = vertices.Length;
		int num2 = num / 2;
		Vector2[] array = new Vector2[num2];
		int num3 = 0;
		int num4 = 0;
		while (num4 < num)
		{
			array[num3].x = vertices[num4];
			array[num3].y = vertices[num4 + 1];
			num4 += 2;
			num3++;
		}
		skeletonUtilityMeshCollider.CachedMesh = SkeletonUtilityTriangulator.CreateMesh(array, depth);
		return skeletonUtilityMeshCollider;
	}

	public static PolygonCollider2D AddBoundingBox2DAsComponent(BoundingBoxAttachment boundingBox, GameObject gameObject, bool isTrigger = true)
	{
		if (boundingBox == null)
		{
			return null;
		}
		PolygonCollider2D polygonCollider2D = gameObject.AddComponent<PolygonCollider2D>();
		polygonCollider2D.isTrigger = isTrigger;
		float[] vertices = boundingBox.Vertices;
		int num = vertices.Length;
		int num2 = num / 2;
		Vector2[] array = new Vector2[num2];
		int num3 = 0;
		int num4 = 0;
		while (num4 < num)
		{
			array[num3].x = vertices[num4];
			array[num3].y = vertices[num4 + 1];
			num4 += 2;
			num3++;
		}
		polygonCollider2D.SetPath(0, array);
		return polygonCollider2D;
	}

	public static Bounds GetBoundingBoxBounds(BoundingBoxAttachment boundingBox, float depth = 0f)
	{
		float[] vertices = boundingBox.Vertices;
		int num = vertices.Length;
		Bounds result = new Bounds
		{
			center = new Vector3(vertices[0], vertices[1], 0f)
		};
		for (int i = 2; i < num; i += 2)
		{
			result.Encapsulate(new Vector3(vertices[i], vertices[i + 1], 0f));
		}
		Vector3 size = result.size;
		size.z = depth;
		result.size = size;
		return result;
	}

	public static Mesh GetBoundingBoxMesh(BoundingBoxAttachment boundingBox, float depth = 0.125f)
	{
		float[] vertices = boundingBox.Vertices;
		int num = vertices.Length;
		int num2 = num / 2;
		Vector2[] array = new Vector2[num2];
		int num3 = 0;
		int num4 = 0;
		while (num4 < num)
		{
			array[num3].x = vertices[num4];
			array[num3].y = vertices[num4 + 1];
			num4 += 2;
			num3++;
		}
		return SkeletonUtilityTriangulator.CreateMesh(array, depth);
	}

	private void Update()
	{
		if (boneRoot != null && skeletonRenderer.skeleton != null)
		{
			Vector3 one = Vector3.one;
			if (skeletonRenderer.skeleton.FlipX)
			{
				one.x = -1f;
			}
			if (skeletonRenderer.skeleton.FlipY)
			{
				one.y = -1f;
			}
			boneRoot.localScale = one;
		}
	}

	private void OnEnable()
	{
		if (skeletonRenderer == null)
		{
			skeletonRenderer = GetComponent<SkeletonRenderer>();
		}
		if (skeletonAnimation == null)
		{
			skeletonAnimation = GetComponent<SkeletonAnimation>();
			if (skeletonAnimation == null)
			{
				skeletonAnimation = GetComponent<SkeletonAnimator>();
			}
		}
		SkeletonRenderer obj = skeletonRenderer;
		obj.OnReset = (SkeletonRenderer.SkeletonRendererDelegate)Delegate.Remove(obj.OnReset, new SkeletonRenderer.SkeletonRendererDelegate(HandleRendererReset));
		SkeletonRenderer obj2 = skeletonRenderer;
		obj2.OnReset = (SkeletonRenderer.SkeletonRendererDelegate)Delegate.Combine(obj2.OnReset, new SkeletonRenderer.SkeletonRendererDelegate(HandleRendererReset));
		if (skeletonAnimation != null)
		{
			skeletonAnimation.UpdateLocal -= UpdateLocal;
			skeletonAnimation.UpdateLocal += UpdateLocal;
		}
		CollectBones();
	}

	private void Start()
	{
	}

	private void OnDisable()
	{
		SkeletonRenderer obj = skeletonRenderer;
		obj.OnReset = (SkeletonRenderer.SkeletonRendererDelegate)Delegate.Remove(obj.OnReset, new SkeletonRenderer.SkeletonRendererDelegate(HandleRendererReset));
		if (skeletonAnimation != null)
		{
			skeletonAnimation.UpdateLocal -= UpdateLocal;
			skeletonAnimation.UpdateWorld -= UpdateWorld;
			skeletonAnimation.UpdateComplete -= UpdateComplete;
		}
	}

	private void HandleRendererReset(SkeletonRenderer r)
	{
		if (this.OnReset != null)
		{
			this.OnReset();
		}
		CollectBones();
	}

	public void RegisterBone(SkeletonUtilityBone bone)
	{
		if (!utilityBones.Contains(bone))
		{
			utilityBones.Add(bone);
			needToReprocessBones = true;
		}
	}

	public void UnregisterBone(SkeletonUtilityBone bone)
	{
		utilityBones.Remove(bone);
	}

	public void RegisterConstraint(SkeletonUtilityConstraint constraint)
	{
		if (!utilityConstraints.Contains(constraint))
		{
			utilityConstraints.Add(constraint);
			needToReprocessBones = true;
		}
	}

	public void UnregisterConstraint(SkeletonUtilityConstraint constraint)
	{
		utilityConstraints.Remove(constraint);
	}

	public void CollectBones()
	{
		if (skeletonRenderer.skeleton == null)
		{
			return;
		}
		if (boneRoot != null)
		{
			List<string> list = new List<string>();
			ExposedList<IkConstraint> ikConstraints = skeletonRenderer.skeleton.IkConstraints;
			int i = 0;
			for (int count = ikConstraints.Count; i < count; i++)
			{
				list.Add(ikConstraints.Items[i].Target.Data.Name);
			}
			foreach (SkeletonUtilityBone utilityBone in utilityBones)
			{
				if (utilityBone.bone == null)
				{
					return;
				}
				if (utilityBone.mode == SkeletonUtilityBone.Mode.Override)
				{
					hasTransformBones = true;
				}
				if (list.Contains(utilityBone.bone.Data.Name))
				{
					hasUtilityConstraints = true;
				}
			}
			if (utilityConstraints.Count > 0)
			{
				hasUtilityConstraints = true;
			}
			if (skeletonAnimation != null)
			{
				skeletonAnimation.UpdateWorld -= UpdateWorld;
				skeletonAnimation.UpdateComplete -= UpdateComplete;
				if (hasTransformBones || hasUtilityConstraints)
				{
					skeletonAnimation.UpdateWorld += UpdateWorld;
				}
				if (hasUtilityConstraints)
				{
					skeletonAnimation.UpdateComplete += UpdateComplete;
				}
			}
			needToReprocessBones = false;
		}
		else
		{
			utilityBones.Clear();
			utilityConstraints.Clear();
		}
	}

	private void UpdateLocal(SkeletonRenderer anim)
	{
		if (needToReprocessBones)
		{
			CollectBones();
		}
		if (utilityBones == null)
		{
			return;
		}
		foreach (SkeletonUtilityBone utilityBone in utilityBones)
		{
			utilityBone.transformLerpComplete = false;
		}
		UpdateAllBones();
	}

	private void UpdateWorld(SkeletonRenderer anim)
	{
		UpdateAllBones();
		foreach (SkeletonUtilityConstraint utilityConstraint in utilityConstraints)
		{
			utilityConstraint.DoUpdate();
		}
	}

	private void UpdateComplete(SkeletonRenderer anim)
	{
		UpdateAllBones();
	}

	private void UpdateAllBones()
	{
		if (boneRoot == null)
		{
			CollectBones();
		}
		if (utilityBones == null)
		{
			return;
		}
		foreach (SkeletonUtilityBone utilityBone in utilityBones)
		{
			utilityBone.DoUpdate();
		}
	}

	public Transform GetBoneRoot()
	{
		if (boneRoot != null)
		{
			return boneRoot;
		}
		boneRoot = new GameObject("SkeletonUtility-Root").transform;
		boneRoot.parent = base.transform;
		if ((bool)boneRoot.gameObject && (bool)base.gameObject)
		{
			boneRoot.gameObject.layer = base.gameObject.layer;
		}
		boneRoot.localPosition = Vector3.zero;
		boneRoot.localRotation = Quaternion.identity;
		boneRoot.localScale = Vector3.one;
		return boneRoot;
	}

	public GameObject SpawnRoot(SkeletonUtilityBone.Mode mode, bool pos, bool rot, bool sca)
	{
		GetBoneRoot();
		Skeleton skeleton = skeletonRenderer.skeleton;
		GameObject result = SpawnBone(skeleton.RootBone, boneRoot, mode, pos, rot, sca);
		CollectBones();
		return result;
	}

	public GameObject SpawnHierarchy(SkeletonUtilityBone.Mode mode, bool pos, bool rot, bool sca)
	{
		GetBoneRoot();
		Skeleton skeleton = skeletonRenderer.skeleton;
		GameObject result = SpawnBoneRecursively(skeleton.RootBone, boneRoot, mode, pos, rot, sca);
		CollectBones();
		return result;
	}

	public GameObject SpawnBoneRecursively(Bone bone, Transform parent, SkeletonUtilityBone.Mode mode, bool pos, bool rot, bool sca)
	{
		GameObject gameObject = SpawnBone(bone, parent, mode, pos, rot, sca);
		ExposedList<Bone> children = bone.Children;
		int i = 0;
		for (int count = children.Count; i < count; i++)
		{
			Bone bone2 = children.Items[i];
			SpawnBoneRecursively(bone2, gameObject.transform, mode, pos, rot, sca);
		}
		return gameObject;
	}

	public GameObject SpawnBone(Bone bone, Transform parent, SkeletonUtilityBone.Mode mode, bool pos, bool rot, bool sca)
	{
		GameObject gameObject = new GameObject(bone.Data.Name);
		gameObject.transform.parent = parent;
		if ((bool)parent && (bool)parent.gameObject)
		{
			gameObject.layer = parent.gameObject.layer;
		}
		SkeletonUtilityBone skeletonUtilityBone = gameObject.AddComponent<SkeletonUtilityBone>();
		skeletonUtilityBone.skeletonUtility = this;
		skeletonUtilityBone.position = pos;
		skeletonUtilityBone.rotation = rot;
		skeletonUtilityBone.scale = sca;
		skeletonUtilityBone.mode = mode;
		skeletonUtilityBone.zPosition = true;
		skeletonUtilityBone.Reset();
		skeletonUtilityBone.bone = bone;
		skeletonUtilityBone.boneName = bone.Data.Name;
		skeletonUtilityBone.valid = true;
		if (mode == SkeletonUtilityBone.Mode.Override)
		{
			if (rot)
			{
				gameObject.transform.localRotation = Quaternion.Euler(0f, 0f, skeletonUtilityBone.bone.RotationIK);
			}
			if (pos)
			{
				gameObject.transform.localPosition = new Vector3(skeletonUtilityBone.bone.X, skeletonUtilityBone.bone.Y, 0f);
			}
			gameObject.transform.localScale = new Vector3(skeletonUtilityBone.bone.scaleX, skeletonUtilityBone.bone.scaleY, 0f);
		}
		return gameObject;
	}

	public void SpawnSubRenderers(bool disablePrimaryRenderer)
	{
		Renderer component = GetComponent<Renderer>();
		MeshFilter component2 = GetComponent<MeshFilter>();
		if (!component2)
		{
			return;
		}
		int subMeshCount = component2.sharedMesh.subMeshCount;
		for (int i = 0; i < subMeshCount; i++)
		{
			GameObject gameObject = new GameObject("Submesh " + i, typeof(MeshFilter), typeof(MeshRenderer));
			gameObject.transform.parent = base.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localScale = Vector3.one;
			if ((bool)base.gameObject)
			{
				gameObject.layer = base.gameObject.layer;
			}
			SkeletonUtilitySubmeshRenderer skeletonUtilitySubmeshRenderer = gameObject.AddComponent<SkeletonUtilitySubmeshRenderer>();
			Renderer component3 = skeletonUtilitySubmeshRenderer.GetComponent<Renderer>();
			component3.sortingLayerName = component.sortingLayerName;
			component3.sortingOrder = i * 10;
			skeletonUtilitySubmeshRenderer.submeshIndex = i;
		}
		skeletonRenderer.CollectSubmeshRenderers();
		if (disablePrimaryRenderer)
		{
			component.enabled = false;
		}
	}

	public GameObject SpawnHierarchyWithBoundingBox(bool is2D, SkeletonUtilityBone.Mode mode, bool pos, bool rot, bool sca)
	{
		GetBoneRoot();
		Skeleton skeleton = skeletonRenderer.skeleton;
		GameObject result = SpawnBoneRecursivelyWithBoundingBox(is2D, skeleton.RootBone, boneRoot, mode, pos, rot, sca);
		CollectBones();
		return result;
	}

	public GameObject SpawnBoneRecursivelyWithBoundingBox(bool is2D, Bone bone, Transform parent, SkeletonUtilityBone.Mode mode, bool pos, bool rot, bool sca)
	{
		GameObject gameObject = SpawnBone(bone, parent, mode, pos, rot, sca);
		ExposedList<Bone> children = bone.Children;
		int i = 0;
		for (int count = children.Count; i < count; i++)
		{
			Bone bone2 = children.Items[i];
			SpawnBoneRecursivelyWithBoundingBox(is2D, bone2, gameObject.transform, mode, pos, rot, sca);
		}
		if (is2D)
		{
			SpawnBoundingBox2D(bone, gameObject.transform);
		}
		else
		{
			SpawnBoundingBox3D(bone, gameObject.transform);
		}
		return gameObject;
	}

	public SkeletonUtilityMeshCollider SpawnBoundingBox3D(Bone bone, Transform parent)
	{
		SkeletonUtilityMeshCollider result = null;
		Skeleton skeleton = skeletonRenderer.skeleton;
		Skin skin = skeleton.Skin ?? skeleton.Data.DefaultSkin;
		List<Attachment> list = new List<Attachment>();
		foreach (Slot slot in skeleton.Slots)
		{
			if (slot.bone != bone)
			{
				continue;
			}
			skin.FindAttachmentsForSlot(skeleton.Slots.IndexOf(slot), list);
			foreach (Attachment item in list)
			{
				BoundingBoxAttachment boundingBoxAttachment = item as BoundingBoxAttachment;
				if (boundingBoxAttachment != null)
				{
					result = AddBoundingBox3D(skeleton, skin.Name, slot.ToString(), item.Name, parent);
				}
			}
		}
		return result;
	}

	public PolygonCollider2D SpawnBoundingBox2D(Bone bone, Transform parent)
	{
		PolygonCollider2D result = null;
		Skeleton skeleton = skeletonRenderer.skeleton;
		Skin skin = skeleton.Skin ?? skeleton.Data.DefaultSkin;
		List<Attachment> list = new List<Attachment>();
		foreach (Slot slot in skeleton.Slots)
		{
			if (slot.bone != bone)
			{
				continue;
			}
			skin.FindAttachmentsForSlot(skeleton.Slots.IndexOf(slot), list);
			foreach (Attachment item in list)
			{
				BoundingBoxAttachment boundingBoxAttachment = item as BoundingBoxAttachment;
				if (boundingBoxAttachment != null)
				{
					result = AddBoundingBox2D(skeleton, skin.Name, slot.ToString(), item.Name, parent);
				}
			}
		}
		return result;
	}
}
