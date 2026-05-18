using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using AD.Tools;
using UnityEditor;
using UnityEngine;

namespace AD.Editor
{
    public static class ADEditorUtils
    {
        public static void ApplyPrefabInstance(this GameObject instance)
        {
            var prefabAssetPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(instance);
            var prefabAsset = PrefabUtility.LoadPrefabContents(prefabAssetPath);
            if (prefabAsset.GetComponentsInChildren<Transform>().Select(e => e.gameObject)
                .Aggregate(false, (seed, go) => seed | go.RemoveInvalidMonoBehaviours()))
            {
                PrefabUtility.SaveAsPrefabAsset(prefabAsset, prefabAssetPath);
                PrefabUtility.UnloadPrefabContents(prefabAsset);
            }


            foreach (var go in instance.GetComponentsInChildren<Transform>().Select(e => e.gameObject))
                go.RemoveInvalidMonoBehaviours();

            PrefabUtility.ApplyPrefabInstance(instance, InteractionMode.UserAction);
        }

        private static readonly PropertyInfo InspectorModePropertyInfo = typeof(SerializedObject).GetProperty(
            "inspectorMode",
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        private static bool RemoveInvalidMonoBehaviours(this GameObject go)
        {
            bool anyModify = false;

            using (var so = new SerializedObject(go))
            {
                InspectorModePropertyInfo.SetValue(so, InspectorMode.Debug);
                var componentsProp = so.FindProperty("m_Component");
                for (int i = componentsProp.arraySize - 1; i >= 0; i--)
                {
                    var monoProp = componentsProp
                        .GetArrayElementAtIndex(i)
                        .FindPropertyRelative("component");
                    if (monoProp.objectReferenceValue == null)
                    {
                        monoProp.FindPropertyRelative("m_FileID").intValue = 0;
                        monoProp.FindPropertyRelative("m_PathID").intValue = 0;
                        anyModify = true;
                    }
                }

                if (anyModify)
                    so.ApplyModifiedPropertiesWithoutUndo();
            }

            if (GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(go) > 0)
            {
                GameObjectUtility.RemoveMonoBehavioursWithMissingScript(go);
                anyModify = true;
            }

            return anyModify;
        }

        private static readonly Regex Indexer = new Regex(@"(?<=data\[).*(?=\])");

        public static object GetValue(this SerializedProperty prop)
        {
            object obj = prop.serializedObject.targetObject;
            var split = prop.propertyPath.Split('.');
            for (var i = 0; i < split.Length; i++)
            {
                if (obj == null) return null;
                var token = split[i];
                if (token == "Array" && i + 1 < split.Length && Indexer.IsMatch(split[i + 1]))
                {
                    int index = int.Parse(Indexer.Match(split[i + 1]).Value);
                    var iter = ((IEnumerable) obj).GetEnumerator();
                    while (index-- >= 0) iter.MoveNext();
                    obj = iter.Current;
                }
                else
                {
                    obj = ReflectorManager.ReflectInstanceField(obj.GetType(), token)?.GetValue(obj);
                }
            }

            return obj;
        }

        public static bool TryGetAttribute<T>(this SerializedProperty prop, out T attribute)
            where T : Attribute
        {
            attribute = default;
            var info = prop?.GetFieldInfo();
            if (info == null) return false;
            if (!info.IsDefined(typeof(T), true)) return false;
            attribute = info.GetAttr<T>();
            return true;
        }

        public static FieldInfo GetFieldInfo(this SerializedProperty prop)
        {
            object obj = prop.serializedObject.targetObject;
            FieldInfo info = null;
            var split = prop.propertyPath.Split('.');
            for (var i = 0; i < split.Length; i++)
            {
                if (obj == null) return null;
                var token = split[i];
                if (token == "Array" && i + 1 < split.Length && Indexer.IsMatch(split[i + 1]))
                {
                    int index = int.Parse(Indexer.Match(split[i + 1]).Value);
                    var iter = ((IEnumerable) obj).GetEnumerator();
                    while (index-- >= 0) iter.MoveNext();
                    obj = iter.Current;
                }
                else
                {
                    obj = (info = ReflectorManager.ReflectInstanceField(obj.GetType(), token))?.GetValue(obj);
                }
            }

            return info;
        }

        public static SerializedProperty Parent(this SerializedProperty prop)
        {
            var path = prop.propertyPath;
            var split = path.Split('.');
            if (split.Length <= 1) return null;
            int length = split.Length;
            if (Indexer.IsMatch(split[split.Length - 1])) length--;
            length--;
            if (length <= 0) return null;
            return prop.serializedObject.FindProperty(string.Join(".", split.Take(length)));
        }

        public static void DrawHeader(this SerializedProperty property)
        {
            var header = property.GetFieldInfo().GetAttr<HeaderAttribute>()?.header;
            if (header != null)
            {
                EditorGUILayout.LabelField(header, EditorStyles.boldLabel);
            }
        }
    }
}