using System;

namespace Spine
{
	public class Bone
	{
		public static bool yDown;

		internal BoneData data;

		internal Skeleton skeleton;

		internal Bone parent;

		internal ExposedList<Bone> children = new ExposedList<Bone>();

		internal float x;

		internal float y;

		internal float rotation;

		internal float rotationIK;

		internal float scaleX;

		internal float scaleY;

		internal bool flipX;

		internal bool flipY;

		internal float m00;

		internal float m01;

		internal float m10;

		internal float m11;

		internal float worldX;

		internal float worldY;

		internal float worldRotation;

		internal float worldScaleX;

		internal float worldScaleY;

		internal bool worldFlipX;

		internal bool worldFlipY;

		public BoneData Data
		{
			get
			{
				return data;
			}
		}

		public Skeleton Skeleton
		{
			get
			{
				return skeleton;
			}
		}

		public Bone Parent
		{
			get
			{
				return parent;
			}
		}

		public ExposedList<Bone> Children
		{
			get
			{
				return children;
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

		public float Rotation
		{
			get
			{
				return rotation;
			}
			set
			{
				rotation = value;
			}
		}

		public float RotationIK
		{
			get
			{
				return rotationIK;
			}
			set
			{
				rotationIK = value;
			}
		}

		public float ScaleX
		{
			get
			{
				return scaleX;
			}
			set
			{
				scaleX = value;
			}
		}

		public float ScaleY
		{
			get
			{
				return scaleY;
			}
			set
			{
				scaleY = value;
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

		public float M00
		{
			get
			{
				return m00;
			}
		}

		public float M01
		{
			get
			{
				return m01;
			}
		}

		public float M10
		{
			get
			{
				return m10;
			}
		}

		public float M11
		{
			get
			{
				return m11;
			}
		}

		public float WorldX
		{
			get
			{
				return worldX;
			}
		}

		public float WorldY
		{
			get
			{
				return worldY;
			}
		}

		public float WorldRotation
		{
			get
			{
				return worldRotation;
			}
		}

		public float WorldScaleX
		{
			get
			{
				return worldScaleX;
			}
		}

		public float WorldScaleY
		{
			get
			{
				return worldScaleY;
			}
		}

		public bool WorldFlipX
		{
			get
			{
				return worldFlipX;
			}
			set
			{
				worldFlipX = value;
			}
		}

		public bool WorldFlipY
		{
			get
			{
				return worldFlipY;
			}
			set
			{
				worldFlipY = value;
			}
		}

		public Bone(BoneData data, Skeleton skeleton, Bone parent)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data cannot be null.");
			}
			if (skeleton == null)
			{
				throw new ArgumentNullException("skeleton cannot be null.");
			}
			this.data = data;
			this.skeleton = skeleton;
			this.parent = parent;
			SetToSetupPose();
		}

		public void UpdateWorldTransform()
		{
			Bone bone = parent;
			float num = x;
			float num2 = y;
			if (bone != null)
			{
				worldX = num * bone.m00 + num2 * bone.m01 + bone.worldX;
				worldY = num * bone.m10 + num2 * bone.m11 + bone.worldY;
				if (data.inheritScale)
				{
					worldScaleX = bone.worldScaleX * scaleX;
					worldScaleY = bone.worldScaleY * scaleY;
				}
				else
				{
					worldScaleX = scaleX;
					worldScaleY = scaleY;
				}
				worldRotation = ((!data.inheritRotation) ? rotationIK : (bone.worldRotation + rotationIK));
				worldFlipX = bone.worldFlipX != flipX;
				worldFlipY = bone.worldFlipY != flipY;
			}
			else
			{
				Skeleton skeleton = this.skeleton;
				bool flag = skeleton.flipX;
				bool flag2 = skeleton.flipY;
				worldX = ((!flag) ? num : (0f - num));
				worldY = ((flag2 == yDown) ? num2 : (0f - num2));
				worldScaleX = scaleX;
				worldScaleY = scaleY;
				worldRotation = rotationIK;
				worldFlipX = flag != flipX;
				worldFlipY = flag2 != flipY;
			}
			float num3 = worldRotation * (float)Math.PI / 180f;
			float num4 = (float)Math.Cos(num3);
			float num5 = (float)Math.Sin(num3);
			if (worldFlipX)
			{
				m00 = (0f - num4) * worldScaleX;
				m01 = num5 * worldScaleY;
			}
			else
			{
				m00 = num4 * worldScaleX;
				m01 = (0f - num5) * worldScaleY;
			}
			if (worldFlipY != yDown)
			{
				m10 = (0f - num5) * worldScaleX;
				m11 = (0f - num4) * worldScaleY;
			}
			else
			{
				m10 = num5 * worldScaleX;
				m11 = num4 * worldScaleY;
			}
		}

		public void SetToSetupPose()
		{
			BoneData boneData = data;
			x = boneData.x;
			y = boneData.y;
			rotation = boneData.rotation;
			rotationIK = rotation;
			scaleX = boneData.scaleX;
			scaleY = boneData.scaleY;
			flipX = boneData.flipX;
			flipY = boneData.flipY;
		}

		public void worldToLocal(float worldX, float worldY, out float localX, out float localY)
		{
			float num = worldX - this.worldX;
			float num2 = worldY - this.worldY;
			float num3 = m00;
			float num4 = m10;
			float num5 = m01;
			float num6 = m11;
			if (worldFlipX != (worldFlipY != yDown))
			{
				num3 = 0f - num3;
				num6 = 0f - num6;
			}
			float num7 = 1f / (num3 * num6 - num5 * num4);
			localX = num * num3 * num7 - num2 * num5 * num7;
			localY = num2 * num6 * num7 - num * num4 * num7;
		}

		public void localToWorld(float localX, float localY, out float worldX, out float worldY)
		{
			worldX = localX * m00 + localY * m01 + this.worldX;
			worldY = localX * m10 + localY * m11 + this.worldY;
		}

		public override string ToString()
		{
			return data.name;
		}
	}
}
