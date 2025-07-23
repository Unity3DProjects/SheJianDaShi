using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AD.Editor.Tools
{
    public class FindAssetsWindow : EditorWindow
    {
        [MenuItem("Tools/资源查找")]
        public static void Open()
        {
            var window = CreateWindow<FindAssetsWindow>();
            window.position = new Rect(Screen.width / 3, Screen.height / 4, 800f, 600f);
        }

        private List<string> assetPathList = new List<string>();

        private Vector2 scrollViewPos = Vector2.zero;

        private int selectedIndex = -1; // 当前选中的按钮索引

        private GUIStyle normalStyle;

        private GUIStyle selectedStyle;


        private void OnGUI()
        {
            normalStyle = new GUIStyle(GUI.skin.button);
            selectedStyle = new GUIStyle(GUI.skin.button);

            normalStyle.normal.textColor = Color.white;
            normalStyle.hover.textColor = Color.gray;

            selectedStyle.normal.background = Texture2D.blackTexture;
            selectedStyle.normal.textColor = Color.cyan;
            selectedStyle.hover.textColor = Color.gray;


            GUILayout.Space(5);
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("查找法线贴图"))
            {
                FindTextures(TextureImporterType.NormalMap);
            }

            if (GUILayout.Button("查找光照贴图"))
            {
                FindTextures(TextureImporterType.Lightmap);
            }  
            
            if (GUILayout.Button("查找精灵图"))
            {
                FindTextures(TextureImporterType.Sprite);
            }

            if (GUILayout.Button("清除", GUILayout.Width(60)))
            {
                assetPathList.Clear();
            }

            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            var style = GUI.skin.horizontalSlider;
            EditorGUILayout.LabelField("", style);
            if (assetPathList == null)
                return;
            GUILayout.Label($"查找到资源数量:{assetPathList.Count}");


            scrollViewPos = EditorGUILayout.BeginScrollView(scrollViewPos);
            if (assetPathList != null)
            {
                for (int i = 0; i < assetPathList.Count; i++)
                {
                    GUILayout.BeginHorizontal();


                    if (GUILayout.Button($"{assetPathList[i]}    {i}",
                            selectedIndex == i ? selectedStyle : normalStyle))
                    {
                        selectedIndex = i;
                        Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(assetPathList[i]);
                    }

                    if (GUILayout.Button("删除资源", GUILayout.Width(80)))
                    {
                        AssetDatabase.DeleteAsset(assetPathList[i]);
                        assetPathList.Remove(assetPathList[i]);
                        if (selectedIndex == i) selectedIndex = -1; // 如果删除的是选中项，重置选中索引
                    }

                    GUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.EndScrollView();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("删除所有资源", GUILayout.Width(100)))
            {
                List<string> failedPaths = new List<string>();
                foreach (var path in assetPathList)
                {
                    AssetDatabase.DeleteAsset(path);
                }

                assetPathList.Clear();

                foreach (var failedPath in failedPaths)
                {
                    Debug.Log($"{failedPath},删除失败");
                }
            }

            GUILayout.EndHorizontal();
        }

        private void FindTextures(TextureImporterType textureImporterType)
        {
            assetPathList.Clear();
            var guids = AssetDatabase.FindAssets("t:Texture");
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
                if (asset != null && asset is Texture2D)
                {
                    var texureImportor = AssetImporter.GetAtPath(path) as TextureImporter;
                    if (texureImportor == null)
                        continue;
                    if (texureImportor.textureType == textureImporterType)
                    {
                        assetPathList.Add(path);
                    }
                }
            }
        }
    }
}