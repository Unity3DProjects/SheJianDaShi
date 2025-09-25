#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(ConditionalHeaderFieldAttribute))]
public class ConditionalHeaderFieldDrawer : PropertyDrawer
{
 
    private const float HeaderSpacing =2f;
    private static Dictionary<string, SerializedProperty> _propertyCache = new Dictionary<string, SerializedProperty>();
 
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var attr = (ConditionalHeaderFieldAttribute)attribute;
        bool shouldShow = ShouldShow(property, attr);

        if (shouldShow)
        {
            // 1. 绘制 Header
            Rect headerRect = new Rect(
                position.x,
                position.y,
                position.width,
                EditorGUIUtility.singleLineHeight
            );
            EditorGUI.LabelField(headerRect, attr.Header, EditorStyles.boldLabel);

            // 2. 绘制字段（保留Unity默认折叠逻辑）
            Rect fieldRect = new Rect(
                position.x,
                position.y + EditorGUIUtility.singleLineHeight + HeaderSpacing,
                position.width,
                EditorGUI.GetPropertyHeight(property, label, true)
            );
            EditorGUI.PropertyField(fieldRect, property, new GUIContent(property.displayName), true);
        }
        
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var attr = (ConditionalHeaderFieldAttribute)attribute;
        bool shouldShow = ShouldShow(property, attr);

        if (!shouldShow) return 0;

        // 总高度 = Header高度 + 字段高度（含折叠状态）
        return EditorGUIUtility.singleLineHeight 
               + EditorGUI.GetPropertyHeight(property, label, true)
               + HeaderSpacing;
    }

    private bool ShouldShow(SerializedProperty property, ConditionalHeaderFieldAttribute attr)
    {
        // 复用原有 ConditionalField 的条件检查逻辑
        var fakeAttribute = new ConditionalFieldAttribute(attr.Logic, attr.Conditions);
        return new ConditionalFieldDrawer().ShouldShow(property, fakeAttribute);
    }
}
#endif

