using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AD.NodeTree;
using AD.Tools;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Tree = AD.NodeTree.Tree;

namespace AD.BlackBag.Shield
{
    public class ShieldManager : ADRootMono<ShieldManager>, IADInit<ADManager>
    {
        [SerializeField] private TreeDataContainer container = new TreeDataContainer();
        [SerializeField] private TreeDataContainer overrideContainer = new TreeDataContainer();
        internal Tree Rule { get; private set; }

        public TreeDataContainer Container =>
            ADManager.Instance.OverrideShieldType == EShieldType.None
                ? container
                : overrideContainer;

        protected override void OnAwake()
        {
        }

        public void Init(ADManager manager)
        {
            if (manager.OverrideShieldType == EShieldType.None)
            {
                Rule = new Tree(manager.Operation.CreateRule(manager.Platform));
                Rule.Init(container);
            }
            else
            {
                Rule = new Tree(Build(manager.OverrideShieldType));
                Rule.Init(overrideContainer);
            }
        }

        internal static INode Build(EShieldType type)
        {
            var dictionary = ADUtils.GetEnumTypeBind<EShieldType>();
            var shieldTypes =
                typeof(EShieldType)
                    .GetEnumValues()
                    .OfType<EShieldType>()
                    .Where(shieldType => type.HasFlag(shieldType))
                    .ToList();
            var root = new ParallelNode();
            foreach (var shieldType in shieldTypes)
                if (dictionary.TryGetValue(shieldType, out var nodeType) && nodeType != null)
                    if (Activator.CreateInstance(nodeType) is INode node)
                        root.AddNode(node);

            return root;
        }
#if UNITY_EDITOR
        public static event Action EditorUpdate;

        public bool EditorReInit(Tree tree)
        {
            if (!container.ReadFromTree(tree)) return false;
            EditorUtility.SetDirty(this);
            EditorUpdate?.Invoke();
            return true;
        }

        [SerializeField, HideInInspector] private EShieldType preOverrideShieldType;
        public bool EditorInitOverride(EShieldType shieldType)
        {
            if (shieldType == preOverrideShieldType) return false;
            preOverrideShieldType = shieldType;
            overrideContainer.ReadFromTree(new Tree(Build(shieldType)));
            EditorUtility.SetDirty(this);
            EditorUpdate?.Invoke();
            return true;
        }
#endif
        /*private List<IShield> CurrentShields => overrideShields.Count > 0 ? overrideShields : shields;

        protected override void OnAwake()
        {
            shields.Sort();
        }

        private Coroutine initCrt;

        public void Init()
        {
            if (initCrt != null) StopCoroutine(initCrt);
            initCrt = StartCoroutine(Init(this));
        }


#if UNITY_EDITOR
        [SerializeField, HideInInspector] private EShieldType preShieldType;
        public static Action EditorUpdate;

        public bool EditorReInit(EShieldType shieldType)
        {
            if (shieldType == preShieldType) return false;
            shields.Clear();
            var typeBind = ADUtils.GetEnumTypeBind<EShieldType>();
            foreach (var type in Enum.GetValues(typeof(EShieldType)).OfType<EShieldType>())
            {
                if (!shieldType.HasFlag(type) || !typeBind.TryGetValue(type, out var t)) continue;
                try
                {
                    shields.Add((IShield) Activator.CreateInstance(t));
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }

            shields.Sort();
            preShieldType = shieldType;
            EditorUtility.SetDirty(this);
            EditorUpdate?.Invoke();
            return true;
        }

        [SerializeField, HideInInspector] private EShieldType preOverrideShieldType;
        public bool EditorInitOverride(EShieldType shieldType)
        {
            if (shieldType == preOverrideShieldType) return false;
            overrideShields.Clear();
            var typeBind = ADUtils.GetEnumTypeBind<EShieldType>();
            foreach (var type in Enum.GetValues(typeof(EShieldType)).OfType<EShieldType>())
            {
                if (!shieldType.HasFlag(type) || !typeBind.TryGetValue(type, out var t)) continue;
                try
                {
                    overrideShields.Add((IShield) Activator.CreateInstance(t));
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }

            preOverrideShieldType = shieldType;
            EditorUtility.SetDirty(this);
            EditorUpdate?.Invoke();
            return true;
        }
#endif

        public bool CanShowBlackAd => CurrentShields.Count != 0 && CurrentShields.All(e => e.CanShowBlackAd);

        public bool CanShowWhiteAd => CurrentShields.All(e => e.CanShowWhiteAd);
        public bool CanShowBox => CurrentShields.Count != 0 && CurrentShields.All(e => e.CanShowBox);
        public IDictionary<string, Tree.Value> Data { get; set; }

        public IEnumerator Init(ShieldManager manager)
        {
            Debug.Log("ShieldManager Init");
            foreach (var shield in CurrentShields)
            {
                try
                {
                    StartCoroutine(shield.Init(manager));
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }

            yield break;
        }

        public int CompareTo(IShield other)
        {
            return 0;
        }*/
    }
}