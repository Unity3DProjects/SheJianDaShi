using System;

namespace Spine
{
	public class IkConstraint
	{
		private const float radDeg = 180f / (float)Math.PI;

		internal IkConstraintData data;

		internal ExposedList<Bone> bones = new ExposedList<Bone>();

		internal Bone target;

		internal int bendDirection;

		internal float mix;

		public IkConstraintData Data
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

		public Bone Target
		{
			get
			{
				return target;
			}
			set
			{
				target = value;
			}
		}

		public int BendDirection
		{
			get
			{
				return bendDirection;
			}
			set
			{
				bendDirection = value;
			}
		}

		public float Mix
		{
			get
			{
				return mix;
			}
			set
			{
				mix = value;
			}
		}

		public IkConstraint(IkConstraintData data, Skeleton skeleton)
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
			mix = data.mix;
			bendDirection = data.bendDirection;
			bones = new ExposedList<Bone>(data.bones.Count);
			foreach (BoneData bone in data.bones)
			{
				bones.Add(skeleton.FindBone(bone.name));
			}
			target = skeleton.FindBone(data.target.name);
		}

		public void apply()
		{
			Bone bone = target;
			ExposedList<Bone> exposedList = bones;
			switch (exposedList.Count)
			{
			case 1:
				apply(exposedList.Items[0], bone.worldX, bone.worldY, mix);
				break;
			case 2:
				apply(exposedList.Items[0], exposedList.Items[1], bone.worldX, bone.worldY, bendDirection, mix);
				break;
			}
		}

		public override string ToString()
		{
			return data.name;
		}

		public static void apply(Bone bone, float targetX, float targetY, float alpha)
		{
			float num = ((bone.data.inheritRotation && bone.parent != null) ? bone.parent.worldRotation : 0f);
			float rotation = bone.rotation;
			float num2 = (float)Math.Atan2(targetY - bone.worldY, targetX - bone.worldX) * (180f / (float)Math.PI);
			if (bone.worldFlipX != (bone.worldFlipY != Bone.yDown))
			{
				num2 = 0f - num2;
			}
			num2 -= num;
			bone.rotationIK = rotation + (num2 - rotation) * alpha;
		}

		public static void apply(Bone parent, Bone child, float targetX, float targetY, int bendDirection, float alpha)
		{
			float rotation = child.rotation;
			float rotation2 = parent.rotation;
			if (alpha == 0f)
			{
				child.rotationIK = rotation;
				parent.rotationIK = rotation2;
				return;
			}
			Bone parent2 = parent.parent;
			float localX;
			float localY;
			if (parent2 != null)
			{
				parent2.worldToLocal(targetX, targetY, out localX, out localY);
				targetX = (localX - parent.x) * parent2.worldScaleX;
				targetY = (localY - parent.y) * parent2.worldScaleY;
			}
			else
			{
				targetX -= parent.x;
				targetY -= parent.y;
			}
			if (child.parent == parent)
			{
				localX = child.x;
				localY = child.y;
			}
			else
			{
				child.parent.localToWorld(child.x, child.y, out localX, out localY);
				parent.worldToLocal(localX, localY, out localX, out localY);
			}
			float num = localX * parent.worldScaleX;
			float num2 = localY * parent.worldScaleY;
			float num3 = (float)Math.Atan2(num2, num);
			float num4 = (float)Math.Sqrt(num * num + num2 * num2);
			float num5 = child.data.length * child.worldScaleX;
			float num6 = 2f * num4 * num5;
			if (num6 < 0.0001f)
			{
				child.rotationIK = rotation + ((float)Math.Atan2(targetY, targetX) * (180f / (float)Math.PI) - rotation2 - rotation) * alpha;
				return;
			}
			float num7 = (targetX * targetX + targetY * targetY - num4 * num4 - num5 * num5) / num6;
			if (num7 < -1f)
			{
				num7 = -1f;
			}
			else if (num7 > 1f)
			{
				num7 = 1f;
			}
			float num8 = (float)Math.Acos(num7) * (float)bendDirection;
			float num9 = num4 + num5 * num7;
			float num10 = num5 * (float)Math.Sin(num8);
			float num11 = (float)Math.Atan2(targetY * num9 - targetX * num10, targetX * num9 + targetY * num10);
			float num12 = (num11 - num3) * (180f / (float)Math.PI) - rotation2;
			if (num12 > 180f)
			{
				num12 -= 360f;
			}
			else if (num12 < -180f)
			{
				num12 += 360f;
			}
			parent.rotationIK = rotation2 + num12 * alpha;
			num12 = (num8 + num3) * (180f / (float)Math.PI) - rotation;
			if (num12 > 180f)
			{
				num12 -= 360f;
			}
			else if (num12 < -180f)
			{
				num12 += 360f;
			}
			child.rotationIK = rotation + (num12 + parent.worldRotation - child.parent.worldRotation) * alpha;
		}
	}
}
