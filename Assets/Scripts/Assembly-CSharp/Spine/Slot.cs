using System;

namespace Spine
{
	public class Slot
	{
		internal SlotData data;

		internal Bone bone;

		internal float r;

		internal float g;

		internal float b;

		internal float a;

		internal Attachment attachment;

		internal float attachmentTime;

		internal float[] attachmentVertices = new float[0];

		internal int attachmentVerticesCount;

		public SlotData Data
		{
			get
			{
				return data;
			}
		}

		public Bone Bone
		{
			get
			{
				return bone;
			}
		}

		public Skeleton Skeleton
		{
			get
			{
				return bone.skeleton;
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

		public Attachment Attachment
		{
			get
			{
				return attachment;
			}
			set
			{
				attachment = value;
				attachmentTime = bone.skeleton.time;
				attachmentVerticesCount = 0;
			}
		}

		public float AttachmentTime
		{
			get
			{
				return bone.skeleton.time - attachmentTime;
			}
			set
			{
				attachmentTime = bone.skeleton.time - value;
			}
		}

		public float[] AttachmentVertices
		{
			get
			{
				return attachmentVertices;
			}
			set
			{
				attachmentVertices = value;
			}
		}

		public int AttachmentVerticesCount
		{
			get
			{
				return attachmentVerticesCount;
			}
			set
			{
				attachmentVerticesCount = value;
			}
		}

		public Slot(SlotData data, Bone bone)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data cannot be null.");
			}
			if (bone == null)
			{
				throw new ArgumentNullException("bone cannot be null.");
			}
			this.data = data;
			this.bone = bone;
			SetToSetupPose();
		}

		internal void SetToSetupPose(int slotIndex)
		{
			r = data.r;
			g = data.g;
			b = data.b;
			a = data.a;
			Attachment = ((data.attachmentName != null) ? bone.skeleton.GetDefaultAttachment(slotIndex, data.attachmentName) : null);
		}

		public void SetToSetupPose()
		{
			SetToSetupPose(bone.skeleton.data.slots.IndexOf(data));
		}

		public override string ToString()
		{
			return data.name;
		}
	}
}
