using System.Collections.Generic;
using System.Reflection;
using AD.Attributes;
using UnityEditor;
using UnityEngine;

namespace AD.Editor
{
    internal class AdDecoratorDrawer : BasePropertyDrawer
    {
        public override bool CanDisplay(SerializedProperty property)
        {
            return !property.TryGetAttribute<AdDecoratorAttribute>(out var attribute) || attribute.Display;
        }

        public override void Before(SerializedProperty property)
        {
            if (!property.TryGetAttribute<AdDecoratorAttribute>(out var attribute)) return;
            if (attribute.Space >= 0)
                GUILayout.Space(attribute.Space);
        }

        private Stack<bool> stateStack = new Stack<bool>();
        public override void LayoutBefore(SerializedProperty property, ref GUIContent label)
        {
            if (!property.TryGetAttribute<AdDecoratorAttribute>(out var attribute)) return;
            if (attribute.Name != null)
                label = new GUIContent(attribute.Name, label.text);
            stateStack.Push(GUI.enabled);
            GUI.enabled &= attribute.Enable;
        }

        public override void LayoutAfter(SerializedProperty property, ref GUIContent label)
        {
            if (!property.TryGetAttribute<AdDecoratorAttribute>(out var attribute)) return;
            GUI.enabled = stateStack.Pop();
        }
    }
}