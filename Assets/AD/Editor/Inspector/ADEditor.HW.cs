using UnityEditor;
using UnityEngine;

namespace AD.Editor
{
    internal partial class ADEditor
    {
        private class HWCustom : ICustom
        {
            public EPlatform Platform => EPlatform.HuaWei;
            private ADEditor editor;

            private ADAnalyticsManager analyticsManager;

            private ADAnalyticsManager AnalyticsManager
            {
                get
                {
                    if (analyticsManager == null) analyticsManager = Object.FindObjectOfType<ADAnalyticsManager>();
                    return analyticsManager;
                }
            }

            public void Init(ADEditor adEditor)
            {
                editor = adEditor;
                editor.updateCallbackReceiver.AddItem(UpdateType.ADParam, OnParamUpdate);
            }


            public void LayoutAfterParam()
            {
                if (AnalyticsManager != null)
                {
                    using (var adaSO = new SerializedObject(AnalyticsManager))
                    {
                        var isActiveAdaProp = adaSO.FindProperty("isActive");
                        EditorGUILayout.PropertyField(isActiveAdaProp);
                        if (isActiveAdaProp.boolValue)
                        {
                            EditorGUILayout.PropertyField(adaSO.FindProperty("m_Pkg"));
                            EditorGUILayout.PropertyField(adaSO.FindProperty("m_App_Id"), new GUIContent("默认C开头"));
                        }

                        adaSO.ApplyModifiedPropertiesWithoutUndo();
                    }
                }
            }

            private void OnParamUpdate()
            {
                if (editor == null) return;
                if (editor.ADManager == null) return;
                if (AnalyticsManager == null) return;
                if (editor.ADManager == null) return;
                if (!(editor.ADManager.ADParam is ADHuaWeiParam param)) return;
                using (var adaSO = new SerializedObject(AnalyticsManager))
                {
                    adaSO.FindProperty("m_Pkg").stringValue = param.appid;

                    adaSO.ApplyModifiedPropertiesWithoutUndo();
                }
            }
        }
    }
}