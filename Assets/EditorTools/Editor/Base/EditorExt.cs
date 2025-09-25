using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public static class EditorExt
{
    public static string[] FindAssetAtFullPath(this BaseEditorWindow baseEditorWindow, string fileName,
        string extentionName,
        string[] searchfolder)
    {
        string[] objs = AssetDatabase.FindAssets(fileName, searchfolder)
            .Select(AssetDatabase.GUIDToAssetPath)
            .Where(path => path != null && extentionName == Path.GetExtension(path))
            .ToArray();
        return objs;
    }

    public static T[] FindAndLoadAssetAtPath<T>(this BaseEditorWindow baseEditorWindow, string filter,
        string[] searchfolder) where T : Object
    {
        T[] objs = AssetDatabase.FindAssets(filter, searchfolder)
            .Select(AssetDatabase.GUIDToAssetPath)
            .Select(AssetDatabase.LoadAssetAtPath<T>)
            .Where(mono => mono != null).ToArray();
        return objs;
    }

    public static void RenderAssetAtPath<T>(this BaseEditorWindow baseEditorWindow, string path) where T : Object
    {
        AssetDatabase.LoadAssetAtPath<T>(path).Render(typeof(T));
    }

    public static void RenderAssetAtPath(this BaseEditorWindow baseEditorWindow, string path, Type type)
    {
        AssetDatabase.LoadAssetAtPath(path, type).Render(type);
    }

    public static Object Render(this Object obj, Type type, bool allowSceneObjects = true,
        params GUILayoutOption[] options)
    {
        return EditorGUILayout.ObjectField(obj, type, allowSceneObjects, options);
    }

    public static Object Render(this Object obj, string label, Type type, bool allowSceneObjects = true,
        params GUILayoutOption[] options)
    {
        return EditorGUILayout.ObjectField(label, obj, type, allowSceneObjects, options);
    }
}