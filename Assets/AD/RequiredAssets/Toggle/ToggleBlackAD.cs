using System;
using UnityEngine;
using UnityEngine.UI;

public class ToggleBlackAD : MonoBehaviour
{
    private void Awake()
    {
        var toggle = GetComponent<Toggle>();

        if (toggle)
        {
            toggle.onValueChanged.AddListener(value =>
            {
                if (value)
                {
                    ADManager.Instance.ShowBlackAd();
                }
            });
        }
    }
}