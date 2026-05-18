using System;
using System.Collections;
using System.Collections.Generic;
using AD.Editor;
using UnityEditor;
using UnityEngine;

internal class ADAssets : SingletonSO<ADAssets>
{
    [SerializeField] internal GameObject[] landscapeGos;
    [SerializeField] internal GameObject[] portraitGos;
    [SerializeField] internal PlatformComponents platform2Gos;
    [SerializeField] internal GameObject adManagerGo;

    protected override void AfterCreate()
    {
        Init();
    }

    internal void Init()
    {
        platform2Gos.Clear();
        platform2Gos.Add(EPlatform.HuaWei, new Components()
        {
            gos = new List<GameObject>()
            {
                AssetDatabase.LoadAssetAtPath<GameObject>(
                    "Assets/AD/Runtime/RawAssets/HuaWei_RPK/Prefabs/AD.prefab"),
                AssetDatabase.LoadAssetAtPath<GameObject>(
                    "Assets/AD/Runtime/RawAssets/HuaWei_RPK/Prefabs/ADAnalyticsManager.prefab"),
            }
        });
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }

    [Serializable]
    internal class PlatformComponents : AD.Editor.SerializableDictionary<EPlatform, Components>
    {
    }

    [Serializable]
    internal class Components
    {
        public List<GameObject> gos;
    }
}