namespace Spine
{
	public class BoundingBoxAttachment : Attachment
	{
		internal float[] vertices;

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

		public BoundingBoxAttachment(string name)
			: base(name)
		{
		}

		public void ComputeWorldVertices(Bone bone, float[] worldVertices)
		{
			float num = bone.skeleton.x + bone.worldX;
			float num2 = bone.skeleton.y + bone.worldY;
			float m = bone.m00;
			float m2 = bone.m01;
			float m3 = bone.m10;
			float m4 = bone.m11;
			float[] array = vertices;
			int i = 0;
			for (int num3 = array.Length; i < num3; i += 2)
			{
				float num4 = array[i];
				float num5 = array[i + 1];
				worldVertices[i] = num4 * m + num5 * m2 + num;
				worldVertices[i + 1] = num4 * m3 + num5 * m4 + num2;
			}
		}

		public override Attachment CopyAttachment()
		{
			BoundingBoxAttachment boundingBoxAttachment = new BoundingBoxAttachment(Name);
			if (Vertices != null)
			{
				boundingBoxAttachment.Vertices = (float[])Vertices.Clone();
			}
			return boundingBoxAttachment;
		}
	}
}
