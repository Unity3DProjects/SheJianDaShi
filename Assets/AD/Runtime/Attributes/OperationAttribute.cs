using System;

namespace AD.Runtime.Attributes
{
    public class OperationAttribute : Attribute
    {
        public Type ImplementationType;

        public OperationAttribute(Type implementationType)
        {
            ImplementationType = implementationType;
        }
    }
}
