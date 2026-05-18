using System;
using UnityEngine;

namespace AD.Attributes
{
    public class AdParamBindAttribute : BaseDecoratorAttribute
    {
        public AdType Type { get; }

        public AdParamBindAttribute(AdType type)
        {
            Type = type;
        }
    }
}