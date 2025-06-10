#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ConditionalFieldAttribute))]
public class ConditionalFieldDrawer : PropertyDrawer
{
    private Dictionary<string, SerializedProperty> _propertyCache = new Dictionary<string, SerializedProperty>();

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var attribute = (ConditionalFieldAttribute)this.attribute;
        bool shouldShow = ShouldShow(property, attribute);
        
        if(shouldShow)
            EditorGUI.PropertyField(position, property, label, true);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var attribute = (ConditionalFieldAttribute)this.attribute;
        bool shouldShow = ShouldShow(property, attribute);
        return shouldShow ? EditorGUI.GetPropertyHeight(property) : 0;
    }

    public bool ShouldShow(SerializedProperty property, ConditionalFieldAttribute attribute)
    {
        // bool finalResult = attribute.Logic switch
        // {
        //     ConditionalLogic.All => true,
        //     ConditionalLogic.Any => false,
        //     ConditionalLogic.None => true,
        //     _ => true
        // };
        bool finalResult;
        switch (attribute.Logic)
        {
            case ConditionalLogic.All:
                finalResult = true;
                break;
            case ConditionalLogic.Any:
                finalResult = false;
                break;
            case ConditionalLogic.None:
                finalResult = true;
                break;
            default:
                finalResult = true;
                break;
        }

        string parentPath = GetParentPropertyPath(property);

        for (int i = 0; i < attribute.ConditionPaths.Length; i++)
        {
            string fullConditionPath = BuildFullConditionPath(parentPath, attribute.ConditionPaths[i]);
            SerializedProperty conditionProperty = GetCachedProperty(property.serializedObject, fullConditionPath);

            if (conditionProperty == null)
            {
                Debug.LogWarning($"找不到条件字段: {fullConditionPath}");
                continue;
            }

            object conditionValue = GetPropertyValue(conditionProperty);
            object targetValue = attribute.TargetValues[i];
            bool currentResult = CheckSingleCondition(conditionValue, targetValue);

            // finalResult = attribute.Logic switch
            // {
            //     ConditionalLogic.All => finalResult && currentResult,
            //     ConditionalLogic.Any => finalResult || currentResult,
            //     ConditionalLogic.None => finalResult && !currentResult,
            //     _ => finalResult
            // };
            switch (attribute.Logic)
            {
                case ConditionalLogic.All:
                    finalResult = finalResult && currentResult;
                    break;
                case ConditionalLogic.Any:
                    finalResult = finalResult || currentResult;
                    break;
                case ConditionalLogic.None:
                    finalResult = finalResult && !currentResult;
                    break;
                default:
                    finalResult = finalResult;
                    break;
            }
        }

        return finalResult;
    }

    // 新增：强制刷新所有Inspector窗口
    private static void ForceRefreshInspectors()
    {
        var inspectorType = Type.GetType("UnityEditor.InspectorWindow, UnityEditor");
        var inspectors = Resources.FindObjectsOfTypeAll(inspectorType);
        foreach (EditorWindow window in inspectors) window.Repaint();
        EditorApplication.QueuePlayerLoopUpdate();
    }
// 新增方法：获取父级路径
    private string GetParentPropertyPath(SerializedProperty property)
    {
        string path = property.propertyPath;
        int lastDot = path.LastIndexOf('.');
        return lastDot > 0 ? path.Substring(0, lastDot) : "";
    }

// 新增方法：构建完整条件路径
    private string BuildFullConditionPath(string parentPath, string conditionPath)
    {
        // 处理绝对路径（以"."开头）
        if (conditionPath.StartsWith("."))
        {
            return conditionPath.Substring(1);
        }

        // 拼接父级路径
        return string.IsNullOrEmpty(parentPath) 
            ? conditionPath 
            : $"{parentPath}.{conditionPath}";
    }

    private SerializedProperty GetCachedProperty(SerializedObject obj, string path)
    {
        if (!_propertyCache.TryGetValue(path, out var prop))
        {
            prop = obj.FindProperty(path);
            _propertyCache[path] = prop;
        }
        return prop;
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

    private bool CheckSingleCondition(object conditionValue, object targetValue)
    {
        // 处理空值条件（布尔类型）
        // if (targetValue == null)
        // {
        //     return conditionValue switch
        //     {
        //         bool b => b, // 布尔字段：返回其实际值
        //         UnityEngine.Object obj => obj != null,// 对象引用：检查是否非空
        //         _ => true// 其他类型：始终显示
        //     };
        // }
        if (targetValue == null)
        {
            if (conditionValue is bool b)
            {
                return b;
            }
            else if (conditionValue is UnityEngine.Object obj)
            {
                return obj != null;
            }
            else
            {
                return true;
            }
        }


        // 枚举处理
        if (targetValue is Enum)
            targetValue = Convert.ToInt32(targetValue);

        // 数值类型兼容
        if (conditionValue is IConvertible c1 && targetValue is IConvertible c2)
        {
            try
            {
                return Convert.ToDouble(c1) == Convert.ToDouble(c2);
            }
            catch { /* 忽略转换错误 */ }
        }

        // 字符串处理
        if (targetValue is string str)
            return (conditionValue?.ToString() ?? "") == str;

        // 对象引用处理
        if (targetValue is UnityEngine.Object)
            return conditionValue == targetValue;

        return Equals(conditionValue, targetValue);
    }
}
#endif