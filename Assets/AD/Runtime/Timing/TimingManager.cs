using System;
using System.Collections.Generic;
using System.Linq;
using AD.Tools;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AD
{
    /// <summary>
    /// 负责处理定时的附加选项
    /// </summary>
    public class TimingManager : SingletonMonoBase<TimingManager>, IADInit
    {
        public Dictionary<TimingComponentType, TimingComponent> Components
        {
            get
            {
                return ADUtils.GetEnumTypeBind<TimingComponentType>()
                    .ToDictionary(
                        pair => pair.Key,
                        pair => (TimingComponent) gameObject.KeepOne(pair.Value));
            }
        }

        public void Init()
        {
            var components = Components;
            foreach (var component in components) component.Value.Init();
            foreach (var component in components) component.Value.Reset();
            ADManager.Instance.Show(AdSourceType.SetupTiming);
        }

        public void StartTiming(TimingComponentType type)
        {
            if (Components.TryGetValue(type, out var component))
            {
                component.StartTiming();
            }
        }

        private void Reset()
        {
            var _ = Components;
        }
    }
}