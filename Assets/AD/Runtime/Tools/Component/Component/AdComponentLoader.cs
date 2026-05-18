using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class AdComponentLoader
{
    public static void CreateAt(GameObject parentGo, Vector2 position)
    {
        var o = Object.Instantiate(AdComp, parentGo.transform);
        o.GetComponent<RectTransform>().anchoredPosition = position;
    }

    private static GameObject AdComp;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        AdComp = Resources.Load<GameObject>("AdComponent");
    }
}