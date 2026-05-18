using System;
using UnityEngine;
using UnityEngine.UI;

public class ToggleBlackAD : MonoBehaviour
{
    private Toggle toggle;

    protected void Start()
    {
        toggle = GetComponent<Toggle>();
        if (toggle != null)
            toggle.onValueChanged.AddListener(OnToggle);
    }

    protected void OnDestroy()
    {
        if (toggle != null)
            toggle.onValueChanged.RemoveListener(OnToggle);
    }

    private void OnToggle(bool value)
    {
        if (value)
            ADManager.Instance.ShowBlackAd();
    }
}