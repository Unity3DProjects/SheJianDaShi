using System;
using System.Collections.Generic;
using UnityEngine;

namespace Spine
{
	public class Skin
	{
		private class AttachmentKeyTupleComparer : IEqualityComparer<AttachmentKeyTuple>
		{
			internal static readonly AttachmentKeyTupleComparer Instance = new AttachmentKeyTupleComparer();

			bool IEqualityComparer<AttachmentKeyTuple>.Equals(AttachmentKeyTuple o1, AttachmentKeyTuple o2)
			{
				return o1.SlotIndex == o2.SlotIndex && o1.NameHashCode == o2.NameHashCode && o1.Name == o2.Name;
			}

			int IEqualityComparer<AttachmentKeyTuple>.GetHashCode(AttachmentKeyTuple o)
			{
				return o.SlotIndex;
			}
		}

		private class AttachmentKeyTuple
		{
			public readonly int SlotIndex;

			public readonly string Name;

			public readonly int NameHashCode;

			public AttachmentKeyTuple(int slotIndex, string name)
			{
				SlotIndex = slotIndex;
				Name = name;
				NameHashCode = Name.GetHashCode();
			}
		}

		internal string name;

		private Dictionary<AttachmentKeyTuple, Attachment> attachments = new Dictionary<AttachmentKeyTuple, Attachment>(AttachmentKeyTupleComparer.Instance);

		public string Name
		{
			get
			{
				return name;
			}
		}

		public Skin(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name cannot be null.");
			}
			this.name = name;
		}

		public Skin Copy()
		{
			Skin skin = new Skin(name);
			foreach (KeyValuePair<AttachmentKeyTuple, Attachment> attachment in attachments)
			{
				skin.AddAttachment(attachment.Key.SlotIndex, attachment.Key.Name, attachment.Value.CopyAttachment());
			}
			return skin;
		}

		public void AddAttachment(int slotIndex, string name, Attachment attachment)
		{
			if (attachment == null)
			{
				throw new ArgumentNullException("attachment cannot be null.");
			}
			attachments[new AttachmentKeyTuple(slotIndex, name)] = attachment;
		}

		public void SetAttachment(int slotIndex, string name, Attachment attachment)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name cannot be null.");
			}
			if (attachment == null)
			{
				throw new ArgumentNullException("attachment cannot be null.");
			}
			attachments.Remove(new AttachmentKeyTuple(slotIndex, name));
			attachments.Add(new AttachmentKeyTuple(slotIndex, name), attachment);
		}

		public Attachment GetAttachment(int slotIndex, string name)
		{
			Attachment value;
			attachments.TryGetValue(new AttachmentKeyTuple(slotIndex, name), out value);
			return value;
		}

		public void FindNamesForSlot(int slotIndex, List<string> names)
		{
			if (names == null)
			{
				throw new ArgumentNullException("names cannot be null.");
			}
			foreach (AttachmentKeyTuple key in attachments.Keys)
			{
				if (key.SlotIndex == slotIndex)
				{
					names.Add(key.Name);
				}
			}
		}

		public void FindAttachmentsForSlot(int slotIndex, List<Attachment> attachments)
		{
			if (attachments == null)
			{
				throw new ArgumentNullException("attachments cannot be null.");
			}
			foreach (KeyValuePair<AttachmentKeyTuple, Attachment> attachment in this.attachments)
			{
				if (attachment.Key.SlotIndex == slotIndex)
				{
					attachments.Add(attachment.Value);
				}
			}
		}

		public override string ToString()
		{
			return name;
		}

		internal void AttachAll(Skeleton skeleton, Skin oldSkin)
		{
			foreach (KeyValuePair<AttachmentKeyTuple, Attachment> attachment2 in oldSkin.attachments)
			{
				int slotIndex = attachment2.Key.SlotIndex;
				Slot slot = skeleton.slots.Items[slotIndex];
				if (slot.attachment == attachment2.Value)
				{
					Attachment attachment = GetAttachment(slotIndex, attachment2.Key.Name);
					if (attachment != null)
					{
						slot.Attachment = attachment;
					}
				}
			}
		}

		public bool isValid()
		{
			bool result = true;
			foreach (Attachment value in attachments.Values)
			{
				RegionAttachment regionAttachment = value as RegionAttachment;
				if (regionAttachment != null)
				{
					Material material = regionAttachment.RendererObject as Material;
					if (material == null)
					{
						result = false;
						break;
					}
				}
			}
			return result;
		}
	}
}
