using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AD.Editor
{
    public class ADParamDrawer
    {
        private static List<BasePropertyDrawer> drawers;

        private static List<BasePropertyDrawer> Drawers
        {
            get
            {
                if (drawers == null)
                    drawers = ADUtils.GetSubClassInstanceOf<BasePropertyDrawer>();
                return drawers;
            }
        }

        private static List<BasePropertyDrawer> tempList = new List<BasePropertyDrawer>();

        public static void OnGUI(SerializedProperty property)
        {
            tempList.Clear();
            tempList.AddRange(Drawers);
            EditorGUI.BeginChangeCheck();
            bool display = true;
            foreach (var drawer in tempList) display &= drawer.CanDisplay(property);
            if (!display) return;
            var label = new GUIContent(property.displayName, property.tooltip);
            foreach (var drawer in tempList) drawer.Before(property);
            EditorGUILayout.BeginHorizontal();
            foreach (var drawer in tempList) drawer.LayoutBefore(property, ref label);
            if (drawers.All(e => !e.OverridePropertyLayout(property,label)))
                EditorGUILayout.PropertyField(property, label, true);
            tempList.Reverse();
            foreach (var drawer in tempList) drawer.LayoutAfter(property, ref label);
            EditorGUILayout.EndHorizontal();
            foreach (var drawer in tempList) drawer.After(property);
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(ADManager.Instance);
                property.serializedObject.ApplyModifiedProperties();
            }
        }
    }
}