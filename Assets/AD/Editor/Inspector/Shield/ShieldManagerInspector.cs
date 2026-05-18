using System;
using System.Collections.Generic;
using System.Linq;
using AD.BlackBag;
using AD.BlackBag.Shield;
using AD.NodeTree;
using AD.Tools;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;
using Tree = AD.NodeTree.Tree;

namespace AD.Editor.Inspector
{
    [CustomEditor(typeof(ShieldManager)), CanEditMultipleObjects]
    internal class ShieldManagerInspector : BaseEditorInspector
    {
        private Type defaultInspectorType;

        private IDictionary<string, Type> managedTypeFullName2InspectorTypes
            = new Dictionary<string, Type>();

        private BiMap<SerializedProperty, INodeValueInspector> customInspectors
            = new BiMap<SerializedProperty, INodeValueInspector>();

        private IDictionary<INodeValueInspector, bool> inspector2Foldout
            = new Dictionary<INodeValueInspector, bool>();

        private IDictionary<string, List<INodeValueInspector>> group2Inspectors
            = new Dictionary<string, List<INodeValueInspector>>();

        internal IEnumerable<INodeValueInspector> TimeInspectors =>
            customInspectors.Values.OfType<TimeSelectValueInspector>();


        private TreeDataDisplayDB displayDB;

        private TreeDataDisplayDB DisplayDB
        {
            get
            {
                if (displayDB != null) return displayDB;
                displayDB = CreateInstance<TreeDataDisplayDB>();
                return displayDB;
            }
        }

        private SerializedObject displayDBSerializedObject;

        private bool GetDisplaySerializedObject(bool safe, out SerializedObject so)
        {
            if (displayDB == null)
            {
                displayDBSerializedObject?.Dispose();
                displayDBSerializedObject = null;
            }

            if (safe)
            {
                if (displayDBSerializedObject == null)
                    displayDBSerializedObject = new SerializedObject(DisplayDB);
                so = displayDBSerializedObject;
                return true;
            }
            else
            {
                so = displayDBSerializedObject;
                return so != null;
            }
        }

        private void ReleaseDisplay()
        {
            if (displayDB == null) return;
            DestroyImmediate(displayDB);
            if (GetDisplaySerializedObject(false, out var so))
                so.Dispose();
        }

        private void OnEnable()
        {
            Init();
            ShieldManager.EditorUpdate += Init;
            CompilationPipeline.compilationFinished += Init;
        }

        private void OnDisable()
        {
            ShieldManager.EditorUpdate -= Init;
            CompilationPipeline.compilationFinished -= Init;
            try
            {
                foreach (var key in customInspectors.Keys)
                    key.Dispose();
            }
            catch
            {
            }

            customInspectors.Clear();
            inspector2Foldout.Clear();
            managedTypeFullName2InspectorTypes.Clear();
            defaultInspectorType = null;

            ReleaseDisplay();
        }

        protected override void Layout()
        {
            Space(3);
            using (new EditorGUI.DisabledScope(true))
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
            if (!(target is ShieldManager manager)) return;
            serializedObject.Update();
            if (ADManager.Instance)
            {
                if (Event.current.type == EventType.Repaint)
                {
                    if (manager.EditorReInit(new Tree(ADEditorCache.OperationCache[ADManager.Instance.OperationType]
                            .CreateRule(ADManager.Instance.Platform)))
                        | manager.EditorInitOverride(ADManager.Instance.OverrideShieldType))
                    {
                        return;
                    }
                }
            }


            var labelStyle = new GUIStyle(EditorStyles.largeLabel)
            {
                fontStyle = FontStyle.Bold
            };
            if (ADManager.Instance.OverrideShieldType != EShieldType.None)
            {
                Module(() => { EditorGUILayout.LabelField("使用覆盖屏蔽方式", labelStyle); },
                    ColorR(new Color(1, 0.5f, 0.4f, 1)));
            }
            else
            {
                Module(() => { EditorGUILayout.LabelField("使用默认屏蔽方式", labelStyle); },
                    ColorR(new Color(0.5f, 1f, 0.4f, 1)));
            }

            Space(3);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("重置所有", GUILayout.Height(24))) ResetAll();
            EditorGUILayout.EndHorizontal();
            EditorGUI.BeginChangeCheck();
            ForeachInspector(inspector =>
            {
                EditorGUILayout.BeginHorizontal();
                bool foldout = inspector2Foldout[inspector];
                inspector2Foldout[inspector] =
                    EditorGUILayout.Foldout(inspector2Foldout[inspector],
                        inspector.Key.description ?? inspector.Key.key, true);
                bool hasReset = inspector.Value.Resettable;
                bool reset = hasReset && GUILayout.Button("重置", GUILayout.Width(80));
                EditorGUILayout.EndHorizontal();
                if (foldout)
                    using (new EditorGUI.IndentLevelScope())
                        inspector.Layout();


                if (reset)
                {
                    inspector.Value.Reset();
                    if (customInspectors.TryGetKey(inspector, out var so))
                    {
                        so.serializedObject.Update();
                    }
                }

                Space(5);
            });
            if (EditorGUI.EndChangeCheck())
            {
                Save();
            }

            Space(5);
            ColorSpace(new Color(0.5f, 0.7f, 0.7f), 15);
        }

        internal void Save()
        {
            EditorUtility.SetDirty(target);
        }

        private void ResetAll()
        {
            ShieldManager.Instance.Container.Data.Clear();
            EditorUtility.SetDirty(ShieldManager.Instance);
            serializedObject.ApplyModifiedProperties();
            ReleaseDisplay();
        }

        private void Init(object o)
        {
            customInspectors.Clear();
            inspector2Foldout.Clear();
            ReleaseDisplay();

            CreateRule(out var tree, out var container);
            DisplayDB.Init(tree, container);
        }

        private void Init()
        {
            managedTypeFullName2InspectorTypes.Clear();
            foreach (var type in ADUtils.GetSubClassOf<INodeValueInspector>())
            {
                if (type != typeof(DefaultInspector))
                {
                    var typeBind = type.GetTypeBind();
                    if (typeBind != null && typeof(Tree.Value).IsAssignableFrom(typeBind))
                        managedTypeFullName2InspectorTypes.Add(typeBind.GetManagedTypeFullName(), type);
                }
                else defaultInspectorType = type;
            }

            try
            {
                foreach (var key in customInspectors.Keys)
                    key.Dispose();
            }
            catch
            {
            }

            customInspectors.Clear();
            inspector2Foldout.Clear();
            ReleaseDisplay();
            CreateRule(out var tree, out var container);
            DisplayDB.Init(tree, container);
        }

        private void ForeachInspector(Action<INodeValueInspector> action)
        {
            if (!ADManager.Instance || !GetDisplaySerializedObject(true, out var so)) return;
            var group2Tuple =
                new Dictionary<string, List<(SerializedProperty valueProp, TreeDataDisplayDB.Entry entry)>>();
            var property = so.FindProperty("values");
            var arraySize = property.arraySize;
            for (int i = 0; i < arraySize; i++)
            {
                var ele = property.GetArrayElementAtIndex(i);
                var entry = DisplayDB.values[i];
                var valueProp = ele.FindPropertyRelative("value").Copy();
                group2Tuple.AddItem(entry.key.Group, (valueProp, entry));
            }

            if (group2Tuple.TryGetValue(Tree.Key.NoGroup, out var noGroupTuples))
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                Space();
                using (new EditorGUI.IndentLevelScope())
                {
                    bool first = true;
                    foreach (var tuple in noGroupTuples)
                    {
                        if (!first) Line(5);
                        action(GetOrCreateInspector(tuple.valueProp, tuple.entry));
                        first = false;
                    }
                }

                EditorGUILayout.EndVertical();
            }

            foreach (var pair in group2Tuple.Where(e => e.Key != Tree.Key.NoGroup))
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.LabelField($"{pair.Key} 分组:", new GUIStyle(EditorStyles.largeLabel)
                {
                    fontSize = 16,
                    fontStyle = FontStyle.Bold,
                }, GUILayout.Height(24));
                using (new EditorGUI.IndentLevelScope())
                {
                    foreach (var tuple in pair.Value)
                    {
                        Line(5);
                        action(GetOrCreateInspector(tuple.valueProp, tuple.entry));
                    }
                }

                EditorGUILayout.EndVertical();
            }

            // var property = so.FindProperty("values");
            // var arraySize = property.arraySize;
            // for (int i = 0; i < arraySize; i++)
            // {
            //     var ele = property.GetArrayElementAtIndex(i);
            //     var entry = DisplayDB.values[i];
            //     var valueProp = ele.FindPropertyRelative("value").Copy();
            //     if (!TryFindInspector(valueProp, out var inspector))
            //     {
            //         if (!managedTypeFullName2InspectorTypes.TryGetValue(valueProp.managedReferenceFullTypename,
            //             out var type))
            //             type = defaultInspectorType;
            //         customInspectors.Add(valueProp,
            //             inspector = (INodeValueInspector) Activator.CreateInstance(type));
            //         group2Inspectors.AddItem(inspector.Key.Group, inspector);
            //         inspector.Init(entry.key, entry.value, valueProp);
            //         inspector2Foldout.Add(inspector, inspector.DefaultFoldout);
            //     }
            //
            //     action(inspector);
            // }
        }

        private INodeValueInspector GetOrCreateInspector(SerializedProperty property, TreeDataDisplayDB.Entry entry)
        {
            if (!TryFindInspector(property, out var inspector))
            {
                if (!managedTypeFullName2InspectorTypes.TryGetValue(property.managedReferenceFullTypename,
                    out var type))
                    type = defaultInspectorType;
                customInspectors.Add(property,
                    inspector = (INodeValueInspector) Activator.CreateInstance(type));
                inspector.Init(entry.key, entry.value, property);
                group2Inspectors.AddItem(inspector.Key.Group, inspector);
                inspector2Foldout.Add(inspector, inspector.DefaultFoldout);
            }

            return inspector;
        }

        private void CreateRule(out Tree tree, out TreeDataContainer container)
        {
            INode rule;
            if (ADManager.Instance.OverrideShieldType != EShieldType.None)
            {
                rule = ShieldManager.Build(ADManager.Instance.OverrideShieldType);
            }
            else
            {
                rule = ADEditorCache.OperationCache[ADManager.Instance.OperationType]
                    .CreateRule(ADManager.Instance.Platform);
            }

            tree = new Tree(rule);
            container = ShieldManager.Instance.Container;
            container.ReadFromTree(tree);
        }

        private bool TryFindInspector(SerializedProperty ele, out INodeValueInspector inspector)
        {
            foreach (var pair in customInspectors)
            {
                if (SerializedProperty.EqualContents(ele, pair.Key))
                {
                    inspector = pair.Value;
                    return true;
                }
            }

            inspector = default;
            return false;
        }

        private static void Line(int space = 14)
        {
            GUILayout.Space(2);
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(space));
            GUILayout.Space(2);
        }
    }
}