using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AD.Editor.Tools
{
    public static class EditorHelper
    {
        public static void ModifyAllComponents<T>(Action<T> OnModify) where T : Component
        {
            var components = ScanComponentInPrefab<T>(out var prefabs);
            if (prefabs != null)
            {
                foreach (var prefab in prefabs)
                {
                    try
                    {
                        T[] componentsInChildren = prefab.GetComponentsInChildren<T>(true);
                        foreach (T component in componentsInChildren)
                        {
                            OnModify?.Invoke(component);
                        }

                        PrefabUtility.SavePrefabAsset(prefab);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e.Message);
                    }
                }

                Debug.Log("修改预制体结束");
            }

            var firstScene = EditorBuildSettings.scenes[0];
            foreach (var scene in EditorBuildSettings.scenes)
            {
                try
                {
                    if (scene.enabled)
                    {
                        Scene loadedScene = EditorSceneManager.OpenScene(scene.path, OpenSceneMode.Single);

                        GameObject[] rootObjects = loadedScene.GetRootGameObjects();
                        foreach (GameObject rootObject in rootObjects)
                        {
                            T[] componentsInChildren = rootObject.GetComponentsInChildren<T>(true);
                            foreach (T component in componentsInChildren)
                            {
                                OnModify?.Invoke(component);
                            }
                        }

                        EditorSceneManager.SaveScene(loadedScene);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }
            }

            Debug.Log("修改场景结束");
            EditorSceneManager.OpenScene(firstScene.path, OpenSceneMode.Single);
        }

        /// <summary>
        /// 搜集包含组件的预制体
        /// </summary>
        /// <typeparam name="T">Mono组件</typeparam>
        public static List<T> ScanComponentInPrefab<T>(out List<GameObject> prefabs) where T : Component
        {
            prefabs = new List<GameObject>();
            var ObjectsPath = new List<string>();
            List<T> components = new List<T>();
            string[] prefabGUIDs = AssetDatabase.FindAssets("t:Prefab");
            foreach (string guid in prefabGUIDs)
            {
                string prefabPath = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

                if (prefab != null)
                {
                    var _components = prefab.GetComponentsInChildren<T>(true);
                    if (_components != null)
                    {
                        if (_components.Length > 0)
                        {
                            prefabs.Add(prefab);
                            ObjectsPath.Add(prefabPath);
                            components.AddRange(_components);
                        }
                    }
                }
            }

            return components;
        }
    }
}