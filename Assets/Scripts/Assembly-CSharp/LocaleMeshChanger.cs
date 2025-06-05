using System.Collections.Generic;
using UnityEngine;

public class LocaleMeshChanger : MonoBehaviour
{
	[SerializeField]
	private GUILayouter[] rootLayouters;

	private Managers cachedManagers;

	protected Managers CachedManagers
	{
		get
		{
			if (cachedManagers == null)
			{
				cachedManagers = SingletonMonoBehaviour<Managers>.Instance;
			}
			return cachedManagers;
		}
	}

	private void Start()
	{
		RefreshMeshFonts();
	}

	private void OnEnable()
	{
		RefreshMeshFonts();
	}

	public void RefreshMeshFonts()
	{
		tk2dTextMesh[] componentsInChildren = GetComponentsInChildren<tk2dTextMesh>(true);
		List<tk2dFontData> defaultLanguageFonts = CachedManagers.CachedLocalisationManager.DefaultLanguageFonts;
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (defaultLanguageFonts == null)
			{
				continue;
			}
			int num = defaultLanguageFonts.IndexOf(componentsInChildren[i].DefaultFontData);
			if (num != -1)
			{
				tk2dFontData tk2dFontData2 = CachedManagers.CachedLocalisationManager.CurrentLanguageFonts[num];
				if (tk2dFontData2 != null)
				{
					componentsInChildren[i].font = tk2dFontData2;
					componentsInChildren[i].ForceBuild();
				}
			}
		}
		for (int j = 0; j < rootLayouters.Length; j++)
		{
			if (rootLayouters[j] != null)
			{
				rootLayouters[j].ResetLayouter();
			}
		}
	}
}
