using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUID : MonoBehaviour
{
    [SerializeField]
    public string guid="0";

    void Awake()
    {
        // if (string.IsNullOrEmpty(guid))
        // {
        //     guid = Guid.NewGuid().ToString();
        // }
    }

    public string GetGUID() => guid;
}
