using System;

namespace AD.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum)]
    public class ADParamDecoratorEnumTypeAttribute : Attribute
    {
        public string Name { get; }

        public ADParamDecoratorEnumTypeAttribute(string name)
        {
            Name = name;
        }
    }
}