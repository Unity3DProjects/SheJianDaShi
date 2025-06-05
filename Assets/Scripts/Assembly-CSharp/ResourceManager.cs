using System;
using System.Collections.Generic;
using MiniJSON;
using UnityEngine;

public class ResourceManager : SingletonMonoBehaviour<ResourceManager>
{
	[Serializable]
	private struct CharacterSpriteCollection
	{
		public CharacterNameType characterType;

		public tk2dSpriteCollectionData[] spriteCollections;
	}

	[Serializable]
	private struct GeneralSpriteCollection
	{
		public SpriteCollectionType spriteCollectionType;

		public tk2dSpriteCollectionData[] spriteCollections;
	}

	[SerializeField]
	private List<CharacterSpriteCollection> charactersSpriteCollections;

	[SerializeField]
	private List<GeneralSpriteCollection> generalSpriteCollections;

	[SerializeField]
	private List<tk2dSpriteCollectionData> skeletonsSpiteCollections;

	[SerializeField]
	private List<string> activeSpriteCollections = new List<string>();

	private void OnEnable()
	{
		CustomGUI.OnDebugGUI += CustomGUI_OnDebugGUI;
		tk2dSpriteCollectionData.OnSpriteCollectionInitialize += Tk2dSpriteCollectionData_OnSpriteCollectionInitialize;
		tk2dSpriteCollectionData.OnSpriteCollectionReset += Tk2dSpriteCollectionData_OnSpriteCollectionReset;
	}

	private void OnDisable()
	{
		CustomGUI.OnDebugGUI -= CustomGUI_OnDebugGUI;
		tk2dSpriteCollectionData.OnSpriteCollectionInitialize -= Tk2dSpriteCollectionData_OnSpriteCollectionInitialize;
		tk2dSpriteCollectionData.OnSpriteCollectionReset -= Tk2dSpriteCollectionData_OnSpriteCollectionReset;
	}

	public void UnloadEachCharacterSpriteCollections(List<CharacterNameType> excludes = null, float delay = -1f)
	{
		if (excludes == null)
		{
			excludes = new List<CharacterNameType>();
		}
		List<tk2dSpriteCollectionData> excludedSpriteCollections = new List<tk2dSpriteCollectionData>();
		int i = 0;
		for (int count = excludes.Count; i < count; i++)
		{
			int num = charactersSpriteCollections.FindIndex((CharacterSpriteCollection data) => data.characterType == excludes[i]);
			if (num < 0)
			{
				continue;
			}
			tk2dSpriteCollectionData[] spriteCollections = charactersSpriteCollections[num].spriteCollections;
			for (int num2 = 0; num2 < spriteCollections.Length; num2++)
			{
				if (!excludedSpriteCollections.Contains(spriteCollections[num2]))
				{
					excludedSpriteCollections.Add(spriteCollections[num2]);
				}
			}
		}
		Action callback = delegate
		{
			int j = 0;
			for (int count2 = charactersSpriteCollections.Count; j < count2; j++)
			{
				CharacterSpriteCollection characterSpriteCollection = charactersSpriteCollections[j];
				if (!excludes.Contains(characterSpriteCollection.characterType))
				{
					int k = 0;
					for (int num3 = characterSpriteCollection.spriteCollections.Length; k < num3; k++)
					{
						tk2dSpriteCollectionData tk2dSpriteCollectionData2 = characterSpriteCollection.spriteCollections[k];
						if (tk2dSpriteCollectionData2 != null && tk2dSpriteCollectionData2.RetainCount <= 0 && !excludedSpriteCollections.Contains(tk2dSpriteCollectionData2))
						{
							tk2dSpriteCollectionData2.ResetPlatformData();
						}
					}
				}
			}
		};
		if (delay < 0f)
		{
			callback();
			return;
		}
		SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
		{
			callback();
		}, delay);
	}

	public void UnloadSkeletonSpriteCollections(float delay = 0f)
	{
		SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
		{
			int i = 0;
			for (int count = skeletonsSpiteCollections.Count; i < count; i++)
			{
				tk2dSpriteCollectionData tk2dSpriteCollectionData2 = skeletonsSpiteCollections[i];
				if (tk2dSpriteCollectionData2 != null)
				{
					tk2dSpriteCollectionData2.ResetPlatformData();
				}
			}
		}, delay);
	}

	public void UnloadCharacterSpriteCollections(CharacterNameType characterType, float delay = -1f)
	{
		Action callback = delegate
		{
			int i = 0;
			for (int count = charactersSpriteCollections.Count; i < count; i++)
			{
				CharacterSpriteCollection characterSpriteCollection = charactersSpriteCollections[i];
				if (characterSpriteCollection.characterType == characterType)
				{
					int j = 0;
					for (int num = characterSpriteCollection.spriteCollections.Length; j < num; j++)
					{
						tk2dSpriteCollectionData tk2dSpriteCollectionData2 = characterSpriteCollection.spriteCollections[j];
						if (tk2dSpriteCollectionData2 != null && tk2dSpriteCollectionData2.RetainCount <= 0)
						{
							tk2dSpriteCollectionData2.ResetPlatformData();
						}
					}
				}
			}
		};
		if (delay < 0f)
		{
			callback();
			return;
		}
		SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
		{
			callback();
		}, delay);
	}

	public void UnloadGeneralSpriteCollections(float delay = -1f)
	{
		Action callback = delegate
		{
			int i = 0;
			for (int count = generalSpriteCollections.Count; i < count; i++)
			{
				GeneralSpriteCollection generalSpriteCollection = generalSpriteCollections[i];
				int j = 0;
				for (int num = generalSpriteCollection.spriteCollections.Length; j < num; j++)
				{
					tk2dSpriteCollectionData tk2dSpriteCollectionData2 = generalSpriteCollection.spriteCollections[j];
					if (tk2dSpriteCollectionData2 != null && tk2dSpriteCollectionData2.RetainCount <= 0)
					{
						tk2dSpriteCollectionData2.ResetPlatformData();
					}
				}
			}
		};
		if (delay < 0f)
		{
			callback();
			return;
		}
		SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
		{
			callback();
		}, delay);
	}

	private void SpriteCollectionStateWasChanged(string spriteCollectionName, bool isInitializing)
	{
		if (string.IsNullOrEmpty(spriteCollectionName))
		{
			return;
		}
		spriteCollectionName = spriteCollectionName.Replace("@2x", string.Empty);
		spriteCollectionName = spriteCollectionName.Replace("@4x", string.Empty);
		if (isInitializing)
		{
			if (!activeSpriteCollections.Contains(spriteCollectionName))
			{
				activeSpriteCollections.Add(spriteCollectionName);
			}
		}
		else if (activeSpriteCollections.Contains(spriteCollectionName))
		{
			activeSpriteCollections.Remove(spriteCollectionName);
		}
	}

	private void CustomGUI_OnDebugGUI()
	{
		if (GUI.Button(new Rect(100f, 0f, 100f, 100f), "Collections"))
		{
			CustomDebug.Log(Json.Serialize(activeSpriteCollections));
		}
	}

	private void Tk2dSpriteCollectionData_OnSpriteCollectionInitialize(tk2dSpriteCollectionData spriteCollection)
	{
		if (spriteCollection != null)
		{
			SpriteCollectionStateWasChanged(spriteCollection.spriteCollectionName, true);
		}
	}

	private void Tk2dSpriteCollectionData_OnSpriteCollectionReset(tk2dSpriteCollectionData spriteCollection)
	{
		if (spriteCollection != null)
		{
			SpriteCollectionStateWasChanged(spriteCollection.spriteCollectionName, false);
		}
	}
}
