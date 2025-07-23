using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace AD.Editor.Tools
{
    public class EditorSceneWindow : EditorWindow
    {
        private static EditorBuildSettingsScene[] scens;

        private static EditorBuildSettingsScene[] Scens
        {
            get
            {
                scens = EditorBuildSettings.scenes;
                return scens;
            }
        }

        // [MenuItem("Tools/SceneWindow")]
        public static void Open()
        {
            EditorWindow.GetWindowWithRect(typeof(EditorSceneWindow),
                new Rect(Screen.width / 2, Screen.height / 2, 250, 500));
        }

        private void OnGUI()
        {
        }

        static Vector2 scrollPosition = Vector2.zero;

        public static void Render()
        {
            GUILayout.BeginVertical();

            scrollPosition = GUILayout.BeginScrollView(scrollPosition);
            for (int i = 0; i < Scens.Length; i++)
            {
                if (Scens[i].enabled)
                {
                    var path = Scens[i].path;

                    var scene = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);
                    if (scene == null)
                        continue;

                    if (GUILayout.Button($"{scene.name}       {i}",GUILayout.Height(30)))
                    {
                        EditorSceneManager.OpenScene(path);
                    }
                }
            }

            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }
    }
}