using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

[ExecuteInEditMode]
public class tmBatchedInstance : MonoBehaviour
{
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action m_OnDestroyEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<tmBatchedInstance> m_OnActivated;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<tmBatchedInstance> m_OnDisabled;

	[SerializeField]
	private List<tmBatchObject> parts = new List<tmBatchObject>();

	private List<tmBatchObject> batchedParts = new List<tmBatchObject>();

	public bool shouldMove;

	private bool modified;

	private bool orderDirty;

	[SerializeField]
	protected int vertexCount;

	private Mesh mesh;

	private Transform cachedTransform;

	private Renderer cachedRenderer;

	private SkinnedMeshRenderer cachedSkinnedRenderer;

	private HashSet<tmBatchObject> forcedParts = new HashSet<tmBatchObject>();

	private CombineInstance[] tempCombos;

	public Renderer CachedRender
	{
		get
		{
			if (cachedRenderer == null)
			{
				cachedRenderer = GetComponent<Renderer>();
			}
			return cachedRenderer;
		}
	}

	public SkinnedMeshRenderer CachedSkinnedRender
	{
		get
		{
			if (cachedSkinnedRenderer == null)
			{
				cachedSkinnedRenderer = GetComponent<SkinnedMeshRenderer>();
			}
			return cachedSkinnedRenderer;
		}
	}

	public Material SharedMaterial
	{
		get
		{
			return CachedRender.sharedMaterial;
		}
		set
		{
			CachedRender.sharedMaterial = value;
		}
	}

	public bool Modified
	{
		get
		{
			return modified;
		}
		set
		{
			if (!modified && value && (bool)SingletonMonoBehaviour<tmBatchingManager>.InstanceIfExist)
			{
				tmBatchingManager.Instance.RegisterModifiedBatchedInstance(this);
			}
			modified = value;
		}
	}

	public bool OrderDirty
	{
		get
		{
			return orderDirty;
		}
		set
		{
			if (!orderDirty && value)
			{
				Modified = true;
			}
			orderDirty = value;
		}
	}

	public Mesh Mesh
	{
		get
		{
			if (mesh == null)
			{
				mesh = new Mesh();
				mesh.name = "batch_" + base.gameObject.name;
				MeshFilter component = GetComponent<MeshFilter>();
				if (component != null)
				{
					component.sharedMesh = mesh;
				}
				SkinnedMeshRenderer component2 = GetComponent<SkinnedMeshRenderer>();
				if (component2 != null)
				{
					component2.sharedMesh = mesh;
				}
			}
			return mesh;
		}
		set
		{
			mesh = value;
			MeshFilter component = GetComponent<MeshFilter>();
			if (component != null)
			{
				component.sharedMesh = mesh;
			}
			SkinnedMeshRenderer component2 = GetComponent<SkinnedMeshRenderer>();
			if (component2 != null)
			{
				component2.sharedMesh = mesh;
			}
		}
	}

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

	public int VertexCount
	{
		get
		{
			return vertexCount;
		}
	}

	public bool ForcedBatching
	{
		get
		{
			return forcedParts.Count > 0;
		}
	}

	protected List<tmBatchObject> BatchedParts
	{
		get
		{
			return batchedParts;
		}
	}

	public event Action OnDestroyEvent
	{
		add
		{
			Action action = this.m_OnDestroyEvent;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnDestroyEvent, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = this.m_OnDestroyEvent;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnDestroyEvent, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<tmBatchedInstance> OnActivated
	{
		add
		{
			Action<tmBatchedInstance> action = tmBatchedInstance.m_OnActivated;
			Action<tmBatchedInstance> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref tmBatchedInstance.m_OnActivated, (Action<tmBatchedInstance>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<tmBatchedInstance> action = tmBatchedInstance.m_OnActivated;
			Action<tmBatchedInstance> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref tmBatchedInstance.m_OnActivated, (Action<tmBatchedInstance>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<tmBatchedInstance> OnDisabled
	{
		add
		{
			Action<tmBatchedInstance> action = tmBatchedInstance.m_OnDisabled;
			Action<tmBatchedInstance> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref tmBatchedInstance.m_OnDisabled, (Action<tmBatchedInstance>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<tmBatchedInstance> action = tmBatchedInstance.m_OnDisabled;
			Action<tmBatchedInstance> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref tmBatchedInstance.m_OnDisabled, (Action<tmBatchedInstance>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public bool CanAdd(tmBatchObject part)
	{
		return vertexCount + part.Mesh.vertexCount < 65535;
	}

	public void Add(tmBatchObject part)
	{
		if (part.Mesh != null && !parts.Contains(part))
		{
			part.BatchInstance = this;
			parts.Add(part);
			Modified = true;
			OrderDirty = true;
			vertexCount += part.Mesh.vertexCount;
			if (part.ForcedBatching)
			{
				AddToForced(part);
			}
		}
	}

	public bool TryAdd(tmBatchObject part)
	{
		if (CanAdd(part))
		{
			Add(part);
			return true;
		}
		return false;
	}

	public void Remove(tmBatchObject part)
	{
		if (parts.Contains(part))
		{
			part.Batched = false;
			part.BatchInstance = null;
			parts.Remove(part);
			Modified = true;
			vertexCount -= part.Mesh.vertexCount;
			if (part.ForcedBatching)
			{
				RemoveFromForced(part);
			}
		}
	}

	public void ForcedBatchChanged(tmBatchObject part)
	{
		if (part.ForcedBatching)
		{
			AddToForced(part);
		}
		else
		{
			RemoveFromForced(part);
		}
	}

	protected virtual void PostRecombine(CombineInstance[] combos)
	{
	}

	protected virtual void ClearMesh()
	{
	}

	public void Recombine()
	{
		if (OrderDirty && parts.Count > 0)
		{
			parts.Sort((tmBatchObject a, tmBatchObject b) => b.CachedTransform.position.z.CompareTo(a.CachedTransform.position.z));
		}
		OrderDirty = false;
		batchedParts.Clear();
		foreach (tmBatchObject part in parts)
		{
			if (part.Mesh != null && part.Mesh.vertexCount > 0)
			{
				batchedParts.Add(part);
			}
		}
		if (batchedParts.Count > 0)
		{
			CachedTransform.position = Vector3.zero;
			tempCombos = ArrayExtention.EnsureLength(tempCombos, batchedParts.Count);
			int num = 0;
			for (int count = batchedParts.Count; num < count; num++)
			{
				tmBatchObject tmBatchObject2 = batchedParts[num];
				tmBatchObject2.Batched = true;
				tempCombos[num].mesh = tmBatchObject2.Mesh;
				tempCombos[num].subMeshIndex = 0;
				tempCombos[num].transform = CachedTransform.worldToLocalMatrix * tmBatchObject2.CachedTransform.localToWorldMatrix;
			}
			Mesh.CombineMeshes(tempCombos, true);
			PostRecombine(tempCombos);
		}
		else
		{
			Mesh.Clear();
			ClearMesh();
		}
		Modified = false;
	}

	public void Clear()
	{
		foreach (tmBatchObject part in parts)
		{
			if ((bool)part)
			{
				part.Batched = false;
				part.BatchInstance = null;
			}
		}
		parts.Clear();
		if ((bool)mesh)
		{
			mesh.Clear();
		}
		vertexCount = 0;
	}

	protected virtual void Awake()
	{
		base.gameObject.AddComponent<MeshRenderer>();
		base.gameObject.AddComponent<MeshFilter>();
	}

	private void OnEnable()
	{
		if (tmBatchedInstance.OnActivated != null)
		{
			tmBatchedInstance.OnActivated(this);
		}
	}

	private void OnDisable()
	{
		if (tmBatchedInstance.OnDisabled != null)
		{
			tmBatchedInstance.OnDisabled(this);
		}
	}

	private void OnDestroy()
	{
		if (Application.isPlaying)
		{
			Clear();
			UnityEngine.Object.Destroy(mesh);
		}
		if (this.OnDestroyEvent != null)
		{
			this.OnDestroyEvent();
		}
	}

	public void UpdatePosition(float delta)
	{
		if (shouldMove)
		{
			CachedTransform.position -= Vector3.forward * delta;
		}
	}

	private void AddToForced(tmBatchObject part)
	{
		forcedParts.Add(part);
	}

	private void RemoveFromForced(tmBatchObject part)
	{
		forcedParts.Remove(part);
	}
}
