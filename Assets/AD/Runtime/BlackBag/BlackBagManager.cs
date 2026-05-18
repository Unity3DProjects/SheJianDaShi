using System.Reflection;
using System.Threading.Tasks;
using AD.BlackBag.Shield;
using AD.Operation;
using AD.Tools;
#if UNITY_EDITOR
using System.Collections;
using UnityEditor;
#endif
using UnityEngine;

namespace AD.BlackBag
{
    public class BlackBagManager : Singleton<BlackBagManager>, IADInit<BaseOperation>
    {
        private EPlatform Platform => ADManager.Instance.Platform;

        private BaseOperation Operation { get; set; }

        public bool NeedPrivacy => Operation.NeedPrivacy(Platform);
        public bool NeedHealth => Operation.NeedHealth(Platform);
        public bool NeedBannerAD => Operation.NeedBannerAD(Platform);
        public bool NeedCustomAD => Operation.NeedCustomAD(Platform);
        public bool NeedBlackCustomAD => Operation.NeedBlackCustomAD(Platform);
        public bool NeedInterstitialAD => Operation.NeedInterstitialAD(Platform);
        public bool NeedBlackInterstitialAD => Operation.NeedBlackInterstitialAD(Platform);

        private BoolArray
            white = new BoolArray() {true},
            black = new BoolArray(),
            box = new BoolArray();

        public bool CanShowWhiteAD
        {
            get => white.Count != 0 && white.AndResult;
            internal set => white.Add(value);
        }

        public bool CanShowBlackAD
        {
            get => black.Count != 0 && black.AndResult;
            internal set => black.Add(value);
        }

        public bool CanShowBox
        {
            get => box.Count != 0 && box.AndResult;
            internal set => box.Add(value);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init(BaseOperation operation)
        {
            Operation = operation;

#if UNITY_EDITOR
            if (EditorApplication.isPlaying)
            {
                var fieldInfo = typeof(OperationType).GetField(ADManager.Instance.OperationType.ToString());
                var nameAtt = fieldInfo.GetAttr<InspectorNameAttribute>();
                if (nameAtt != null)
                    $"当前运营:{nameAtt.displayName}".Log();
                ADManager.Instance.StartCoroutine(DelayLog());
            }
#endif
        }


#if UNITY_EDITOR
        private IEnumerator DelayLog()
        {
            if (EditorApplication.isPlaying)
            {
                yield return new WaitForSecondsRealtime(3);
                $"当前隐私协议开启状态: {NeedPrivacy}".Log();
                $"黑包广告状态: {CanShowBlackAD}".Log();
                $"黑包宝箱状态: {CanShowBox}".Log();
            }
        }
#endif
    }
}