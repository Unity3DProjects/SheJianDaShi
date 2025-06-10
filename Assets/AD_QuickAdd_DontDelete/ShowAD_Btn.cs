using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 该按钮注册事件加广告
/// </summary>
public class ShowAD_Btn : MonoBehaviour
{
    [Header("原生类型，转化时需在特定位置选上指定的类型，以便后续路总、烨子做区分")]
    public CustomType_E customType=CustomType_E.其他模板;
    [Header("是否为白包，不选则黑包")]
    public bool isWhiteAD;
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            if(isWhiteAD) 
                ADManager.Instance.ShowAD_White(customType);
            else
               ADManager.Instance.ShowAD_Black(customType);
            
        });
    }
}
