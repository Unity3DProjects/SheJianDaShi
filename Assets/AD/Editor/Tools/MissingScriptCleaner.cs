using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AD.Editor.Tools
{
    public class MissingScriptCleaner : EditorWindow
    {
        // [MenuItem("Tools/空引用清理")]
        public static void ShowWindow()
        {
            var rect = new Rect(new Vector2(Screen.width / 2, Screen.height / 2), new Vector2(600, 420));
            var window = GetWindowWithRect(typeof(MissingScriptCleaner), rect, true, "清理空引用");
        }

        private static string selectedFolderPath;

        private void OnGUI()
        {
       
        }

        public static void Render()
        {
            GUILayout.BeginVertical();

            if (GUILayout.Button("清理场景丢失的脚本"))
            {
                RemoveMissingScriptsInScene();
            }

            GUILayout.BeginHorizontal();
            EditorGUILayout.TextField("选择的修改路径：", selectedFolderPath);
            if (GUILayout.Button("选中当前路径", GUILayout.MaxWidth(100)))
            {
                if (Selection.assetGUIDs.Length > 0)
                {
                    string guid = Selection.assetGUIDs[0];
                    selectedFolderPath = AssetDatabase.GUIDToAssetPath(guid);
                }
                else
                {
                    Debug.LogError("没有选中文件！");
                }
            }

            if (GUILayout.Button("选择路径", GUILayout.MaxWidth(80)))
            {
                var paht = EditorUtility.OpenFolderPanel("选择路径", Application.dataPath, "");
                selectedFolderPath = paht.Replace(Application.dataPath, "Assets");
            }

            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            if (GUILayout.Button("清理路径下所有的预制体空引用", GUILayout.MaxHeight(30)))
            {
                ChangeSelectFloud();
            }

            GUILayout.EndVertical();
        }
    

        public static void RemoveMissingScriptsInScene()
        {
            int count = 0;


            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                GameObject[] rootObjects = scene.GetRootGameObjects();

                foreach (GameObject obj in rootObjects)
                {
                    count += RemoveMissingScriptsInGameObject(obj);
                }

                EditorSceneManager.MarkSceneDirty(scene);
            }
        
            AssetDatabase.SaveAssets();
            Debug.Log($"✅ 清理完成！已删除 {count} 个丢失的脚本引用。");
        }


        private static int RemoveMissingScriptsInGameObject(GameObject obj)
        {
            int count = GameObjectUtility.RemoveMonoBehavioursWithMissingScript(obj);

            foreach (Transform child in obj.transform)
            {
                count += RemoveMissingScriptsInGameObject(child.gameObject);
            }

            return count;
        }


        public static void ChangeSelectFloud()
        {
            var selectedObjects = new List<GameObject>();

            if (!string.IsNullOrEmpty(selectedFolderPath))
            {
                string[] prefabGUIDs = AssetDatabase.FindAssets("t:Prefab", new[] { selectedFolderPath });
                foreach (string guid in prefabGUIDs)
                {
                    string prefabPath = AssetDatabase.GUIDToAssetPath(guid);
                    GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

                    if (prefab != null)
                    {
                        selectedObjects.Add(prefab);
                    }
                }
            }

            RemoveMissingScriptsInGameObjects(selectedObjects);
        }


        private static int RemoveMissingScriptsInGameObjects(List<GameObject> Objs)
        {
            int count = 0;
            foreach (var go in Objs)
            {
                count += GameObjectUtility.RemoveMonoBehavioursWithMissingScript(go);
                foreach (Transform child in go.transform)
                {
                    count += RemoveMissingScriptsInGameObject(child.gameObject);
                }
            }

            Debug.Log($"已清理{count}个丢失引用");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return count;
        }
    }
}