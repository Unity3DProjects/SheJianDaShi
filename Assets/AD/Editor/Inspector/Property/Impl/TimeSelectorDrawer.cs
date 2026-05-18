using System;
using System.Globalization;
using AD.Attributes;
using AD.Tools;
using UnityEditor;
using UnityEngine;

namespace AD.Editor
{
    internal class TimeSelectorDrawer : BasePropertyDrawer
    {
        public override void LayoutAfter(SerializedProperty property, ref GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.String
                || !property.TryGetAttribute<TimeSelectorAttribute>(out _))
                return;
            var value = property.stringValue;
            var time = DateTime.TryParseExact(
                value,
                TimeCheck.Format,
                null,
                DateTimeStyles.None,
                out var dt)
                ? dt
                : DateTime.Now;

            if (GUILayout.Button("选择时间", GUILayout.Width(80)))
            {
                TimeSelector.ShowTimeSelectorWindow(
                    GUIUtility.GUIToScreenPoint(Event.current.mousePosition),
                    $"选择 {label.text}",
                    time, result =>
                    {
                        property.stringValue = result.ToString(TimeCheck.Format);
                        property.serializedObject.ApplyModifiedProperties();
                        GUI.changed = true;
                    });
            }
        }
    }
}