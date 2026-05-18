using System.Reflection;
using AD.Attributes;
using UnityEditor;

namespace AD.Editor
{
    internal class AdParamBindDrawer : BasePropertyDrawer
    {
        public override bool CanDisplay(SerializedProperty property)
        {
            return !property.TryGetAttribute<AdParamBindAttribute>(out var attribute)
                   || (attribute.Type & ADEditorCache
                       .OperationCache[ADManager.Instance.OperationType]
                       .AllowedAdType(ADManager.Instance.Platform)) != 0;
        }
    }
}