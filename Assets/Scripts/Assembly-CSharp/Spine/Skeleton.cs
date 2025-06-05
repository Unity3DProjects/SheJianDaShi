using System;

namespace Spine
{
	public class Skeleton
	{
		internal SkeletonData data;

		internal ExposedList<Bone> bones;

		internal ExposedList<Slot> slots;

		internal ExposedList<Slot> drawOrder;

		internal ExposedList<IkConstraint> ikConstraints;

		private ExposedList<ExposedList<Bone>> boneCache = new ExposedList<ExposedList<Bone>>();

		internal Skin skin;

		internal Skin initialSkin;

		internal float r = 1f;

		internal float g = 1f;

		internal float b = 1f;

		internal float a = 1f;

		internal float time;

		internal bool flipX;

		internal bool flipY;

		internal float x;

		internal float y;

		public SkeletonData Data
		{
			get
			{
				return data;
			}
		}

		public ExposedList<Bone> Bones
		{
			get
			{
				return bones;
			}
		}

		public ExposedList<Slot> Slots
		{
			get
			{
				return slots;
			}
		}

		public ExposedList<Slot> DrawOrder
		{
			get
			{
				return drawOrder;
			}
		}

		public ExposedList<IkConstraint> IkConstraints
		{
			get
			{
				return ikConstraints;
			}
			set
			{
				ikConstraints = value;
			}
		}

		public Skin Skin
		{
			get
			{
				return skin;
			}
			set
			{
				skin = value;
			}
		}

		public float R
		{
			get
			{
				return r;
			}
			set
			{
				r = value;
			}
		}

		public float G
		{
			get
			{
				return g;
			}
			set
			{
				g = value;
			}
		}

		public float B
		{
			get
			{
				return b;
			}
			set
			{
				b = value;
			}
		}

		public float A
		{
			get
			{
				return a;
			}
			set
			{
				a = value;
			}
		}

		public float Time
		{
			get
			{
				return time;
			}
			set
			{
				time = value;
			}
		}

		public float X
		{
			get
			{
				return x;
			}
			set
			{
				x = value;
			}
		}

		public float Y
		{
			get
			{
				return y;
			}
			set
			{
				y = value;
			}
		}

		public bool FlipX
		{
			get
			{
				return flipX;
			}
			set
			{
				flipX = value;
			}
		}

		public bool FlipY
		{
			get
			{
				return flipY;
			}
			set
			{
				flipY = value;
			}
		}

		public Bone RootBone
		{
			get
			{
				return (bones.Count != 0) ? bones.Items[0] : null;
			}
		}

		public Skeleton(SkeletonData data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data cannot be null.");
			}
			this.data = data;
			bones = new ExposedList<Bone>(data.bones.Count);
			foreach (BoneData bone3 in data.bones)
			{
				Bone bone = ((bone3.parent != null) ? bones.Items[data.bones.IndexOf(bone3.parent)] : null);
				Bone item = new Bone(bone3, this, bone);
				if (bone != null)
				{
					bone.children.Add(item);
				}
				bones.Add(item);
			}
			slots = new ExposedList<Slot>(data.slots.Count);
			drawOrder = new ExposedList<Slot>(data.slots.Count);
			foreach (SlotData slot in data.slots)
			{
				Bone bone2 = bones.Items[data.bones.IndexOf(slot.boneData)];
				Slot item2 = new Slot(slot, bone2);
				slots.Add(item2);
				drawOrder.Add(item2);
			}
			ikConstraints = new ExposedList<IkConstraint>(data.ikConstraints.Count);
			foreach (IkConstraintData ikConstraint in data.ikConstraints)
			{
				ikConstraints.Add(new IkConstraint(ikConstraint, this));
			}
			UpdateCache();
		}

		public void UpdateCache()
		{
			ExposedList<ExposedList<Bone>> exposedList = boneCache;
			ExposedList<IkConstraint> exposedList2 = ikConstraints;
			int count = exposedList2.Count;
			int num = count + 1;
			if (exposedList.Count > num)
			{
				exposedList.RemoveRange(num, exposedList.Count - num);
			}
			int i = 0;
			for (int count2 = exposedList.Count; i < count2; i++)
			{
				exposedList.Items[i].Clear();
			}
			while (exposedList.Count < num)
			{
				exposedList.Add(new ExposedList<Bone>());
			}
			ExposedList<Bone> exposedList3 = exposedList.Items[0];
			int j = 0;
			for (int count3 = bones.Count; j < count3; j++)
			{
				Bone bone = bones.Items[j];
				Bone bone2 = bone;
				while (true)
				{
					int k;
					for (k = 0; k < count; k++)
					{
						IkConstraint ikConstraint = exposedList2.Items[k];
						Bone bone3 = ikConstraint.bones.Items[0];
						Bone bone4 = ikConstraint.bones.Items[ikConstraint.bones.Count - 1];
						while (bone2 != bone4)
						{
							if (bone4 == bone3)
							{
								goto IL_013d;
							}
							bone4 = bone4.parent;
						}
						goto IL_00fa;
						IL_013d:;
					}
					bone2 = bone2.parent;
					if (bone2 == null)
					{
						exposedList3.Add(bone);
						break;
					}
					continue;
					IL_00fa:
					exposedList.Items[k].Add(bone);
					exposedList.Items[k + 1].Add(bone);
					break;
				}
			}
		}

		public void UpdateWorldTransform()
		{
			ExposedList<Bone> exposedList = bones;
			int i = 0;
			for (int count = exposedList.Count; i < count; i++)
			{
				Bone bone = exposedList.Items[i];
				bone.rotationIK = bone.rotation;
			}
			ExposedList<ExposedList<Bone>> exposedList2 = boneCache;
			ExposedList<IkConstraint> exposedList3 = ikConstraints;
			int num = 0;
			int num2 = exposedList2.Count - 1;
			while (true)
			{
				ExposedList<Bone> exposedList4 = exposedList2.Items[num];
				int j = 0;
				for (int count2 = exposedList4.Count; j < count2; j++)
				{
					exposedList4.Items[j].UpdateWorldTransform();
				}
				if (num == num2)
				{
					break;
				}
				exposedList3.Items[num].apply();
				num++;
			}
		}

		public void SetToSetupPose()
		{
			SetBonesToSetupPose();
			SetSlotsToSetupPose();
		}

		public void SetBonesToSetupPose()
		{
			ExposedList<Bone> exposedList = bones;
			int i = 0;
			for (int count = exposedList.Count; i < count; i++)
			{
				exposedList.Items[i].SetToSetupPose();
			}
			ExposedList<IkConstraint> exposedList2 = ikConstraints;
			int j = 0;
			for (int count2 = exposedList2.Count; j < count2; j++)
			{
				IkConstraint ikConstraint = exposedList2.Items[j];
				ikConstraint.bendDirection = ikConstraint.data.bendDirection;
				ikConstraint.mix = ikConstraint.data.mix;
			}
		}

		public void SetSlotsToSetupPose()
		{
			ExposedList<Slot> exposedList = slots;
			drawOrder.Clear();
			int i = 0;
			for (int count = exposedList.Count; i < count; i++)
			{
				drawOrder.Add(exposedList.Items[i]);
			}
			int j = 0;
			for (int count2 = exposedList.Count; j < count2; j++)
			{
				exposedList.Items[j].SetToSetupPose(j);
				SetSkinAttachment(j, exposedList.Items[j].Attachment);
			}
		}

		public Bone FindBone(string boneName)
		{
			if (boneName == null)
			{
				throw new ArgumentNullException("boneName cannot be null.");
			}
			ExposedList<Bone> exposedList = bones;
			int i = 0;
			for (int count = exposedList.Count; i < count; i++)
			{
				Bone bone = exposedList.Items[i];
				if (bone.data.name == boneName)
				{
					return bone;
				}
			}
			return null;
		}

		public int FindBoneIndex(string boneName)
		{
			if (boneName == null)
			{
				throw new ArgumentNullException("boneName cannot be null.");
			}
			ExposedList<Bone> exposedList = bones;
			int i = 0;
			for (int count = exposedList.Count; i < count; i++)
			{
				if (exposedList.Items[i].data.name == boneName)
				{
					return i;
				}
			}
			return -1;
		}

		public Slot FindSlot(string slotName)
		{
			if (slotName == null)
			{
				throw new ArgumentNullException("slotName cannot be null.");
			}
			ExposedList<Slot> exposedList = slots;
			int i = 0;
			for (int count = exposedList.Count; i < count; i++)
			{
				Slot slot = exposedList.Items[i];
				if (slot.data.name == slotName)
				{
					return slot;
				}
			}
			return null;
		}

		public int FindSlotIndex(string slotName)
		{
			if (slotName == null)
			{
				throw new ArgumentNullException("slotName cannot be null.");
			}
			ExposedList<Slot> exposedList = slots;
			int i = 0;
			for (int count = exposedList.Count; i < count; i++)
			{
				if (exposedList.Items[i].data.name.Equals(slotName))
				{
					return i;
				}
			}
			return -1;
		}

		public void SetSkin(string skinName)
		{
			Skin skin = data.FindSkin(skinName);
			if (skin == null)
			{
				throw new ArgumentException("Skin not found: " + skinName);
			}
			SetSkin(skin);
		}

		public void SetInitialSkin(string skinName)
		{
			Skin skin = data.FindSkin(skinName);
			if (skin == null)
			{
				throw new ArgumentException("Skin not found: " + skinName);
			}
			if (skin != initialSkin)
			{
				initialSkin = skin;
			}
			SetSkin(skin);
		}

		public void SetSkin(Skin newSkin)
		{
			if (newSkin != null)
			{
				newSkin = newSkin.Copy();
				if (skin != null)
				{
					newSkin.AttachAll(this, skin);
				}
				else
				{
					ExposedList<Slot> exposedList = slots;
					int i = 0;
					for (int count = exposedList.Count; i < count; i++)
					{
						Slot slot = exposedList.Items[i];
						string attachmentName = slot.data.attachmentName;
						if (attachmentName != null)
						{
							Attachment attachment = newSkin.GetAttachment(i, attachmentName);
							if (attachment != null)
							{
								slot.Attachment = attachment;
							}
						}
					}
				}
			}
			skin = newSkin;
		}

		public Attachment GetAttachment(string slotName, string attachmentName)
		{
			return GetAttachment(data.FindSlotIndex(slotName), attachmentName);
		}

		public Attachment GetAttachment(int slotIndex, string attachmentName)
		{
			if (attachmentName == null)
			{
				throw new ArgumentNullException("attachmentName cannot be null.");
			}
			if (skin != null)
			{
				Attachment attachment = skin.GetAttachment(slotIndex, attachmentName);
				if (attachment != null)
				{
					return attachment;
				}
			}
			if (initialSkin != null)
			{
				Attachment attachment2 = initialSkin.GetAttachment(slotIndex, attachmentName);
				if (attachment2 != null)
				{
					return attachment2;
				}
			}
			if (data.defaultSkin != null)
			{
				return data.defaultSkin.GetAttachment(slotIndex, attachmentName);
			}
			return null;
		}

		public Attachment GetDefaultAttachment(string slotName, string attachmentName)
		{
			return GetDefaultAttachment(data.FindSlotIndex(slotName), attachmentName);
		}

		public Attachment GetDefaultAttachment(int slotIndex, string attachmentName)
		{
			if (attachmentName == null)
			{
				throw new ArgumentNullException("attachmentName cannot be null.");
			}
			if (initialSkin != null)
			{
				Attachment attachment = initialSkin.GetAttachment(slotIndex, attachmentName);
				if (attachment != null)
				{
					return attachment;
				}
			}
			if (data.defaultSkin != null)
			{
				return data.defaultSkin.GetAttachment(slotIndex, attachmentName);
			}
			return null;
		}

		public void SetAttachment(string slotName, string attachmentName)
		{
			if (slotName == null)
			{
				throw new ArgumentNullException("slotName cannot be null.");
			}
			ExposedList<Slot> exposedList = slots;
			int i = 0;
			for (int count = exposedList.Count; i < count; i++)
			{
				Slot slot = exposedList.Items[i];
				if (!(slot.data.name == slotName))
				{
					continue;
				}
				Attachment attachment = null;
				if (attachmentName != null)
				{
					attachment = GetAttachment(i, attachmentName);
					if (attachment == null)
					{
						throw new Exception("Attachment not found: " + attachmentName + ", for slot: " + slotName);
					}
				}
				slot.Attachment = attachment;
				return;
			}
			throw new Exception("Slot not found: " + slotName);
		}

		public void SetSkinAttachment(string slotName, Attachment newAttachment)
		{
			if (slotName == null)
			{
				throw new ArgumentNullException("slotName cannot be null.");
			}
			int num = FindSlotIndex(slotName);
			if (num > -1)
			{
				SetSkinAttachment(num, newAttachment);
			}
		}

		public void SetSkinAttachment(Slot slot, Attachment newAttachment)
		{
			if (slot != null)
			{
				int num = slots.IndexOf(slot);
				if (num > -1)
				{
					SetSkinAttachment(num, newAttachment);
				}
			}
		}

		public void SetSkinAttachment(int slotIndex, Attachment newAttachment)
		{
			if (newAttachment != null && skin != null)
			{
				skin.SetAttachment(slotIndex, newAttachment.Name, newAttachment);
			}
		}

		public IkConstraint FindIkConstraint(string ikConstraintName)
		{
			if (ikConstraintName == null)
			{
				throw new ArgumentNullException("ikConstraintName cannot be null.");
			}
			ExposedList<IkConstraint> exposedList = ikConstraints;
			int i = 0;
			for (int count = exposedList.Count; i < count; i++)
			{
				IkConstraint ikConstraint = exposedList.Items[i];
				if (ikConstraint.data.name == ikConstraintName)
				{
					return ikConstraint;
				}
			}
			return null;
		}

		public void Update(float delta)
		{
			time += delta;
		}
	}
}
