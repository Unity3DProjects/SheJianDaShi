using System;

namespace AD.Runtime.Attributes
{
    public class GamePlatformAttribute : Attribute
    {
        public Type Type;

        public string Desc;

        public GamePlatformAttribute(Type type, string desc)
        {
            Type = type;
            Desc = desc;
        }
    }
}