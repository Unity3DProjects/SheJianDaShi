#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;
using Object = UnityEngine.Object;


[CustomPropertyDrawer(typeof(L_AD.ButtonAttribute))]
public class ButtonDrawer : PropertyDrawer
{
   // 缓存反射结果
    private static Dictionary<string, MethodInfo> _methodCache = new Dictionary<string, MethodInfo>();
    
    // 按钮样式
    private static GUIStyle _buttonStyle;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        InitializeStyles();
        
        var buttonAttribute = (L_AD.ButtonAttribute)attribute;
        bool shouldShow = CheckCondition(property, buttonAttribute);

        if (!shouldShow) return;

        var buttonText = GetButtonText(property, buttonAttribute);
        // var buttonRect = CalculateButtonRect(position);
        Rect buttonRect = new Rect(
            position.x,
            position.y,
            EditorGUIUtility.currentViewWidth - 100f, // ← 修改后的宽度
            EditorGUIUtility.singleLineHeight
        );
        DrawButton(buttonRect, buttonText, property, buttonAttribute);
    }

    private void InitializeStyles()
    {
        if (_buttonStyle == null)
        {
            _buttonStyle = new GUIStyle(GUI.skin.button)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 12,
                fixedHeight = 22,
                margin = new RectOffset(5, 5, 2, 2)
            };
        }
    }

    private bool CheckCondition(SerializedProperty property, L_AD.ButtonAttribute attribute)
    {
        if (string.IsNullOrEmpty(attribute.ConditionPath)) return true;

        SerializedProperty conditionProp = 
            property.serializedObject.FindProperty(attribute.ConditionPath);

        if (conditionProp == null)
        {
            Debug.LogWarning($"找不到条件字段: {attribute.ConditionPath}");
            return true;
        }

        object conditionValue = GetPropertyValue(conditionProp);
        object targetValue = attribute.TargetValue;

        return CompareValues(conditionValue, targetValue);
    }

    private object GetPropertyValue(SerializedProperty prop)
    {
        switch (prop.propertyType)
        {
            case SerializedPropertyType.Boolean: return prop.boolValue;
            case SerializedPropertyType.Integer: return prop.intValue;
            case SerializedPropertyType.Float: return prop.floatValue;
            case SerializedPropertyType.String: return prop.stringValue;
            case SerializedPropertyType.Enum: return prop.intValue;
            case SerializedPropertyType.ObjectReference: return prop.objectReferenceValue;
            default: return null;
        }
    }

    private bool CompareValues(object a, object b)
    {
        if (a == null || b == null) return a == b;

        // 处理枚举比较
        if (b is Enum) b = Convert.ToInt32(b);
        if (a is Enum) a = Convert.ToInt32(a);

        // 处理数值类型
        if (a is IConvertible numA && b is IConvertible numB)
        {
            try
            {
                return Convert.ToDouble(numA) == Convert.ToDouble(numB);
            }
            catch { /* 忽略转换错误 */ }
        }

        // 处理字符串
        if (b is string strB)
            return (a?.ToString() ?? "") == strB;

        return a.Equals(b);
    }

    private string GetButtonText(SerializedProperty property, L_AD.ButtonAttribute attribute)
    {
        return string.IsNullOrEmpty(attribute.Label) ? 
            property.displayName : 
            attribute.Label;
    }

    private Rect CalculateButtonRect(Rect position)
    {
        return new Rect(
            position.x,
            position.y,
            position.width,
            EditorGUIUtility.singleLineHeight
        );
    }

    private void DrawButton(Rect rect, string text, SerializedProperty property, L_AD.ButtonAttribute attribute)
    {
        if (GUI.Button(rect, text, _buttonStyle))
        {
            HandleButtonClick(property.serializedObject.targetObject, attribute.MethodName);
        }
    }

    private void HandleButtonClick(Object target, string methodName)
    {
        if (target == null) return;

        // 处理多对象编辑
        if (Selection.gameObjects.Length > 1)
        {
            foreach (var obj in Selection.gameObjects)
            {
                HandleSingleObject(obj, methodName);
            }
        }
        else
        {
            HandleSingleObject(target, methodName);
        }
    }

    private void HandleSingleObject(Object target, string methodName)
    {
        if (string.IsNullOrEmpty(methodName))
        {
            // 直接修改bool字段值
            if (target is MonoBehaviour behaviour)
            {
                Undo.RecordObject(behaviour, "Toggle Bool");
                var field = behaviour.GetType().GetField("_buttonField");
                if (field != null && field.FieldType == typeof(bool))
                {
                    bool currentValue = (bool)field.GetValue(behaviour);
                    field.SetValue(behaviour, !currentValue);
                    EditorUtility.SetDirty(behaviour);
                    RepaintAllInspectors(); 
                }
            }
            return;
        }

        MethodInfo method = GetCachedMethod(target.GetType(), methodName);

        if (method == null)
        {
            Debug.LogWarning($"找不到方法: {methodName}");
            return;
        }

        // 安全验证
        if (method.GetParameters().Length > 0)
        {
            Debug.LogWarning($"方法 {methodName} 必须是无参数的");
            return;
        }

        Undo.RecordObject(target, $"Invoke {methodName}");
        method.Invoke(target, null);
        EditorUtility.SetDirty(target);
        // 修改后添加
        if (method != null)
        {
            // 记录 Undo
            Undo.RecordObject(target, $"Invoke {methodName}");
        
            // 调用方法
            method.Invoke(target, null);
        
            // 强制刷新
            EditorUtility.SetDirty(target);
            // AssetDatabase.SaveAssets(); // 可选：立即保存变更
            RepaintAllInspectors(); 
            // 关键：强制所有 Inspector 刷新
        }
    }
// 添加刷新方法
    private static void RepaintAllInspectors()
    {
        var windows = Resources.FindObjectsOfTypeAll<EditorWindow>();
        foreach (var window in windows)
        {
            if (window.GetType().Name == "InspectorWindow")
            {
                window.Repaint();
            }
        }
      
    }
  
    private MethodInfo GetCachedMethod(Type type, string methodName)
    {
        string key = $"{type.FullName}.{methodName}";
        
        if (!_methodCache.TryGetValue(key, out var method))
        {
            method = type.GetMethod(methodName, 
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            _methodCache[key] = method;
        }
        
        return method;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var buttonAttribute = (L_AD.ButtonAttribute)attribute;
        bool shouldShow = CheckCondition(property, buttonAttribute);
        return shouldShow ? EditorGUIUtility.singleLineHeight + 4 : 0;
    }
}
#endif

