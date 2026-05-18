using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AD.Attributes;
using AD.Tools;
using UnityEngine;

namespace AD.Decorator
{
    [Serializable]
    public class ADParamDecoratorManager : IADInit<IADParam>
    {
        [SerializeField] public DecoratorDictionary decorators = new DecoratorDictionary();

        public void Init(IADParam param)
        {
        }

        public void Reset()
        {
            decorators.Clear();
        }

        [Serializable]
        public class DecoratorDictionary : SerializableRefDictionary<string, List<IADParamDecorator>, string, string>
        {
            protected override string SerializeKey(string key)
            {
                return key;
            }

            protected override bool TryDeserializeKey(string keySave, out string key)
            {
                key = keySave;
                return true;
            }

            private const char Split = '#';
            private const char Mark = '\\';
            private static readonly string SplitStr = Split.ToString();
            private static readonly string MarkStr = Mark.ToString();
            private static readonly string RepSplitStr = MarkStr + SplitStr;

            protected override string SerializeValue(List<IADParamDecorator> value)
            {
                return string.Join(SplitStr,
                    value
                        .Select(Serialize)
                        .Select(JsonUtility.ToJson)
                        .Select(e => e.Replace(SplitStr, RepSplitStr)));
            }

            protected override bool TryDeserializeValue(string valueSave, out List<IADParamDecorator> value)
            {
                value = new List<IADParamDecorator>();
                int length = valueSave.Length, preSplit = -1, i = 0;
                do
                {
                    if (i == length || valueSave[i] == Split && i != 0 && valueSave[i - 1] != Mark)
                    {
                        if (TryDeserialize(
                            JsonUtility.FromJson<SaveData>(
                                valueSave
                                    .Substring(preSplit + 1, i - (preSplit + 1))
                                    .Replace(RepSplitStr, SplitStr)),
                            out var decorator))
                            value.Add(decorator);
                        preSplit = i;
                    }

                    i++;
                } while (preSplit != length);

                return true;
            }

            [Serializable]
            private class SaveData
            {
                public string asm;
                public string type;
                public string data;
            }

            private static IDictionary<string, Assembly> cache;

            private bool TryDeserialize(SaveData data, out IADParamDecorator decorator)
            {
                decorator = default;
                if (data == null) return false;
                if (cache == null)
                {
                    cache = AppDomain.CurrentDomain.GetAssemblies().ToDictionary(e => e.GetName().Name);
                }

                if (!cache.TryGetValue(data.asm, out var assembly)) return false;
                Type type;
                try
                {
                    type = assembly.GetType(data.type);
                }
                catch
                {
                    type = null;
                }

                if (type == null) return false;
                if (JsonUtility.FromJson(data.data, type) is IADParamDecorator v)
                {
                    decorator = v;
                    return true;
                }

                try
                {
                    decorator = (IADParamDecorator) Activator.CreateInstance(type);
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            private SaveData Serialize(IADParamDecorator decorator)
            {
                return new SaveData()
                {
                    asm = decorator.GetType().Assembly.GetName().Name,
                    type = decorator.GetType().FullName,
                    data = JsonUtility.ToJson(decorator)
                };
            }

            public override string ToString()
            {
                var builder = new StringBuilder();
                builder.Append("{\n");
                foreach (var pair in this)
                {
                    builder.Append("\t").Append(pair).Append("\n");
                }

                return builder.Append("}").ToString();
            }
        }
    }

    public static class DecoratorExtensions
    {
        public static bool Pass(this IADParam param, string field)
        {
            if (param == null || field == null) return true;
            var decorator = param.Decorator;
            if (decorator == null) return true;
            var fieldInfo = ReflectorManager.ReflectInstanceField(param.GetType(), field);
            if (fieldInfo == null) return true;
            var attr = fieldInfo.GetAttr<DecoratableAttribute>();
            if (attr == null) return true;
            if (!decorator.decorators.TryGetValue(field, out var list)) return true;
            return list.All(e => e.Pass);
        }
    }
}