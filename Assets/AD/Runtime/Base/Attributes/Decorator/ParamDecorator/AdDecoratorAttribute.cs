using System;

namespace AD.Attributes
{
    public class AdDecoratorAttribute : BaseDecoratorAttribute
    {
        public float Space { get; set; } = -1;
        public string Name { get; set; }
        public bool Display { get; set; } = true;
        public bool Enable { get; set; } = true;
    }
}