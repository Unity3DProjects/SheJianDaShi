using System;

namespace AD.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class OverrideableShieldTypeAttribute : Attribute
    {
        public bool OnlyOne { get; }

        public OverrideableShieldTypeAttribute(bool onlyOne)
        {
            OnlyOne = onlyOne;
        }

        public OverrideableShieldTypeAttribute() : this(false)
        {
        }
    }
}