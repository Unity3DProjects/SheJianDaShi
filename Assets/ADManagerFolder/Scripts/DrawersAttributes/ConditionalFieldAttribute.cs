using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ConditionalLogic { All, Any, None }
public class ConditionalFieldAttribute : PropertyAttribute
{
    public string[] ConditionPaths { get; }
    public object[] TargetValues { get; }
    public ConditionalLogic Logic { get; }

    // 多条件构造函数
    public ConditionalFieldAttribute(
        ConditionalLogic logic = ConditionalLogic.All,
        params object[] conditions
    )
    {
        Logic = logic;
        ConditionPaths = new string[conditions.Length / 2];
        TargetValues = new object[conditions.Length / 2];

        for (int i = 0; i < conditions.Length; i += 2)
        {
            ConditionPaths[i/2] = conditions[i] as string;
            TargetValues[i/2] = conditions[i+1];
        }
    }

    // 单条件兼容构造函数
    /// <param name="targetValue">目标值，留空时：对bool字段默认为true，对Object字段检查非空</param>
    public ConditionalFieldAttribute(string conditionPath, object targetValue = null)
    {
        Logic = ConditionalLogic.All;
        ConditionPaths = new[] { conditionPath };
        TargetValues = new[] { targetValue };
    }
}