using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionalHeaderFieldAttribute : PropertyAttribute
{
    public string Header { get; }
    public string ConditionPath { get; }
    public object TargetValue { get; }
    public ConditionalLogic Logic { get; }
    public object[] Conditions { get; }

    // 单条件构造函数
    public ConditionalHeaderFieldAttribute(string header, string conditionPath, object targetValue = null)
    {
        Header = header;
        ConditionPath = conditionPath;
        TargetValue = targetValue;
        Logic = ConditionalLogic.All;
        Conditions = new object[] { conditionPath, targetValue };
    }

    // 多条件构造函数
    public ConditionalHeaderFieldAttribute(string header, ConditionalLogic logic = ConditionalLogic.All, params object[] conditions)
    {
        Header = header;
        Logic = logic;
        Conditions = conditions;
    }
}
