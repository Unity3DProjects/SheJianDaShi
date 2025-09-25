using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 给按钮添加宝箱
/// </summary>
public class AddBox_Btn : MonoBehaviour
{
    
    private void OnEnable()
    {
       GetComponent<Button>().onClick.AddListener(() =>
       {
           ADManager.Instance.ShowAD_Box();
           ADManager.Instance.ShowAD_Black();
       });
    }
}
