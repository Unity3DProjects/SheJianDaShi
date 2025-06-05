using System.Collections;
using System.Collections.Generic;
using Spine;
using UnityEngine;

[RequireComponent(typeof(SkeletonRenderer))]
public class SkeletonRagdoll : MonoBehaviour
{
	private static Transform helper;

	[Header("Colliders")]
	public PhysicMaterial material;

	[Header("Hierarchy")]
	[SpineBone("", "")]
	public string startingBoneName = string.Empty;

	[SpineBone("", "")]
	public List<string> stopBoneNames = new List<string>();

	[Header("Parameters")]
	public bool applyOnStart;

	[Tooltip("Set RootRigidbody IsKinematic to true when Apply is called.")]
	public bool pinStartBone;

	[Tooltip("Enable Collision between adjacent ragdoll elements (IE: Neck and Head)")]
	public bool enableJointCollision;

	public bool useGravity = true;

	[Tooltip("Warning!  You will have to re-enable and tune mix values manually if attempting to remove the ragdoll system.")]
	public bool disableIK = true;

	[Tooltip("If no BoundingBox Attachment is attached to a bone, this becomes the default Width or Radius of a Bone's ragdoll Rigidbody")]
	public float thickness = 0.125f;

	[Tooltip("Default rotational limit value.  Min is negative this value, Max is this value.")]
	public float rotationLimit = 20f;

	[SpineBone("", "")]
	public List<string> boneNamesWithoutRotationLimit = new List<string>();

	public float rootMass = 20f;

	[Tooltip("If your ragdoll seems unstable or uneffected by limits, try lowering this value.")]
	[Range(0.01f, 1f)]
	public float massFalloffFactor = 0.4f;

	[Tooltip("The layer assigned to all of the rigidbody parts.")]
	public int colliderLayer;

	[Range(0f, 1f)]
	public float mix = 1f;

	private Rigidbody rootRigidbody;

	private ISkeletonAnimation skeletonAnim;

	private Skeleton skeleton;

	private Dictionary<Bone, Transform> boneTable = new Dictionary<Bone, Transform>();

	private Bone startingBone;

	private Transform ragdollRoot;

	private Vector3 rootOffset;

	private bool isActive;

	public Rigidbody RootRigidbody
	{
		get
		{
			return rootRigidbody;
		}
	}

	public Vector3 RootOffset
	{
		get
		{
			return rootOffset;
		}
	}

	public Vector3 EstimatedSkeletonPosition
	{
		get
		{
			return rootRigidbody.position - rootOffset;
		}
	}

	public bool IsActive
	{
		get
		{
			return isActive;
		}
	}

	private IEnumerator Start()
	{
		skeletonAnim = (ISkeletonAnimation)GetComponent<SkeletonRenderer>();
		if (helper == null)
		{
			helper = new GameObject("Helper").transform;
			helper.hideFlags = HideFlags.HideInHierarchy;
		}
		if (applyOnStart)
		{
			yield return null;
			Apply();
		}
	}

	public Coroutine SmoothMix(float target, float duration)
	{
		return StartCoroutine(SmoothMixCoroutine(target, duration));
	}

	private IEnumerator SmoothMixCoroutine(float target, float duration)
	{
		float startTime = Time.time;
		float startMix = mix;
		while (mix > 0f)
		{
			mix = Mathf.SmoothStep(startMix, target, (Time.time - startTime) / duration);
			yield return null;
		}
	}

	public void SetSkeletonPosition(Vector3 worldPosition)
	{
		if (!isActive)
		{
			Debug.LogWarning("Can't call SetSkeletonPosition while Ragdoll is not active!");
			return;
		}
		Vector3 vector = worldPosition - base.transform.position;
		base.transform.position = worldPosition;
		foreach (Transform value in boneTable.Values)
		{
			value.position -= vector;
		}
		UpdateWorld(null);
		skeleton.UpdateWorldTransform();
	}

	public Rigidbody[] GetRigidbodyArray()
	{
		if (!isActive)
		{
			return new Rigidbody[0];
		}
		Rigidbody[] array = new Rigidbody[boneTable.Count];
		int num = 0;
		foreach (Transform value in boneTable.Values)
		{
			array[num] = value.GetComponent<Rigidbody>();
			num++;
		}
		return array;
	}

	public Rigidbody GetRigidbody(string boneName)
	{
		Bone bone = skeleton.FindBone(boneName);
		if (bone == null)
		{
			return null;
		}
		if (boneTable.ContainsKey(bone))
		{
			return boneTable[bone].GetComponent<Rigidbody>();
		}
		return null;
	}

	public void Remove()
	{
		isActive = false;
		foreach (Transform value in boneTable.Values)
		{
			Object.Destroy(value.gameObject);
		}
		Object.Destroy(ragdollRoot.gameObject);
		boneTable.Clear();
		skeletonAnim.UpdateWorld -= UpdateWorld;
	}

	public void Apply()
	{
		isActive = true;
		skeleton = skeletonAnim.Skeleton;
		mix = 1f;
		Bone bone = (startingBone = skeleton.FindBone(startingBoneName));
		RecursivelyCreateBoneProxies(bone);
		rootRigidbody = boneTable[bone].GetComponent<Rigidbody>();
		rootRigidbody.isKinematic = pinStartBone;
		rootRigidbody.mass = rootMass;
		List<Collider> list = new List<Collider>();
		foreach (KeyValuePair<Bone, Transform> item in boneTable)
		{
			Bone key = item.Key;
			Transform value = item.Value;
			Bone bone2 = null;
			Transform transform = base.transform;
			Collider component = value.GetComponent<Collider>();
			if (material != null)
			{
				component.material = material;
			}
			list.Add(component);
			if (key != startingBone)
			{
				bone2 = key.Parent;
				transform = boneTable[bone2];
			}
			else
			{
				ragdollRoot = new GameObject("RagdollRoot").transform;
				ragdollRoot.parent = base.transform;
				if (key == skeleton.RootBone)
				{
					ragdollRoot.localPosition = new Vector3(key.WorldX, key.WorldY, 0f);
					ragdollRoot.localRotation = Quaternion.Euler(0f, 0f, GetCompensatedRotationIK(key));
					transform = ragdollRoot;
				}
				else
				{
					ragdollRoot.localPosition = new Vector3(key.Parent.WorldX, key.Parent.WorldY, 0f);
					ragdollRoot.localRotation = Quaternion.Euler(0f, 0f, GetCompensatedRotationIK(key.Parent));
					transform = ragdollRoot;
				}
				rootOffset = value.position - base.transform.position;
			}
			Rigidbody component2 = transform.GetComponent<Rigidbody>();
			if (component2 != null)
			{
				HingeJoint hingeJoint = value.gameObject.AddComponent<HingeJoint>();
				hingeJoint.connectedBody = component2;
				Vector3 connectedAnchor = transform.InverseTransformPoint(value.position);
				connectedAnchor.x *= 1f;
				hingeJoint.connectedAnchor = connectedAnchor;
				hingeJoint.axis = Vector3.forward;
				hingeJoint.GetComponent<Rigidbody>().mass = hingeJoint.connectedBody.mass * massFalloffFactor;
				JointLimits limits = default(JointLimits);
				if (!boneNamesWithoutRotationLimit.Contains(key.Data.Name))
				{
					limits.min = 0f - rotationLimit;
					limits.max = rotationLimit;
					hingeJoint.limits = limits;
					hingeJoint.useLimits = true;
				}
				hingeJoint.enableCollision = enableJointCollision;
			}
		}
		for (int i = 0; i < list.Count; i++)
		{
			for (int j = 0; j < list.Count; j++)
			{
				if (i != j)
				{
					Physics.IgnoreCollision(list[i], list[j]);
				}
			}
		}
		SkeletonUtilityBone[] componentsInChildren = GetComponentsInChildren<SkeletonUtilityBone>();
		if (componentsInChildren.Length > 0)
		{
			List<string> list2 = new List<string>();
			SkeletonUtilityBone[] array = componentsInChildren;
			foreach (SkeletonUtilityBone skeletonUtilityBone in array)
			{
				if (skeletonUtilityBone.mode == SkeletonUtilityBone.Mode.Override)
				{
					list2.Add(skeletonUtilityBone.gameObject.name);
					Object.Destroy(skeletonUtilityBone.gameObject);
				}
			}
			if (list2.Count > 0)
			{
				string text = "Destroyed Utility Bones: ";
				for (int l = 0; l < list2.Count; l++)
				{
					text += list2[l];
					if (l != list2.Count - 1)
					{
						text += ",";
					}
				}
				Debug.LogWarning(text);
			}
		}
		if (disableIK)
		{
			foreach (IkConstraint ikConstraint in skeleton.IkConstraints)
			{
				ikConstraint.Mix = 0f;
			}
		}
		skeletonAnim.UpdateWorld += UpdateWorld;
	}

	private void RecursivelyCreateBoneProxies(Bone b)
	{
		if (stopBoneNames.Contains(b.Data.Name))
		{
			return;
		}
		GameObject gameObject = new GameObject(b.Data.Name);
		gameObject.layer = colliderLayer;
		Transform transform = gameObject.transform;
		boneTable.Add(b, transform);
		transform.parent = base.transform;
		transform.localPosition = new Vector3(b.WorldX, b.WorldY, 0f);
		transform.localRotation = Quaternion.Euler(0f, 0f, (!(b.WorldFlipX ^ b.WorldFlipY)) ? b.WorldRotation : (0f - b.WorldRotation));
		transform.localScale = new Vector3(b.WorldScaleX, b.WorldScaleY, 1f);
		float length = b.Data.Length;
		List<Collider> list = AttachBoundingBoxRagdollColliders(b);
		if (length == 0f)
		{
			if (list.Count == 0)
			{
				SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
				sphereCollider.radius = thickness / 2f;
			}
		}
		else if (list.Count == 0)
		{
			BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
			boxCollider.size = new Vector3(length, thickness, thickness);
			boxCollider.center = new Vector3(((!b.WorldFlipX) ? length : (0f - length)) / 2f, 0f);
		}
		Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
		rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;
		foreach (Bone child in b.Children)
		{
			RecursivelyCreateBoneProxies(child);
		}
	}

	private List<Collider> AttachBoundingBoxRagdollColliders(Bone b)
	{
		List<Collider> list = new List<Collider>();
		Transform transform = boneTable[b];
		GameObject gameObject = transform.gameObject;
		Skin skin = skeleton.Skin;
		if (skin == null)
		{
			skin = skeleton.Data.DefaultSkin;
		}
		bool worldFlipX = b.WorldFlipX;
		bool worldFlipY = b.WorldFlipY;
		List<Attachment> list2 = new List<Attachment>();
		foreach (Slot slot in skeleton.Slots)
		{
			if (slot.Bone != b)
			{
				continue;
			}
			skin.FindAttachmentsForSlot(skeleton.Slots.IndexOf(slot), list2);
			foreach (Attachment item in list2)
			{
				if (!(item is BoundingBoxAttachment) || !item.Name.ToLower().Contains("ragdoll"))
				{
					continue;
				}
				SkeletonUtilityMeshCollider skeletonUtilityMeshCollider = SkeletonUtility.AddBoundingBox3DAsComponent((BoundingBoxAttachment)item, gameObject, false, thickness);
				if (worldFlipX || worldFlipY)
				{
					Vector3[] vertices = skeletonUtilityMeshCollider.CachedMesh.vertices;
					for (int i = 0; i < vertices.Length; i++)
					{
						if (worldFlipX)
						{
							vertices[i].x *= -1f;
						}
						if (worldFlipY)
						{
							vertices[i].y *= -1f;
						}
					}
					skeletonUtilityMeshCollider.CachedMesh.vertices = vertices;
				}
				list.Add(skeletonUtilityMeshCollider.CachedCollider);
			}
		}
		return list;
	}

	private void UpdateWorld(SkeletonRenderer skeletonRenderer)
	{
		foreach (KeyValuePair<Bone, Transform> item in boneTable)
		{
			Bone key = item.Key;
			Transform value = item.Value;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			Bone bone = null;
			Transform transform = base.transform;
			if (key != startingBone)
			{
				bone = key.Parent;
				transform = boneTable[bone];
				flag2 = bone.WorldFlipX;
				flag3 = bone.WorldFlipY;
			}
			else
			{
				bone = key.Parent;
				transform = ragdollRoot;
				if (key.Parent != null)
				{
					flag2 = key.worldFlipX;
					flag3 = key.WorldFlipY;
				}
				else
				{
					flag2 = key.Skeleton.FlipX;
					flag3 = key.Skeleton.FlipY;
				}
			}
			flag = flag2 ^ flag3;
			helper.position = transform.position;
			helper.rotation = transform.rotation;
			helper.localScale = new Vector3((!flag2) ? transform.localScale.x : (0f - transform.localScale.x), (!flag3) ? transform.localScale.y : (0f - transform.localScale.y), 1f);
			Vector3 position = value.position;
			position = helper.InverseTransformPoint(position);
			key.X = Mathf.Lerp(key.X, position.x, mix);
			key.Y = Mathf.Lerp(key.Y, position.y, mix);
			Vector3 vector = helper.InverseTransformDirection(value.right);
			float num = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
			if (key.WorldFlipX ^ key.WorldFlipY)
			{
				num *= -1f;
			}
			if (bone != null && (key.WorldFlipX ^ key.WorldFlipY) != flag)
			{
				num -= GetCompensatedRotationIK(bone) * 2f;
			}
			key.Rotation = Mathf.Lerp(key.Rotation, num, mix);
			key.RotationIK = Mathf.Lerp(key.rotationIK, num, mix);
		}
	}

	private float GetCompensatedRotationIK(Bone b)
	{
		Bone parent = b.Parent;
		float num = b.RotationIK;
		while (parent != null)
		{
			num += parent.RotationIK;
			parent = parent.parent;
		}
		return num;
	}
}
