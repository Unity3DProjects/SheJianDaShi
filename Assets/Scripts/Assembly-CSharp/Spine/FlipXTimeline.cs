namespace Spine
{
	public class FlipXTimeline : Timeline
	{
		internal int boneIndex;

		internal float[] frames;

		public int BoneIndex
		{
			get
			{
				return boneIndex;
			}
			set
			{
				boneIndex = value;
			}
		}

		public float[] Frames
		{
			get
			{
				return frames;
			}
			set
			{
				frames = value;
			}
		}

		public int FrameCount
		{
			get
			{
				return frames.Length >> 1;
			}
		}

		public FlipXTimeline(int frameCount)
		{
			frames = new float[frameCount << 1];
		}

		public void SetFrame(int frameIndex, float time, bool flip)
		{
			frameIndex *= 2;
			frames[frameIndex] = time;
			frames[frameIndex + 1] = (flip ? 1 : 0);
		}

		public void Apply(Skeleton skeleton, float lastTime, float time, ExposedList<Event> firedEvents, float alpha)
		{
			float[] array = frames;
			if (time < array[0])
			{
				if (lastTime > time)
				{
					Apply(skeleton, lastTime, 2.1474836E+09f, null, 0f);
				}
				return;
			}
			if (lastTime > time)
			{
				lastTime = -1f;
			}
			int num = ((!(time >= array[array.Length - 2])) ? Animation.binarySearch(array, time, 2) : array.Length) - 2;
			if (!(array[num] < lastTime))
			{
				SetFlip(skeleton.bones.Items[boneIndex], array[num + 1] != 0f);
			}
		}

		protected virtual void SetFlip(Bone bone, bool flip)
		{
			bone.flipX = flip;
		}
	}
}
