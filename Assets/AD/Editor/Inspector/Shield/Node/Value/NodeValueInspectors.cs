using System;
using System.Globalization;
using AD.Attributes;
using AD.NodeTree;
using AD.Tools;
using UnityEditor;
using UnityEngine;

namespace AD.Editor.Inspector
{
    [TypeBind(typeof(AreaShieldNode.AreaValue))]
    internal class AreaShieldInspector : BaseInspector
    {
        public override bool DefaultFoldout => true;

        public override void Layout()
        {
            EditorGUI.BeginChangeCheck();
            var areaProp = Property.FindPropertyRelative("area");
            EditorGUILayout.PropertyField(areaProp);
            for (int i = 0; i < areaProp.arraySize; i++)
            {
                var areaNameProp = areaProp.GetArrayElementAtIndex(i);
                var areaName = areaNameProp.stringValue;
                var newAreaName =
                    AreaShieldNode.T_Replace(AreaShieldNode.T_Replace(areaName, "市"), "省");
                if (newAreaName != areaName) areaNameProp.stringValue = newAreaName;
            }


            if (EditorGUI.EndChangeCheck())
            {
                Property.serializedObject.ApplyModifiedProperties();
            }
        }
    }


    [TypeBind(typeof(ZDTXNode.ZDTXValue))]
    internal class ZDTXValueInspector : BaseInspector
    {
        public override bool DefaultFoldout => true;
    }

    internal abstract class TimeSelectValueInspector : BaseInspector
    {
        GUILayoutOption[] options = {GUILayout.Width(90), GUILayout.Height(26)};
        GUILayoutOption[] options2 = {GUILayout.Width(120), GUILayout.Height(26)};
        public override bool DefaultFoldout => true;

        public override void Layout()
        {
            EditorGUI.BeginChangeCheck();
            GUILayout.Space(5);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            bool setupTime = GUILayout.Button("设置屏蔽时间", options),
                setupModifyTime = GUILayout.Button("设置修改包屏蔽时间", options2);

            EditorGUILayout.EndHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            var property = Property.FindPropertyRelative(nameof(TimeSelectValue.date));
            EditorGUILayout.PropertyField(property);
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
                    $"选择 {Key.description ?? Key.key}",
                    time, result =>
                    {
                        property.stringValue = result.ToString(TimeCheck.Format);
                        property.serializedObject.ApplyModifiedProperties();
                        MarkDirty();
                    });
            }

            if (setupTime) property.stringValue = SetupTime();
            if (setupModifyTime) property.stringValue = SetupModifyTime();
            EditorGUILayout.EndHorizontal();

            if (EditorGUI.EndChangeCheck())
            {
                Property.serializedObject.ApplyModifiedProperties();
            }
        }

        protected abstract string SetupTime();
        protected abstract string SetupModifyTime();
    }

    [TypeBind(typeof(WhiteTimeSelectValue))]
    internal class WhiteTimeSelectValueInspector : TimeSelectValueInspector
    {
        protected override string SetupTime()
        {
            return ADEditorCache.OperationCache[ADManager.Instance.OperationType]
                .GetShieldTime(ADManager.Instance.Platform);
        }

        protected override string SetupModifyTime()
        {
            return ADEditorCache.OperationCache[ADManager.Instance.OperationType]
                .GetModifyPackageShieldTime(ADManager.Instance.Platform);
        }
    }

    [TypeBind(typeof(BlackTimeSelectValue))]
    internal class BlackTimeSelectValueInspector : TimeSelectValueInspector
    {
        protected override string SetupTime()
        {
            return ADEditorCache.OperationCache[ADManager.Instance.OperationType]
                .GetBlackShieldTime(ADManager.Instance.Platform);
        }

        protected override string SetupModifyTime()
        {
            return ADEditorCache.OperationCache[ADManager.Instance.OperationType]
                .GetModifyPackageBlackShieldTime(ADManager.Instance.Platform);
        }
    }

    [TypeBind(typeof(TimingData))]
    internal class TimingDataNodeInspector : BaseInspector
    {
        public override bool DefaultFoldout => true;

        public override void Layout()
        {
            EditorGUI.BeginChangeCheck();
            var enableProp = Property.FindPropertyRelative("enabled");
            EditorGUILayout.PropertyField(enableProp);
            if (enableProp.boolValue)
            {
                using (new EditorGUI.IndentLevelScope())
                {
                    EditorGUILayout.PropertyField(Property.FindPropertyRelative("firstTime"));
                    EditorGUILayout.PropertyField(Property.FindPropertyRelative("repeatTime"));
                }
            }

            if (EditorGUI.EndChangeCheck())
            {
                Property.serializedObject.ApplyModifiedProperties();
            }
        }
    }
}