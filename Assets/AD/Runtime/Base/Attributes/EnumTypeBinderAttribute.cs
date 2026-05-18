using System;

namespace AD.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumTypeBinderAttribute : Attribute
    {
        public Type Type { get; }

        public EnumTypeBinderAttribute(Type type)
        {
            Type = type;
        }
    }
}