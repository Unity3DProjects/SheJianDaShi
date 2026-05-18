using System;
using System.Collections.Generic;
using System.Reflection;
using AD.Attributes;
using AD.Decorator;
using UnityEditor;
using UnityEngine;

namespace AD.Editor
{
    internal class ADParamDecoratorDrawer : BasePropertyDrawer
    {
        private List<IADParamDecorator> tempList = new List<IADParamDecorator>();
        private IDictionary<IADParamDecorator, bool> dec2Foldout = new Dictionary<IADParamDecorator, bool>();

        public override void LayoutAfter(SerializedProperty property, ref GUIContent label)
        {
            var info = property.GetFieldInfo();
            if (info == null) return;
            var attr = info.GetAttr<DecoratableAttribute>();
            if (attr == null) return;
            if (!(property.Parent()?.GetValue() is IADParam param)) return;
            var decorator = param.Decorator;
            if (decorator == null)
            {
                param.Decorator = new ADParamDecoratorManager();
                EditorUtility.SetDirty(ADManager.Instance);
            }

            decorator = param.Decorator;
            if (decorator == null) return;

            if (GUILayout.Button("添加组件", GUILayout.Width(80)))
            {
                var menu = new GenericMenu();
                foreach (var pair in ADEditorCache.DecoratorEnumCache)
                {
                    menu.AddItem(new GUIContent(pair.Key), false, () =>
                    {
                        decorator.decorators.AddItem(info.Name,
                            (IADParamDecorator) Activator.CreateInstance(pair.Value));
                        EditorUtility.SetDirty(ADManager.Instance);
                    });
                }

                menu.ShowAsContext();
            }
        }

        public override void After(SerializedProperty property)
        {
            var info = property.GetFieldInfo();
            if (info == null) return;
            var attr = info.GetAttr<DecoratableAttribute>();
            if (attr == null) return;
            if (!(property.Parent()?.GetValue() is IADParam param)) return;
            var decorator = param.Decorator;
            if (decorator == null) return;
            if (decorator.decorators.TryGetValue(info.Name, out var decorators))
            {
                GUILayout.BeginVertical();
                tempList.Clear();
                tempList.AddRange(decorators);
                EditorGUI.indentLevel++;
                for (var i = 0; i < tempList.Count; i++)
                {
                    var dec = tempList[i];
                    GUILayout.BeginHorizontal();
                    var foldout =
                        dec2Foldout[dec] =
                            EditorGUILayout.Foldout(dec2Foldout.GetOrInit(dec, false),
                                ADEditorCache.DecoratorEnumCache.GetKey(dec.GetType()));
                    var delete = GUILayout.Button("移除", GUILayout.Width(60));
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    if (foldout)
                        dec.Layout();
                    GUILayout.EndHorizontal();

                    if (delete)
                    {
                        decorators.RemoveAt(i);
                        if (decorators.Count == 0) decorator.decorators.Remove(info.Name);
                        EditorUtility.SetDirty(ADManager.Instance);
                    }
                }

                EditorGUI.indentLevel--;

                GUILayout.EndVertical();
            }
        }
    }
}