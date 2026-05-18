using System;
using AD.BlackBag.Shield;
using AD.NodeTree;
using UnityEditor;

namespace AD.Editor
{
    internal interface INodeValueInspector
    {
        Tree.Key Key { get; }
        Tree.Value Value { get; }
        void Init(Tree.Key key, Tree.Value value, SerializedProperty property);
        bool DefaultFoldout { get; }
        void Layout();
    }

    internal abstract class BaseInspector : INodeValueInspector
    {
        public Tree.Key Key { get; private set; }
        public Tree.Value Value { get; private set; }
        protected SerializedProperty Property { get; private set; }

        public void Init(Tree.Key key, Tree.Value value, SerializedProperty property)
        {
            Key = key;
            Value = value;
            Property = property;
        }

        public virtual bool DefaultFoldout => false;

        public virtual void Layout()
        {
            EditorGUI.BeginChangeCheck();

            using (var prop = Property.Copy())
            {
                var depth = prop.depth;
                for (bool enterChild = true;
                    prop.Next(enterChild) && prop.depth > depth;
                    enterChild = false)
                {
                    if (EditorGUILayout.PropertyField(prop, true))
                        Property.serializedObject.ApplyModifiedPropertiesWithoutUndo();
                }
            }

            if (EditorGUI.EndChangeCheck())
            {
                MarkDirty();
            }
        }

        protected void MarkDirty()
        {
            Property.serializedObject.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(ShieldManager.Instance);
        }
    }

    internal class DefaultInspector : BaseInspector
    {
    }
}