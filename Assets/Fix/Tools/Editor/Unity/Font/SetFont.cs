using System;
using System.Collections.Generic;
using System.Linq;
#if USE_UNITY_TMP
using TMPro;
#endif
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;


namespace Fix.Editor
{
    public class SetFont : FixEditorWindow
    {
        private const string Title = "设置字体";
        private static readonly string[] FindRoot = new[] {"Assets"};

        private static readonly ISet<Type> Name2FontAssetType = new HashSet<Type>();

        private static readonly IDictionary<Type, List<Action<Object>>> Type2Handler =
            new Dictionary<Type, List<Action<Object>>>();

        private static readonly int MainTex = Shader.PropertyToID("_MainTex");

        [MenuItem(FixRoot + Title, priority = 103)]
        [FixEditor(FixRoot + nameof(Font) + "/" + Title)]
        private static void ShowWindow()
        {
            GetWindowWithRect<SetFont>(new Rect(Screen.width / 2, Screen.height / 2, 500, 500)).titleContent =
                new GUIContent(Title);
        }

        private readonly List<Entry> type2Asset = new List<Entry>();

        private void Init()
        {
            Name2FontAssetType.Add(typeof(UnityEngine.Font));
#if USE_UNITY_TMP
            Name2FontAssetType.Add(typeof(TMP_FontAsset));
            Type2Handler.AddItem(typeof(TMP_FontAsset), new Action<Object>(asset =>
            {
                if (!(asset is TMP_FontAsset targetFont)) return;
                var fontAtlas = new HashSet<Texture2D>(FindMainProjectItem<TMP_FontAsset>()
                    .SelectMany(e => e.atlasTextures)
                    .Distinct());
                var targetAtlas = targetFont.atlasTexture;
                foreach (var material in AssetDatabase.FindAssets("t:Material", new string[] {"Assets"})
                    .Select(AssetDatabase.GUIDToAssetPath)
                    .Select(AssetDatabase.LoadMainAssetAtPath)
                    .OfType<Material>())
                {
                    if (!fontAtlas.Contains(material.GetTexture(MainTex))) continue;
                    material.SetTexture(MainTex, targetAtlas);
                    EditorUtility.SetDirty(material);
                }
            }));
#else
            if ("TMPro.TMP_FontAsset".TryGetType(out var type))
            {
                Name2FontAssetType.Add(type);
                Type2Handler.AddItem(type, new Action<Object>(asset =>
                {
                    var fontAtlas = new HashSet<Texture2D>(FindMainProjectItem(type)
                        .SelectMany(e =>
                        {
                            var atlas = new List<Texture2D>();
                            using (var so = new SerializedObject(e))
                            {
                                var atlasProp = so.FindProperty("m_AtlasTextures");
                                for (int i = 0; i < atlasProp.arraySize; i++)
                                {
                                    atlas.Add((Texture2D) atlasProp.GetArrayElementAtIndex(i).objectReferenceValue);
                                }
                            }

                            return atlas;
                        })
                        .Distinct());
                    Texture2D targetAtlas;
                    using (var so = new SerializedObject(asset))
                    {
                        targetAtlas = (Texture2D) so.FindProperty("m_AtlasTextures")
                            .GetArrayElementAtIndex(0).objectReferenceValue;
                    }

                    foreach (var material in AssetDatabase.FindAssets("t:Material", new string[] {"Assets"})
                        .Select(AssetDatabase.GUIDToAssetPath)
                        .Select(AssetDatabase.LoadMainAssetAtPath)
                        .OfType<Material>())
                    {
                        if (!fontAtlas.Contains(material.GetTexture(MainTex))) continue;
                        material.SetTexture(MainTex, targetAtlas);
                        EditorUtility.SetDirty(material);
                    }
                }));
            }
#endif
            
        }

        private void OnEnable()
        {
            Init();
            foreach (var type in Name2FontAssetType)
            {
                type2Asset.Add(new Entry(type, default));
            }
        }

        private void OnGUI()
        {
            HorizontalRegion(() => { GUILayout.Label("选择字体"); });

            GUILayout.Space(20);
            VerticalRegion(() =>
            {
                foreach (var pair in type2Asset)
                {
                    HorizontalRegion(() =>
                    {
                        pair.asset = EditorGUILayout.ObjectField(pair.type.FullName, pair.asset,
                            pair.type, false);
                    });
                }
            });
            GUILayout.Space(40);
            HorizontalRegion(() =>
            {
                GUILayout.Space(150);
                if (GUILayout.Button("更换字体", GUILayout.Height(40), GUILayout.Width(200)))
                {
                    DoSetFont();
                }
            });
        }

        private void DoSetFont()
        {
            if (type2Asset.All(e => e.asset == null))
            {
                ShowNotification(new GUIContent("未选择字体"));
                return;
            }

            try
            {
                var mapper = type2Asset.Where(e => e.asset != null).ToDictionary(e => e.asset, e => new List<Object>());
                var type2SelectedAsset = type2Asset.Where(e => e.asset != null).ToDictionary(e => e.type, e => e.asset);

                foreach (var o in
                    Name2FontAssetType.SelectMany(FindProjectFonts))
                {
                    if (mapper.ContainsKey(o) || !type2SelectedAsset.ContainsKey(o.GetType())) continue;
                    mapper[type2SelectedAsset[o.GetType()]].Add(o);
                }

                var list = new List<KeyValuePair<string, string>>();
                foreach (var pair in mapper)
                {
                    if (!pair.Key.TryGetRefString(out var @new)) throw new Exception("Invalid asset : " + pair.Key);

                    foreach (var o in pair.Value)
                    {
                        if (!o.TryGetRefString(out var @old)) throw new Exception("Invalid asset : " + pair.Key);
                        list.Add(new KeyValuePair<string, string>(@old, @new));
                    }
                }

                FixEditorExtension.BatchReplaceRef(list);
                AssetDatabase.StartAssetEditing();
                try
                {
                    foreach (var pair in type2SelectedAsset)
                    {
                        if (!Type2Handler.TryGetValue(pair.Key, out var actions)) continue;
                        foreach (var action in actions)
                            action(pair.Value);
                    }
                }
                finally
                {
                    AssetDatabase.StopAssetEditing();
                    AssetDatabase.Refresh();
                }
            }
            finally
            {
                ShowNotification(new GUIContent("替换结束"));
            }
        }

        private static IEnumerable<Object> FindMainProjectItem(Type type)
        {
            return AssetDatabase.FindAssets($"t:{type.Name}", new string[] {"Assets"})
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadMainAssetAtPath)
                .Concat(Resources.FindObjectsOfTypeAll(type))
                .Distinct()
                .Where(type.IsInstanceOfType);
        }

        private static IEnumerable<T> FindMainProjectItem<T>()
            where T : Object
        {
            var type = typeof(T);
            return AssetDatabase.FindAssets($"t:{type.Name}", new string[] {"Assets"})
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadMainAssetAtPath)
                .Concat(Resources.FindObjectsOfTypeAll(type))
                .Distinct()
                .OfType<T>();
        }

        private static IEnumerable<Object> FindProjectFonts(Type type)
        {
            return AssetDatabase.FindAssets($"t:{type.Name}", new string[] {"Assets"})
                .Select(AssetDatabase.GUIDToAssetPath)
                .Where(e => AssetDatabase.GetMainAssetTypeAtPath(e) == type)
                .Select(AssetDatabase.LoadAssetAtPath<Object>)
                .Concat(Resources.FindObjectsOfTypeAll(type))
                .Distinct();
        }

        private class Entry
        {
            public Type type;
            public Object asset;

            public Entry(Type type, Object asset)
            {
                this.type = type;
                this.asset = asset;
            }

            public override string ToString()
            {
                return $"{nameof(type)}: {type}, {nameof(asset)}: {asset}";
            }
        }
    }
}