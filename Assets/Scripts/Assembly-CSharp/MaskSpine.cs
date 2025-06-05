using System;
using System.Collections.Generic;
using Spine;
using UnityEngine;

public class MaskSpine : MonoBehaviour
{
	[SerializeField]
	private Shader shaderMask;

	[SerializeField]
	private SkeletonAnimation skeletonAnimation;

	[SpineSlot("", "", false, dataField = "skeletonAnimation")]
	[SerializeField]
	private string slotName;

	[SerializeField]
	private bool isAnimationUpdateMask;

	[SerializeField]
	private List<MaskedSpine> fixedMaskedFill = new List<MaskedSpine>();

	[SerializeField]
	private List<MaskedSpine> fixedMaskedCut = new List<MaskedSpine>();

	private MaskSpineProperty cachedPropertyMain;

	private MaskSpineProperty[] cachedPropertyMaskedFill = new MaskSpineProperty[2];

	private MaskSpineProperty[] cachedPropertyMaskedCut = new MaskSpineProperty[2];

	private Slot cachedSlot;

	private Bone cachedBone;

	private SkeletonUtilityBone cachedUtilityBone;

	private Transform cachedTransform;

	private Transform cachedUtilityBoneTransform;

	private Transform cachedSkeletonTransform;

	private float[] tempVert = new float[8];

	private bool isInitOnAwake;

	private bool isInit;

	private bool isLoad;

	private Material defaultMaterial;

	private RegionAttachment defaultAttachment;

	private RegionAttachment newMaskAttachment;

	private Material newMaskMaterial;

	private MaskedSpine[] currentMaskedFill = new MaskedSpine[2];

	private MaskedSpine[] currentMaskedCut = new MaskedSpine[2];

	public Transform CachedTransform
	{
		get
		{
			if (cachedTransform == null)
			{
				cachedTransform = base.transform;
			}
			return cachedTransform;
		}
	}

	public Transform CachedUtilityBoneTransform
	{
		get
		{
			if (cachedUtilityBoneTransform == null && CachedUtilityBone != null)
			{
				cachedUtilityBoneTransform = CachedUtilityBone.transform;
			}
			return cachedUtilityBoneTransform;
		}
	}

	public SkeletonUtilityBone CachedUtilityBone
	{
		get
		{
			if (cachedUtilityBone == null && (bool)skeletonAnimation)
			{
				SkeletonUtility component = skeletonAnimation.gameObject.GetComponent<SkeletonUtility>();
				if (component != null && component.utilityBones != null)
				{
					for (int i = 0; i < component.utilityBones.Count; i++)
					{
						if (component.utilityBones[i].boneName == CachedBone.Data.Name)
						{
							cachedUtilityBone = component.utilityBones[i];
							break;
						}
					}
				}
			}
			return cachedUtilityBone;
		}
	}

	public Transform CachedSkeletonTransform
	{
		get
		{
			if (cachedSkeletonTransform == null && skeletonAnimation != null)
			{
				cachedSkeletonTransform = skeletonAnimation.transform;
			}
			return cachedSkeletonTransform;
		}
	}

	public Slot CachedSlot
	{
		get
		{
			if (cachedSlot == null && skeletonAnimation != null && skeletonAnimation.Skeleton != null)
			{
				cachedSlot = skeletonAnimation.Skeleton.FindSlot(slotName);
			}
			return cachedSlot;
		}
	}

	public Bone CachedBone
	{
		get
		{
			if (cachedBone == null && CachedSlot != null)
			{
				cachedBone = CachedSlot.Bone;
			}
			return cachedBone;
		}
	}

	private bool IsValid
	{
		get
		{
			return newMaskAttachment != null && newMaskMaterial != null;
		}
	}

	private void Awake()
	{
		cachedPropertyMain = MaskSpineProperty.CachePropertyMask();
		for (int i = 0; i < 2; i++)
		{
			cachedPropertyMaskedCut[i] = MaskSpineProperty.CachePropertyMaskedCut(i);
			cachedPropertyMaskedFill[i] = MaskSpineProperty.CachePropertyMaskedFill(i);
			currentMaskedFill[i] = null;
			currentMaskedCut[i] = null;
		}
		if ((bool)skeletonAnimation)
		{
			skeletonAnimation.UpdateComplete += SkeletonAnimation_UpdateComplete;
			SkeletonAnimation obj = skeletonAnimation;
			obj.OnReset = (SkeletonRenderer.SkeletonRendererDelegate)Delegate.Combine(obj.OnReset, new SkeletonRenderer.SkeletonRendererDelegate(SkeletonAnimation_OnReset));
		}
	}

	private void OnEnable()
	{
		if (isInitOnAwake)
		{
			Init();
		}
	}

	private void Start()
	{
		if (fixedMaskedFill != null)
		{
			for (int i = 0; i < fixedMaskedFill.Count; i++)
			{
				AddMaskedFill(fixedMaskedFill[i]);
			}
		}
		if (fixedMaskedCut != null)
		{
			for (int j = 0; j < fixedMaskedCut.Count; j++)
			{
				AddMaskedCut(fixedMaskedCut[j]);
			}
		}
	}

	private void OnDisable()
	{
		Deinit();
	}

	private void OnDestroy()
	{
		cachedPropertyMain = null;
		for (int i = 0; i < 2; i++)
		{
			cachedPropertyMaskedCut[i] = null;
			cachedPropertyMaskedFill[i] = null;
			currentMaskedFill[i] = null;
			currentMaskedCut[i] = null;
		}
		if ((bool)skeletonAnimation)
		{
			skeletonAnimation.UpdateComplete -= SkeletonAnimation_UpdateComplete;
			SkeletonAnimation obj = skeletonAnimation;
			obj.OnReset = (SkeletonRenderer.SkeletonRendererDelegate)Delegate.Remove(obj.OnReset, new SkeletonRenderer.SkeletonRendererDelegate(SkeletonAnimation_OnReset));
		}
	}

	public void Init()
	{
		if (isInit)
		{
			return;
		}
		isLoad = false;
		if ((bool)shaderMask && (bool)skeletonAnimation && skeletonAnimation.Skeleton != null)
		{
			isLoad = true;
			skeletonAnimation.immutableTriangles = false;
			if (CachedSlot != null && CachedBone != null)
			{
				isInit = true;
				Validate();
			}
		}
	}

	public void Deinit()
	{
		if (isInit)
		{
			if (IsValid)
			{
				defaultAttachment.RendererObject = defaultMaterial;
				CachedSlot.Attachment = defaultAttachment;
				skeletonAnimation.Skeleton.SetSkinAttachment(CachedSlot, defaultAttachment);
			}
			cachedSlot = null;
			cachedBone = null;
			defaultAttachment = null;
			defaultMaterial = null;
			newMaskMaterial = null;
			newMaskAttachment = null;
			isInit = false;
		}
	}

	public static MaskSpine CreateMask(GameObject go, Shader shader, SkeletonAnimation animation, string slot)
	{
		MaskSpine maskSpine = go.GetComponent<MaskSpine>();
		if (maskSpine == null)
		{
			maskSpine = go.AddComponent<MaskSpine>();
		}
		maskSpine.shaderMask = shader;
		maskSpine.skeletonAnimation = animation;
		maskSpine.slotName = slot;
		return maskSpine;
	}

	public void Validate()
	{
		if (!isInit || IsValid || CachedBone == null || CachedBone.Skeleton == null)
		{
			return;
		}
		defaultAttachment = CachedBone.Skeleton.GetDefaultAttachment(CachedSlot.Data.Name, CachedSlot.Data.AttachmentName) as RegionAttachment;
		if (defaultAttachment == null)
		{
			return;
		}
		defaultMaterial = defaultAttachment.RendererObject as Material;
		if (defaultMaterial != null)
		{
			newMaskMaterial = UnityEngine.Object.Instantiate(defaultMaterial);
			newMaskMaterial.shader = shaderMask;
			newMaskMaterial.renderQueue = defaultMaterial.renderQueue;
			newMaskAttachment = (RegionAttachment)defaultAttachment.CopyAttachment();
			newMaskAttachment.RendererObject = newMaskMaterial;
			SetProperty(cachedPropertyMain, GetMaskUVRect(), GetMaskVertices());
			for (int i = 0; i < 2; i++)
			{
				SetProperty(cachedPropertyMaskedFill[i], Texture2D.blackTexture, Vector4.zero);
				SetProperty(cachedPropertyMaskedCut[i], Texture2D.blackTexture, Vector4.zero);
			}
			CachedSlot.Attachment = newMaskAttachment;
			skeletonAnimation.Skeleton.SetSkinAttachment(CachedSlot, newMaskAttachment);
		}
	}

	public void AddMaskedFill(MaskedSpine maskedFill)
	{
		Init();
		if (isInit && (bool)maskedFill)
		{
			AddMasked(currentMaskedFill, maskedFill, cachedPropertyMaskedFill);
		}
	}

	public void AddMaskedCut(MaskedSpine maskedCut)
	{
		Init();
		if (isInit && (bool)maskedCut)
		{
			AddMasked(currentMaskedCut, maskedCut, cachedPropertyMaskedCut);
		}
	}

	public void RemoveMaskedFill(MaskedSpine maskedFill)
	{
		if (isInit && (bool)maskedFill)
		{
			RemoveMasked(currentMaskedFill, maskedFill, cachedPropertyMaskedFill);
		}
	}

	public void RemoveMaskedCut(MaskedSpine maskedCut)
	{
		if (isInit && (bool)maskedCut)
		{
			RemoveMasked(currentMaskedCut, maskedCut, cachedPropertyMaskedCut);
		}
	}

	private void AddMasked(MaskedSpine[] currentMasked, MaskedSpine masked, MaskSpineProperty[] cachedProperty)
	{
		if (currentMasked.ContainsMasked(masked))
		{
			return;
		}
		masked.SetDepth(GetMaskDepth());
		for (int i = 0; i < 2; i++)
		{
			if (currentMasked[i] == null)
			{
				currentMasked[i] = masked;
				Validate();
				UpdateMask();
				Matrix4x4 matrix = masked.CachedTransform.worldToLocalMatrix * CachedSkeletonTransform.localToWorldMatrix;
				Vector2[] vertices = masked.GetVertices();
				Vector4 uVRect = masked.GetUVRect();
				Texture mainTexture = masked.CachedMaterial.mainTexture;
				SetProperty(cachedProperty[i], mainTexture, uVRect, vertices, matrix);
				break;
			}
		}
	}

	private void RemoveMasked(MaskedSpine[] currentMasked, MaskedSpine masked, MaskSpineProperty[] cachedProperty)
	{
		if (!currentMasked.ContainsMasked(masked))
		{
			return;
		}
		for (int i = 0; i < 2; i++)
		{
			if (currentMasked[i] == masked)
			{
				currentMasked[i] = null;
				SetProperty(cachedProperty[i], Texture2D.blackTexture, Vector4.zero);
				break;
			}
		}
	}

	private void SetProperty(MaskSpineProperty cachedProperty, Texture texture, Vector4 rect)
	{
		if (IsValid)
		{
			newMaskMaterial.SetTexture(cachedProperty.texture, texture);
			newMaskMaterial.SetVector(cachedProperty.rect, rect);
		}
	}

	private void SetProperty(MaskSpineProperty cachedProperty, Vector4 rect)
	{
		if (IsValid)
		{
			newMaskMaterial.SetVector(cachedProperty.rect, rect);
		}
	}

	private void SetProperty(MaskSpineProperty cachedProperty, Vector2[] vert)
	{
		if (IsValid)
		{
			newMaskMaterial.SetVector(cachedProperty.vert1, vert[0]);
			newMaskMaterial.SetVector(cachedProperty.vert2, vert[1]);
			newMaskMaterial.SetVector(cachedProperty.vert3, vert[2]);
		}
	}

	private void SetProperty(MaskSpineProperty cachedProperty, Matrix4x4 matrix)
	{
		if (IsValid)
		{
			newMaskMaterial.SetMatrix(cachedProperty.transform, matrix);
		}
	}

	private void SetProperty(MaskSpineProperty cachedProperty, Vector4 rect, Vector2[] vert)
	{
		SetProperty(cachedProperty, rect);
		SetProperty(cachedProperty, vert);
	}

	private void SetProperty(MaskSpineProperty cachedProperty, Texture texture, Vector4 rect, Vector2[] vert)
	{
		SetProperty(cachedProperty, texture, rect);
		SetProperty(cachedProperty, vert);
	}

	private void SetProperty(MaskSpineProperty cachedProperty, Texture texture, Vector4 rect, Vector2[] vert, Matrix4x4 matrix)
	{
		SetProperty(cachedProperty, texture, rect, vert);
		SetProperty(cachedProperty, matrix);
	}

	private void SetProperty(MaskSpineProperty cachedProperty, Vector2[] vert, Matrix4x4 matrix)
	{
		SetProperty(cachedProperty, vert);
		SetProperty(cachedProperty, matrix);
	}

	private Vector4 GetMaskUVRect()
	{
		Vector4 zero = Vector4.zero;
		if (IsValid)
		{
			Texture mainTexture = newMaskMaterial.mainTexture;
			zero.x = newMaskAttachment.UVs[0];
			zero.y = newMaskAttachment.UVs[1];
			if (!newMaskAttachment.RegionRotate)
			{
				zero.z = newMaskAttachment.RegionWidth / (float)mainTexture.width;
				zero.w = newMaskAttachment.RegionHeight / (float)mainTexture.height;
			}
			else
			{
				zero.z = newMaskAttachment.RegionHeight / (float)mainTexture.width;
				zero.w = newMaskAttachment.RegionWidth / (float)mainTexture.height;
			}
		}
		return zero;
	}

	private Vector2[] GetMaskVertices()
	{
		Vector2[] array = new Vector2[3]
		{
			Vector2.zero,
			Vector2.one,
			Vector2.one
		};
		if (IsValid && CachedBone != null)
		{
			newMaskAttachment.ComputeWorldVertices(CachedBone, tempVert);
			Vector2 vector = new Vector2(tempVert[0], tempVert[1]);
			Vector2 vector2 = new Vector2(tempVert[6], tempVert[7]);
			Vector2 vector3 = new Vector2(tempVert[2], tempVert[3]);
			array[0] = vector;
			if (!newMaskAttachment.RegionRotate)
			{
				array[1] = vector2;
				array[2] = vector3;
			}
			else
			{
				array[1] = vector3;
				array[2] = vector2;
			}
		}
		return array;
	}

	private float GetMaskDepth()
	{
		float result = 0f;
		if ((bool)skeletonAnimation && skeletonAnimation.Skeleton != null)
		{
			int num = skeletonAnimation.Skeleton.DrawOrder.IndexOf(CachedSlot);
			float zSpacing = skeletonAnimation.zSpacing;
			result = (float)num * zSpacing;
		}
		return result;
	}

	private void UpdateMask()
	{
		if (currentMaskedFill != null)
		{
			for (int i = 0; i < 2; i++)
			{
				if ((bool)currentMaskedFill[i])
				{
					Matrix4x4 matrix = currentMaskedFill[i].CachedTransform.worldToLocalMatrix * CachedSkeletonTransform.localToWorldMatrix;
					Vector2[] vertices = currentMaskedFill[i].GetVertices();
					SetProperty(cachedPropertyMaskedFill[i], vertices, matrix);
				}
			}
		}
		if (currentMaskedCut != null)
		{
			for (int j = 0; j < 2; j++)
			{
				if ((bool)currentMaskedCut[j])
				{
					Matrix4x4 matrix2 = currentMaskedCut[j].CachedTransform.worldToLocalMatrix * CachedSkeletonTransform.localToWorldMatrix;
					Vector2[] vertices2 = currentMaskedCut[j].GetVertices();
					SetProperty(cachedPropertyMaskedCut[j], vertices2, matrix2);
				}
			}
		}
		SetProperty(cachedPropertyMain, GetMaskVertices());
	}

	private void SkeletonAnimation_UpdateComplete(SkeletonRenderer skeletonRenderer)
	{
		if (isInit && isAnimationUpdateMask && IsValid)
		{
			UpdateMask();
		}
	}

	private void SkeletonAnimation_OnReset(SkeletonRenderer obj)
	{
		if (isLoad)
		{
			isInit = false;
			cachedSlot = null;
			cachedBone = null;
			defaultMaterial = null;
			defaultAttachment = null;
			newMaskAttachment = null;
			newMaskMaterial = null;
			Init();
		}
	}
}
