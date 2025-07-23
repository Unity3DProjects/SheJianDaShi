using System;
using System.Linq;
using AD.Editor.Macro;
using AD.Runtime.TreasureBox.Scripts;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AD.Editor.AD
{
    [CustomEditor(typeof(ADManager)), CanEditMultipleObjects]
    public class ADEditor : UnityEditor.Editor
    {
        private static int height = 22;

        GUILayoutOption[] options = { GUILayout.MaxWidth(110), GUILayout.Height(height) };
        GUILayoutOption[] options2 = { GUILayout.MaxWidth(36), GUILayout.Height(height) };
        private const int Landscape = 0, Portrait = 1;
        private const string LEnd = nameof(Landscape), PEnd = nameof(Portrait);

        public override void OnInspectorGUI()
        {
            int updateFlag = 0;
            var primaryP = (target as ADManager).Platform;

            var adManager = (target as ADManager);

            GUILayout.BeginHorizontal();
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

                UnityEditor.PrefabUtility.ApplyPrefabInstance(adManager.gameObject, InteractionMode.UserAction);
            }

            GUILayout.EndHorizontal();
            base.OnInspectorGUI();
            serializedObject.Update();
            var ADManager = target as ADManager;
            if (ADManager.ADParam == null)
            {
                switch (ADManager.Platform)
                {
                    case EPlatform.HuaWei:
                        ADManager.ADParam = new ADHuaWeiParam();
                        break;
                    default:
                        ADManager.ADParam = new ADNormalParam();
                        break;
                }

                serializedObject.ApplyModifiedPropertiesWithoutUndo();
            }


            GUILayout.Space(15);

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("设置时间", options))
            {
                if (ADManager.ADParam is ADNormalParam adNormalParam)
                {
                    switch (ADManager.Platform)
                    {
                        case EPlatform.VIVO:
                            adNormalParam.showDateStr = GetVIVOBlockTime();
                            adNormalParam.blackDate = GetVIVOBlockTime();
                            break;
                        case EPlatform.OPPO:
                            adNormalParam.showDateStr = GetOppoTime();
                            adNormalParam.blackDate = GetOPPOBlackTime();
                            break;
                        default:
                            adNormalParam.showDateStr = GetVIVOBlockTime();
                            adNormalParam.blackDate = GetVIVOBlockTime();
                            break;
                    }
                }


                EditorUtility.SetDirty(ADManager);
                AssetDatabase.SaveAssets();
            }

            if (GUILayout.Button("设置修改包时间", options))
            {
                if (ADManager.ADParam is ADNormalParam adNormalParam)
                {
                    switch (ADManager.Platform)
                    {
                        case EPlatform.VIVO:
                            adNormalParam.showDateStr = GetVIVOBlockTime();
                            adNormalParam.blackDate = GetVIVOBlockTime();
                            break;
                        case EPlatform.OPPO:
                            adNormalParam.showDateStr = GetOppoTime();
                            adNormalParam.blackDate = GetOppoTime();
                            break;
                        default:
                            adNormalParam.showDateStr = GetVIVOBlockTime();
                            adNormalParam.blackDate = GetVIVOBlockTime();
                            break;
                    }
                }


                EditorUtility.SetDirty(ADManager);
                AssetDatabase.SaveAssets();
            }

            if (GUILayout.Button("横屏", options2))
            {
                if (ADManager.ADParam is ADNormalParam adNormalParam && adNormalParam.Orientation != Landscape)
                {
                    adNormalParam.Orientation = Landscape;
                    updateFlag |= 0b10;
                }
                else if (ADManager.ADParam is ADHuaWeiParam adHuaWeiParam && adHuaWeiParam.Orientation != Landscape)
                {
                    adHuaWeiParam.Orientation = Landscape;
                    updateFlag |= 0b10;
                }


                EditorUtility.SetDirty(ADManager);
                AssetDatabase.SaveAssets();
            }

            if (GUILayout.Button("竖屏", options2))
            {
                if (ADManager.ADParam is ADNormalParam adNormalParam && adNormalParam.Orientation != Portrait)
                {
                    adNormalParam.Orientation = Portrait;
                    updateFlag |= 0b10;
                }
                else if (ADManager.ADParam is ADHuaWeiParam adHuaWeiParam && adHuaWeiParam.Orientation != Portrait)
                {
                    adHuaWeiParam.Orientation = Portrait;
                    updateFlag |= 0b10;
                }


                EditorUtility.SetDirty(ADManager);
                AssetDatabase.SaveAssets();
            }
            GUILayout.EndHorizontal();


            serializedObject.ApplyModifiedProperties();
            int ori = -1;
            if (ADManager.ADParam is ADNormalParam p1)
                ori = p1.Orientation;
            else if (ADManager.ADParam is ADHuaWeiParam p2)
                ori = p2.Orientation;
            if ((target as ADManager).Platform != primaryP) updateFlag |= 0b11;
            UpdateAD((target as ADManager).Platform, ori, updateFlag);
        }

        /// <summary>
        /// 设置VIVO屏蔽时间
        /// </summary>
        private string GetVIVOBlockTime()
        {
            // 获取当前时间  
            DateTime currentTime = DateTime.Now;

            currentTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 19, 0, 0);

            DateTime futureTime = currentTime.AddDays(2);

            switch (currentTime.DayOfWeek)
            {
                case DayOfWeek.Thursday:
                    futureTime = currentTime.AddDays(1);
                    break;
                case DayOfWeek.Friday:
                    if (DateTime.Now.Hour < 12)
                    {
                        futureTime = currentTime;
                    }
                    else
                    {
                        futureTime = currentTime.AddDays(3);
                    }

                    break;
                case DayOfWeek.Saturday:
                    futureTime = currentTime.AddDays(3);
                    break;
            }

            string formattedTime = futureTime.ToString("yyyy-MM-dd HH:mm:ss");
            return formattedTime;
        }

        /// <summary>
        /// 获取OPPO白包时间
        /// </summary>
        /// <returns></returns>
        private string GetOppoTime()
        {
            // 获取当前时间  
            DateTime currentTime = DateTime.Now;
            DateTime toNight = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 19, 0, 0);
            DateTime futureTime = toNight.AddDays(1);

            var format = "yyyy-MM-dd HH:mm:ss";

            switch (toNight.DayOfWeek)
            {
                case DayOfWeek.Friday:
                {
                    if (currentTime.Hour < 16)
                    {
                        return toNight.ToString(format);
                    }
                    else
                    {
                        futureTime = toNight.AddDays(3);
                        return futureTime.ToString(format);
                    }
                }
                case DayOfWeek.Saturday:
                    futureTime = toNight.AddDays(2);
                    return futureTime.ToString(format);
            }


            if (currentTime.Hour < 17)
            {
                return toNight.ToString(format);
            }
            else
            {
                futureTime = toNight.AddDays(1);

                return futureTime.ToString(format);
            }
        }

        /// <summary>
        /// 获取OPPO黑包时间
        /// </summary>
        /// <returns></returns>
        private string GetOPPOBlackTime()
        {
            DateTime dateTime = GetDate(GetOppoTime());

            var futureTime = dateTime.AddDays(5);

            string formattedTime = futureTime.ToString("yyyy-MM-dd HH:mm:ss");
            return formattedTime;
        }

        private static DateTime GetDate(string date)
        {
            try
            {
                var _date = Convert.ToDateTime(date);
                return _date;
            }
            catch (Exception e)
            {
                throw new InvalidCastException("广告日期设置错误：字符串无法转换为 DateTime");
            }
        }

        private void UpdateAD(EPlatform platform, int orientation, int updateFlag)
        {
            if ((updateFlag & 0b1) != 0) UpdateMacro(platform);
            if ((updateFlag & 0b10) != 0) UpdateScene(platform, orientation);
        }

        private void UpdateMacro(EPlatform platform)
        {
            MacroToggleWindow.SetPlatform(platform);
        }

        private void UpdateScene(EPlatform platform, int orientation)
        {
            Func<GameObject, bool> func;
            switch (orientation)
            {
                case Landscape:
                    func = IsLandscape;
                    break;
                case Portrait:
                    func = IsPortrait;
                    break;
                default:
                    return;
            }

            RepWithPrefab<TimingManager>(
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
                    RepWithPrefab<HWAdAdapter>(
                        FindPrefabAssetWithType<HWAdAdapter>()
                            .FirstOrDefault());
                    RepWithPrefab<ADAnalyticsManager>(
                        FindPrefabAssetWithType<ADAnalyticsManager>()
                            .FirstOrDefault());
                    break;
                default:
                    RepWithPrefab<HWAdAdapter>(null);
                    RepWithPrefab<ADAnalyticsManager>(null);
                    break;
            }
        }

        private static GameObject[] FindPrefabAssetWithType<T>()
        {
            return AssetDatabase
                .FindAssets("t:Prefab", new string[] { "Assets" })
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
        }
    }
}