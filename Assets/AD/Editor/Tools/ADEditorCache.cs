using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AD.AdAdapter;
using AD.Attributes;
using AD.Decorator;
using AD.Operation;
using AD.Tools;
using UnityEngine;

namespace AD.Editor
{
    internal static class ADEditorCache
    {
        public static IDictionary<EPlatform, IAdAdapter> AdAdapterCache
        {
            get
            {
                InitAdAdapter();
                return adAdapterCache;
            }
        }

        private static IDictionary<EPlatform, IAdAdapter> adAdapterCache;

        private static void InitAdAdapter()
        {
            if (adAdapterCache == null)
            {
                
                adAdapterCache = ADUtils.GetEnumTypeBind<EPlatform>()
                    .ToDictionary(e => e.Key, e => (IAdAdapter) Activator.CreateInstance(e.Value));
            }
        }
        public static IDictionary<OperationType, BaseOperation> OperationCache
        {
            get
            {
                InitOp();
                return opCache;
            }
        }

        private static IDictionary<OperationType, BaseOperation> opCache;

        private static void InitOp()
        {
            if (opCache == null)
            {
                opCache = ADUtils.GetOperationBind()
                    .ToDictionary(e => e.Key, e => (BaseOperation) Activator.CreateInstance(e.Value));
            }
        }

        public static List<ShieldTypeData> ShieldTypeDataCache
        {
            get
            {
                InitST();
                return stCache;
            }
        }

        private static List<ShieldTypeData> stCache;

        private static void InitST()
        {
            if (stCache == null)
            {
                stCache = new List<ShieldTypeData>();
                var type = typeof(EShieldType);
                foreach (var shieldType in Enum.GetValues(type).OfType<EShieldType>())
                {
                    var fieldInfo = type.GetField(shieldType.ToString());
                    var displayName = fieldInfo.IsDefined(typeof(InspectorNameAttribute))
                        ? fieldInfo.GetAttr<InspectorNameAttribute>().displayName
                        : shieldType.ToString();
                    stCache.Add(new ShieldTypeData()
                    {
                        type = shieldType,
                        name = displayName,
                        value = (int) shieldType,
                    });
                }
            }
        }

        public static int OverrideableOnlyOneMask
        {
            get
            {
                InitOST();
                return overrideableOnlyOneMask;
            }
        }


        private static int overrideableOnlyOneMask;

        public static int OverrideableMask
        {
            get
            {
                InitOST();
                return overrideableMask;
            }
        }

        private static int overrideableMask;

        public static List<OverrideableShieldTypeData> OverrideableShieldTypeDataCache
        {
            get
            {
                InitOST();
                return ostCache;
            }
        }

        private static List<OverrideableShieldTypeData> ostCache;

        private static void InitOST()
        {
            if (ostCache == null)
            {
                ostCache = new List<OverrideableShieldTypeData>();
                var type = typeof(EShieldType);
                foreach (var shieldType in Enum.GetValues(type).OfType<EShieldType>())
                {
                    var fieldInfo = type.GetField(shieldType.ToString());
                    if (fieldInfo.IsDefined(typeof(OverrideableShieldTypeAttribute)))
                    {
                        var displayName = fieldInfo.IsDefined(typeof(InspectorNameAttribute))
                            ? fieldInfo.GetAttr<InspectorNameAttribute>().displayName
                            : shieldType.ToString();
                        var onlyOne = fieldInfo.GetAttr<OverrideableShieldTypeAttribute>().OnlyOne;
                        ostCache.Add(new OverrideableShieldTypeData()
                        {
                            type = shieldType,
                            name = displayName,
                            onlyOne = onlyOne,
                            value = (int) shieldType,
                        });
                        overrideableMask |= (int) shieldType;
                        if (onlyOne) overrideableOnlyOneMask |= (int) shieldType;
                    }
                }
            }
        }


        public static BiMap<string, Type> DecoratorEnumCache
        {
            get
            {
                InitDecoratorEnum();
                return decoratorEnumCache;
            }
        }

        private static BiMap<string, Type> decoratorEnumCache;

        private static void InitDecoratorEnum()
        {
            if (decoratorEnumCache == null)
            {
                decoratorEnumCache = new BiMap<string, Type>();
                foreach (var type in ADUtils.GetSubClassOf<IADParamDecorator>()
                    .Where(e => e.IsDefined(typeof(ADParamDecoratorEnumTypeAttribute), false)))
                {
                    var name = type.GetAttr<ADParamDecoratorEnumTypeAttribute>().Name;
                    if (!decoratorEnumCache.ContainsKey(name)) decoratorEnumCache.Add(name, type);
                }
            }
        }
    }

    #region class

    public class ShieldTypeData
    {
        public EShieldType type;
        public string name;
        public int value;
    }

    public class OverrideableShieldTypeData
    {
        public EShieldType type;
        public string name;
        public bool onlyOne;
        public int value;
    }

    #endregion
}