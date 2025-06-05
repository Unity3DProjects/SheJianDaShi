using System;
using System.Collections.Generic;
using System.IO;

namespace Spine
{
	public class SkeletonJson
	{
		private AttachmentLoader attachmentLoader;

		public float Scale { get; set; }

		public SkeletonJson(params Atlas[] atlasArray)
			: this(new AtlasAttachmentLoader(atlasArray))
		{
		}

		public SkeletonJson(AttachmentLoader attachmentLoader)
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
			using (StreamReader reader = new StreamReader(path))
			{
				SkeletonData skeletonData = ReadSkeletonData(reader);
				skeletonData.name = Path.GetFileNameWithoutExtension(path);
				return skeletonData;
			}
		}

		public SkeletonData ReadSkeletonData(TextReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader cannot be null.");
			}
			SkeletonData skeletonData = new SkeletonData();
			Dictionary<string, object> dictionary = Json.Deserialize(reader) as Dictionary<string, object>;
			if (dictionary == null)
			{
				throw new Exception("Invalid JSON.");
			}
			if (dictionary.ContainsKey("skeleton"))
			{
				Dictionary<string, object> dictionary2 = (Dictionary<string, object>)dictionary["skeleton"];
				skeletonData.hash = (string)dictionary2["hash"];
				skeletonData.version = (string)dictionary2["spine"];
				skeletonData.width = GetFloat(dictionary2, "width", 0f);
				skeletonData.height = GetFloat(dictionary2, "height", 0f);
			}
			foreach (Dictionary<string, object> item in (List<object>)dictionary["bones"])
			{
				BoneData boneData = null;
				if (item.ContainsKey("parent"))
				{
					boneData = skeletonData.FindBone((string)item["parent"]);
					if (boneData == null)
					{
						throw new Exception("Parent bone not found: " + item["parent"]);
					}
				}
				BoneData boneData2 = new BoneData((string)item["name"], boneData);
				boneData2.length = GetFloat(item, "length", 0f) * Scale;
				boneData2.x = GetFloat(item, "x", 0f) * Scale;
				boneData2.y = GetFloat(item, "y", 0f) * Scale;
				boneData2.rotation = GetFloat(item, "rotation", 0f);
				boneData2.scaleX = GetFloat(item, "scaleX", 1f);
				boneData2.scaleY = GetFloat(item, "scaleY", 1f);
				boneData2.flipX = GetBoolean(item, "flipX", false);
				boneData2.flipY = GetBoolean(item, "flipY", false);
				boneData2.inheritScale = GetBoolean(item, "inheritScale", true);
				boneData2.inheritRotation = GetBoolean(item, "inheritRotation", true);
				skeletonData.bones.Add(boneData2);
			}
			if (dictionary.ContainsKey("ik"))
			{
				foreach (Dictionary<string, object> item2 in (List<object>)dictionary["ik"])
				{
					IkConstraintData ikConstraintData = new IkConstraintData((string)item2["name"]);
					foreach (string item3 in (List<object>)item2["bones"])
					{
						BoneData boneData3 = skeletonData.FindBone(item3);
						if (boneData3 == null)
						{
							throw new Exception("IK bone not found: " + item3);
						}
						ikConstraintData.bones.Add(boneData3);
					}
					string text2 = (string)item2["target"];
					ikConstraintData.target = skeletonData.FindBone(text2);
					if (ikConstraintData.target == null)
					{
						throw new Exception("Target bone not found: " + text2);
					}
					ikConstraintData.bendDirection = (GetBoolean(item2, "bendPositive", true) ? 1 : (-1));
					ikConstraintData.mix = GetFloat(item2, "mix", 1f);
					skeletonData.ikConstraints.Add(ikConstraintData);
				}
			}
			if (dictionary.ContainsKey("slots"))
			{
				foreach (Dictionary<string, object> item4 in (List<object>)dictionary["slots"])
				{
					string name = (string)item4["name"];
					string text3 = (string)item4["bone"];
					BoneData boneData4 = skeletonData.FindBone(text3);
					if (boneData4 == null)
					{
						throw new Exception("Slot bone not found: " + text3);
					}
					SlotData slotData = new SlotData(name, boneData4);
					if (item4.ContainsKey("color"))
					{
						string hexString = (string)item4["color"];
						slotData.r = ToColor(hexString, 0);
						slotData.g = ToColor(hexString, 1);
						slotData.b = ToColor(hexString, 2);
						slotData.a = ToColor(hexString, 3);
					}
					if (item4.ContainsKey("attachment"))
					{
						slotData.attachmentName = (string)item4["attachment"];
					}
					if (item4.ContainsKey("blend"))
					{
						slotData.blendMode = (BlendMode)Enum.Parse(typeof(BlendMode), (string)item4["blend"], false);
					}
					else
					{
						slotData.blendMode = BlendMode.normal;
					}
					skeletonData.slots.Add(slotData);
				}
			}
			if (dictionary.ContainsKey("skins"))
			{
				foreach (KeyValuePair<string, object> item5 in (Dictionary<string, object>)dictionary["skins"])
				{
					Skin skin = new Skin(item5.Key);
					foreach (KeyValuePair<string, object> item6 in (Dictionary<string, object>)item5.Value)
					{
						int slotIndex = skeletonData.FindSlotIndex(item6.Key);
						foreach (KeyValuePair<string, object> item7 in (Dictionary<string, object>)item6.Value)
						{
							Attachment attachment = ReadAttachment(skin, item7.Key, (Dictionary<string, object>)item7.Value);
							if (attachment != null)
							{
								skin.AddAttachment(slotIndex, item7.Key, attachment);
							}
						}
					}
					skeletonData.skins.Add(skin);
					if (skin.name == "default")
					{
						skeletonData.defaultSkin = skin;
					}
				}
			}
			if (dictionary.ContainsKey("events"))
			{
				foreach (KeyValuePair<string, object> item8 in (Dictionary<string, object>)dictionary["events"])
				{
					Dictionary<string, object> map = (Dictionary<string, object>)item8.Value;
					EventData eventData = new EventData(item8.Key);
					eventData.Int = GetInt(map, "int", 0);
					eventData.Float = GetFloat(map, "float", 0f);
					eventData.String = GetString(map, "string", null);
					skeletonData.events.Add(eventData);
				}
			}
			if (dictionary.ContainsKey("animations"))
			{
				foreach (KeyValuePair<string, object> item9 in (Dictionary<string, object>)dictionary["animations"])
				{
					ReadAnimation(item9.Key, (Dictionary<string, object>)item9.Value, skeletonData);
				}
			}
			skeletonData.bones.TrimExcess();
			skeletonData.slots.TrimExcess();
			skeletonData.skins.TrimExcess();
			skeletonData.events.TrimExcess();
			skeletonData.animations.TrimExcess();
			skeletonData.ikConstraints.TrimExcess();
			return skeletonData;
		}

		private Attachment ReadAttachment(Skin skin, string name, Dictionary<string, object> map)
		{
			if (map.ContainsKey("name"))
			{
				name = (string)map["name"];
			}
			AttachmentType attachmentType = AttachmentType.region;
			if (map.ContainsKey("type"))
			{
				attachmentType = (AttachmentType)Enum.Parse(typeof(AttachmentType), (string)map["type"], false);
			}
			string path = name;
			if (map.ContainsKey("path"))
			{
				path = (string)map["path"];
			}
			switch (attachmentType)
			{
			case AttachmentType.region:
			{
				RegionAttachment regionAttachment = attachmentLoader.NewRegionAttachment(skin, name, path);
				if (regionAttachment == null)
				{
					return null;
				}
				regionAttachment.Path = path;
				regionAttachment.x = GetFloat(map, "x", 0f) * Scale;
				regionAttachment.y = GetFloat(map, "y", 0f) * Scale;
				regionAttachment.scaleX = GetFloat(map, "scaleX", 1f);
				regionAttachment.scaleY = GetFloat(map, "scaleY", 1f);
				regionAttachment.rotation = GetFloat(map, "rotation", 0f);
				regionAttachment.width = GetFloat(map, "width", 32f) * Scale;
				regionAttachment.height = GetFloat(map, "height", 32f) * Scale;
				regionAttachment.UpdateOffset();
				if (map.ContainsKey("color"))
				{
					string hexString2 = (string)map["color"];
					regionAttachment.r = ToColor(hexString2, 0);
					regionAttachment.g = ToColor(hexString2, 1);
					regionAttachment.b = ToColor(hexString2, 2);
					regionAttachment.a = ToColor(hexString2, 3);
				}
				return regionAttachment;
			}
			case AttachmentType.mesh:
			{
				MeshAttachment meshAttachment = attachmentLoader.NewMeshAttachment(skin, name, path);
				if (meshAttachment == null)
				{
					return null;
				}
				meshAttachment.Path = path;
				meshAttachment.vertices = GetFloatArray(map, "vertices", Scale);
				meshAttachment.triangles = GetIntArray(map, "triangles");
				meshAttachment.regionUVs = GetFloatArray(map, "uvs", 1f);
				meshAttachment.UpdateUVs();
				if (map.ContainsKey("color"))
				{
					string hexString = (string)map["color"];
					meshAttachment.r = ToColor(hexString, 0);
					meshAttachment.g = ToColor(hexString, 1);
					meshAttachment.b = ToColor(hexString, 2);
					meshAttachment.a = ToColor(hexString, 3);
				}
				meshAttachment.HullLength = GetInt(map, "hull", 0) * 2;
				if (map.ContainsKey("edges"))
				{
					meshAttachment.Edges = GetIntArray(map, "edges");
				}
				meshAttachment.Width = (float)GetInt(map, "width", 0) * Scale;
				meshAttachment.Height = (float)GetInt(map, "height", 0) * Scale;
				return meshAttachment;
			}
			case AttachmentType.skinnedmesh:
			{
				SkinnedMeshAttachment skinnedMeshAttachment = attachmentLoader.NewSkinnedMeshAttachment(skin, name, path);
				if (skinnedMeshAttachment == null)
				{
					return null;
				}
				skinnedMeshAttachment.Path = path;
				float[] floatArray = GetFloatArray(map, "uvs", 1f);
				float[] floatArray2 = GetFloatArray(map, "vertices", 1f);
				List<float> list = new List<float>(floatArray.Length * 3 * 3);
				List<int> list2 = new List<int>(floatArray.Length * 3);
				float scale = Scale;
				int i = 0;
				int num = floatArray2.Length;
				while (i < num)
				{
					int num2 = (int)floatArray2[i++];
					list2.Add(num2);
					for (int num3 = i + num2 * 4; i < num3; i += 4)
					{
						list2.Add((int)floatArray2[i]);
						list.Add(floatArray2[i + 1] * scale);
						list.Add(floatArray2[i + 2] * scale);
						list.Add(floatArray2[i + 3]);
					}
				}
				skinnedMeshAttachment.bones = list2.ToArray();
				skinnedMeshAttachment.weights = list.ToArray();
				skinnedMeshAttachment.triangles = GetIntArray(map, "triangles");
				skinnedMeshAttachment.regionUVs = floatArray;
				skinnedMeshAttachment.UpdateUVs();
				if (map.ContainsKey("color"))
				{
					string hexString3 = (string)map["color"];
					skinnedMeshAttachment.r = ToColor(hexString3, 0);
					skinnedMeshAttachment.g = ToColor(hexString3, 1);
					skinnedMeshAttachment.b = ToColor(hexString3, 2);
					skinnedMeshAttachment.a = ToColor(hexString3, 3);
				}
				skinnedMeshAttachment.HullLength = GetInt(map, "hull", 0) * 2;
				if (map.ContainsKey("edges"))
				{
					skinnedMeshAttachment.Edges = GetIntArray(map, "edges");
				}
				skinnedMeshAttachment.Width = (float)GetInt(map, "width", 0) * Scale;
				skinnedMeshAttachment.Height = (float)GetInt(map, "height", 0) * Scale;
				return skinnedMeshAttachment;
			}
			case AttachmentType.boundingbox:
			{
				BoundingBoxAttachment boundingBoxAttachment = attachmentLoader.NewBoundingBoxAttachment(skin, name);
				if (boundingBoxAttachment == null)
				{
					return null;
				}
				boundingBoxAttachment.vertices = GetFloatArray(map, "vertices", Scale);
				return boundingBoxAttachment;
			}
			default:
				return null;
			}
		}

		private float[] GetFloatArray(Dictionary<string, object> map, string name, float scale)
		{
			List<object> list = (List<object>)map[name];
			float[] array = new float[list.Count];
			if (scale == 1f)
			{
				int i = 0;
				for (int count = list.Count; i < count; i++)
				{
					array[i] = (float)list[i];
				}
			}
			else
			{
				int j = 0;
				for (int count2 = list.Count; j < count2; j++)
				{
					array[j] = (float)list[j] * scale;
				}
			}
			return array;
		}

		private int[] GetIntArray(Dictionary<string, object> map, string name)
		{
			List<object> list = (List<object>)map[name];
			int[] array = new int[list.Count];
			int i = 0;
			for (int count = list.Count; i < count; i++)
			{
				array[i] = (int)(float)list[i];
			}
			return array;
		}

		private float GetFloat(Dictionary<string, object> map, string name, float defaultValue)
		{
			if (!map.ContainsKey(name))
			{
				return defaultValue;
			}
			return (float)map[name];
		}

		private int GetInt(Dictionary<string, object> map, string name, int defaultValue)
		{
			if (!map.ContainsKey(name))
			{
				return defaultValue;
			}
			return (int)(float)map[name];
		}

		private bool GetBoolean(Dictionary<string, object> map, string name, bool defaultValue)
		{
			if (!map.ContainsKey(name))
			{
				return defaultValue;
			}
			return (bool)map[name];
		}

		private string GetString(Dictionary<string, object> map, string name, string defaultValue)
		{
			if (!map.ContainsKey(name))
			{
				return defaultValue;
			}
			return (string)map[name];
		}

		private float ToColor(string hexString, int colorIndex)
		{
			if (hexString.Length != 8)
			{
				throw new ArgumentException("Color hexidecimal length must be 8, recieved: " + hexString);
			}
			return (float)Convert.ToInt32(hexString.Substring(colorIndex * 2, 2), 16) / 255f;
		}

		private void ReadAnimation(string name, Dictionary<string, object> map, SkeletonData skeletonData)
		{
			ExposedList<Timeline> exposedList = new ExposedList<Timeline>();
			float num = 0f;
			float scale = Scale;
			if (map.ContainsKey("slots"))
			{
				foreach (KeyValuePair<string, object> item2 in (Dictionary<string, object>)map["slots"])
				{
					string key = item2.Key;
					int slotIndex = skeletonData.FindSlotIndex(key);
					Dictionary<string, object> dictionary = (Dictionary<string, object>)item2.Value;
					foreach (KeyValuePair<string, object> item3 in dictionary)
					{
						List<object> list = (List<object>)item3.Value;
						string key2 = item3.Key;
						if (key2 == "color")
						{
							ColorTimeline colorTimeline = new ColorTimeline(list.Count);
							colorTimeline.slotIndex = slotIndex;
							int num2 = 0;
							foreach (Dictionary<string, object> item4 in list)
							{
								float time = (float)item4["time"];
								string hexString = (string)item4["color"];
								colorTimeline.SetFrame(num2, time, ToColor(hexString, 0), ToColor(hexString, 1), ToColor(hexString, 2), ToColor(hexString, 3));
								ReadCurve(colorTimeline, num2, item4);
								num2++;
							}
							exposedList.Add(colorTimeline);
							num = Math.Max(num, colorTimeline.frames[colorTimeline.FrameCount * 5 - 5]);
							continue;
						}
						if (key2 == "attachment")
						{
							AttachmentTimeline attachmentTimeline = new AttachmentTimeline(list.Count);
							attachmentTimeline.slotIndex = slotIndex;
							int num3 = 0;
							foreach (Dictionary<string, object> item5 in list)
							{
								float time2 = (float)item5["time"];
								attachmentTimeline.SetFrame(num3++, time2, (string)item5["name"]);
							}
							exposedList.Add(attachmentTimeline);
							num = Math.Max(num, attachmentTimeline.frames[attachmentTimeline.FrameCount - 1]);
							continue;
						}
						throw new Exception("Invalid timeline type for a slot: " + key2 + " (" + key + ")");
					}
				}
			}
			if (map.ContainsKey("bones"))
			{
				foreach (KeyValuePair<string, object> item6 in (Dictionary<string, object>)map["bones"])
				{
					string key3 = item6.Key;
					int num4 = skeletonData.FindBoneIndex(key3);
					if (num4 == -1)
					{
						throw new Exception("Bone not found: " + key3);
					}
					Dictionary<string, object> dictionary4 = (Dictionary<string, object>)item6.Value;
					foreach (KeyValuePair<string, object> item7 in dictionary4)
					{
						List<object> list2 = (List<object>)item7.Value;
						string key4 = item7.Key;
						switch (key4)
						{
						case "rotate":
						{
							RotateTimeline rotateTimeline = new RotateTimeline(list2.Count);
							rotateTimeline.boneIndex = num4;
							int num6 = 0;
							foreach (Dictionary<string, object> item8 in list2)
							{
								float time4 = (float)item8["time"];
								rotateTimeline.SetFrame(num6, time4, (float)item8["angle"]);
								ReadCurve(rotateTimeline, num6, item8);
								num6++;
							}
							exposedList.Add(rotateTimeline);
							num = Math.Max(num, rotateTimeline.frames[rotateTimeline.FrameCount * 2 - 2]);
							break;
						}
						case "translate":
						case "scale":
						{
							float num7 = 1f;
							TranslateTimeline translateTimeline;
							if (key4 == "scale")
							{
								translateTimeline = new ScaleTimeline(list2.Count);
							}
							else
							{
								translateTimeline = new TranslateTimeline(list2.Count);
								num7 = scale;
							}
							translateTimeline.boneIndex = num4;
							int num8 = 0;
							foreach (Dictionary<string, object> item9 in list2)
							{
								float time5 = (float)item9["time"];
								float num9 = ((!item9.ContainsKey("x")) ? 0f : ((float)item9["x"]));
								float num10 = ((!item9.ContainsKey("y")) ? 0f : ((float)item9["y"]));
								translateTimeline.SetFrame(num8, time5, num9 * num7, num10 * num7);
								ReadCurve(translateTimeline, num8, item9);
								num8++;
							}
							exposedList.Add(translateTimeline);
							num = Math.Max(num, translateTimeline.frames[translateTimeline.FrameCount * 3 - 3]);
							break;
						}
						case "flipX":
						case "flipY":
						{
							bool flag = key4 == "flipX";
							FlipXTimeline flipXTimeline = ((!flag) ? new FlipYTimeline(list2.Count) : new FlipXTimeline(list2.Count));
							flipXTimeline.boneIndex = num4;
							string key5 = ((!flag) ? "y" : "x");
							int num5 = 0;
							foreach (Dictionary<string, object> item10 in list2)
							{
								float time3 = (float)item10["time"];
								flipXTimeline.SetFrame(num5, time3, item10.ContainsKey(key5) && (bool)item10[key5]);
								num5++;
							}
							exposedList.Add(flipXTimeline);
							num = Math.Max(num, flipXTimeline.frames[flipXTimeline.FrameCount * 2 - 2]);
							break;
						}
						default:
							throw new Exception("Invalid timeline type for a bone: " + key4 + " (" + key3 + ")");
						}
					}
				}
			}
			if (map.ContainsKey("ik"))
			{
				foreach (KeyValuePair<string, object> item11 in (Dictionary<string, object>)map["ik"])
				{
					IkConstraintData item = skeletonData.FindIkConstraint(item11.Key);
					List<object> list3 = (List<object>)item11.Value;
					IkConstraintTimeline ikConstraintTimeline = new IkConstraintTimeline(list3.Count);
					ikConstraintTimeline.ikConstraintIndex = skeletonData.ikConstraints.IndexOf(item);
					int num11 = 0;
					foreach (Dictionary<string, object> item12 in list3)
					{
						float time6 = (float)item12["time"];
						float mix = ((!item12.ContainsKey("mix")) ? 1f : ((float)item12["mix"]));
						bool flag2 = !item12.ContainsKey("bendPositive") || (bool)item12["bendPositive"];
						ikConstraintTimeline.SetFrame(num11, time6, mix, flag2 ? 1 : (-1));
						ReadCurve(ikConstraintTimeline, num11, item12);
						num11++;
					}
					exposedList.Add(ikConstraintTimeline);
					num = Math.Max(num, ikConstraintTimeline.frames[ikConstraintTimeline.FrameCount * 3 - 3]);
				}
			}
			if (map.ContainsKey("ffd"))
			{
				foreach (KeyValuePair<string, object> item13 in (Dictionary<string, object>)map["ffd"])
				{
					Skin skin = skeletonData.FindSkin(item13.Key);
					foreach (KeyValuePair<string, object> item14 in (Dictionary<string, object>)item13.Value)
					{
						int slotIndex2 = skeletonData.FindSlotIndex(item14.Key);
						foreach (KeyValuePair<string, object> item15 in (Dictionary<string, object>)item14.Value)
						{
							List<object> list4 = (List<object>)item15.Value;
							FFDTimeline fFDTimeline = new FFDTimeline(list4.Count);
							Attachment attachment = skin.GetAttachment(slotIndex2, item15.Key);
							if (attachment == null)
							{
								throw new Exception("FFD attachment not found: " + item15.Key);
							}
							fFDTimeline.slotIndex = slotIndex2;
							fFDTimeline.attachment = attachment;
							int num12 = ((!(attachment is MeshAttachment)) ? (((SkinnedMeshAttachment)attachment).Weights.Length / 3 * 2) : ((MeshAttachment)attachment).vertices.Length);
							int num13 = 0;
							foreach (Dictionary<string, object> item16 in list4)
							{
								float[] array;
								if (!item16.ContainsKey("vertices"))
								{
									array = ((!(attachment is MeshAttachment)) ? new float[num12] : ((MeshAttachment)attachment).vertices);
								}
								else
								{
									List<object> list5 = (List<object>)item16["vertices"];
									array = new float[num12];
									int num14 = GetInt(item16, "offset", 0);
									if (scale == 1f)
									{
										int i = 0;
										for (int count = list5.Count; i < count; i++)
										{
											array[i + num14] = (float)list5[i];
										}
									}
									else
									{
										int j = 0;
										for (int count2 = list5.Count; j < count2; j++)
										{
											array[j + num14] = (float)list5[j] * scale;
										}
									}
									if (attachment is MeshAttachment)
									{
										float[] vertices = ((MeshAttachment)attachment).vertices;
										for (int k = 0; k < num12; k++)
										{
											array[k] += vertices[k];
										}
									}
								}
								fFDTimeline.SetFrame(num13, (float)item16["time"], array);
								ReadCurve(fFDTimeline, num13, item16);
								num13++;
							}
							exposedList.Add(fFDTimeline);
							num = Math.Max(num, fFDTimeline.frames[fFDTimeline.FrameCount - 1]);
						}
					}
				}
			}
			if (map.ContainsKey("drawOrder") || map.ContainsKey("draworder"))
			{
				List<object> list6 = (List<object>)map[(!map.ContainsKey("drawOrder")) ? "draworder" : "drawOrder"];
				DrawOrderTimeline drawOrderTimeline = new DrawOrderTimeline(list6.Count);
				int count3 = skeletonData.slots.Count;
				int num15 = 0;
				foreach (Dictionary<string, object> item17 in list6)
				{
					int[] array2 = null;
					if (item17.ContainsKey("offsets"))
					{
						array2 = new int[count3];
						for (int num16 = count3 - 1; num16 >= 0; num16--)
						{
							array2[num16] = -1;
						}
						List<object> list7 = (List<object>)item17["offsets"];
						int[] array3 = new int[count3 - list7.Count];
						int num17 = 0;
						int num18 = 0;
						foreach (Dictionary<string, object> item18 in list7)
						{
							int num19 = skeletonData.FindSlotIndex((string)item18["slot"]);
							if (num19 == -1)
							{
								throw new Exception("Slot not found: " + item18["slot"]);
							}
							while (num17 != num19)
							{
								array3[num18++] = num17++;
							}
							int num20 = num17 + (int)(float)item18["offset"];
							array2[num20] = num17++;
						}
						while (num17 < count3)
						{
							array3[num18++] = num17++;
						}
						for (int num21 = count3 - 1; num21 >= 0; num21--)
						{
							if (array2[num21] == -1)
							{
								array2[num21] = array3[--num18];
							}
						}
					}
					drawOrderTimeline.SetFrame(num15++, (float)item17["time"], array2);
				}
				exposedList.Add(drawOrderTimeline);
				num = Math.Max(num, drawOrderTimeline.frames[drawOrderTimeline.FrameCount - 1]);
			}
			if (map.ContainsKey("events"))
			{
				List<object> list8 = (List<object>)map["events"];
				EventTimeline eventTimeline = new EventTimeline(list8.Count);
				int num22 = 0;
				foreach (Dictionary<string, object> item19 in list8)
				{
					EventData eventData = skeletonData.FindEvent((string)item19["name"]);
					if (eventData == null)
					{
						throw new Exception("Event not found: " + item19["name"]);
					}
					Event obj = new Event(eventData);
					obj.Int = GetInt(item19, "int", eventData.Int);
					obj.Float = GetFloat(item19, "float", eventData.Float);
					obj.String = GetString(item19, "string", eventData.String);
					eventTimeline.SetFrame(num22++, (float)item19["time"], obj);
				}
				exposedList.Add(eventTimeline);
				num = Math.Max(num, eventTimeline.frames[eventTimeline.FrameCount - 1]);
			}
			exposedList.TrimExcess();
			skeletonData.animations.Add(new Animation(name, exposedList, num));
		}

		private void ReadCurve(CurveTimeline timeline, int frameIndex, Dictionary<string, object> valueMap)
		{
			if (valueMap.ContainsKey("curve"))
			{
				object obj = valueMap["curve"];
				if (obj.Equals("stepped"))
				{
					timeline.SetStepped(frameIndex);
				}
				else if (obj is List<object>)
				{
					List<object> list = (List<object>)obj;
					timeline.SetCurve(frameIndex, (float)list[0], (float)list[1], (float)list[2], (float)list[3]);
				}
			}
		}
	}
}
