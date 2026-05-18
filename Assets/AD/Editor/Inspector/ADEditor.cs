using System;
using System.Collections.Generic;
using System.Linq;
using AD.Editor.Macro;
using AD.BlackBag.Shield;
using AD.Decorator;
using AD.Editor.Inspector;
using AD.Tools;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
using Tree = AD.NodeTree.Tree;

namespace AD.Editor
{
    [CustomEditor(typeof(ADManager)), CanEditMultipleObjects]
    internal partial class ADEditor : BaseEditorInspector
    {
        private const int height = 22;

        private void OnEnable()
        {
            var macroPlatform = MacroToggleWindow.GetCurrentPlatform();
            var currentPlatform = ((ADManager)target).Platform;
            if (macroPlatform == null || macroPlatform != currentPlatform)
            {
                MacroToggleWindow.SetPlatform(currentPlatform);
            }

            if (titleSo == null)
            {
                titleSo = AssetDatabase
                    .FindAssets("t:ScriptableObject TitleSO")
                    .Select(AssetDatabase.GUIDToAssetPath)
                    .Select(AssetDatabase.LoadAssetAtPath<TitleSO>)
                    .First(so => so != null);
            }

            valueReader.Clear();
            valueReader.Add(UpdateType.Platform, manager => manager.Platform.ToString());
            valueReader.Add(UpdateType.Operation, manager => manager.OperationType.ToString());
            valueReader.Add(UpdateType.ADParam, manager => JsonUtility.ToJson(manager.ADParam));
            valueReader.Add(UpdateType.GameOrientation, manager => manager.ADParam.GameOrientation.ToString());
            valueReader.Add(UpdateType.OverrideShield, manager => manager.OverrideShieldType.ToString());

            updateCallbackReceiver.Clear();
            updateCallbackReceiver.AddItem(UpdateType.Platform, UpdateScene);
            updateCallbackReceiver.AddItem(UpdateType.Operation, UpdateScene);
            updateCallbackReceiver.AddItem(UpdateType.GameOrientation, UpdateScene);

            updateCallbackReceiver.AddItem(UpdateType.Platform, UpdateShield);
            updateCallbackReceiver.AddItem(UpdateType.Operation, UpdateShield);

            updateCallbackReceiver.AddItem(UpdateType.OverrideShield, UpdateOverrideShield);

            updateCallbackReceiver.AddItem(UpdateType.Operation, UpdateDecorator);
            updateCallbackReceiver.AddItem(UpdateType.Platform, UpdateDecorator);

            updateCallbackReceiver.AddItem(UpdateType.Platform, UpdateMacro);

            customs = ADUtils.GetSubClassInstanceOf<ICustom>();
            customs.RemoveAll(e => e.Platform != currentPlatform);
            customs.ForEach(e => e.Init(this));
        }


        GUILayoutOption[] options = {GUILayout.MaxWidth(90), GUILayout.Height(height)};
        GUILayoutOption[] options2 = {GUILayout.MaxWidth(64), GUILayout.Height(30)};

        private List<ICustom> customs;
        private TitleSO titleSo;

        private ADManager ADManager;

        private IDictionary<UpdateType, List<Action>> updateCallbackReceiver =
            new Dictionary<UpdateType, List<Action>>();

        private IDictionary<UpdateType, string> oldValue =
            new Dictionary<UpdateType, string>();

        private IDictionary<UpdateType, Func<ADManager, string>> valueReader =
            new Dictionary<UpdateType, Func<ADManager, string>>();

        private UpdateType updateFlag;

        protected override void Layout()
        {
            if (!(target is ADManager adManager)) return;
            oldValue.Clear();
            updateFlag = 0;
            ADManager = adManager;

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            RenderAgreement();
            RenderTitle();
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            serializedObject.Update();
            if (ADManager.ADParam == null)
            {
                if (ADManager.SetupADParam())
                {
                    EditorUtility.SetDirty(ADManager.gameObject);
                    serializedObject.ApplyModifiedPropertiesWithoutUndo();
                }

                return;
            }

            foreach (var pair in valueReader) oldValue.Add(pair.Key, pair.Value(ADManager));

            serializedObject.UpdateIfRequiredOrScript();
            var iterator = serializedObject.GetIterator();
            for (bool enterChildren = true; iterator.NextVisible(enterChildren); enterChildren = false)
            {
                switch (iterator.propertyPath)
                {
                    case "operationType":
                    {
                        EditorGUILayout.BeginHorizontal();

                        EditorGUILayout.PropertyField(iterator, true);
                        EditorGUILayout.BeginVertical(GUILayout.Width(54));
                        EditorGUILayout.Space(26);
                        if (GUILayout.Button("Ping", GUILayout.Width(54)))
                            PingOperationScript();
                        EditorGUILayout.EndVertical();
                        EditorGUILayout.EndHorizontal();

                        break;
                    }
                    case "overrideShieldType":
                        Space(10);
                        EditorGUILayout.BeginHorizontal();

                        var headerStyle = new GUIStyle(GUI.skin.label);
                        headerStyle.fontStyle = FontStyle.Bold;
                        EditorGUILayout.LabelField("屏蔽", headerStyle, GUILayout.Width(EditorGUIUtility.labelWidth));

                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();

                        EditorGUILayout.BeginVertical();

                        // EditorGUILayout.BeginHorizontal();
                        /*EditorGUILayout.LabelField("默认屏蔽类型", GUILayout.Width(EditorGUIUtility.labelWidth));
                        var stMask = (int) ADEditorCache.OperationCache[ADManager.OperationType]
                            .InitShieldType(ADManager.Platform);
                        var stDisplay = stMask == 0
                            ? ADEditorCache.ShieldTypeDataCache.First(e => e.value == 0).name
                            : string.Join(" || ", ADEditorCache.ShieldTypeDataCache.Where(e => (e.value & stMask) != 0)
                                .Select(e => e.name));
                        if (EditorGUILayout.DropdownButton(new GUIContent(stDisplay), FocusType.Passive))
                        {
                            var menu = new GenericMenu();
                            foreach (var data in ADEditorCache.ShieldTypeDataCache.Where(e => (e.value & stMask) != 0))
                                menu.AddItem(new GUIContent(data.name), true, null);

                            menu.ShowAsContext();
                        }*/

                        // EditorGUILayout.EndHorizontal();

                        // GUILayout.Space(10);
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("覆盖屏蔽类型", GUILayout.Width(EditorGUIUtility.labelWidth));

                        // EditorGUILayout.LabelField(iterator.displayName, GUILayout.Width(EditorGUIUtility.labelWidth));
                        var ostMask = iterator.intValue & ADEditorCache.OverrideableMask;
                        var ostDisplay = ostMask == 0
                            ? ADEditorCache.OverrideableShieldTypeDataCache.First(e => e.value == 0).name
                            : string.Join(" || ", ADEditorCache.OverrideableShieldTypeDataCache
                                .Where(e => (e.value & ostMask) != 0)
                                .Select(e => e.name));
                        if (EditorGUILayout.DropdownButton(new GUIContent(ostDisplay), FocusType.Passive))
                        {
                            var overrideShieldTypeProp = iterator.Copy();
                            var menu = new GenericMenu();
                            foreach (var data in ADEditorCache.OverrideableShieldTypeDataCache)
                            {
                                menu.AddItem(new GUIContent(data.name),
                                    (overrideShieldTypeProp.intValue & data.value) != 0,
                                    () =>
                                    {
                                        if (data.onlyOne
                                            || (overrideShieldTypeProp.intValue &
                                                ADEditorCache.OverrideableOnlyOneMask) != 0)
                                            overrideShieldTypeProp.intValue = data.value;
                                        else overrideShieldTypeProp.intValue ^= data.value;

                                        overrideShieldTypeProp.serializedObject.ApplyModifiedProperties();
                                    });
                            }

                            menu.ShowAsContext();
                        }

                        iterator.intValue &= ADEditorCache.OverrideableMask;
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.EndVertical();
                        EditorGUILayout.EndHorizontal();
                        Space(10);
                        break;
                    case "ADParam":
                        EditorGUILayout.BeginHorizontal();

                        EditorGUI.BeginChangeCheck();

                        RenderADParam(iterator);
                        if (EditorGUI.EndChangeCheck())
                            serializedObject.ApplyModifiedProperties();

                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();
                        var orientationProp = iterator.FindPropertyRelative("Orientation");
                        using (new EditorGUI.DisabledScope(true))
                            EditorGUILayout.PropertyField(orientationProp, true);
                        EditorGUILayout.EndHorizontal();
                        break;
                    default:
                        EditorGUILayout.BeginHorizontal();

                        using (new EditorGUI.DisabledScope("m_Script" == iterator.propertyPath))
                            EditorGUILayout.PropertyField(iterator, true);
                        EditorGUILayout.EndHorizontal();
                        break;
                }
            }

            serializedObject.ApplyModifiedProperties();
            customs.ForEach(e => e.LayoutAfterParam());

            GUILayout.Space(15);

            RenderOrientation();
            Space(10);
            RenderTimeSet();

            Space();
            serializedObject.ApplyModifiedProperties();

            foreach (var pair in valueReader)
                if (oldValue[pair.Key] != pair.Value(ADManager))
                    updateFlag |= pair.Key;
            CallUpdateMethod();
        }

        #region render

        private void RenderAgreement()
        {
            if (GUILayout.Button("设置隐私政策", options))
            {
                var assets = AssetDatabase.FindAssets("t:AgreementDataSO AgreementDataSO");
                if (assets.Length == 1)
                {
                    Selection.activeObject =
                        AssetDatabase.LoadAssetAtPath<AgreementDataSO>(AssetDatabase.GUIDToAssetPath(assets[0]));
                }
                else
                {
                    Debug.Log($"Invalid count of {typeof(AgreementDataSO)} is {assets.Length} != 1");
                }

                foreach (var root in SceneManager.GetActiveScene().GetRootGameObjects()
                    .Where(PrefabUtility.IsAnyPrefabInstanceRoot))
                {
                    root.ApplyPrefabInstance();
                }
            }
        }

        private void RenderTitle()
        {
            if (!ADEditorCache.AdAdapterCache[ADManager.Platform].HasTitle) return;
            if (titleSo != null)
            {
                GUILayout.Space(65);
                using (var so = new SerializedObject(titleSo))
                {
                    EditorGUI.BeginChangeCheck();
                    var originalLabelWidth = EditorGUIUtility.labelWidth;
                    EditorGUIUtility.labelWidth = 30;
                    EditorGUILayout.PropertyField(so.FindProperty("title"), new GUIContent("标题"), false);
                    EditorGUIUtility.labelWidth = originalLabelWidth;
                    if (EditorGUI.EndChangeCheck())
                    {
                        so.ApplyModifiedProperties();
                    }
                }
            }
        }

        private void RenderTimeSet()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            var shieldManagerInspector = Resources.FindObjectsOfTypeAll<ShieldManagerInspector>().FirstOrDefault();
            if (shieldManagerInspector != null)
            {
                EditorGUI.BeginChangeCheck();
                try
                {
                    using (new EditorGUI.IndentLevelScope())
                        foreach (var inspector in shieldManagerInspector.TimeInspectors)
                        {
                            EditorGUILayout.BeginHorizontal();
                            Space(5);
                            GUILayout.Label(inspector.Key.description??inspector.Key.key,EditorStyles.boldLabel);
                            EditorGUILayout.EndHorizontal();
                            inspector.Layout();
                            Space();
                        }
                }
                catch{}
                finally
                {
                    if (EditorGUI.EndChangeCheck())
                        shieldManagerInspector.Save();
                }
            }

            EditorGUILayout.EndVertical();

            // if (GUILayout.Button("设置时间", options))
            // {
            //     ADManager.ADParam.ShieldTime
            //         = ADEditorCache.OperationCache[ADManager.OperationType]
            //             .GetShieldTime(ADManager.Platform);
            //     ADManager.ADParam.BlackShieldTime
            //         = ADEditorCache.OperationCache[ADManager.OperationType]
            //             .GetBlackShieldTime(ADManager.Platform);
            //
            //     EditorUtility.SetDirty(ADManager);
            //     AssetDatabase.SaveAssets();
            // }
            //
            // if (GUILayout.Button("设置修改包时间", options))
            // {
            //     ADManager.ADParam.ShieldTime
            //         = ADEditorCache.OperationCache[ADManager.OperationType]
            //             .GetModifyPackageShieldTime(ADManager.Platform);
            //     ADManager.ADParam.BlackShieldTime
            //         = ADEditorCache.OperationCache[ADManager.OperationType]
            //             .GetModifyPackageBlackShieldTime(ADManager.Platform);
            //
            //     EditorUtility.SetDirty(ADManager);
            //     AssetDatabase.SaveAssets();
            // }
            //
            // if (GUILayout.Button("设置不屏蔽时间", options))
            // {
            //     ADManager.ADParam.ShieldTime
            //         = ADManager.ADParam.BlackShieldTime
            //             = TimeCheck.GetNotShieldTime();
            //
            //     EditorUtility.SetDirty(ADManager);
            //     AssetDatabase.SaveAssets();
            // }
        }

        private void RenderOrientation()
        {
            GUILayout.BeginHorizontal();
            var buttonStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = 13
            };
            if (GUILayout.Button("横屏", buttonStyle, options2))
            {
                if (ADManager.ADParam.GameOrientation != ADEditorConst.Landscape)
                {
                    ADManager.ADParam.GameOrientation = ADEditorConst.Landscape;
                }


                EditorUtility.SetDirty(ADManager);
                AssetDatabase.SaveAssets();
            }

            if (GUILayout.Button("竖屏", buttonStyle, options2))
            {
                if (ADManager.ADParam.GameOrientation != ADEditorConst.Portrait)
                {
                    ADManager.ADParam.GameOrientation = ADEditorConst.Portrait;
                }


                EditorUtility.SetDirty(ADManager);
                AssetDatabase.SaveAssets();
            }

            GUILayout.EndHorizontal();
        }

        private bool foldout = true;

        private void RenderADParam(SerializedProperty property)
        {
            EditorGUILayout.BeginVertical();
            property = property.Copy();
            property.DrawHeader();
            EditorGUILayout.BeginHorizontal();
            foldout = EditorGUILayout.Foldout(foldout, property.name);
            EditorGUILayout.EndHorizontal();
            using (new EditorGUI.IndentLevelScope())
            {
                if (foldout)
                {
                    var depth = property.depth;
                    for (bool enterChild = true;
                        property.Next(enterChild) && property.depth > depth;
                        enterChild = false)
                    {
                        ADParamDrawer.OnGUI(property.Copy());
                    }
                }
            }

            EditorGUILayout.EndVertical();
        }

        #endregion

        #region updateReceiver

        private void CallUpdateMethod()
        {
            foreach (var type in Enum.GetValues(typeof(UpdateType))
                .OfType<UpdateType>()
                .Where(e => updateFlag.HasFlag(e)))
            {
                if (!updateCallbackReceiver.TryGetValue(type, out var list)) continue;
                foreach (var action in list)
                {
                    try
                    {
                        action.Invoke();
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
            }

            serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }


        private void UpdateMacro()
        {
            MacroToggleWindow.SetPlatform(ADManager.Platform);
        }

        private void UpdateShield()
        {
            var shieldMsgManager = Object.FindObjectOfType<ShieldManager>();
            shieldMsgManager.EditorReInit(new Tree(ADEditorCache.OperationCache[ADManager.Instance.OperationType]
                .CreateRule(ADManager.Instance.Platform)));

            serializedObject.FindProperty("overrideShieldType").enumValueIndex = 0;
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(shieldMsgManager);
        }

        private void UpdateOverrideShield()
        {
            // if (ShieldManager.Instance)
            // {
            //     ShieldManager.Instance.EditorInitOverride(ADManager.OverrideShieldType);
            //     EditorUtility.SetDirty(ShieldManager.Instance);
            // }
        }

        private void UpdateDecorator()
        {
            var decoratorManager = ADManager.ADParam.Decorator;
            if (decoratorManager == null)
                ADManager.ADParam.Decorator = new ADParamDecoratorManager();
            decoratorManager = ADManager.ADParam.Decorator;
            if (decoratorManager == null) return;
            decoratorManager.Reset();
            ADEditorCache.OperationCache[ADManager.OperationType].InitDecorator(ADManager.Platform, decoratorManager);
            EditorUtility.SetDirty(ADManager);
        }

        private void UpdateScene()
        {
            var adAssets = ADAssets.Instance;
            IEnumerable<GameObject> gos;
            switch (ADManager.ADParam.GameOrientation)
            {
                case ADEditorConst.Landscape:
                    gos = adAssets.landscapeGos;
                    break;
                case ADEditorConst.Portrait:
                    gos = adAssets.portraitGos;
                    break;
                default:
                    return;
            }

            foreach (var go in adAssets.landscapeGos) RemoveGameObjectByPrefab(go);
            foreach (var go in adAssets.portraitGos) RemoveGameObjectByPrefab(go);
            foreach (var go in adAssets.platform2Gos.Values.SelectMany(e => e.gos)) RemoveGameObjectByPrefab(go);
            if (adAssets.platform2Gos.TryGetValue(ADManager.Platform, out var components))
                gos = gos.Concat(components.gos);
            foreach (var go in gos)
            {
                CreatePrefabInstance(go);
            }

            /*RepWithPrefab<TimingManager>(
                FindPrefabAssetWithType<TimingManager>()
                    .Where(func)
                    .FirstOrDefault());
            RepWithPrefab<AgreementHealth.Scripts.AgreementHealth>(
                FindPrefabAssetWithType<AgreementHealth.Scripts.AgreementHealth>()
                    .Where(func)
                    .FirstOrDefault());
            switch (platform)
            {
                case EPlatform.HuaWei:
                    RepWithPrefab<HWSDK>(
                        FindPrefabAssetWithType<HWSDK>()
                            .FirstOrDefault());
                    RepWithPrefab<ADAnalyticsManager>(
                        FindPrefabAssetWithType<ADAnalyticsManager>()
                            .FirstOrDefault());
                    break;
                default:
                    RepWithPrefab<HWSDK>(null);
                    RepWithPrefab<ADAnalyticsManager>(null);
                    break;
            }*/
        }

        private static void RemoveGameObjectByPrefab(GameObject prefab)
        {
            foreach (var root in SceneManager
                .GetActiveScene()
                .GetRootGameObjects()
                .Where(PrefabUtility.IsAnyPrefabInstanceRoot)
                .Where(root => PrefabUtility.GetCorrespondingObjectFromOriginalSource(root) == prefab)
                .Where(e => e != null)
            ) DestroyImmediate(root);
        }

        private static void CreatePrefabInstance(GameObject prefab)
        {
            if (prefab != null) PrefabUtility.InstantiatePrefab(prefab);
        }

        /*
        private static GameObject[] FindPrefabAssetWithType<T>()
        {
            return AssetDatabase
                .FindAssets("t:Prefab", new string[] {"Assets"})
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<GameObject>)
                .Where(e => e.GetComponentsInChildren<T>().Length > 0).ToArray();
        }

        private static bool IsOrientation(GameObject go, int orientation)
        {
            switch (orientation)
            {
                case Landscape:
                    return go.name.EndsWith(LEnd);
                case Portrait:
                    return go.name.EndsWith(PEnd);
                default:
                    throw new Exception($"Invalid orientation of {orientation}");
            }
        }

        private static bool IsLandscape(GameObject go) => IsOrientation(go, Landscape);
        private static bool IsPortrait(GameObject go) => IsOrientation(go, Portrait);

        private static void RepWithPrefab<T>(GameObject prefab) where T : MonoBehaviour
        {
            var ts = Object.FindObjectsOfType<T>();
            foreach (var t in ts)
                if (t != null)
                    DestroyImmediate(t.gameObject);
            if (prefab != null) PrefabUtility.InstantiatePrefab(prefab);
        }*/

        [Flags]
        private enum UpdateType
        {
            Platform = 1,
            Operation = 1 << 1,
            ADParam = 1 << 2,
            GameOrientation = 1 << 3,
            OverrideShield = 1 << 4,
        }

        #endregion

        #region ping

        private void PingOperationScript()
        {
            if (ADEditorCache.OperationCache.TryGetValue(ADManager.OperationType, out var operation))
            {
                var script = AssetDatabase
                    .FindAssets($"t:MonoScript {operation.GetType().Name}", new string[] {"Assets"})
                    .Select(AssetDatabase.GUIDToAssetPath)
                    .Select(AssetDatabase.LoadAssetAtPath<MonoScript>)
                    .FirstOrDefault(e => e != null && e.GetClass() == operation.GetType());
                if (script != null) EditorGUIUtility.PingObject(script);
                return;
            }

            Debug.LogError($"找不到 {ADManager.OperationType} 对应的脚本");
        }

        #endregion

        #region custom

        internal interface ICustom
        {
            EPlatform Platform { get; }
            void Init(ADEditor adEditor);
            void LayoutAfterParam();
        }

        internal interface ICustomParamLayout
        {
            int Priority { get; }
            void Layout();
        }

        #endregion
    }
}