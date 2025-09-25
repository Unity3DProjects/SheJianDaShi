using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;


namespace QGMiniGame
{

    [InitializeOnLoad]
    public class QGSettingsHelperInterface
    {
        public static QGSettingsHelper helper = new QGSettingsHelper();
    }

    public class QGSettingsHelper
    {

        static readonly string Key_buildSrc = "dst";
        static readonly string Key_streamingAssetsUrl = "streamingAssetsUrl";
        static readonly string Key_wasmUrl = "wasmUrl";
        static readonly string Key_usSubWasmType = "usSubWasmType";
        static readonly string Key_preloadUrls = "preloadUrl";
        static readonly string Key_useAddressable = "useAddressable";
        static readonly string Key_useWebgl2 = "useWebgl2";
        static readonly string Key_useCodeSize = "useCodeSize";
        static readonly string Key_usTargetBgType = "usBgImageSrcType";
        static readonly string Key_targetBg = "bgImageSrc";
        static readonly string Key_targetIcon = "icon";
        static readonly string Key_targetPackage = "package";
        static readonly string Key_targetName = "name";
        static readonly string Key_targetVN = "versionName";
        static readonly string Key_targetVC = "versionCode";
        static readonly string Key_targetMniPV = "minPlatformVersion";
        static readonly string Key_targetOrientation = "deviceOrientation";


        private Dictionary<string, string> formInputData = new Dictionary<string, string>();
        private Dictionary<string, int> formIntPopupData = new Dictionary<string, int>();
        private Dictionary<string, bool> formCheckboxData = new Dictionary<string, bool>();


        private bool foldBaseInfo = true;
        private bool foldLoadingConfig = true;
        private bool foldDebugOptions = true;
        private bool foldOther = true;
        public Texture tex;

        public void OnSettingsGUI(QGEditorWindowNew qGEditorWindow)
        {

            GUIStyle linkStyle = new GUIStyle(GUI.skin.label);
            linkStyle.normal.textColor = Color.yellow;
            linkStyle.hover.textColor = Color.yellow;
            linkStyle.stretchWidth = false;
            linkStyle.alignment = TextAnchor.UpperLeft;
            linkStyle.wordWrap = true;

            foldBaseInfo = EditorGUILayout.Foldout(foldBaseInfo, "基本信息");
            if (foldBaseInfo)
            {
                EditorGUILayout.BeginVertical("frameBox", GUILayout.ExpandWidth(true));

                //this.formInput(Key_targetIcon, "icon");
                this.formInput(Key_targetPackage, "小游戏包名");
                this.formInput(Key_targetName, "小游戏名称");
                this.formInput(Key_targetVC, "小游戏版本号");
                this.formInput(Key_targetVN, "小游戏版本名称");
                this.formInput(Key_targetMniPV, "最小平台版本号");
                this.formIntPopup(Key_targetOrientation, "游戏方向",
                    new[] { "Portrait", "Landscape" }, (int[])Enum.GetValues(typeof(Orientation)));

                GUILayout.BeginHorizontal();
                if (!formInputData.ContainsKey(Key_buildSrc))
                {
                    formInputData[Key_buildSrc] = "";
                }
                EditorGUILayout.LabelField(string.Empty, GUILayout.Width(10));
                GUILayout.Label("导出路径", GUILayout.Width(140));
                formInputData[Key_buildSrc] = GUILayout.TextField(formInputData[Key_buildSrc], GUILayout.MaxWidth(EditorGUIUtility.currentViewWidth - 270));
                if (GUILayout.Button(new GUIContent("打开"), GUILayout.Width(40)))
                {
                    if (!formInputData[Key_buildSrc].Trim().Equals(string.Empty))
                    {
                        EditorUtility.RevealInFinder(formInputData[Key_buildSrc]);
                    }
                    GUIUtility.ExitGUI();
                }
                if (GUILayout.Button(new GUIContent("选择"), GUILayout.Width(40)))
                {
                    var dstPath = EditorUtility.SaveFolderPanel("选择你的游戏导出目录", string.Empty, string.Empty);
                    if (dstPath != string.Empty)
                    {
                        formInputData[Key_buildSrc] = dstPath;
                    }
                    GUIUtility.ExitGUI();
                }
                GUILayout.EndHorizontal();

                EditorGUILayout.EndVertical();
            }

            foldLoadingConfig = EditorGUILayout.Foldout(foldLoadingConfig, "启动Loading配置");
            if (foldLoadingConfig)
            {
                EditorGUILayout.BeginVertical("frameBox", GUILayout.ExpandWidth(true));

                // 自定义loading Image图片
                this.formIntPopup(Key_usTargetBgType, "启动背景图/视频封面", new[] { "手动输入", "纹理选择" }, new[] { 0, 1 });
                if (getDataPop(Key_usTargetBgType) == 0)
                {
                    this.subFormInput(Key_targetBg, "手动输入(?)", "支持使用远程资源URL和本地地址");
                } else
                {
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(string.Empty, GUILayout.Width(35));
                    tex = (Texture)EditorGUILayout.ObjectField("纹理选择", tex, typeof(Texture2D), false);
                    string currentBgSrc = AssetDatabase.GetAssetPath(tex);
                    this.formInputData[Key_targetBg] = currentBgSrc;
                    EditorGUILayout.LabelField(string.Empty, GUILayout.Width(15));
                    GUILayout.EndHorizontal();
                }

                //1: useSubPkgLoading 使用分包加载自定义Loading
                //2: useSelfLoading 使用自有服务器资源 自定义Loading
                int[] values = (int[])Enum.GetValues(typeof(SubWasmPkgType));
                this.formIntPopup(Key_usSubWasmType, "首包资源加载方式", new[] { "整包加载", "自定义CDN分包", "vivo分包" }, values);
                if (IsUseSelfLoading())
                {
                    this.subFormInput(Key_wasmUrl, "CDN 地址(?)", "使用CDN方式，需要手动输入静态资源地址，否则为空");
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(string.Empty, GUILayout.Width(25));
                    var tips1 = new GUIStyle();
                    tips1.fontSize = 12;
                    tips1.normal.textColor = Color.yellow;
                    tips1.margin.left = 20;
                    tips1.margin.bottom = 10;
                    tips1.margin.right = 20;
                    GUILayout.TextField("备注：该地址配置上线时请注意版本号的区分，不能多个导出版本混用同一个线上文件", tips1);
                    GUILayout.EndHorizontal();
                }
                this.formCheckbox(Key_useAddressable, "使用Addressable(?)", "参考Unity官方提供的Addressable");
                if (IsUseAddressable())
                {
                    this.subFormInput(Key_streamingAssetsUrl, "URL(?)", "使用Addressable则需要填写");
                }
                this.formInput(Key_preloadUrls, "预下载文件列表(?)", "使用;间隔，最长5个文件");

                EditorGUILayout.EndVertical();
            }

            foldDebugOptions = EditorGUILayout.Foldout(foldDebugOptions, "调试编译选项");
            if (foldDebugOptions)
            {
                EditorGUILayout.BeginVertical("frameBox", GUILayout.ExpandWidth(true));
                this.formCheckbox(Key_useWebgl2, "启用Webgl2.0");
                this.formCheckbox(Key_useCodeSize, "启动代码裁剪(?)", "IL2CPP Code Generation");
                EditorGUILayout.EndVertical();
            }

#if UNITY_2021_1_OR_NEWER

#else
            var labelStyle = new GUIStyle(EditorStyles.boldLabel);
            labelStyle.fontSize = 14;
            labelStyle.margin.left = 20;
            labelStyle.margin.top = 10;
            labelStyle.margin.bottom = 10;

            var picTips = new GUIStyle();
            picTips.fontSize = 12;
            picTips.normal.textColor = Color.yellow;
            picTips.margin.left = 20;
            picTips.margin.top = 5;
            picTips.margin.right = 20;
            GUILayout.TextField("备注：当前Unity引擎版本低于2021，建议升级使用引擎版本自带的纹理ASTC压缩格式", picTips);
#endif
        }

        public void OnOtherGUI(QGEditorWindowNew qGEditorWindow)
        {
            foldOther = EditorGUILayout.Foldout(foldOther, "更多");
            if (foldOther)
            {
                EditorGUILayout.BeginVertical("frameBox", GUILayout.ExpandWidth(true));
                var labelStyle = new GUIStyle(EditorStyles.boldLabel);
                labelStyle.fontSize = 14;
                labelStyle.margin.left = 20;
                labelStyle.margin.top = 10;
                labelStyle.margin.bottom = 10;

                GUILayout.BeginHorizontal();
                GUIStyle toolButtonStyle = new GUIStyle(GUI.skin.button);
                toolButtonStyle.fontSize = 12;
                toolButtonStyle.margin.left = 20;
                toolButtonStyle.margin.top = 10;
                var isUpdateBtnPressed = GUILayout.Button("检查更新", toolButtonStyle, GUILayout.Height(20), GUILayout.Width(100));
                if (isUpdateBtnPressed)
                {
                    QGGameTools.CheckUpdate();
                }

                toolButtonStyle.normal.textColor = Color.green;

                var isHelpBtnPressed = GUILayout.Button("使用文档与教程", toolButtonStyle, GUILayout.Height(20), GUILayout.Width(100));
                if (isHelpBtnPressed)
                {
                    QGGameTools.OpenUnityGame();
                }

                toolButtonStyle.normal.textColor = Color.white;

                var isAssetBundlePressed = GUILayout.Button("AB打包工具", toolButtonStyle, GUILayout.Height(20), GUILayout.Width(100));
                if (isAssetBundlePressed)
                {
                    QGGameTools.AssetBundleBuild();
                }

                var isPerformanceToolPressed = GUILayout.Button("性能优化工具", toolButtonStyle, GUILayout.Height(20), GUILayout.Width(100));
                if (isPerformanceToolPressed)
                {
                    QGGameTools.PerformanceTool();
                }

                toolButtonStyle.normal.textColor = Color.white;
                GUILayout.EndHorizontal();

                GUILayout.Label("意见问题反馈", labelStyle);

                GUILayout.BeginHorizontal();
                //var isVivoGameBtnPressed = GUILayout.Button("开发平台", toolButtonStyle, GUILayout.Height(20), GUILayout.Width(100));
                //if (isVivoGameBtnPressed)
                //{
                //    QGGameTools.OpenVivoGame();
                //}

                var isIssueBtnPressed = GUILayout.Button("意见反馈", toolButtonStyle, GUILayout.Height(20), GUILayout.Width(100));
                if (isIssueBtnPressed)
                {
                    QGGameTools.OpenIssueGihub();
                }

                var isQuestionBtnPressed = GUILayout.Button("常见问题", toolButtonStyle, GUILayout.Height(20), GUILayout.Width(100));
                if (isQuestionBtnPressed)
                {
                    QGGameTools.OpenQuestionGithub();
                }


                GUILayout.EndHorizontal();

                var tips = new GUIStyle();
                tips.fontSize = 12;
                tips.normal.textColor = Color.yellow;
                tips.margin.left = 20;
                tips.margin.top = 5;
                tips.margin.right = 20;
                GUILayout.TextField("备注：更多文档可参考《使用文档与教程》", tips);
                EditorGUILayout.EndVertical();
            }
        }

        private string getDataInput(string target)
        {
            if (this.formInputData.ContainsKey(target))
                return this.formInputData[target];
            return "";
        }
        private int getDataPop(string target)
        {
            if (this.formIntPopupData.ContainsKey(target))
                return this.formIntPopupData[target];
            return 0;
        }
        private bool getDataCheckbox(string target)
        {
            if (this.formCheckboxData.ContainsKey(target))
                return this.formCheckboxData[target];
            return false;
        }
        private void setData(string target, string value)
        {
            if (formInputData.ContainsKey(target))
            {
                formInputData[target] = value;
            }
            else
            {
                formInputData.Add(target, value);
            }
        }
        private void setData(string target, bool value)
        {
            if (formCheckboxData.ContainsKey(target))
            {
                formCheckboxData[target] = value;
            }
            else
            {
                formCheckboxData.Add(target, value);
            }
        }
        private void setData(string target, int value)
        {
            if (formIntPopupData.ContainsKey(target))
            {
                formIntPopupData[target] = value;
            }
            else
            {
                formIntPopupData.Add(target, value);
            }
        }
        public void formInput(string target, string label, string help = null)
        {
            if (!formInputData.ContainsKey(target))
            {
                formInputData[target] = "";
            }
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(string.Empty, GUILayout.Width(10));
            if (help == null)
            {
                GUILayout.Label(label, GUILayout.Width(140));
            }
            else
            {
                GUILayout.Label(new GUIContent(label, help), GUILayout.Width(140));
            }
            formInputData[target] = GUILayout.TextField(formInputData[target], GUILayout.MaxWidth(EditorGUIUtility.currentViewWidth - 195));
            GUILayout.EndHorizontal();
        }
        public void subFormInput(string target, string label, string help = null)
        {
            if (!formInputData.ContainsKey(target))
            {
                formInputData[target] = "";
            }
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(string.Empty, GUILayout.Width(35));
            if (help == null)
            {
                GUILayout.Label(label, GUILayout.Width(115));
            }
            else
            {
                GUILayout.Label(new GUIContent(label, help), GUILayout.Width(115));
            }
            formInputData[target] = GUILayout.TextField(formInputData[target], GUILayout.MaxWidth(EditorGUIUtility.currentViewWidth - 195));
            GUILayout.EndHorizontal();
        }
        private void formIntPopup(string target, string label, string[] options, int[] values)
        {
            if (!formIntPopupData.ContainsKey(target))
            {
                formIntPopupData[target] = 0;
            }
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(string.Empty, GUILayout.Width(10));
            GUILayout.Label(label, GUILayout.Width(140));
            formIntPopupData[target] = EditorGUILayout.IntPopup(formIntPopupData[target], options, values, GUILayout.MaxWidth(EditorGUIUtility.currentViewWidth - 195));
            GUILayout.EndHorizontal();
        }
        private void formCheckbox(string target, string label, string help = null, bool disable = false, Action<bool> setting = null)
        {
            if (!formCheckboxData.ContainsKey(target))
            {
                formCheckboxData[target] = false;
            }
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(string.Empty, GUILayout.Width(10));
            if (help == null)
            {
                GUILayout.Label(label, GUILayout.Width(140));
            }
            else
            {
                GUILayout.Label(new GUIContent(label, help), GUILayout.Width(140));
            }
            EditorGUI.BeginDisabledGroup(disable);
            formCheckboxData[target] = EditorGUILayout.Toggle(disable ? false : formCheckboxData[target]);

            if (setting != null)
            {
                EditorGUILayout.LabelField("", GUILayout.Width(10));
                // 配置按钮
                if (GUILayout.Button(new GUIContent("设置"), GUILayout.Width(40), GUILayout.Height(18)))
                {
                    setting?.Invoke(true);
                }
                EditorGUILayout.LabelField("", GUILayout.MinWidth(10));
            }

            EditorGUI.EndDisabledGroup();

            if (setting == null)
                EditorGUILayout.LabelField(string.Empty);
            GUILayout.EndHorizontal();
        }

        internal string GetBuildSrc()
        {
            return getDataInput(Key_buildSrc);
        }

        internal void SetBuildSrc(string buildSrc)
        {
            formInputData[Key_buildSrc] = buildSrc;
        }

        internal string GetStreamingAssetsUrl()
        {
            return getDataInput(Key_streamingAssetsUrl);
        }

        internal void SetStreamingAssetsUrl(string streamingAssetsUrl)
        {
            formInputData[Key_streamingAssetsUrl] = streamingAssetsUrl;
        }

        internal string GetWasmUrl()
        {
            return getDataInput(Key_wasmUrl);
        }

        internal void SetWasmUrl(string wasmUrl)
        {
            formInputData[Key_wasmUrl] = wasmUrl;
        }

        internal int GetSubWasmType()
        {
            return getDataPop(Key_usSubWasmType);
        }

        internal bool IsUseSelfLoading()
        {
            return GetSubWasmType() == ((int)SubWasmPkgType.CDN);
        }

        internal void SetUseSelfLoading()
        {
            setData(Key_usSubWasmType, ((int)SubWasmPkgType.CDN));
        }

        internal bool IsUseSubPkgLoading()
        {
            return GetSubWasmType() == ((int)SubWasmPkgType.VIVO);
        }

        internal void SetUseSubPkgLoading()
        {
            setData(Key_usSubWasmType, ((int)SubWasmPkgType.VIVO));
        }

        internal bool IsUseAddressable()
        {
            return getDataCheckbox(Key_useAddressable);
        }

        internal void SetUseAddressable()
        {
            setData(Key_useAddressable, true);
        }

        internal string GetPreLoadUrls()
        {
            return getDataInput(Key_preloadUrls);
        }

        internal void SetPreLoadUrls(String preloadUrls)
        {
            formInputData[Key_preloadUrls] = preloadUrls;
        }

        internal bool IsUseWebgl2()
        {
            return getDataCheckbox(Key_useWebgl2);
        }

        internal void SetIsUseWebgl2(bool useWebgl2)
        {
            setData(Key_useWebgl2, useWebgl2);
        }

        internal bool IsUseCodeSize()
        {
            return getDataCheckbox(Key_useCodeSize);
        }

        internal void SetIsUseCodeSize(bool useCodeSize)
        {
            setData(Key_useCodeSize, useCodeSize);
        }

        internal string GetBgImageSrc()
        {
            return getDataInput(Key_targetBg);
        }

        internal void SetBgImageSrc(string bgImageSrc)
        {
            formInputData[Key_targetBg] = bgImageSrc;
        }

        internal void SetIcon(string icon)
        {
            formInputData[Key_targetIcon] = icon;
        }

        internal void SetPackage(string package)
        {
            formInputData[Key_targetPackage] = package;
        }

        internal void SetName(string name)
        {
            formInputData[Key_targetName] = name;
        }

        internal void SetVersionCode(string versionCode)
        {
            formInputData[Key_targetVC] = versionCode;
        }

        internal void SetVersionName(string versionName)
        {
            formInputData[Key_targetVN] = versionName;
        }

        internal void SetMinPlatformVersion(string minPlatformVersion)
        {
            formInputData[Key_targetMniPV] = minPlatformVersion;
        }

        internal string GetIcon()
        {
            return formInputData[Key_targetIcon];
        }

        internal string GetPackage()
        {
            return formInputData[Key_targetPackage];
        }

        internal string GetName()
        {
            return formInputData[Key_targetName];
        }

        internal string GetVersionCode()
        {
            return formInputData[Key_targetVC];
        }

        internal string GetVersionName()
        {
            return formInputData[Key_targetVN];
        }

        internal string GetMinPlatformVersion()
        {
            return formInputData[Key_targetMniPV];
        }

        internal void SetDeviceOrientation(string orientation)
        {
            if (orientation == "landscape")
            {
                setData(Key_targetOrientation, ((int)Orientation.Landscape));
            } else
            {
                setData(Key_targetOrientation, ((int)Orientation.Portrait));
            }
        }

        internal string GetDeviceOrientation()
        {
            if (getDataPop(Key_targetOrientation) == ((int)Orientation.Landscape))
            {
                return "landscape";
            } else
            {
                return "portrait";
            }
        }

        internal void SetUsTargetBgType(int usTargetBgType)
        {
            setData(Key_usTargetBgType, usTargetBgType);
        }

        internal int GetUsTargetBgType()
        {
            return getDataPop(Key_usTargetBgType);
        }
    }

}