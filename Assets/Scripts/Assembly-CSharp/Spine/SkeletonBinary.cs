using System;
using System.Collections.Generic;
using System.IO;

namespace Spine
{
	public class SkeletonBinary
	{
		public const int TIMELINE_SCALE = 0;

		public const int TIMELINE_ROTATE = 1;

		public const int TIMELINE_TRANSLATE = 2;

		public const int TIMELINE_ATTACHMENT = 3;

		public const int TIMELINE_COLOR = 4;

		public const int TIMELINE_FLIPX = 5;

		public const int TIMELINE_FLIPY = 6;

		public const int CURVE_LINEAR = 0;

		public const int CURVE_STEPPED = 1;

		public const int CURVE_BEZIER = 2;

		private AttachmentLoader attachmentLoader;

		private char[] chars = new char[32];

		private byte[] buffer = new byte[4];

		public float Scale { get; set; }

		public SkeletonBinary(params Atlas[] atlasArray)
			: this(new AtlasAttachmentLoader(atlasArray))
		{
		}

		public SkeletonBinary(AttachmentLoader attachmentLoader)
		{
			if (attachmentLoader == null)
			{
				throw new ArgumentNullException("attachmentLoader cannot be null.");
			}
			this.attachmentLoader = attachmentLoader;
			Scale = 1f;
		}

		public SkeletonData ReadSkeletonData(string path)
		{
			using (BufferedStream input = new BufferedStream(new FileStream(path, FileMode.Open)))
			{
				SkeletonData skeletonData = ReadSkeletonData(input);
				skeletonData.name = Path.GetFileNameWithoutExtension(path);
				return skeletonData;
			}
		}

		public SkeletonData ReadSkeletonData(Stream input)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input cannot be null.");
			}
			float scale = Scale;
			SkeletonData skeletonData = new SkeletonData();
			skeletonData.hash = ReadString(input);
			if (skeletonData.hash.Length == 0)
			{
				skeletonData.hash = null;
			}
			skeletonData.version = ReadString(input);
			if (skeletonData.version.Length == 0)
			{
				skeletonData.version = null;
			}
			skeletonData.width = ReadFloat(input);
			skeletonData.height = ReadFloat(input);
			bool flag = ReadBoolean(input);
			if (flag)
			{
				skeletonData.imagesPath = ReadString(input);
				if (skeletonData.imagesPath.Length == 0)
				{
					skeletonData.imagesPath = null;
				}
			}
			int i = 0;
			for (int num = ReadInt(input, true); i < num; i++)
			{
				string name = ReadString(input);
				BoneData parent = null;
				int num2 = ReadInt(input, true) - 1;
				if (num2 != -1)
				{
					parent = skeletonData.bones.Items[num2];
				}
				BoneData boneData = new BoneData(name, parent);
				boneData.x = ReadFloat(input) * scale;
				boneData.y = ReadFloat(input) * scale;
				boneData.scaleX = ReadFloat(input);
				boneData.scaleY = ReadFloat(input);
				boneData.rotation = ReadFloat(input);
				boneData.length = ReadFloat(input) * scale;
				boneData.flipX = ReadBoolean(input);
				boneData.flipY = ReadBoolean(input);
				boneData.inheritScale = ReadBoolean(input);
				boneData.inheritRotation = ReadBoolean(input);
				if (flag)
				{
					ReadInt(input);
				}
				skeletonData.bones.Add(boneData);
			}
			int j = 0;
			for (int num3 = ReadInt(input, true); j < num3; j++)
			{
				IkConstraintData ikConstraintData = new IkConstraintData(ReadString(input));
				int k = 0;
				for (int num4 = ReadInt(input, true); k < num4; k++)
				{
					ikConstraintData.bones.Add(skeletonData.bones.Items[ReadInt(input, true)]);
				}
				ikConstraintData.target = skeletonData.bones.Items[ReadInt(input, true)];
				ikConstraintData.mix = ReadFloat(input);
				ikConstraintData.bendDirection = ReadSByte(input);
				skeletonData.ikConstraints.Add(ikConstraintData);
			}
			int l = 0;
			for (int num5 = ReadInt(input, true); l < num5; l++)
			{
				string name2 = ReadString(input);
				BoneData boneData2 = skeletonData.bones.Items[ReadInt(input, true)];
				SlotData slotData = new SlotData(name2, boneData2);
				int num6 = ReadInt(input);
				slotData.r = (float)((num6 & 0xFF000000u) >> 24) / 255f;
				slotData.g = (float)((num6 & 0xFF0000) >> 16) / 255f;
				slotData.b = (float)((num6 & 0xFF00) >> 8) / 255f;
				slotData.a = (float)(num6 & 0xFF) / 255f;
				slotData.attachmentName = ReadString(input);
				slotData.blendMode = (BlendMode)ReadInt(input, true);
				skeletonData.slots.Add(slotData);
			}
			Skin skin = ReadSkin(input, "default", flag);
			if (skin != null)
			{
				skeletonData.defaultSkin = skin;
				skeletonData.skins.Add(skin);
			}
			int m = 0;
			for (int num7 = ReadInt(input, true); m < num7; m++)
			{
				skeletonData.skins.Add(ReadSkin(input, ReadString(input), flag));
			}
			int n = 0;
			for (int num8 = ReadInt(input, true); n < num8; n++)
			{
				EventData eventData = new EventData(ReadString(input));
				eventData.Int = ReadInt(input, false);
				eventData.Float = ReadFloat(input);
				eventData.String = ReadString(input);
				skeletonData.events.Add(eventData);
			}
			int num9 = 0;
			for (int num10 = ReadInt(input, true); num9 < num10; num9++)
			{
				ReadAnimation(ReadString(input), input, skeletonData);
			}
			skeletonData.bones.TrimExcess();
			skeletonData.slots.TrimExcess();
			skeletonData.skins.TrimExcess();
			skeletonData.events.TrimExcess();
			skeletonData.animations.TrimExcess();
			skeletonData.ikConstraints.TrimExcess();
			return skeletonData;
		}

		private Skin ReadSkin(Stream input, string skinName, bool nonessential)
		{
			int num = ReadInt(input, true);
			if (num == 0)
			{
				return null;
			}
			Skin skin = new Skin(skinName);
			for (int i = 0; i < num; i++)
			{
				int slotIndex = ReadInt(input, true);
				int j = 0;
				for (int num2 = ReadInt(input, true); j < num2; j++)
				{
					string text = ReadString(input);
					skin.AddAttachment(slotIndex, text, ReadAttachment(input, skin, text, nonessential));
				}
			}
			return skin;
		}

		private Attachment ReadAttachment(Stream input, Skin skin, string attachmentName, bool nonessential)
		{
			float scale = Scale;
			string text = ReadString(input);
			if (text == null)
			{
				text = attachmentName;
			}
			switch ((AttachmentType)input.ReadByte())
			{
			case AttachmentType.region:
			{
				string text3 = ReadString(input);
				if (text3 == null)
				{
					text3 = text;
				}
				RegionAttachment regionAttachment = attachmentLoader.NewRegionAttachment(skin, text, text3);
				if (regionAttachment == null)
				{
					return null;
				}
				regionAttachment.Path = text3;
				regionAttachment.x = ReadFloat(input) * scale;
				regionAttachment.y = ReadFloat(input) * scale;
				regionAttachment.scaleX = ReadFloat(input);
				regionAttachment.scaleY = ReadFloat(input);
				regionAttachment.rotation = ReadFloat(input);
				regionAttachment.width = ReadFloat(input) * scale;
				regionAttachment.height = ReadFloat(input) * scale;
				int num5 = ReadInt(input);
				regionAttachment.r = (float)((num5 & 0xFF000000u) >> 24) / 255f;
				regionAttachment.g = (float)((num5 & 0xFF0000) >> 16) / 255f;
				regionAttachment.b = (float)((num5 & 0xFF00) >> 8) / 255f;
				regionAttachment.a = (float)(num5 & 0xFF) / 255f;
				regionAttachment.UpdateOffset();
				return regionAttachment;
			}
			case AttachmentType.boundingbox:
			{
				BoundingBoxAttachment boundingBoxAttachment = attachmentLoader.NewBoundingBoxAttachment(skin, text);
				if (boundingBoxAttachment == null)
				{
					return null;
				}
				boundingBoxAttachment.vertices = ReadFloatArray(input, scale);
				return boundingBoxAttachment;
			}
			case AttachmentType.mesh:
			{
				string text4 = ReadString(input);
				if (text4 == null)
				{
					text4 = text;
				}
				MeshAttachment meshAttachment = attachmentLoader.NewMeshAttachment(skin, text, text4);
				if (meshAttachment == null)
				{
					return null;
				}
				meshAttachment.Path = text4;
				meshAttachment.regionUVs = ReadFloatArray(input, 1f);
				meshAttachment.triangles = ReadShortArray(input);
				meshAttachment.vertices = ReadFloatArray(input, scale);
				meshAttachment.UpdateUVs();
				int num6 = ReadInt(input);
				meshAttachment.r = (float)((num6 & 0xFF000000u) >> 24) / 255f;
				meshAttachment.g = (float)((num6 & 0xFF0000) >> 16) / 255f;
				meshAttachment.b = (float)((num6 & 0xFF00) >> 8) / 255f;
				meshAttachment.a = (float)(num6 & 0xFF) / 255f;
				meshAttachment.HullLength = ReadInt(input, true) * 2;
				if (nonessential)
				{
					meshAttachment.Edges = ReadIntArray(input);
					meshAttachment.Width = ReadFloat(input) * scale;
					meshAttachment.Height = ReadFloat(input) * scale;
				}
				return meshAttachment;
			}
			case AttachmentType.skinnedmesh:
			{
				string text2 = ReadString(input);
				if (text2 == null)
				{
					text2 = text;
				}
				SkinnedMeshAttachment skinnedMeshAttachment = attachmentLoader.NewSkinnedMeshAttachment(skin, text, text2);
				if (skinnedMeshAttachment == null)
				{
					return null;
				}
				skinnedMeshAttachment.Path = text2;
				float[] array = ReadFloatArray(input, 1f);
				int[] triangles = ReadShortArray(input);
				int num = ReadInt(input, true);
				List<float> list = new List<float>(array.Length * 3 * 3);
				List<int> list2 = new List<int>(array.Length * 3);
				for (int i = 0; i < num; i++)
				{
					int num2 = (int)ReadFloat(input);
					list2.Add(num2);
					for (int num3 = i + num2 * 4; i < num3; i += 4)
					{
						list2.Add((int)ReadFloat(input));
						list.Add(ReadFloat(input) * scale);
						list.Add(ReadFloat(input) * scale);
						list.Add(ReadFloat(input));
					}
				}
				skinnedMeshAttachment.bones = list2.ToArray();
				skinnedMeshAttachment.weights = list.ToArray();
				skinnedMeshAttachment.triangles = triangles;
				skinnedMeshAttachment.regionUVs = array;
				skinnedMeshAttachment.UpdateUVs();
				int num4 = ReadInt(input);
				skinnedMeshAttachment.r = (float)((num4 & 0xFF000000u) >> 24) / 255f;
				skinnedMeshAttachment.g = (float)((num4 & 0xFF0000) >> 16) / 255f;
				skinnedMeshAttachment.b = (float)((num4 & 0xFF00) >> 8) / 255f;
				skinnedMeshAttachment.a = (float)(num4 & 0xFF) / 255f;
				skinnedMeshAttachment.HullLength = ReadInt(input, true) * 2;
				if (nonessential)
				{
					skinnedMeshAttachment.Edges = ReadIntArray(input);
					skinnedMeshAttachment.Width = ReadFloat(input) * scale;
					skinnedMeshAttachment.Height = ReadFloat(input) * scale;
				}
				return skinnedMeshAttachment;
			}
			default:
				return null;
			}
		}

		private float[] ReadFloatArray(Stream input, float scale)
		{
			int num = ReadInt(input, true);
			float[] array = new float[num];
			if (scale == 1f)
			{
				for (int i = 0; i < num; i++)
				{
					array[i] = ReadFloat(input);
				}
			}
			else
			{
				for (int j = 0; j < num; j++)
				{
					array[j] = ReadFloat(input) * scale;
				}
			}
			return array;
		}

		private int[] ReadShortArray(Stream input)
		{
			int num = ReadInt(input, true);
			int[] array = new int[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = (input.ReadByte() << 8) + input.ReadByte();
			}
			return array;
		}

		private int[] ReadIntArray(Stream input)
		{
			int num = ReadInt(input, true);
			int[] array = new int[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = ReadInt(input, true);
			}
			return array;
		}

		private void ReadAnimation(string name, Stream input, SkeletonData skeletonData)
		{
			ExposedList<Timeline> exposedList = new ExposedList<Timeline>();
			float scale = Scale;
			float num = 0f;
			int i = 0;
			for (int num2 = ReadInt(input, true); i < num2; i++)
			{
				int slotIndex = ReadInt(input, true);
				int j = 0;
				for (int num3 = ReadInt(input, true); j < num3; j++)
				{
					int num4 = input.ReadByte();
					int num5 = ReadInt(input, true);
					switch (num4)
					{
					case 4:
					{
						ColorTimeline colorTimeline = new ColorTimeline(num5);
						colorTimeline.slotIndex = slotIndex;
						for (int l = 0; l < num5; l++)
						{
							float time = ReadFloat(input);
							int num6 = ReadInt(input);
							float r = (float)((num6 & 0xFF000000u) >> 24) / 255f;
							float g = (float)((num6 & 0xFF0000) >> 16) / 255f;
							float b = (float)((num6 & 0xFF00) >> 8) / 255f;
							float a = (float)(num6 & 0xFF) / 255f;
							colorTimeline.SetFrame(l, time, r, g, b, a);
							if (l < num5 - 1)
							{
								ReadCurve(input, l, colorTimeline);
							}
						}
						exposedList.Add(colorTimeline);
						num = Math.Max(num, colorTimeline.frames[num5 * 5 - 5]);
						break;
					}
					case 3:
					{
						AttachmentTimeline attachmentTimeline = new AttachmentTimeline(num5);
						attachmentTimeline.slotIndex = slotIndex;
						for (int k = 0; k < num5; k++)
						{
							attachmentTimeline.SetFrame(k, ReadFloat(input), ReadString(input));
						}
						exposedList.Add(attachmentTimeline);
						num = Math.Max(num, attachmentTimeline.frames[num5 - 1]);
						break;
					}
					}
				}
			}
			int m = 0;
			for (int num7 = ReadInt(input, true); m < num7; m++)
			{
				int boneIndex = ReadInt(input, true);
				int n = 0;
				for (int num8 = ReadInt(input, true); n < num8; n++)
				{
					int num9 = input.ReadByte();
					int num10 = ReadInt(input, true);
					object obj;
					switch (num9)
					{
					case 1:
					{
						RotateTimeline rotateTimeline = new RotateTimeline(num10);
						rotateTimeline.boneIndex = boneIndex;
						for (int num13 = 0; num13 < num10; num13++)
						{
							rotateTimeline.SetFrame(num13, ReadFloat(input), ReadFloat(input));
							if (num13 < num10 - 1)
							{
								ReadCurve(input, num13, rotateTimeline);
							}
						}
						exposedList.Add(rotateTimeline);
						num = Math.Max(num, rotateTimeline.frames[num10 * 2 - 2]);
						continue;
					}
					case 0:
					case 2:
					{
						float num11 = 1f;
						TranslateTimeline translateTimeline;
						if (num9 == 0)
						{
							translateTimeline = new ScaleTimeline(num10);
						}
						else
						{
							translateTimeline = new TranslateTimeline(num10);
							num11 = scale;
						}
						translateTimeline.boneIndex = boneIndex;
						for (int num12 = 0; num12 < num10; num12++)
						{
							translateTimeline.SetFrame(num12, ReadFloat(input), ReadFloat(input) * num11, ReadFloat(input) * num11);
							if (num12 < num10 - 1)
							{
								ReadCurve(input, num12, translateTimeline);
							}
						}
						exposedList.Add(translateTimeline);
						num = Math.Max(num, translateTimeline.frames[num10 * 3 - 3]);
						continue;
					}
					case 5:
						obj = new FlipXTimeline(num10);
						break;
					case 6:
						obj = new FlipYTimeline(num10);
						break;
					default:
						continue;
					}
					FlipXTimeline flipXTimeline = (FlipXTimeline)obj;
					flipXTimeline.boneIndex = boneIndex;
					for (int num14 = 0; num14 < num10; num14++)
					{
						flipXTimeline.SetFrame(num14, ReadFloat(input), ReadBoolean(input));
					}
					exposedList.Add(flipXTimeline);
					num = Math.Max(num, flipXTimeline.frames[num10 * 2 - 2]);
				}
			}
			int num15 = 0;
			for (int num16 = ReadInt(input, true); num15 < num16; num15++)
			{
				IkConstraintData item = skeletonData.ikConstraints.Items[ReadInt(input, true)];
				int num17 = ReadInt(input, true);
				IkConstraintTimeline ikConstraintTimeline = new IkConstraintTimeline(num17);
				ikConstraintTimeline.ikConstraintIndex = skeletonData.ikConstraints.IndexOf(item);
				for (int num18 = 0; num18 < num17; num18++)
				{
					ikConstraintTimeline.SetFrame(num18, ReadFloat(input), ReadFloat(input), ReadSByte(input));
					if (num18 < num17 - 1)
					{
						ReadCurve(input, num18, ikConstraintTimeline);
					}
				}
				exposedList.Add(ikConstraintTimeline);
				num = Math.Max(num, ikConstraintTimeline.frames[num17 * 3 - 3]);
			}
			int num19 = 0;
			for (int num20 = ReadInt(input, true); num19 < num20; num19++)
			{
				Skin skin = skeletonData.skins.Items[ReadInt(input, true)];
				int num21 = 0;
				for (int num22 = ReadInt(input, true); num21 < num22; num21++)
				{
					int slotIndex2 = ReadInt(input, true);
					int num23 = 0;
					for (int num24 = ReadInt(input, true); num23 < num24; num23++)
					{
						Attachment attachment = skin.GetAttachment(slotIndex2, ReadString(input));
						int num25 = ReadInt(input, true);
						FFDTimeline fFDTimeline = new FFDTimeline(num25);
						fFDTimeline.slotIndex = slotIndex2;
						fFDTimeline.attachment = attachment;
						for (int num26 = 0; num26 < num25; num26++)
						{
							float time2 = ReadFloat(input);
							int num27 = ((!(attachment is MeshAttachment)) ? (((SkinnedMeshAttachment)attachment).weights.Length / 3 * 2) : ((MeshAttachment)attachment).vertices.Length);
							int num28 = ReadInt(input, true);
							float[] array;
							if (num28 == 0)
							{
								array = ((!(attachment is MeshAttachment)) ? new float[num27] : ((MeshAttachment)attachment).vertices);
							}
							else
							{
								array = new float[num27];
								int num29 = ReadInt(input, true);
								num28 += num29;
								if (scale == 1f)
								{
									for (int num30 = num29; num30 < num28; num30++)
									{
										array[num30] = ReadFloat(input);
									}
								}
								else
								{
									for (int num31 = num29; num31 < num28; num31++)
									{
										array[num31] = ReadFloat(input) * scale;
									}
								}
								if (attachment is MeshAttachment)
								{
									float[] vertices = ((MeshAttachment)attachment).vertices;
									int num32 = 0;
									for (int num33 = array.Length; num32 < num33; num32++)
									{
										array[num32] += vertices[num32];
									}
								}
							}
							fFDTimeline.SetFrame(num26, time2, array);
							if (num26 < num25 - 1)
							{
								ReadCurve(input, num26, fFDTimeline);
							}
						}
						exposedList.Add(fFDTimeline);
						num = Math.Max(num, fFDTimeline.frames[num25 - 1]);
					}
				}
			}
			int num34 = ReadInt(input, true);
			if (num34 > 0)
			{
				DrawOrderTimeline drawOrderTimeline = new DrawOrderTimeline(num34);
				int count = skeletonData.slots.Count;
				for (int num35 = 0; num35 < num34; num35++)
				{
					int num36 = ReadInt(input, true);
					int[] array2 = new int[count];
					for (int num37 = count - 1; num37 >= 0; num37--)
					{
						array2[num37] = -1;
					}
					int[] array3 = new int[count - num36];
					int num38 = 0;
					int num39 = 0;
					for (int num40 = 0; num40 < num36; num40++)
					{
						int num41 = ReadInt(input, true);
						while (num38 != num41)
						{
							array3[num39++] = num38++;
						}
						array2[num38 + ReadInt(input, true)] = num38++;
					}
					while (num38 < count)
					{
						array3[num39++] = num38++;
					}
					for (int num42 = count - 1; num42 >= 0; num42--)
					{
						if (array2[num42] == -1)
						{
							array2[num42] = array3[--num39];
						}
					}
					drawOrderTimeline.SetFrame(num35, ReadFloat(input), array2);
				}
				exposedList.Add(drawOrderTimeline);
				num = Math.Max(num, drawOrderTimeline.frames[num34 - 1]);
			}
			int num43 = ReadInt(input, true);
			if (num43 > 0)
			{
				EventTimeline eventTimeline = new EventTimeline(num43);
				for (int num44 = 0; num44 < num43; num44++)
				{
					float time3 = ReadFloat(input);
					EventData eventData = skeletonData.events.Items[ReadInt(input, true)];
					Event obj2 = new Event(eventData);
					obj2.Int = ReadInt(input, false);
					obj2.Float = ReadFloat(input);
					obj2.String = ((!ReadBoolean(input)) ? eventData.String : ReadString(input));
					eventTimeline.SetFrame(num44, time3, obj2);
				}
				exposedList.Add(eventTimeline);
				num = Math.Max(num, eventTimeline.frames[num43 - 1]);
			}
			exposedList.TrimExcess();
			skeletonData.animations.Add(new Animation(name, exposedList, num));
		}

		private void ReadCurve(Stream input, int frameIndex, CurveTimeline timeline)
		{
			switch (input.ReadByte())
			{
			case 1:
				timeline.SetStepped(frameIndex);
				break;
			case 2:
				timeline.SetCurve(frameIndex, ReadFloat(input), ReadFloat(input), ReadFloat(input), ReadFloat(input));
				break;
			}
		}

		private sbyte ReadSByte(Stream input)
		{
			int num = input.ReadByte();
			if (num == -1)
			{
				throw new EndOfStreamException();
			}
			return (sbyte)num;
		}

		private bool ReadBoolean(Stream input)
		{
			return input.ReadByte() != 0;
		}

		private float ReadFloat(Stream input)
		{
			buffer[3] = (byte)input.ReadByte();
			buffer[2] = (byte)input.ReadByte();
			buffer[1] = (byte)input.ReadByte();
			buffer[0] = (byte)input.ReadByte();
			return BitConverter.ToSingle(buffer, 0);
		}

		private int ReadInt(Stream input)
		{
			return (input.ReadByte() << 24) + (input.ReadByte() << 16) + (input.ReadByte() << 8) + input.ReadByte();
		}

		private int ReadInt(Stream input, bool optimizePositive)
		{
			int num = input.ReadByte();
			int num2 = num & 0x7F;
			if ((num & 0x80) != 0)
			{
				num = input.ReadByte();
				num2 |= (num & 0x7F) << 7;
				if ((num & 0x80) != 0)
				{
					num = input.ReadByte();
					num2 |= (num & 0x7F) << 14;
					if ((num & 0x80) != 0)
					{
						num = input.ReadByte();
						num2 |= (num & 0x7F) << 21;
						if ((num & 0x80) != 0)
						{
							num = input.ReadByte();
							num2 |= (num & 0x7F) << 28;
						}
					}
				}
			}
			return (!optimizePositive) ? ((num2 >> 1) ^ -(num2 & 1)) : num2;
		}

		private string ReadString(Stream input)
		{
			int num = ReadInt(input, true);
			switch (num)
			{
			case 0:
				return null;
			case 1:
				return string.Empty;
			default:
			{
				num--;
				char[] array = chars;
				if (array.Length < num)
				{
					array = (chars = new char[num]);
				}
				int num2 = 0;
				int num3 = 0;
				while (num2 < num)
				{
					num3 = input.ReadByte();
					if (num3 > 127)
					{
						break;
					}
					array[num2++] = (char)num3;
				}
				if (num2 < num)
				{
					ReadUtf8_slow(input, num, num2, num3);
				}
				return new string(array, 0, num);
			}
			}
		}

		private void ReadUtf8_slow(Stream input, int charCount, int charIndex, int b)
		{
			char[] array = chars;
			while (true)
			{
				switch (b >> 4)
				{
				case 0:
				case 1:
				case 2:
				case 3:
				case 4:
				case 5:
				case 6:
				case 7:
					array[charIndex] = (char)b;
					break;
				case 12:
				case 13:
					array[charIndex] = (char)(((b & 0x1F) << 6) | (input.ReadByte() & 0x3F));
					break;
				case 14:
					array[charIndex] = (char)(((b & 0xF) << 12) | ((input.ReadByte() & 0x3F) << 6) | (input.ReadByte() & 0x3F));
					break;
				}
				if (++charIndex >= charCount)
				{
					break;
				}
				b = input.ReadByte() & 0xFF;
			}
		}
	}
}
