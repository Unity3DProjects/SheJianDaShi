using System;
using AD.BlackBag;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class BtnAddToDesk : MonoBehaviour
{
#if UNITY_WEBGL

    private void Start()
    {
        if (ADManager.Instance.AllowedButtonType().HasFlag(AdButtonType.添加桌面))
            GetComponent<Button>()?.onClick.AddListener(OnClick);
        else Destroy(gameObject);
    }

    private void OnDestroy()
    {
        GetComponent<Button>()?.onClick.RemoveAllListeners();
    }

    public void OnClick()
    {
        ADManager.Instance.CreateShortcutButton();
    }
#endif
}