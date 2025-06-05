using System;
using System.Collections.Generic;
using Spine;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SkeletonRenderer : MonoBehaviour
{
	public delegate void SkeletonRendererDelegate(SkeletonRenderer skeletonRenderer);

	private class LastState
	{
		public struct AddSubmeshArguments
		{
			public Material material;

			public int startSlot;

			public int endSlot;

			public int triangleCount;

			public int firstVertex;

			public bool lastSubmesh;

			public AddSubmeshArguments(Material material, int startSlot, int endSlot, int triangleCount, int firstVertex, bool lastSubmesh)
			{
				this.material = material;
				this.startSlot = startSlot;
				this.endSlot = endSlot;
				this.triangleCount = triangleCount;
				this.firstVertex = firstVertex;
				this.lastSubmesh = lastSubmesh;
			}

			public bool Equals(ref AddSubmeshArguments other)
			{
				return !object.ReferenceEquals(material, null) && !object.ReferenceEquals(other.material, null) && material.GetInstanceID() == other.material.GetInstanceID() && startSlot == other.startSlot && endSlot == other.endSlot && triangleCount == other.triangleCount && firstVertex == other.firstVertex;
			}
		}

		public bool immutableTrianglesMesh1;

		public bool immutableTrianglesMesh2;

		public int vertexCount;

		public readonly ExposedList<bool> attachmentsFlipStateTemp = new ExposedList<bool>();

		public readonly ExposedList<bool> attachmentsFlipStateMesh1 = new ExposedList<bool>();

		public readonly ExposedList<bool> attachmentsFlipStateMesh2 = new ExposedList<bool>();

		public readonly ExposedList<int> attachmentsTriangleCountTemp = new ExposedList<int>();

		public readonly ExposedList<int> attachmentsTriangleCountMesh1 = new ExposedList<int>();

		public readonly ExposedList<int> attachmentsTriangleCountMesh2 = new ExposedList<int>();

		public readonly ExposedList<AddSubmeshArguments> addSubmeshArgumentsTemp = new ExposedList<AddSubmeshArguments>();

		public readonly ExposedList<AddSubmeshArguments> addSubmeshArgumentsMesh1 = new ExposedList<AddSubmeshArguments>();

		public readonly ExposedList<AddSubmeshArguments> addSubmeshArgumentsMesh2 = new ExposedList<AddSubmeshArguments>();
	}

	public SkeletonRendererDelegate OnReset;

	[NonSerialized]
	public bool valid;

	[NonSerialized]
	public Skeleton skeleton;

	[SerializeField]
	private SkeletonDataAsset skeletonDataAsset;

	public string initialSkinName;

	public bool calculateNormals;

	public bool calculateTangents;

	public float zSpacing;

	public bool renderMeshes = true;

	public bool immutableTriangles;

	public bool frontFacing;

	public bool logErrors;

	[SpineSlot("", "", false)]
	public string[] submeshSeparators = new string[0];

	[HideInInspector]
	public List<Slot> submeshSeparatorSlots = new List<Slot>();

	private MeshRenderer meshRenderer;

	private MeshFilter meshFilter;

	private Mesh mesh1;

	private Mesh mesh2;

	private bool useMesh1;

	private float[] tempVertices = new float[8];

	private Vector3[] vertices;

	private Color32[] colors;

	private Vector2[] uvs;

	private Material[] sharedMaterials = new Material[0];

	private readonly ExposedList<Material> submeshMaterials = new ExposedList<Material>();

	private readonly ExposedList<Submesh> submeshes = new ExposedList<Submesh>();

	private SkeletonUtilitySubmeshRenderer[] submeshRenderers;

	private LastState lastState = new LastState();

	public SkeletonDataAsset SkeletonDataAsset
	{
		get
		{
			return skeletonDataAsset;
		}
		set
		{
			if (skeletonDataAsset != value)
			{
				ChangeRenderTargets(skeletonDataAsset, false);
				ChangeRenderTargets(value, true);
				skeletonDataAsset = value;
			}
		}
	}

	public MeshRenderer MeshRenderer
	{
		get
		{
			return meshRenderer;
		}
	}

	public virtual void Reset()
	{
		if (meshFilter != null)
		{
			meshFilter.sharedMesh = null;
		}
		meshRenderer = GetComponent<MeshRenderer>();
		if (meshRenderer != null)
		{
			meshRenderer.sharedMaterials = new Material[0];
		}
		if (mesh1 != null)
		{
			if (Application.isPlaying)
			{
				UnityEngine.Object.Destroy(mesh1);
			}
			else
			{
				UnityEngine.Object.DestroyImmediate(mesh1);
			}
		}
		if (mesh2 != null)
		{
			if (Application.isPlaying)
			{
				UnityEngine.Object.Destroy(mesh2);
			}
			else
			{
				UnityEngine.Object.DestroyImmediate(mesh2);
			}
		}
		lastState = new LastState();
		mesh1 = null;
		mesh2 = null;
		vertices = null;
		colors = null;
		uvs = null;
		sharedMaterials = new Material[0];
		submeshMaterials.Clear();
		submeshes.Clear();
		skeleton = null;
		valid = false;
		if (!SkeletonDataAsset)
		{
			if (logErrors)
			{
				Debug.LogError("Missing SkeletonData asset.", this);
			}
			return;
		}
		SkeletonData skeletonData = SkeletonDataAsset.GetSkeletonData(false);
		if (skeletonData != null)
		{
			valid = true;
			meshFilter = GetComponent<MeshFilter>();
			meshRenderer = GetComponent<MeshRenderer>();
			mesh1 = newMesh();
			mesh2 = newMesh();
			vertices = new Vector3[0];
			skeleton = new Skeleton(skeletonData);
			if (initialSkinName != null && initialSkinName.Length > 0)
			{
				skeleton.SetInitialSkin(initialSkinName);
			}
			submeshSeparatorSlots.Clear();
			for (int i = 0; i < submeshSeparators.Length; i++)
			{
				submeshSeparatorSlots.Add(skeleton.FindSlot(submeshSeparators[i]));
			}
			CollectSubmeshRenderers();
			LateUpdate();
			if (OnReset != null)
			{
				OnReset(this);
			}
		}
	}

	public void CollectSubmeshRenderers()
	{
		submeshRenderers = GetComponentsInChildren<SkeletonUtilitySubmeshRenderer>();
	}

	public virtual void Awake()
	{
		ChangeRenderTargets(SkeletonDataAsset, true);
		if (!valid)
		{
			Reset();
		}
	}

	private void ChangeRenderTargets(SkeletonDataAsset currentDataAsset, bool isAdd)
	{
		if (!(currentDataAsset != null) || currentDataAsset.spriteCollections == null)
		{
			return;
		}
		for (int i = 0; i < currentDataAsset.spriteCollections.Length; i++)
		{
			tk2dSpriteCollectionData tk2dSpriteCollectionData2 = currentDataAsset.spriteCollections[i];
			if ((bool)tk2dSpriteCollectionData2)
			{
				if (isAdd)
				{
					tk2dSpriteCollectionData2.AddRetain(this);
				}
				else
				{
					tk2dSpriteCollectionData2.RemoveRetain(this);
				}
			}
		}
	}

	private void OnEnable()
	{
		if (!valid)
		{
			Reset();
		}
	}

	public void Initialize()
	{
		LateUpdate();
		LateUpdate();
	}

	public virtual void OnDestroy()
	{
		ChangeRenderTargets(SkeletonDataAsset, false);
		if (mesh1 != null)
		{
			if (Application.isPlaying)
			{
				UnityEngine.Object.Destroy(mesh1);
			}
			else
			{
				UnityEngine.Object.DestroyImmediate(mesh1);
			}
		}
		if (mesh2 != null)
		{
			if (Application.isPlaying)
			{
				UnityEngine.Object.Destroy(mesh2);
			}
			else
			{
				UnityEngine.Object.DestroyImmediate(mesh2);
			}
		}
		mesh1 = null;
		mesh2 = null;
	}

	private Mesh newMesh()
	{
		Mesh mesh = new Mesh();
		mesh.name = "Skeleton Mesh";
		mesh.hideFlags = HideFlags.HideAndDontSave;
		mesh.MarkDynamic();
		return mesh;
	}

	public virtual void LateUpdate()
	{
		if (!valid || (!meshRenderer.enabled && submeshRenderers.Length == 0))
		{
			return;
		}
		int num = 0;
		int num2 = 0;
		int firstVertex = 0;
		int startSlot = 0;
		Material material = null;
		ExposedList<Slot> drawOrder = skeleton.drawOrder;
		int count = drawOrder.Count;
		int count2 = submeshSeparatorSlots.Count;
		bool flag = renderMeshes;
		ExposedList<int> attachmentsTriangleCountTemp = lastState.attachmentsTriangleCountTemp;
		attachmentsTriangleCountTemp.GrowIfNeeded(count);
		attachmentsTriangleCountTemp.Count = count;
		ExposedList<bool> attachmentsFlipStateTemp = lastState.attachmentsFlipStateTemp;
		attachmentsFlipStateTemp.GrowIfNeeded(count);
		attachmentsFlipStateTemp.Count = count;
		ExposedList<LastState.AddSubmeshArguments> addSubmeshArgumentsTemp = lastState.addSubmeshArgumentsTemp;
		addSubmeshArgumentsTemp.Clear(false);
		for (int i = 0; i < count; i++)
		{
			Slot slot = drawOrder.Items[i];
			Bone bone = slot.bone;
			Attachment attachment = slot.attachment;
			bool flag2 = bone.worldScaleX >= 0f;
			bool flag3 = bone.worldScaleY >= 0f;
			bool flag4 = (flag2 && flag3) || (!flag2 && !flag3);
			bool flag5 = frontFacing && bone.worldFlipX != bone.worldFlipY == flag4;
			attachmentsFlipStateTemp.Items[i] = flag5;
			attachmentsTriangleCountTemp.Items[i] = -1;
			RegionAttachment regionAttachment = attachment as RegionAttachment;
			object rendererObject;
			int num3;
			int num4;
			if (regionAttachment != null)
			{
				rendererObject = regionAttachment.RendererObject;
				num3 = 4;
				num4 = 6;
			}
			else
			{
				if (!flag)
				{
					continue;
				}
				MeshAttachment meshAttachment = attachment as MeshAttachment;
				if (meshAttachment != null)
				{
					rendererObject = meshAttachment.RendererObject;
					num3 = meshAttachment.vertices.Length >> 1;
					num4 = meshAttachment.triangles.Length;
				}
				else
				{
					SkinnedMeshAttachment skinnedMeshAttachment = attachment as SkinnedMeshAttachment;
					if (skinnedMeshAttachment == null)
					{
						continue;
					}
					rendererObject = skinnedMeshAttachment.RendererObject;
					num3 = skinnedMeshAttachment.uvs.Length >> 1;
					num4 = skinnedMeshAttachment.triangles.Length;
				}
			}
			Material material2 = ((rendererObject.GetType() != typeof(Material)) ? ((Material)((AtlasRegion)rendererObject).page.rendererObject) : ((Material)rendererObject));
			if ((material != null && material.GetInstanceID() != material2.GetInstanceID()) || (count2 > 0 && submeshSeparatorSlots.Contains(slot)))
			{
				addSubmeshArgumentsTemp.Add(new LastState.AddSubmeshArguments(material, startSlot, i, num2, firstVertex, false));
				num2 = 0;
				firstVertex = num;
				startSlot = i;
			}
			material = material2;
			num2 += num4;
			num += num3;
			attachmentsTriangleCountTemp.Items[i] = num4;
		}
		addSubmeshArgumentsTemp.Add(new LastState.AddSubmeshArguments(material, startSlot, count, num2, firstVertex, true));
		bool flag6 = CheckIfMustUpdateMeshStructure(attachmentsTriangleCountTemp, attachmentsFlipStateTemp, addSubmeshArgumentsTemp);
		if (flag6)
		{
			submeshMaterials.Clear();
			int j = 0;
			for (int count3 = addSubmeshArgumentsTemp.Count; j < count3; j++)
			{
				LastState.AddSubmeshArguments addSubmeshArguments = addSubmeshArgumentsTemp.Items[j];
				AddSubmesh(addSubmeshArguments.material, addSubmeshArguments.startSlot, addSubmeshArguments.endSlot, addSubmeshArguments.triangleCount, addSubmeshArguments.firstVertex, addSubmeshArguments.lastSubmesh, attachmentsFlipStateTemp);
			}
			if (submeshMaterials.Count == sharedMaterials.Length)
			{
				submeshMaterials.CopyTo(sharedMaterials);
			}
			else
			{
				sharedMaterials = submeshMaterials.ToArray();
			}
			meshRenderer.sharedMaterials = sharedMaterials;
		}
		Vector3[] array = vertices;
		bool flag7 = num > array.Length;
		if (flag7)
		{
			array = (vertices = new Vector3[num]);
			colors = new Color32[num];
			uvs = new Vector2[num];
			mesh1.Clear();
			mesh2.Clear();
		}
		else
		{
			Vector3 zero = Vector3.zero;
			int k = num;
			for (int vertexCount = lastState.vertexCount; k < vertexCount; k++)
			{
				array[k] = zero;
			}
		}
		lastState.vertexCount = num;
		float num5 = zSpacing;
		float[] array2 = tempVertices;
		Vector2[] array3 = uvs;
		Color32[] array4 = colors;
		int num6 = 0;
		float num7 = skeleton.a * 255f;
		float r = skeleton.r;
		float g = skeleton.g;
		float b = skeleton.b;
		int num8 = 0;
		Color32 color = default(Color32);
		do
		{
			Slot slot2 = drawOrder.Items[num8];
			Attachment attachment2 = slot2.attachment;
			RegionAttachment regionAttachment2 = attachment2 as RegionAttachment;
			if (regionAttachment2 != null)
			{
				regionAttachment2.ComputeWorldVertices(slot2.bone, array2);
				float z = (float)num8 * num5;
				float x = array2[0];
				float y = array2[1];
				float x2 = array2[2];
				float y2 = array2[3];
				float x3 = array2[4];
				float y3 = array2[5];
				float x4 = array2[6];
				float y4 = array2[7];
				array[num6].x = x;
				array[num6].y = y;
				array[num6].z = z;
				array[num6 + 1].x = x4;
				array[num6 + 1].y = y4;
				array[num6 + 1].z = z;
				array[num6 + 2].x = x2;
				array[num6 + 2].y = y2;
				array[num6 + 2].z = z;
				array[num6 + 3].x = x3;
				array[num6 + 3].y = y3;
				array[num6 + 3].z = z;
				color.a = (byte)(num7 * slot2.a * regionAttachment2.a);
				color.r = (byte)(r * slot2.r * regionAttachment2.r * (float)(int)color.a);
				color.g = (byte)(g * slot2.g * regionAttachment2.g * (float)(int)color.a);
				color.b = (byte)(b * slot2.b * regionAttachment2.b * (float)(int)color.a);
				if (slot2.data.blendMode == BlendMode.additive)
				{
					color.a = 0;
				}
				array4[num6] = color;
				array4[num6 + 1] = color;
				array4[num6 + 2] = color;
				array4[num6 + 3] = color;
				float[] array5 = regionAttachment2.uvs;
				array3[num6].x = array5[0];
				array3[num6].y = array5[1];
				array3[num6 + 1].x = array5[6];
				array3[num6 + 1].y = array5[7];
				array3[num6 + 2].x = array5[2];
				array3[num6 + 2].y = array5[3];
				array3[num6 + 3].x = array5[4];
				array3[num6 + 3].y = array5[5];
				num6 += 4;
			}
			else
			{
				if (!flag)
				{
					continue;
				}
				MeshAttachment meshAttachment2 = attachment2 as MeshAttachment;
				if (meshAttachment2 != null)
				{
					int num9 = meshAttachment2.vertices.Length;
					if (array2.Length < num9)
					{
						array2 = (tempVertices = new float[num9]);
					}
					meshAttachment2.ComputeWorldVertices(slot2, array2);
					color.a = (byte)(num7 * slot2.a * meshAttachment2.a);
					color.r = (byte)(r * slot2.r * meshAttachment2.r * (float)(int)color.a);
					color.g = (byte)(g * slot2.g * meshAttachment2.g * (float)(int)color.a);
					color.b = (byte)(b * slot2.b * meshAttachment2.b * (float)(int)color.a);
					if (slot2.data.blendMode == BlendMode.additive)
					{
						color.a = 0;
					}
					float[] array6 = meshAttachment2.uvs;
					float z2 = (float)num8 * num5;
					int num10 = 0;
					while (num10 < num9)
					{
						float x5 = array2[num10];
						float y5 = array2[num10 + 1];
						array[num6].x = x5;
						array[num6].y = y5;
						array[num6].z = z2;
						array4[num6] = color;
						array3[num6].x = array6[num10];
						array3[num6].y = array6[num10 + 1];
						num10 += 2;
						num6++;
					}
					continue;
				}
				SkinnedMeshAttachment skinnedMeshAttachment2 = attachment2 as SkinnedMeshAttachment;
				if (skinnedMeshAttachment2 != null)
				{
					int num11 = skinnedMeshAttachment2.uvs.Length;
					if (array2.Length < num11)
					{
						array2 = (tempVertices = new float[num11]);
					}
					skinnedMeshAttachment2.ComputeWorldVertices(slot2, array2);
					color.a = (byte)(num7 * slot2.a * skinnedMeshAttachment2.a);
					color.r = (byte)(r * slot2.r * skinnedMeshAttachment2.r * (float)(int)color.a);
					color.g = (byte)(g * slot2.g * skinnedMeshAttachment2.g * (float)(int)color.a);
					color.b = (byte)(b * slot2.b * skinnedMeshAttachment2.b * (float)(int)color.a);
					if (slot2.data.blendMode == BlendMode.additive)
					{
						color.a = 0;
					}
					float[] array7 = skinnedMeshAttachment2.uvs;
					float z3 = (float)num8 * num5;
					int num12 = 0;
					while (num12 < num11)
					{
						float x6 = array2[num12];
						float y6 = array2[num12 + 1];
						array[num6].x = x6;
						array[num6].y = y6;
						array[num6].z = z3;
						array4[num6] = color;
						array3[num6].x = array7[num12];
						array3[num6].y = array7[num12 + 1];
						num12 += 2;
						num6++;
					}
				}
			}
		}
		while (++num8 < count);
		Mesh mesh = ((!useMesh1) ? mesh2 : mesh1);
		meshFilter.sharedMesh = mesh;
		mesh.vertices = array;
		mesh.colors32 = array4;
		mesh.uv = array3;
		mesh.RecalculateBounds();
		if (flag6)
		{
			int num13 = (mesh.subMeshCount = submeshMaterials.Count);
			for (int l = 0; l < num13; l++)
			{
				mesh.SetTriangles(submeshes.Items[l].triangles, l);
			}
		}
		if (flag7 && calculateNormals)
		{
			Vector3[] array8 = new Vector3[num];
			Vector3 vector = new Vector3(0f, 0f, -1f);
			for (int m = 0; m < num; m++)
			{
				array8[m] = vector;
			}
			((!useMesh1) ? mesh1 : mesh2).vertices = array;
			mesh1.normals = array8;
			mesh2.normals = array8;
			if (calculateTangents)
			{
				Vector4[] array9 = new Vector4[num];
				Vector3 vector2 = new Vector3(0f, 0f, 1f);
				for (int n = 0; n < num; n++)
				{
					array9[n] = vector2;
				}
				mesh1.tangents = array9;
				mesh2.tangents = array9;
			}
		}
		ExposedList<int> exposedList;
		ExposedList<LastState.AddSubmeshArguments> exposedList2;
		ExposedList<bool> exposedList3;
		if (useMesh1)
		{
			exposedList = lastState.attachmentsTriangleCountMesh1;
			exposedList2 = lastState.addSubmeshArgumentsMesh1;
			exposedList3 = lastState.attachmentsFlipStateMesh1;
			lastState.immutableTrianglesMesh1 = immutableTriangles;
		}
		else
		{
			exposedList = lastState.attachmentsTriangleCountMesh2;
			exposedList2 = lastState.addSubmeshArgumentsMesh2;
			exposedList3 = lastState.attachmentsFlipStateMesh2;
			lastState.immutableTrianglesMesh2 = immutableTriangles;
		}
		exposedList.GrowIfNeeded(attachmentsTriangleCountTemp.Capacity);
		exposedList.Count = attachmentsTriangleCountTemp.Count;
		attachmentsTriangleCountTemp.CopyTo(exposedList.Items, 0);
		exposedList3.GrowIfNeeded(attachmentsFlipStateTemp.Capacity);
		exposedList3.Count = attachmentsFlipStateTemp.Count;
		attachmentsFlipStateTemp.CopyTo(exposedList3.Items, 0);
		exposedList2.GrowIfNeeded(addSubmeshArgumentsTemp.Count);
		exposedList2.Count = addSubmeshArgumentsTemp.Count;
		addSubmeshArgumentsTemp.CopyTo(exposedList2.Items);
		if (submeshRenderers.Length > 0)
		{
			for (int num14 = 0; num14 < submeshRenderers.Length; num14++)
			{
				SkeletonUtilitySubmeshRenderer skeletonUtilitySubmeshRenderer = submeshRenderers[num14];
				if (skeletonUtilitySubmeshRenderer.submeshIndex < sharedMaterials.Length)
				{
					skeletonUtilitySubmeshRenderer.SetMesh(meshRenderer, (!useMesh1) ? mesh2 : mesh1, sharedMaterials[skeletonUtilitySubmeshRenderer.submeshIndex]);
				}
				else
				{
					skeletonUtilitySubmeshRenderer.GetComponent<Renderer>().enabled = false;
				}
			}
		}
		useMesh1 = !useMesh1;
	}

	private bool CheckIfMustUpdateMeshStructure(ExposedList<int> attachmentsTriangleCountTemp, ExposedList<bool> attachmentsFlipStateTemp, ExposedList<LastState.AddSubmeshArguments> addSubmeshArgumentsTemp)
	{
		if (immutableTriangles != ((!useMesh1) ? lastState.immutableTrianglesMesh2 : lastState.immutableTrianglesMesh1))
		{
			return true;
		}
		ExposedList<int> exposedList;
		ExposedList<LastState.AddSubmeshArguments> exposedList2;
		ExposedList<bool> exposedList3;
		if (useMesh1)
		{
			exposedList = lastState.attachmentsTriangleCountMesh1;
			exposedList2 = lastState.addSubmeshArgumentsMesh1;
			exposedList3 = lastState.attachmentsFlipStateMesh1;
		}
		else
		{
			exposedList = lastState.attachmentsTriangleCountMesh2;
			exposedList2 = lastState.addSubmeshArgumentsMesh2;
			exposedList3 = lastState.attachmentsFlipStateMesh2;
		}
		int count = attachmentsTriangleCountTemp.Count;
		if (exposedList.Count != count)
		{
			return true;
		}
		for (int i = 0; i < count; i++)
		{
			if (exposedList.Items[i] != attachmentsTriangleCountTemp.Items[i])
			{
				return true;
			}
		}
		for (int j = 0; j < count; j++)
		{
			if (exposedList3.Items[j] != attachmentsFlipStateTemp.Items[j])
			{
				return true;
			}
		}
		int count2 = addSubmeshArgumentsTemp.Count;
		if (exposedList2.Count != count2)
		{
			return true;
		}
		for (int k = 0; k < count2; k++)
		{
			if (!exposedList2.Items[k].Equals(ref addSubmeshArgumentsTemp.Items[k]))
			{
				return true;
			}
		}
		return false;
	}

	private void AddSubmesh(Material material, int startSlot, int endSlot, int triangleCount, int firstVertex, bool lastSubmesh, ExposedList<bool> flipStates)
	{
		int count = submeshMaterials.Count;
		submeshMaterials.Add(material);
		if (submeshes.Count <= count)
		{
			submeshes.Add(new Submesh());
		}
		else if (immutableTriangles)
		{
			return;
		}
		Submesh submesh = submeshes.Items[count];
		int[] array = submesh.triangles;
		int num = array.Length;
		if (lastSubmesh && num > triangleCount)
		{
			for (int i = triangleCount; i < num; i++)
			{
				array[i] = 0;
			}
			submesh.triangleCount = triangleCount;
		}
		else if (num != triangleCount)
		{
			array = (submesh.triangles = new int[triangleCount]);
			submesh.triangleCount = 0;
		}
		if (!renderMeshes && !frontFacing)
		{
			if (submesh.firstVertex != firstVertex || submesh.triangleCount < triangleCount)
			{
				submesh.triangleCount = triangleCount;
				submesh.firstVertex = firstVertex;
				int num2 = 0;
				int num3 = 0;
				while (num3 < triangleCount)
				{
					array[num3] = firstVertex;
					array[num3 + 1] = firstVertex + 2;
					array[num3 + 2] = firstVertex + 1;
					array[num3 + 3] = firstVertex + 2;
					array[num3 + 4] = firstVertex + 3;
					array[num3 + 5] = firstVertex + 1;
					num3 += 6;
					firstVertex += 4;
					num2++;
				}
			}
			return;
		}
		ExposedList<Slot> drawOrder = skeleton.DrawOrder;
		int j = startSlot;
		int num4 = 0;
		for (; j < endSlot; j++)
		{
			Slot slot = drawOrder.Items[j];
			Attachment attachment = slot.attachment;
			bool flag = flipStates.Items[j];
			if (attachment is RegionAttachment)
			{
				if (!flag)
				{
					array[num4] = firstVertex;
					array[num4 + 1] = firstVertex + 2;
					array[num4 + 2] = firstVertex + 1;
					array[num4 + 3] = firstVertex + 2;
					array[num4 + 4] = firstVertex + 3;
					array[num4 + 5] = firstVertex + 1;
				}
				else
				{
					array[num4] = firstVertex + 1;
					array[num4 + 1] = firstVertex + 2;
					array[num4 + 2] = firstVertex;
					array[num4 + 3] = firstVertex + 1;
					array[num4 + 4] = firstVertex + 3;
					array[num4 + 5] = firstVertex + 2;
				}
				num4 += 6;
				firstVertex += 4;
				continue;
			}
			MeshAttachment meshAttachment = attachment as MeshAttachment;
			int num5;
			int[] triangles;
			if (meshAttachment != null)
			{
				num5 = meshAttachment.vertices.Length >> 1;
				triangles = meshAttachment.triangles;
			}
			else
			{
				SkinnedMeshAttachment skinnedMeshAttachment = attachment as SkinnedMeshAttachment;
				if (skinnedMeshAttachment == null)
				{
					continue;
				}
				num5 = skinnedMeshAttachment.uvs.Length >> 1;
				triangles = skinnedMeshAttachment.triangles;
			}
			if (flag)
			{
				int num6 = 0;
				int num7 = triangles.Length;
				while (num6 < num7)
				{
					array[num4 + 2] = firstVertex + triangles[num6];
					array[num4 + 1] = firstVertex + triangles[num6 + 1];
					array[num4] = firstVertex + triangles[num6 + 2];
					num6 += 3;
					num4 += 3;
				}
			}
			else
			{
				int num8 = 0;
				int num9 = triangles.Length;
				while (num8 < num9)
				{
					array[num4] = firstVertex + triangles[num8];
					num8++;
					num4++;
				}
			}
			firstVertex += num5;
		}
	}
}
