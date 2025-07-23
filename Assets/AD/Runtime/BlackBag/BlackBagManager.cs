using System;
using System.Reflection;
using System.Threading.Tasks;
using AD.Runtime.Attributes;
using AD.Runtime.BlackBag.Operation;
using AD.Runtime.BlackBag.Shield;
using AD.Runtime.BlackBag.Shield.Interface;
using AD.Runtime.Tools.Tools;
using UnityEditor;
using UnityEngine;

namespace AD.Runtime.BlackBag
{
    [DefaultExecutionOrder(-1000)]
    public class BlackBagManager : SingletonMonoBase<BlackBagManager>
    {
        private BaseOperation _operation;

        [Header("运营类型")] [SerializeField] private OperationType operationType;

        [Header("广告和宝箱屏蔽状态")] [SerializeField] private bool mCanShowAD;

        [SerializeField] private bool mCanShowBox;

        public EPlatform Platform => ADManager.Instance.Platform;
        public OperationType OperationType => operationType;

        public BaseOperation Operation
        {
            get
            {
                if (_operation == null)
                {
                    Init();
                }

                return _operation;
            }
        }

        /// <summary>
        /// 能否展示黑包广告
        /// </summary>
        public bool CanShowAD => Operation.CheckCanShowAD(Platform);

        /// <summary>
        /// 能否展示宝箱
        /// </summary>
        public bool CanShowBox => Operation.CheckCanShowBox(Platform);

        /// <summary>
        /// 是否需要隐私政策
        /// </summary>
        public bool NeedPrivacy => Operation.NeedPrivacy(Platform);

        public override void Awake()
        {
            Init();
        }

#if UNITY_EDITOR
        private async void Start()
        {
            await Task.Delay(3000);
            var ad = CanShowAD;
            var box = CanShowBox;
            $"当前隐私协议开启状态:{_operation.NeedPrivacy(Platform)}".Log();
            $"黑包广告状态：{ad}".Log();
            $"黑包宝箱状态：{box}".Log();
        }
#endif

        /// <summary>
        /// 初始化
        /// </summary>
        public BaseOperation Init()
        {
            var fieldInfo = typeof(OperationType).GetField(OperationType.ToString());
            var operationAtt = fieldInfo.GetCustomAttribute<OperationAttribute>();
            var nameAtt = fieldInfo.GetCustomAttribute<InspectorNameAttribute>();
            if (operationAtt == null) throw new Exception($"当前策略未添加初始化类型!");
            _operation = Activator.CreateInstance(operationAtt.ImplementationType) as BaseOperation;
            if (_operation == null) throw new Exception($"{nameof(BlackBagManager)}:初始化失败!");
#if UNITY_EDITOR
            if (EditorApplication.isPlaying)
            {
                if (nameAtt != null)
                    $"当前运营:{nameAtt.displayName}".Log();
            }
#endif
            return _operation;
        }

        public void OpenBox()
        {
            LuZong.CanShowBox = true;
        }

        private void AddOperation<T>() where T : MonoBehaviour
        {
            var gos = GetComponents<IShield>();
            foreach (var go in gos)
            {
                go.DestroySelf();
            }

            gameObject.AddComponent<T>();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            EditorApplication.delayCall += () =>
            {
                if (EditorApplication.isPlaying) return;
                try
                {
                    switch (OperationType)
                    {
                        case OperationType.WanBan:
                        case OperationType.WanBianTi:
                            if (!TryGetComponent(typeof(WanBanShield), out var WanBan))
                                AddOperation<WanBanShield>();
                            break;
                        case OperationType.ZhiDianTianXia:
                            if (!TryGetComponent(typeof(ZDTXShield), out var zdtx))
                                AddOperation<ZDTXShield>();
                            break;
                        default:
                            if (!TryGetComponent(typeof(AreaShield), out var AreaShield))
                                AddOperation<AreaShield>();
                            break;
                    }
                }
                catch (Exception e)
                {
                }
            };
        }
#endif
    }
}