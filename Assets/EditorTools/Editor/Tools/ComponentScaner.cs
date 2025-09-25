using System.Collections.Generic;
using System.Linq;
using EditorTools.Editor.ComponentModifyer;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ComponentScaner : BaseEditorWindow
{
    private static string searchkey;
    private static List<string> searchNames = new List<string>();
    private static List<int> incetanceID = new List<int>();

    private Vector2 scrollPos = Vector2.zero;

    [MenuItem("Tools/工具/文本查组件", priority = 300)]
    public static void Open() => ShowBigWindow<ComponentScaner>("文本查组件工具");

    private void OnGUI()
    {
        Space(20);

        RenderHorizontal(() =>
        {
            searchkey = EditorGUILayout.TextField(searchkey, GUILayout.Height(25));

            if (GUILayout.Button("在当前场景搜索", GUILayout.Height(25), GUILayout.Width(160)))
            {
                SearchTextByContent();
            }

            if (GUILayout.Button("清空", GUILayout.Height(25), GUILayout.Width(40)))
            {
                searchNames.Clear();
                incetanceID.Clear();
            }
        });

        Space(10);
        RenderLine(Color.cyan);
        Space(20);


        scrollPos = RenderScrollView(scrollPos, () =>
        {
            if (searchNames != null)
            {
                if (searchNames.Count == incetanceID.Count)
                {
                    for (int i = 0; i < searchNames.Count; i++)
                    {
                        EditorUtility.InstanceIDToObject(incetanceID[i]).Render(typeof(GameObject));
                    }
                }
            }
        });
    }


    /// <summary>
    /// 根据文本内容搜索当年场景中的组件返回组件名称
    /// </summary>
    public static void SearchTextByContent()
    {
        searchNames.Clear();
        incetanceID.Clear();
        if (string.IsNullOrEmpty(searchkey))
        {
            Debug.Log("先输入搜索文本");
            return;
        }

        var TMP_Texts = EditorHelper.GetComponentsInCurScene<TMP_Text>();
        var Texts = EditorHelper.GetComponentsInCurScene<Text>();

        if (TMP_Texts != null)
        {
            TMP_Texts.Where(Text => Text.text.Contains(searchkey)).ToList().ForEach(Text =>
            {
                searchNames.Add(Text.name);
                incetanceID.Add(Text.GetInstanceID());
            });
        }

        if (Texts != null)
        {
            Texts.Where(Text => Text.text.Contains(searchkey)).ToList().ForEach(Text =>
            {
                searchNames.Add(Text.name);
                incetanceID.Add(Text.GetInstanceID());
            });
        }
    }
}