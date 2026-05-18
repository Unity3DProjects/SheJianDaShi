using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[DisallowMultipleComponent]
public class NoNGUI : MonoBehaviour
{
    private const string UICameraAssemblyQualifiedName =
        "UICamera, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";

    private const string UICameraTypeName = "UICamera";

    private static readonly Type UICameraType;
    private static readonly HashSet<MonoBehaviour> Components = new HashSet<MonoBehaviour>();
    private static int count;

    static NoNGUI()
    {
        UICameraType = Type.GetType(UICameraAssemblyQualifiedName);
        if (UICameraType != null) return;
        foreach (var assembly in AppDomain
            .CurrentDomain
            .GetAssemblies())
        {
            try
            {
                var type = assembly.GetType(UICameraTypeName);
                if (type == null) continue;
                UICameraType = type;
            }
            catch
            {
            }
        }
    }


    private void OnEnable()
    {
        if (UICameraType == null) return;
        count++;
        StartCoroutine(DelayCheck());
    }

    private void LoadCamera()
    {
        foreach (var uiCamera in Resources.FindObjectsOfTypeAll(UICameraType).OfType<MonoBehaviour>())
        {
            uiCamera.enabled = false;
            Components.Add(uiCamera);
        }
    }

    private IEnumerator DelayCheck()
    {
        LoadCamera();
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(2f, 6f));
            LoadCamera();
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        if (UICameraType == null) return;
        if (--count != 0) return;
        foreach (var uiCamera in Components.Where(e => e != null)) uiCamera.enabled = true;
        Components.Clear();
    }
}