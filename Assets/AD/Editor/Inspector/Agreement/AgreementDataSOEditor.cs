using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AD.BlackBag;
using UnityEditor;
using UnityEngine;

namespace AD.Editor
{
    [CustomEditor(typeof(AgreementDataSO)), CanEditMultipleObjects]
    public class AgreementDataSOEditor : UnityEditor.Editor
    {
        private Task<HttpData> currentTask;
        private string http;

        private void OnDestroy() => currentTask = null;

        [NonSerialized] private ADManager _adManager;


        private void OnEnable()
        {
            var goPaths = AssetDatabase.FindAssets("ADManager t:Prefab")
                .Select(AssetDatabase.GUIDToAssetPath)
                .ToArray();
            if (goPaths.Length >= 1)
            {
                var GO = goPaths.Select(AssetDatabase.LoadAssetAtPath<GameObject>)
                    .First(go => go.GetComponent<ADManager>() != null);
                if (GO != null)
                {
                    _adManager = GO.GetComponent<ADManager>();
                }
            }
        }

        public override void OnInspectorGUI()
        {
            var AgreementDataSO = target as AgreementDataSO;
            SerializedProperty privacyType = serializedObject.FindProperty(nameof(AgreementDataSO.privacyType));
            SerializedProperty privacyCompanyName =
                serializedObject.FindProperty(nameof(AgreementDataSO.privacyCompanyName));
            SerializedProperty contact = serializedObject.FindProperty(nameof(AgreementDataSO.contact));
            SerializedProperty privacyHttp = serializedObject.FindProperty(nameof(AgreementDataSO.privacyHttp));
            SerializedProperty privacyContent = serializedObject.FindProperty(nameof(AgreementDataSO.privacyContent));
            SerializedProperty copyrightType = serializedObject.FindProperty("copyrightType");
            SerializedProperty companyName = serializedObject.FindProperty("companyName");
            SerializedProperty registrationNum = serializedObject.FindProperty("registrationNum");
            SerializedProperty companySprite = serializedObject.FindProperty("companySprite");
            SerializedProperty title = serializedObject.FindProperty("title");
            SerializedProperty ageType = serializedObject.FindProperty("ageType");
            serializedObject.Update();

            var color = GUI.color;
            GUI.color = new Color(1f, 0.2f, 0, 0.9f);
            GUILayout.Label("隐私政策以文件中的为基准，而不是链接", new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
            });
            GUI.color = color;


            #region 隐私政策控件

            bool showPrivacy = false;
            if (_adManager == null
                || ADEditorCache.OperationCache[_adManager.OperationType].NeedPrivacy(_adManager.Platform))
            {
                showPrivacy = true;
                GUILayout.Space(5);
                GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(20));
                GUILayout.Space(5);
                EditorGUILayout.PropertyField(privacyType);
                switch ((EPrivacyType) Enum.Parse(typeof(EPrivacyType),
                    privacyType.enumNames[privacyType.enumValueIndex]))
                {
                    case EPrivacyType.Http:
                        EditorGUILayout.PropertyField(privacyHttp);
                        break;
                    case EPrivacyType.Content:
                        EditorGUILayout.PropertyField(privacyContent);
                        break;
                    case EPrivacyType.Pattern:
                        GUILayout.Label("通过链接获取公司名称和邮箱(弃用)");
                        GUILayout.BeginHorizontal();
                        http = GUILayout.TextField(http);
                        if (GUILayout.Button("访问", GUILayout.Width(60)))
                        {
                            if (currentTask != null) ShowNotification("访问中");
                            else currentTask = Http(http);
                        }

                        GUILayout.EndHorizontal();
                        EditorGUILayout.PropertyField(privacyCompanyName);
                        EditorGUILayout.PropertyField(contact);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            #endregion

            GUILayout.Space(5);
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(20));
            GUILayout.Space(5);
            EditorGUILayout.PropertyField(copyrightType);
            switch ((ECopyrightType) copyrightType.enumValueIndex)
            {
                case ECopyrightType.Text:
                    EditorGUILayout.PropertyField(ageType);
                    EditorGUILayout.PropertyField(companyName);
                    EditorGUILayout.PropertyField(registrationNum);
                    break;
                case ECopyrightType.Picture:
                    EditorGUILayout.PropertyField(companySprite);
                    break;
            }

            if (showPrivacy)
            {
                GUILayout.Space(5);
                GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(10));
                GUILayout.Space(5);
                EditorGUILayout.LabelField("常用联系方式");
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("2572555395@qq.com")) contact.stringValue = "2572555395@qq.com";
                if (GUILayout.Button("gamekf_666@sina.com")) contact.stringValue = "gamekf_666@sina.com";
                if (GUILayout.Button("3082464425@qq.com")) contact.stringValue = "3082464425@qq.com";
                EditorGUILayout.EndHorizontal();
            }


            if (currentTask != null && currentTask.IsCompleted)
            {
                var data = currentTask.Result;
                currentTask = null;
                if (data != null)
                {
                    privacyCompanyName.stringValue = data.company;
                    contact.stringValue = data.email;
                }
            }

            serializedObject.ApplyModifiedProperties();

            GUILayout.Space(5);
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(20));
            GUILayout.Space(5);
            if (GUILayout.Button("确认", GUILayout.Width(60), GUILayout.Height(30)))
            {
                var obj = GameObject.Find("ADManager");
                if (obj == null)
                    return;
                Selection.activeObject = obj;
            }
        }

        private static readonly Regex CompanyMatcher =
            new Regex(@"(?<=(选择由|规定了))([\u4e00-\u9fa5]*有限公司)");

        private static readonly Regex[] EmailMatcher =
            new[]
            {
                new Regex(@"(?<=客服邮箱[：\:]</strong>)(.*@.*)(?=</p>)"),
                new Regex(@"(?<=联系邮箱[：\:](</span><span>))(.*@.*)(?=</span></span>)"),
                new Regex(@"((?<=请通过)(.*@.*)(?=与我们联系))"),
            };

        private async Task<HttpData> Http(string s)
        {
            var client = new HttpClient();
            client.Timeout = new TimeSpan(0, 0, 5);
            try
            {
                var stringAsync = await client.GetStringAsync(s);
                var array = stringAsync.Split('\r', '\n').Where(e => !string.IsNullOrWhiteSpace(e)).ToArray();
                client.Dispose();
                ShowNotification("访问完成");
                var data = new HttpData();
                var tasks = new Task[]
                {
                    new Task(() =>
                    {
                        data.email = EmailMatcher.SelectMany(reg => array.Select(e => reg.Match(e)))
                            .FirstOrDefault(match => match.Success)?.Value ?? "";
                    }),
                    new Task(() =>
                    {
                        data.company = array.Select(e => CompanyMatcher.Match(e)).FirstOrDefault(match => match.Success)
                                           ?.Value ??
                                       "";
                    })
                };
                foreach (var task in tasks) task.Start();
                foreach (var task in tasks) await task;
                return data;
            }
            catch (Exception e)
            {
                ShowNotification("访问异常");
                Debug.LogError(e);
                return null;
            }
        }

        private void ShowNotification(string text)
        {
            EditorWindow.focusedWindow.ShowNotification(new GUIContent(text));
        }

        private class HttpData
        {
            internal string company, email;
        }
    }
}