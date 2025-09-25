using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EditorTools.Editor.ComponentModifyer
{
    public static class EditorHelper
    {
        /// <summary>
        /// 修改所有组件
        /// </summary>
        /// <param name="OnModify"></param>
        /// <typeparam name="T"></typeparam>
        public static void ModifyAllComponents<T>(Action<T> OnModify) where T : Component
        {
            ScanComponentInPrefab<T>(out var prefabs);
            if (prefabs != null)
            {
                float index = 0;
                foreach (var prefab in prefabs)
                {
                    index++;
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

                    EditorUtility.DisplayProgressBar("预制体修改", $"{index} / {prefabs.Count}", index / prefabs.Count);
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
                        float index = 0;
                        foreach (GameObject rootObject in rootObjects)
                        {
                            index++;
                            T[] componentsInChildren = rootObject.GetComponentsInChildren<T>(true);
                            foreach (T component in componentsInChildren)
                            {
                                OnModify?.Invoke(component);
                            }

                            EditorUtility.DisplayProgressBar("场景修改", $"{index} / {rootObjects.Length}",
                                index / rootObjects.Length);
                        }

                        EditorSceneManager.SaveScene(loadedScene);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }
            }

            EditorUtility.ClearProgressBar();
            Debug.Log("修改场景结束");
            EditorSceneManager.OpenScene(firstScene.path, OpenSceneMode.Single);
        }


        /// <summary>
        /// 异步修改所有组件
        /// </summary>
        /// <param name="OnModifyAsync"></param>
        /// <typeparam name="T"></typeparam>
        public static async Task ModifyAllComponentsAsync<T>(Func<T, Task> OnModifyAsync) where T : Component
        {
            ScanComponentInPrefab<T>(out var prefabs);
            if (prefabs != null)
            {
                float index = 0;
                foreach (var prefab in prefabs)
                {
                    index++;
                    try
                    {
                        T[] componentsInChildren = prefab.GetComponentsInChildren<T>(true);
                        foreach (T component in componentsInChildren)
                        {
                            await OnModifyAsync(component);
                        }

                        PrefabUtility.SavePrefabAsset(prefab);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e.Message);
                    }

                    EditorUtility.DisplayProgressBar("预制体修改", $"{index} / {prefabs.Count}", index / prefabs.Count);
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
                        float index = 0;
                        foreach (GameObject rootObject in rootObjects)
                        {
                            index++;
                            T[] componentsInChildren = rootObject.GetComponentsInChildren<T>(true);
                            foreach (T component in componentsInChildren)
                            {
                                await OnModifyAsync(component);
                            }

                            EditorUtility.DisplayProgressBar("场景修改", $"{index} / {rootObjects.Length}",
                                index / rootObjects.Length);
                        }

                        EditorSceneManager.SaveScene(loadedScene);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }
            }

            EditorUtility.ClearProgressBar();
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


        /// <summary>
        /// 获取当前场景所有该类型组件，包括未激活
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> GetComponentsInCurScene<T>() where T : MonoBehaviour
        {
            T[] allTextComponents = Resources.FindObjectsOfTypeAll<T>();
            List<T> activeTextComponents = new List<T>();

            foreach (T textComponent in allTextComponents)
            {
                if (textComponent.gameObject.scene.IsValid())
                {
                    activeTextComponents.Add(textComponent);
                }
            }

            return activeTextComponents;
        }
    }
}