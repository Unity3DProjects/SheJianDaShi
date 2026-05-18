using System;
using System.Collections.Generic;
using AD.NodeTree;

namespace AD.NodeTree
{
    [Serializable]
    public class Value<T> : Tree.Value
    {
        public T value;

        private T defaultValue;
        private bool hasDefaultValue;
        public sealed override bool Resettable => hasDefaultValue;

        protected internal T DefaultValue
        {
            set
            {
                defaultValue = value;
                hasDefaultValue = true;
            }
        }

        public sealed override void Reset()
        {
            if (Resettable)
                value = defaultValue;
        }

        public override void Load(Tree.Value other)
        {
            if (other is Value<T> t)
                value = t.value;
        }

        protected bool Equals(Value<T> other)
        {
            return EqualityComparer<T>.Default.Equals(value, other.value) && EqualityComparer<T>.Default.Equals(defaultValue, other.defaultValue) && hasDefaultValue == other.hasDefaultValue;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Value<T>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = EqualityComparer<T>.Default.GetHashCode(value);
                hashCode = (hashCode * 397) ^ EqualityComparer<T>.Default.GetHashCode(defaultValue);
                hashCode = (hashCode * 397) ^ hasDefaultValue.GetHashCode();
                return hashCode;
            }
        }
    }

    [Serializable]
    public class ArrayValue<T> : Value<T[]>
    {
    }

    [Serializable]
    public class StringValue : Value<string>
    {
    }

    [Serializable]
    public class IntValue : Value<int>
    {
    }

    [Serializable]
    public class FloatValue : Value<float>
    {
    }

    [Serializable]
    public class StringArrayValue : ArrayValue<string>
    {
    }

    [Serializable]
    public class IntArrayValue : ArrayValue<int>
    {
    }

    [Serializable]
    public class FloatArrayValue : ArrayValue<float>
    {
    }
}