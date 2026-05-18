using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AD;
using AD.AdAdapter;
using AD.Attributes;
using AD.BlackBag;
using AD.Operation;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public static class ADUtils
{
    #region timing

    public static TimingComponent Enable(this TimingComponent component) => component.SetState(true);
    public static TimingComponent Disable(this TimingComponent component) => component.SetState(false);

    public static TimingComponent SetState(this TimingComponent component, bool enabled)
    {
        component.enabled = enabled;
        return component;
    }

    public static TimingComponent SetFirstTime(this TimingComponent component, int firstTime)
    {
        component.firstTime = firstTime;
        return component;
    }

    public static TimingComponent SetRepeatTime(this TimingComponent component, int repeatTime)
    {
        component.repeatTime = repeatTime;
        return component;
    }

    #endregion

    #region collection

    public static TValue GetOrInit<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
        TKey key,
        TValue defaultValue)
    {
        if (dictionary.TryGetValue(key, out var result)) return result;
        dictionary.Add(key, defaultValue);
        return defaultValue;
    }

    public static void AddItem<TKey, TValue>(this IDictionary<TKey, List<TValue>> dictionary, TKey key,
        TValue value)
    {
        if (!dictionary.TryGetValue(key, out var list))
            dictionary.Add(key, list = new List<TValue>());
        list.Add(value);
    }

    public static void AddItem<TKey, TValue, TCollection>(this IDictionary<TKey, TCollection> dictionary, TKey key,
        TValue value)
        where TCollection : IList<TValue>, new()
    {
        if (!dictionary.TryGetValue(key, out var list))
            dictionary.Add(key, list = new TCollection());
        list.Add(value);
    }

    public static void RemoveItem<TKey, TValue>(this IDictionary<TKey, List<TValue>> dictionary, TKey key,
        TValue value)
    {
        if (!dictionary.TryGetValue(key, out var list))
            return;
        list.Remove(value);
        if (list.Count == 0)
            dictionary.Remove(key);
    }

    #endregion

    #region EnumTypeBinder

    public static IDictionary<OperationType, Type> GetOperationBind()
    {
        return GetEnumTypeBind<OperationType>();
    }

    public static IDictionary<TEnum, Type> GetEnumTypeBind<TEnum>()
        where TEnum : Enum
    {
        var type = typeof(TEnum);
        var names = Enum.GetNames(type);
        var dictionary = new Dictionary<TEnum, Type>();
        foreach (var name in names)
        {
            var fieldInfo = type.GetField(name);
            if (fieldInfo.IsDefined(typeof(EnumTypeBinderAttribute)))
            {
                dictionary.Add((TEnum) Enum.Parse(type, name),
                    fieldInfo.GetAttr<EnumTypeBinderAttribute>().Type);
            }
        }

        return dictionary;
    }

    #endregion

    #region subclass

    public static List<Type> GetSubClassOf<T>()
    {
        var list = new List<Type>();
        foreach (var assembly in AppDomain
            .CurrentDomain
            .GetAssemblies())
        {
            try
            {
                foreach (var type in assembly
                    .GetTypes()
                    .Where(e => typeof(T).IsAssignableFrom(e) && !e.IsAbstract))
                    list.Add(type);
            }
            catch
            {
            }
        }

        return list;
    }

    public static List<T> GetSubClassInstanceOf<T>()
    {
        var list = new List<T>();
        foreach (var assembly in AppDomain
            .CurrentDomain
            .GetAssemblies())
        {
            try
            {
                foreach (var type in assembly
                    .GetTypes()
                    .Where(e => typeof(T).IsAssignableFrom(e) && !e.IsAbstract))
                {
                    if (Activator.CreateInstance(type) is T t)
                        list.Add(t);
                }
            }
            catch
            {
            }
        }

        return list;
    }

    #endregion

    #region attr

    internal static bool TryGetInspectorName<TEnum>(this TEnum t, out string name)
        where TEnum : Enum
    {
        var type = typeof(TEnum);
        var fieldInfo = type.GetField(t.ToString());
        if (fieldInfo.IsDefined(typeof(EnumTypeBinderAttribute)))
        {
            name = fieldInfo.GetAttr<InspectorNameAttribute>().displayName;
            return true;
        }

        name = default;
        return false;
    }

    internal static Type GetTypeBind(this Type type)
    {
        if (type.IsDefined(typeof(TypeBindAttribute)))
            return type.GetAttr<TypeBindAttribute>().Type;
        return null;
    }

    internal static Type GetTypeBind<T>()
    {
        return GetTypeBind(typeof(T));
    }


    internal static bool TryGetAdAdapterType(this EPlatform platform, out Type adAdapterType)
    {
        var fieldInfo = typeof(EPlatform).GetField(platform.ToString());
        var attr = fieldInfo.GetAttr<EnumTypeBinderAttribute>();
        if (attr == null)
        {
            adAdapterType = default;
            return false;
        }

        adAdapterType = attr.Type;
        return attr.Type != null;
    }

    internal static bool TryGetAdParamType(this EPlatform platform, out Type adParamType)
    {
        adParamType = default;
        if (!platform.TryGetAdAdapterType(out var adAdapterType))
            return false;

        var type = adAdapterType;
        while (type != null && type != typeof(object) && !type.IsInterface)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(BaseAdAdapter<>))
            {
                adParamType = type.GetGenericArguments()[0];
                break;
            }

            type = type.BaseType;
        }

        return adParamType != null;
    }

    internal static T GetAttr<T>(this ParameterInfo info) where T : Attribute
    {
        return CustomAttributeExtensions.GetCustomAttribute<T>(info);
    }

    internal static T GetAttr<T>(this FieldInfo info) where T : Attribute
    {
        return CustomAttributeExtensions.GetCustomAttribute<T>(info);
    }

    internal static T GetAttr<T>(this PropertyInfo info) where T : Attribute
    {
        return CustomAttributeExtensions.GetCustomAttribute<T>(info);
    }

    internal static T GetAttr<T>(this MethodInfo info) where T : Attribute
    {
        return CustomAttributeExtensions.GetCustomAttribute<T>(info);
    }

    internal static T GetAttr<T>(this Type info) where T : Attribute
    {
        return CustomAttributeExtensions.GetCustomAttribute<T>(info);
    }

    #endregion

    #region msg

    public static void SendMsgToADManager(string receiverKey, string msg)
    {
        GameObject.Find(nameof(ADManager)).SendMessage(nameof(ADManager.Receiver), JsonUtility.ToJson(
            new ADManager.ReceiveMsg()
            {
                key = receiverKey,
                data = msg
            }));
    }

    #endregion

    #region unity

    public static Component KeepOne(this GameObject go, Type componentType)
    {
        var components = go.GetComponents(componentType);
        if (components.Length == 0) return go.AddComponent(componentType);
        if (components.Length > 1)
        {
            for (var i = 1; i < components.Length; i++)
            {
                if (Application.isPlaying)
                    Object.Destroy(components[i]);
                else Object.DestroyImmediate(components[i]);
            }
        }

        return components[0];
    }

    public static void InitButton(this Button button, params UnityAction[] actions)
    {
        button.onClick = new Button.ButtonClickedEvent();
        foreach (var action in actions)
        {
            button.onClick.AddListener(action);
        }
    }

    public static Color Lerp(Color start, Color end, float percentage)
    {
        return new Color(
            Mathf.Lerp(start.r, end.r, percentage),
            Mathf.Lerp(start.g, end.g, percentage),
            Mathf.Lerp(start.b, end.b, percentage),
            Mathf.Lerp(start.a, end.a, percentage)
        );
    }

    #endregion

    #region ad

    public static bool NeedPrivacy(this BaseOperation operation, EPlatform platform)
    {
        return (operation.AllowedInstanceType(platform) & AdInstanceType.Privacy) != 0;
    }

    public static bool NeedHealth(this BaseOperation operation, EPlatform platform)
    {
        return (operation.AllowedInstanceType(platform) & AdInstanceType.Health) != 0;
    }

    public static bool NeedBannerAD(this BaseOperation operation, EPlatform platform)
    {
        return (operation.AllowedAdType(platform) & AdType.Banner) != 0;
    }

    public static bool NeedCustomAD(this BaseOperation operation, EPlatform platform)
    {
        return (operation.AllowedAdType(platform) & AdType.Custom) != 0;
    }

    public static bool NeedInterstitialAD(this BaseOperation operation, EPlatform platform)
    {
        return (operation.AllowedAdType(platform) & AdType.Interstitial) != 0;
    }

    public static bool NeedBlackCustomAD(this BaseOperation operation, EPlatform platform)
    {
        return (operation.AllowedAdType(platform) & AdType.BlackCustom) != 0;
    }

    public static bool NeedBlackInterstitialAD(this BaseOperation operation, EPlatform platform)
    {
        return (operation.AllowedAdType(platform) & AdType.BlackInterstitial) != 0;
    }

    #endregion

    public static bool IsWhite(this AdSourceType type)
    {
        switch (type)
        {
            case AdSourceType.Pause:
            case AdSourceType.Setting:
            case AdSourceType.GameEnd:
            case AdSourceType.White:
                return true;
            default:
                return false;
        }
    }

    public static bool IsBox(this AdSourceType type)
    {
        switch (type)
        {
            case AdSourceType.Box:
            case AdSourceType.TimingBox:
            case AdSourceType.CloseableBox:
                return true;
            default:
                return false;
        }
    }

    public static bool IsBlack(this AdSourceType type)
    {
        switch (type)
        {
            case AdSourceType.Callback:
            case AdSourceType.Black:
            case AdSourceType.TimingAd:
            case AdSourceType.Other:
                return true;
            default:
                return false;
        }
    }

    public static bool HasBanner(this AdSourceType type)
    {
        return type == AdSourceType.Banner || type.IsBlack();
    }

    public static bool HasCustom(this AdSourceType type)
    {
        return type == AdSourceType.Custom || type.IsWhite() || type.IsBlack();
    }
}