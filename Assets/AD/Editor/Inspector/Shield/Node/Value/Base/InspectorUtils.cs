using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AD.Attributes;
using AD.BlackBag.Shield;
using UnityEngine;

namespace AD.Editor
{
    internal static class InspectorUtils
    {
        // private static IDictionary<string, string> typeFullName2DisplayName;

        // internal static string GetDisplayName<T>()
        //     where T : IShield
        //     => GetDisplayName(typeof(T));

        // internal static string GetDisplayName(Type type) => GetDisplayName(type.FullName);

        // internal static string GetDisplayName(string typeFullName)
        // {
        //     if (typeFullName2DisplayName == null)
        //     {
        //         typeFullName2DisplayName = new Dictionary<string, string>();
        //         var type = typeof(EShieldType);
        //         foreach (var shieldType in Enum.GetValues(type).OfType<EShieldType>())
        //         {
        //             var fieldInfo = type.GetField(shieldType.ToString());
        //             if (!fieldInfo.IsDefined(typeof(EnumTypeBinderAttribute)))
        //                 continue;
        //             var displayName = fieldInfo.IsDefined(typeof(InspectorNameAttribute))
        //                 ? fieldInfo.GetAttr<InspectorNameAttribute>().displayName
        //                 : shieldType.ToString();
        //             typeFullName2DisplayName.Add(
        //                 fieldInfo.GetAttr<EnumTypeBinderAttribute>()
        //                     .Type
        //                     .GetManagedTypeFullName(),
        //                 displayName);
        //         }
        //     }
        //
        //     if (typeFullName2DisplayName.TryGetValue(typeFullName, out var name)) return name;
        //     var split = typeFullName.Split('.');
        //     name = split[split.Length - 1];
        //     return name;
        // }

        internal static string GetManagedTypeFullName(this Type type) =>
            $"{type.Assembly.GetName().Name} {type.FullName.Replace("+","/")}";

        // private static IDictionary<string, Type> managedTypeFullName2ShieldType;

        // internal static IShield CreateShieldByManagedTypeFullName(string managedTypeFullName)
        // {
        //     if (managedTypeFullName2ShieldType == null)
        //     {
        //         managedTypeFullName2ShieldType = new Dictionary<string, Type>();
        //         foreach (var type in ADUtils.GetSubClassOf<IShield>())
        //         {
        //             managedTypeFullName2ShieldType.Add(type.GetManagedTypeFullName(), type);
        //         }
        //     }
        //
        //     return (IShield) Activator.CreateInstance(managedTypeFullName2ShieldType[managedTypeFullName]);
        // }
    }
}