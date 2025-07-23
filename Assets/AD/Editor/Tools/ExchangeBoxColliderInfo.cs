using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AD.Editor.Tools
{
    public class ExchangeBoxColliderInfo : EditorWindow
    {
        private static string savePathDir = "Assets/Config/";
        private static string loadPathDir = "Assets/Config/";

        private static string savePath = "Assets/Config/BoxCollidersInfo.json";
        private static string loadPath = "Assets/Config/BoxCollidersInfo.json";

        // [MenuItem("Tools/BoxCollider Manager")]
        public static void ShowWindow()
        {
            GetWindow<ExchangeBoxColliderInfo>("BoxCollider Manager");
        }

        private void OnGUI()
        {
        }

        public static void ExistOrCreate(string path)
        {
            if (Directory.Exists(path))
            {
                return;
            }

            Directory.CreateDirectory(path);
        }

        public static void Render()
        {
            if (GUILayout.Button("Check and Rename Duplicates"))
            {
                CheckAndRenameDuplicates();
            }

            GUILayout.Label("BoxCollider Manager", EditorStyles.boldLabel);
            savePath = EditorGUILayout.TextField("Save Path", savePath);
            if (GUILayout.Button("Save BoxColliders"))
            {
                SaveBoxColliders();
            }

            loadPath = EditorGUILayout.TextField("Load Path", loadPath);
            if (GUILayout.Button("Load BoxColliders"))
            {
                LoadBoxColliders();
            }
        }

        private static void CheckAndRenameDuplicates()
        {
            var boxColliders = Resources.FindObjectsOfTypeAll<BoxCollider>();
            var nameCount = new Dictionary<string, int>();
            foreach (var boxCollider in boxColliders)
            {
                if (EditorUtility.IsPersistent(boxCollider)) continue;
                string gameObjectName = boxCollider.name;
                if (nameCount.ContainsKey(gameObjectName))
                {
                    nameCount[gameObjectName]++;
                    string newName = $"{gameObjectName}_{nameCount[gameObjectName]}";
                    boxCollider.gameObject.name = newName;
                    Debug.LogWarning("Duplicate game object name found and renamed to: " + newName);
                }
                else
                {
                    nameCount[gameObjectName] = 0;
                }
            }
        }


        private static void SaveBoxColliders()
        {
            var boxColliders = Resources.FindObjectsOfTypeAll<BoxCollider>();
            var boxColliderData = new Dictionary<string, BoxColliderData>();
            foreach (var collider in boxColliders)
            {
                // Skip prefabs and assets that are not part of the active scene
                if (EditorUtility.IsPersistent(collider)) continue;
                string gameObjectName = collider.gameObject.name;
                if (!boxColliderData.ContainsKey(gameObjectName))
                {
                    boxColliderData[gameObjectName] = new BoxColliderData(collider);
                }
                else
                {
                    Debug.LogWarning("Duplicate game object name found: " + gameObjectName);
                }
            }

            string json = JsonUtility.ToJson(new BoxColliderDataList(boxColliderData.Values.ToList()), true);
            ExistOrCreate(savePathDir);
            File.WriteAllText(savePath, json);
            Debug.Log("BoxColliders saved to " + savePath);
            AssetDatabase.Refresh();
        }

        private static void LoadBoxColliders()
        {
            if (!File.Exists(loadPath))
            {
                Debug.LogError("File not found: " + loadPath);
                return;
            }

            string json = File.ReadAllText(loadPath);
            var boxColliderDataList = JsonUtility.FromJson<BoxColliderDataList>(json);
            foreach (var data in boxColliderDataList.items)
            {
                var go = FindGameObjectByName(data.name);
                if (go != null)
                {
                    var IsActive = go.activeSelf;
                    if (!IsActive)
                    {
                        go.SetActive(true);
                    }

                    var collider = go.GetComponent<BoxCollider>();
                    if (collider != null)
                    {
                        Debug.Log("成功");
                        data.ApplyTo(collider);
                        EditorUtility.SetDirty(collider);
                    }

                    go.SetActive(IsActive);
                }
                else
                {
                    Debug.LogError("找不到这个物体--------------" + data.name);
                }
            }

            Debug.Log("BoxColliders loaded from " + loadPath);
        }

        private static GameObject FindGameObjectByName(string GameObjectName)
        {
            var GameObjectList = Resources.FindObjectsOfTypeAll<GameObject>();
            foreach (var gameObject in GameObjectList)
            {
                if (gameObject.name.Equals(GameObjectName))
                {
                    return gameObject;
                }
            }

            return null;
        }
    }

    [System.Serializable]
    public class BoxColliderDataList
    {
        public List<BoxColliderData> items;

        public BoxColliderDataList(List<BoxColliderData> items)
        {
            this.items = items;
        }
    }

    [System.Serializable]
    public class BoxColliderData
    {
        public string name;
        public Vector3 center;
        public Vector3 size;
        public bool isTrigger;

        public BoxColliderData(BoxCollider collider)
        {
            name = collider.gameObject.name;
            center = collider.center;
            size = collider.size;
            isTrigger = collider.isTrigger;
        }

        public void ApplyTo(BoxCollider collider)
        {
            collider.center = center;
            collider.size = size;
            collider.isTrigger = isTrigger;
        }
    }
}