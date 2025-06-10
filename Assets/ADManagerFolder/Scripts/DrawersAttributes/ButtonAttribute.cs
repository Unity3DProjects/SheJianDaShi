using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace L_AD
{
    

public class ButtonAttribute : PropertyAttribute
{
    public string Label { get; }
    public string MethodName { get; }
    public string ConditionPath { get; }
    public object TargetValue { get; }

    public ButtonAttribute(
        string label = null,
        string methodName = null,
        string conditionPath = null,
        object targetValue = null
    ){
        Label = label;
        MethodName = methodName;
        ConditionPath = conditionPath;
        TargetValue = targetValue;
    }
}
}