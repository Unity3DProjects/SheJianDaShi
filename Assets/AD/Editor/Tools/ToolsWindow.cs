using UnityEditor;
using UnityEngine;

namespace AD.Editor.Tools
{
    public class ToolsWindow : EditorWindow
    {
        private const float windowDefaulteWidth = 800;
        private const float windowDefaulteHeight = 600;

        private int toolBarIndex;

        private readonly string[] toolBarName = new[] { "图片处理", "场景切换", "字体替换", "丢失脚本清理", "碰撞体替换" };

        [MenuItem("Tools/工具集", false, -1)]
        public static void OpenWindow()
        {
            var rect = new Rect(
                new Vector2(Screen.width / 2.5f - windowDefaulteWidth / 2, Screen.height / 2 - windowDefaulteHeight / 2),
                new Vector2(windowDefaulteWidth, windowDefaulteHeight));
            var window = GetWindow(typeof(ToolsWindow));
            window.position = rect;
        }

        private void OnGUI()
        {
            GUILayout.BeginVertical();
            toolBarIndex = GUILayout.Toolbar(toolBarIndex, toolBarName);
            GUILayout.Space(1);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Space(10);
            switch (toolBarIndex)
            {
                case 0:
                    TextureProcessorWindow.Render();
                    break;
                case 1:
                    EditorSceneWindow.Render();
                    break;
                case 2:
                    ExchangeFont.Render();
                    break;
                case 3:
                    MissingScriptCleaner.Render();
                    break;
                case 4:
                    ExchangeBoxColliderInfo.Render();
                    break;
            }

            GUILayout.EndVertical();
        }
    }
}