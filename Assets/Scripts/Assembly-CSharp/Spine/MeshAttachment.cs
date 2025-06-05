namespace Spine
{
	public class MeshAttachment : Attachment
	{
		internal float[] vertices;

		internal float[] uvs;

		internal float[] regionUVs;

		internal int[] triangles;

		internal float regionOffsetX;

		internal float regionOffsetY;

		internal float regionWidth;

		internal float regionHeight;

		internal float regionOriginalWidth;

		internal float regionOriginalHeight;

		internal float r = 1f;

		internal float g = 1f;

		internal float b = 1f;

		internal float a = 1f;

		public int HullLength { get; set; }

		public float[] Vertices
		{
			get
			{
				return vertices;
			}
			set
			{
				vertices = value;
			}
		}

		public float[] RegionUVs
		{
			get
			{
				return regionUVs;
			}
			set
			{
				regionUVs = value;
			}
		}

		public float[] UVs
		{
			get
			{
				return uvs;
			}
			set
			{
				uvs = value;
			}
		}

		public int[] Triangles
		{
			get
			{
				return triangles;
			}
			set
			{
				triangles = value;
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

		public string Path { get; set; }

		public object RendererObject { get; set; }

		public float RegionU { get; set; }

		public float RegionV { get; set; }

		public float RegionU2 { get; set; }

		public float RegionV2 { get; set; }

		public bool RegionRotate { get; set; }

		public float RegionOffsetX
		{
			get
			{
				return regionOffsetX;
			}
			set
			{
				regionOffsetX = value;
			}
		}

		public float RegionOffsetY
		{
			get
			{
				return regionOffsetY;
			}
			set
			{
				regionOffsetY = value;
			}
		}

		public float RegionWidth
		{
			get
			{
				return regionWidth;
			}
			set
			{
				regionWidth = value;
			}
		}

		public float RegionHeight
		{
			get
			{
				return regionHeight;
			}
			set
			{
				regionHeight = value;
			}
		}

		public float RegionOriginalWidth
		{
			get
			{
				return regionOriginalWidth;
			}
			set
			{
				regionOriginalWidth = value;
			}
		}

		public float RegionOriginalHeight
		{
			get
			{
				return regionOriginalHeight;
			}
			set
			{
				regionOriginalHeight = value;
			}
		}

		public int[] Edges { get; set; }

		public float Width { get; set; }

		public float Height { get; set; }

		public MeshAttachment(string name)
			: base(name)
		{
		}

		public void UpdateUVs()
		{
			float regionU = RegionU;
			float regionV = RegionV;
			float num = RegionU2 - RegionU;
			float num2 = RegionV2 - RegionV;
			float[] array = regionUVs;
			if (uvs == null || uvs.Length != array.Length)
			{
				uvs = new float[array.Length];
			}
			float[] array2 = uvs;
			if (RegionRotate)
			{
				int i = 0;
				for (int num3 = array2.Length; i < num3; i += 2)
				{
					array2[i] = regionU + array[i + 1] * num;
					array2[i + 1] = regionV + num2 - array[i] * num2;
				}
			}
			else
			{
				int j = 0;
				for (int num4 = array2.Length; j < num4; j += 2)
				{
					array2[j] = regionU + array[j] * num;
					array2[j + 1] = regionV + array[j + 1] * num2;
				}
			}
		}

		public void ComputeWorldVertices(Slot slot, float[] worldVertices)
		{
			Bone bone = slot.bone;
			float num = bone.skeleton.x + bone.worldX;
			float num2 = bone.skeleton.y + bone.worldY;
			float m = bone.m00;
			float m2 = bone.m01;
			float m3 = bone.m10;
			float m4 = bone.m11;
			float[] attachmentVertices = vertices;
			int num3 = attachmentVertices.Length;
			if (slot.attachmentVerticesCount == num3)
			{
				attachmentVertices = slot.AttachmentVertices;
			}
			for (int i = 0; i < num3; i += 2)
			{
				float num4 = attachmentVertices[i];
				float num5 = attachmentVertices[i + 1];
				worldVertices[i] = num4 * m + num5 * m2 + num;
				worldVertices[i + 1] = num4 * m3 + num5 * m4 + num2;
			}
		}

		public override Attachment CopyAttachment()
		{
			MeshAttachment meshAttachment = new MeshAttachment(Name);
			meshAttachment.Path = Path;
			meshAttachment.RendererObject = RendererObject;
			meshAttachment.RegionRotate = RegionRotate;
			meshAttachment.RegionOriginalWidth = RegionOriginalWidth;
			meshAttachment.RegionOriginalHeight = RegionOriginalHeight;
			meshAttachment.RegionWidth = RegionWidth;
			meshAttachment.RegionHeight = RegionHeight;
			meshAttachment.RegionOffsetX = RegionOffsetX;
			meshAttachment.RegionOffsetY = RegionOffsetY;
			meshAttachment.RegionU = RegionU;
			meshAttachment.RegionU2 = RegionU2;
			meshAttachment.RegionV = RegionV;
			meshAttachment.RegionV2 = RegionV2;
			meshAttachment.HullLength = HullLength;
			if (Vertices != null)
			{
				meshAttachment.Vertices = (float[])Vertices.Clone();
			}
			if (RegionUVs != null)
			{
				meshAttachment.RegionUVs = (float[])RegionUVs.Clone();
			}
			if (UVs != null)
			{
				meshAttachment.UVs = (float[])UVs.Clone();
			}
			if (Triangles != null)
			{
				meshAttachment.Triangles = (int[])Triangles.Clone();
			}
			meshAttachment.r = r;
			meshAttachment.g = g;
			meshAttachment.b = b;
			meshAttachment.a = a;
			meshAttachment.UpdateUVs();
			return meshAttachment;
		}
	}
}
