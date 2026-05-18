using System;

namespace AD.NodeTree
{
    public class SetupTiming : OnceInitNode
    {
        #region impl

        public static SetupTiming Box(bool enabled = true, int firstTime = 180, int repeatTime = 120)
        {
            return new SetupTiming(TimingComponentType.Box, new TimingData()
            {
                enabled = enabled,
                firstTime = firstTime,
                repeatTime = repeatTime
            });
        }

        public static SetupTiming AD(bool enabled = true, int firstTime = 45, int repeatTime = 45)
        {
            return new SetupTiming(TimingComponentType.Ad, new TimingData()
            {
                enabled = enabled,
                firstTime = firstTime,
                repeatTime = repeatTime
            });
        }

        public static SetupTiming AddDesk(bool enabled = true, int firstTime = 30, int repeatTime = 30)
        {
            return new SetupTiming(TimingComponentType.AddDesk, new TimingData()
            {
                enabled = enabled,
                firstTime = firstTime,
                repeatTime = repeatTime
            });
        }

        public static SetupTiming AddDeskPanel(bool enabled = true, int firstTime = 35, int repeatTime = 35)
        {
            return new SetupTiming(TimingComponentType.AddDeskPanel, new TimingData()
            {
                enabled = enabled,
                firstTime = firstTime,
                repeatTime = repeatTime
            });
        }

        public static SetupTiming Eye(bool enabled = true, int firstTime = 180, int repeatTime = 120)
        {
            return new SetupTiming(TimingComponentType.Eye, new TimingData()
            {
                enabled = enabled,
                firstTime = firstTime,
                repeatTime = repeatTime
            });
        }

        #endregion

        private TimingComponentType componentType;
        private TimingData timingValue;

        public SetupTiming(TimingComponentType componentType, TimingData timingValue)
        {
            this.componentType = componentType;
            this.timingValue = timingValue;
        }

        public override void InitTree(Tree tree)
        {
            tree.Data.Add(
                new Tree.Key(componentType.ToString(),
                    componentType.TryGetInspectorName(out var name) ? name : componentType.ToString()),
                timingValue);
        }

        protected override void OnCall(AdSourceType type)
        {
            if (type == AdSourceType.SetupTiming)
            {
                if (TimingManager.Instance.Components.TryGetValue(componentType, out var component))
                {
                    component.SetState(timingValue.enabled)
                        .SetFirstTime(timingValue.firstTime)
                        .SetRepeatTime(timingValue.repeatTime);
                    TimingManager.Instance.StartTiming(componentType);
                }

                Complete(true);
            }
        }

        protected override void OnceInit()
        {
        }
    }

    [Serializable]
    public class TimingData : Tree.Value
    {
        public bool enabled;
        public int firstTime, repeatTime;

        public override void Load(Tree.Value other)
        {
            if (other is TimingData t)
            {
                enabled = t.enabled;
                firstTime = t.firstTime;
                repeatTime = t.repeatTime;
            }
        }
    }
}