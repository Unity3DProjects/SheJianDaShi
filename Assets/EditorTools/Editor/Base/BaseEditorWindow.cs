using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

public abstract class BaseEditorWindow : EditorWindow
{
    /// <summary>
    /// 默认返回一个800*600的窗口
    /// </summary>
    /// <param name="title"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    protected static T ShowBigWindow<T>(string title = null, float width = 800, float height = 600)
        where T : BaseEditorWindow
    {
        if (title == null)
        {
            title = typeof(T).Name;
        }

        var window = GetWindowWithRect<T>(new Rect(Screen.width / 4f, Screen.height / 5f, width, height));
        window.titleContent = new GUIContent(title);
        return window;
    }

    /// <summary>
    /// 默认返回一个350*600的垂直窗口
    /// </summary>
    /// <param name="title"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    protected static T ShowVerticalWindow<T>(string title = null, float width = 350, float height = 600)
        where T : BaseEditorWindow
    {
        if (title == null)
        {
            title = typeof(T).Name;
        }

        var window = GetWindowWithRect<T>(new Rect(Screen.width / 4f, Screen.height / 5f, width, height));
        window.titleContent = new GUIContent(title);
        return window;
    }


    #region EditorUtility

    protected static void ShowProgress(string title, string info, float progress)
    {
        EditorUtility.DisplayProgressBar(title, info, progress);
    }

    protected static void HideProgress()
    {
        EditorUtility.ClearProgressBar();
    }

    protected static bool ShowConfirmPanel(Action OnConfirm)
    {
        var isConfirm = EditorUtility.DisplayDialog("确认操作", "确定要执行该操作吗", "是", "否");
        if (isConfirm)
        {
            OnConfirm?.Invoke();
        }

        return isConfirm;
    }

    #endregion

    protected static GUILayoutOption Width(float width)
    {
        return GUILayout.Width(width);
    }

    protected static GUILayoutOption Height(float height)
    {
        return GUILayout.Height(height);
    }


    protected static string RenderFolderSelector(string label, string searchPath)
    {
        RenderHorizontal(() =>
        {
            searchPath = EditorGUILayout.TextField(label, searchPath);
            if (GUILayout.Button("选中当前文件夹", GUILayout.Width(150)))
            {
                searchPath = GetActiveFolderPath();
            }
        });
        return searchPath;
    }

    private static string GetActiveFolderPath()
    {
        string path = "Assets";

        foreach (var obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
        {
            path = AssetDatabase.GetAssetPath(obj);
            if (!string.IsNullOrEmpty(path) && System.IO.File.Exists(path))
            {
                path = System.IO.Path.GetDirectoryName(path);
                break;
            }

            if (AssetDatabase.IsValidFolder(path))
            {
                break;
            }
        }

        return path;
    }

    protected static string RenderFolderSelector2(string label, string searchPath)
    {
        RenderHorizontal(() =>
        {
            searchPath = EditorGUILayout.TextField(label, searchPath);
            if (GUILayout.Button("浏览", GUILayout.Width(60)))
            {
                searchPath = EditorUtility.OpenFolderPanel("选择文件夹", searchPath, "");
                if (string.IsNullOrEmpty(searchPath))
                {
                    searchPath = "Assets/";
                }
                else if (!searchPath.Contains(Application.dataPath))
                {
                    EditorUtility.DisplayDialog("错误", "请选择项目内的Assets文件夹", "确定");
                    searchPath = "Assets/";
                }
                else
                {
                    searchPath = "Assets" + searchPath.Replace(Application.dataPath, "");
                }
            }
        });
        return searchPath;
    }

    /// <summary>
    /// 可拖拽区域
    /// </summary>
    /// <param name="text">说明文本</param>
    /// <param name="heigth">高度</param>
    /// <returns>文件路径</returns>
    protected static string[] RenderDragableArea(string[] filePaths, string text = "拖拽文件路径至此", float heigth = 100f,
        float labelMarginTop = 2f)
    {
        Rect rect = EditorGUILayout.GetControlRect(false, heigth);
        var totalWidth = rect.width;
        rect.x = rect.width * 0.1f;
        rect.width = rect.width * 0.8f;
        EditorGUI.DrawRect(rect, new Color(46 / 255f, 46 / 255f, 46 / 255f, 1));

        var width = text.Length * 13f;
        Rect labelRect = new Rect(totalWidth / 2f - (width / 2f), heigth / labelMarginTop, width, 16);
        EditorGUI.LabelField(labelRect, text);

        var currentEvent = UnityEngine.Event.current;
        if (rect.Contains(currentEvent.mousePosition))
        {
            switch (currentEvent.type)
            {
                case EventType.DragUpdated:
                    DragAndDrop.visualMode = DragAndDropVisualMode.Generic; //到达目标区域的显示方式
                    break;
                case EventType.DragPerform:
                    string[] paths = DragAndDrop.paths;
                    return paths;
            }
        }

        return filePaths;
    }

    /// <summary>
    /// 可拖拽区域
    /// </summary>
    /// <param name="text">说明文本</param>
    /// <param name="heigth">高度</param>
    /// <returns>文件路径</returns>
    protected static string RenderDragableArea(string path, string text = "拖拽文件路径至此", float heigth = 100f)
    {
        Rect rect = EditorGUILayout.GetControlRect(false, heigth);
        var totalWidth = rect.width;
        rect.x = rect.width * 0.1f;
        rect.width = rect.width * 0.8f;
        EditorGUI.DrawRect(rect, new Color(46 / 255f, 46 / 255f, 46 / 255f, 1));

        var width = text.Length * 13f;
        Rect labelRect = new Rect(totalWidth / 2f - (width / 2f), heigth / 2f, width, 16);
        EditorGUI.LabelField(labelRect, text);

        var currentEvent = UnityEngine.Event.current;
        if (rect.Contains(currentEvent.mousePosition))
        {
            switch (currentEvent.type)
            {
                case EventType.DragUpdated:
                    DragAndDrop.visualMode = DragAndDropVisualMode.Generic; //到达目标区域的显示方式
                    break;
                case EventType.DragPerform:
                    string[] paths = DragAndDrop.paths;
                    if (paths.Length > 0)
                        return paths.First();
                    break;
            }
        }

        return path;
    }

    protected static void RenderHorizontal(Action callBack, params GUILayoutOption[] layoutOptions)
    {
        GUILayout.BeginHorizontal(layoutOptions);
        callBack?.Invoke();
        GUILayout.EndHorizontal();
    }

    protected static void RenderVertical(Action callBack, params GUILayoutOption[] layoutOptions)
    {
        GUILayout.BeginVertical(layoutOptions);
        callBack?.Invoke();
        GUILayout.EndVertical();
    }

    protected static Vector2 RenderScrollView(Vector2 scrollPos, Action callBack,
        params GUILayoutOption[] layoutOptions)
    {
        var pos = GUILayout.BeginScrollView(scrollPos, layoutOptions);
        callBack?.Invoke();
        GUILayout.EndScrollView();
        return pos;
    }

    protected static void RenderLine(float lineHeight = 1)
    {
        Rect rect = EditorGUILayout.GetControlRect(false, lineHeight);
        EditorGUI.DrawRect(rect, Color.gray);
    }

    protected static void RenderLine(Color color, float alpha = 0.7f)
    {
        Rect rect = EditorGUILayout.GetControlRect(false, 1);
        color = new Color(color.r, color.g, color.b, alpha);
        EditorGUI.DrawRect(rect, color);
    }


    protected static void RenderLabel(string text, params GUILayoutOption[] layoutOptions)
    {
        GUILayout.Label(text, layoutOptions);
    }

    protected static void RenderLabel(string text, GUIStyle style, params GUILayoutOption[] layoutOptions)
    {
        GUILayout.Label(text, style, layoutOptions);
    }

    protected static void RenderLabel(string text, Color color, params GUILayoutOption[] layoutOptions)
    {
        Color originalColor = GUI.color;
        GUI.color = color;
        GUILayout.Label(text, layoutOptions);
        GUI.color = originalColor;
    }

    protected static string RenderTextField(string label, ref string text, params GUILayoutOption[] options)
    {
        return text = EditorGUILayout.TextField(label, text, options);
    }

    protected static string RenderTextField(string label, string text, params GUILayoutOption[] options)
    {
        return EditorGUILayout.TextField(label, text, options);
    }

    protected static void RenderButton(string title, Action callBack, params GUILayoutOption[] layoutOptions)
    {
        if (GUILayout.Button(title, layoutOptions))
        {
            callBack?.Invoke();
        }
    }

    protected static void RenderButton(string title, Color color, Action callBack,
        params GUILayoutOption[] layoutOptions)
    {
        var originalColor = GUI.color;
        GUI.color = color;

        if (GUILayout.Button(title, layoutOptions))
        {
            callBack?.Invoke();
        }

        GUI.color = originalColor;
    }

    protected static void RenderButton(string title, Action callBack, float height = 24f)
    {
        if (GUILayout.Button(title, GUILayout.Height(height)))
        {
            callBack?.Invoke();
        }
    }

    protected static void Space(float pixels = 10f)
    {
        GUILayout.Space(pixels);
    }

    protected static int RenderIntSlider(int value, int leftValue, int rightValue, params GUILayoutOption[] options)
    {
        return EditorGUILayout.IntSlider(value, leftValue, rightValue, options);
    }

    protected static int RenderIntSlider(string label, int value, int leftValue, int rightValue,
        params GUILayoutOption[] options)
    {
        return EditorGUILayout.IntSlider(label, value, leftValue, rightValue, options);
    }

    protected static float RenderFloatSlider(string label, float value, float leftValue, float rightValue,
        params GUILayoutOption[] options)
    {
        return EditorGUILayout.Slider(label, value, leftValue, rightValue, options);
    }

    protected static bool RenderFoldout(bool foldout, string content)
    {
        return EditorGUILayout.Foldout(foldout, content);
    }

    protected static void RenderHelpBox(string msg, MessageType messageType)
    {
        EditorGUILayout.HelpBox(msg, messageType);
    }

    protected static bool RenderToggle(string label, bool value, params GUILayoutOption[] options)
    {
        return EditorGUILayout.Toggle(label, value, options);
    }

    protected static Enum RenderEnumPopup(string label, Enum selected, params GUILayoutOption[] options)
    {
        return EditorGUILayout.EnumPopup(label, selected, options);
    }

    protected static void Template()
    {
    }
}