using System;

namespace AD.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Struct)]
    public class TypeBindAttribute : Attribute
    {
        public Type Type { get; }

        public TypeBindAttribute(Type type)
        {
            Type = type;
        }
    }
}