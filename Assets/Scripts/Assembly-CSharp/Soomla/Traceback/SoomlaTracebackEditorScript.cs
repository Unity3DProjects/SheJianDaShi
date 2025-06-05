using UnityEngine;

namespace Soomla.Traceback
{
	public class SoomlaTracebackEditorScript : ScriptableObject
	{
		private const string soomlaTracebackSettingsAssetName = "SoomlaTracebackEditorScript";

		private const string soomSettingsPath = "SoomlaTraceback/Resources";

		private const string soomSettingsAssetExtension = ".asset";

		[SerializeField]
		public ObjectDictionary TracebackSettings;

		public static GUILayoutOption FieldHeight = GUILayout.Height(16f);

		public static GUILayoutOption FieldWidth = GUILayout.Width(120f);

		public static GUILayoutOption SpaceWidth = GUILayout.Width(24f);

		public static GUIContent EmptyContent = new GUIContent(string.Empty);

		public static JSONObject versionJson;

		public static WWW www;

		private static SoomlaTracebackEditorScript instance;

		public static SoomlaTracebackEditorScript Instance
		{
			get
			{
				if (instance == null)
				{
					instance = Resources.Load("SoomlaTracebackEditorScript") as SoomlaTracebackEditorScript;
					if (instance == null)
					{
						instance = ScriptableObject.CreateInstance<SoomlaTracebackEditorScript>();
					}
				}
				return instance;
			}
		}

		public static void DirtyEditor()
		{
		}

		public static void SetConfigValue(string prefix, string key, string value)
		{
			PlayerPrefs.SetString("Soomla.Traceback." + prefix + "." + key, value);
			Instance.TracebackSettings["Soomla.Traceback." + prefix + "." + key] = value;
			PlayerPrefs.Save();
		}

		public static string GetConfigValue(string prefix, string key)
		{
			string value;
			if (Instance.TracebackSettings.TryGetValue("Soomla.Traceback." + prefix + "." + key, out value) && value.Length > 0)
			{
				return value;
			}
			value = PlayerPrefs.GetString("Soomla.Traceback." + prefix + "." + key);
			SetConfigValue(prefix, key, value);
			return (value.Length <= 0) ? null : value;
		}
	}
}
