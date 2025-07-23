using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AD.Editor.Macro
{
    public class MacroToggleWindow : EditorWindow
    {
        // 内置的宏定义列表
        private List<string> predefinedMacros =
            Enum.GetNames(typeof(EPlatform)).ToList();

        private Vector2 scrollPosition;
        private bool showHelp = true;

        [MenuItem("Tools/宏定义管理")]
        public static void ShowWindow()
        {
            GetWindow<MacroToggleWindow>("宏定义管理");
        }

        private void OnGUI()
        {
            // 帮助信息折叠区域
            showHelp = EditorGUILayout.Foldout(showHelp, "帮助信息");
            if (showHelp)
            {
                EditorGUILayout.HelpBox(
                    "此面板显示和管理预定义的宏定义。\n" +
                    "绿色表示宏已启用，红色表示未启用。\n" +
                    "点击按钮可以切换宏状态。\n" +
                    "修改后需要重新编译脚本才能生效。",
                    MessageType.Info);
            }

            EditorGUILayout.Space();

            // 获取当前所有宏定义
            ISet<string> currentMacros = GetDefine();

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            // 显示每个宏的开关状态
            foreach (string macro in predefinedMacros)
            {
                EditorGUILayout.BeginHorizontal();

                // 显示宏名称和状态
                bool isActive = currentMacros.Contains(macro);
                GUI.color = isActive ? Color.green : Color.red;
                EditorGUILayout.LabelField(macro, GUILayout.Width(150));
                GUI.color = Color.white;

                // 切换按钮
                if (GUILayout.Button(isActive ? "禁用" : "启用", GUILayout.Width(60)))
                {
                    ToggleMacro(macro, !isActive, currentMacros);
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();
        }

        private void ToggleMacro(string macro, bool enable, ISet<string> currentMacros)
        {
            if (enable)
            {
                currentMacros.Add(macro);
            }
            else
            {
                currentMacros.Remove(macro);
            }

            SetDefine(currentMacros);
        }

        public static void SetPlatform(EPlatform platform)
        {
            var define = GetDefine();
            foreach (var s in Enum.GetNames(typeof(EPlatform))) define.Remove(s);
            define.Add(platform.ToString());
            SetDefine(define);
        }

        public static ISet<string> GetDefine()
        {
            return new HashSet<string>(PlayerSettings.GetScriptingDefineSymbolsForGroup(
                BuildTargetGroup.WebGL).Split(';'));
        }

        public static void SetDefine(IEnumerable<string> macros)
        {
            // 更新PlayerSettings中的宏定义
            PlayerSettings.SetScriptingDefineSymbolsForGroup(
                BuildTargetGroup.WebGL,
                string.Join(";", new HashSet<string>(macros)));

            // 强制重新编译脚本
            AssetDatabase.Refresh();
        }
    }
}